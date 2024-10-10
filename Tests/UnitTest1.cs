using System;
using System.Collections.Generic;
using System.Numerics;
using Xunit;
using DataProcessor;
using Aspose.Cells;
using System.IO;
using HeroesLibrary;
using System.Reflection;

public class DataProcessorTests
{

    // Тесты для класса DataDecriptor

    // Тест на проверку того, что метод дешифровки возвращает непустой список
    [Fact]
    public void DataDecryptor_GetDecryptedRecords_ShouldReturnNonEmptyList()
    {
        // Создаем тестовый список зашифрованных данных
        List<BigInteger> encryptedRecords = new List<BigInteger> { new BigInteger(3) };
        // Создаем список имён свойств
        List<string> propertyNames = new List<string> { "Property1", "Property2" };

        // Создаем экземпляр класса дешифратора с тестовыми данными
        DataDecryptor decryptor = new DataDecryptor(encryptedRecords, propertyNames);

        // Вызываем метод дешифровки для получения исходных данных
        List<List<string>> decryptedRecords = decryptor.GetDecryptedRecords();

        // Assert
        // Проверяем, что список расшифрованных данных не пустой
        Assert.NotNull(decryptedRecords); // Список не должен быть null
        Assert.NotEmpty(decryptedRecords); // Список не должен быть пустым
    }

    /// <summary>
    /// Тест для проверки корректности расшифровки одной записи
    /// </summary>
    [Fact]
    public void GetDecryptedRecords_ShouldReturnCorrectNames_WhenSingleRecordIsEncrypted()
    {
        // Подготовка
        var encryptedRecords = new List<BigInteger> { new BigInteger(3) }; // 3 в двоичной системе - 11, что соответствует Property1 и Property2
        var propertyNames = new List<string> { "Property1", "Property2", "Property3" };

        var decryptor = new DataDecryptor(encryptedRecords, propertyNames);

        // Действие
        var result = decryptor.GetDecryptedRecords();

        // Результат
        Assert.NotNull(result);
        Assert.Single(result); // Ожидаем, что будет одна запись
        Assert.Equal(new List<string> { "Property1", "Property2" }, result[0]); // Проверяем, что расшифровка верна
    }

    /// <summary>
    /// Тест для проверки корректности расшифровки нескольких записей
    /// </summary>
    [Fact]
    public void GetDecryptedRecords_ShouldReturnCorrectNames_WhenMultipleRecordsAreEncrypted()
    {
        // Подготовка
        var encryptedRecords = new List<BigInteger> { new BigInteger(3), new BigInteger(5) }; // 3 -> Property1, Property2; 5 -> Property1, Property3
        var propertyNames = new List<string> { "Property1", "Property2", "Property3" };

        var decryptor = new DataDecryptor(encryptedRecords, propertyNames);

        // Действие
        var result = decryptor.GetDecryptedRecords();

        // Результат
        Assert.NotNull(result);
        Assert.Equal(2, result.Count); // Ожидаем две расшифрованные записи

        Assert.Equal(new List<string> { "Property1", "Property2" }, result[0]); // Первая запись
        Assert.Equal(new List<string> { "Property1", "Property3" }, result[1]); // Вторая запись
    }

    /// <summary>
    /// Тест для проверки корректности расшифровки записи, содержащей только один бит
    /// </summary>
    [Fact]
    public void GetDecryptedRecords_ShouldReturnCorrectName_WhenSingleBitIsSet()
    {
        // Подготовка
        var encryptedRecords = new List<BigInteger> { new BigInteger(1) }; // 1 в двоичной системе - 01, что соответствует только Property1
        var propertyNames = new List<string> { "Property1", "Property2", "Property3" };

        var decryptor = new DataDecryptor(encryptedRecords, propertyNames);

        // Действие
        var result = decryptor.GetDecryptedRecords();

        // Результат
        Assert.NotNull(result);
        Assert.Single(result); // Ожидаем, что будет одна запись
        Assert.Equal(new List<string> { "Property1" }, result[0]); // Проверяем, что расшифровка верна
    }




    // Тесты для класса DataWriter

    // Тест на проверку корректной записи зашифрованных данных в файл
    [Fact]
    public void DataWriter_WriteEncryptedDataToFile_ShouldCreateFile()
    {
        // Подготовка
        // Создаем список имён свойств
        List<string> propertyNames = new List<string> { "Property1", "Property2" };
        // Создаем список зашифрованных записей
        List<BigInteger> encryptedRecords = new List<BigInteger> { new BigInteger(10), new BigInteger(20) };
        // Указываем путь к тестовому файлу
        string filePath = "test_output.txt";

        // Вызываем метод записи данных в файл
        DataWriter.WriteEncryptedDataToFile(propertyNames, encryptedRecords, filePath);

        // Проверяем, что файл был создан
        Assert.True(File.Exists(filePath));

        // Удаление файла после проверки, чтобы не засорять систему
        File.Delete(filePath);
    }




    // Тесты для класса DataEncryptor

    [Fact]
    public void DataEncryptor_GetEncryptedRecords_ShouldReturnNonEmptyList()
    {
        // Создание workbook с помощью Aspose.Cells
        Workbook workbook = new Workbook();
        Worksheet worksheet = workbook.Worksheets[0];
        Cells cells = worksheet.Cells;

        // Заполнение данных в ячейках для теста
        cells[0, 0].PutValue("Название");
        cells[0, 1].PutValue("Число значений");
        cells[1, 0].PutValue("Item1");
        cells[1, 1].PutValue(5);
        cells[2, 0].PutValue("Item2");
        cells[2, 1].PutValue(10);

        // Данные для шифрования
        List<List<string>> data = new List<List<string>>()
            {
                new List<string> { "Item1", "5" },
                new List<string> { "Item2", "10" }
            };

        // Имена колонок
        List<string> columnNames = new List<string> { "Название", "Число значений" };

        // Сопоставление колонок
        Dictionary<string, string> mappings = new Dictionary<string, string>()
            {
                { "Название", "Name" },
                { "Число значений", "ValueCount" }
            };

        // Создание объекта DataEncryptor
        DataEncryptor encryptor = new DataEncryptor(workbook, data, columnNames, mappings);

        // Выполнение метода для шифрования
        var result = encryptor.GetEncryptedRecords();

        // Проверка, что результат не пустой
        Assert.NotNull(result); // Проверяем, что результат не null
        Assert.NotEmpty(result); // Проверяем, что список не пустой
    }

    /// <summary>
    /// Тест на проверку, если значение клетки меньше минимально допустимого значения.
    /// Должен вернуть -1.
    /// </summary>
    [Fact]
    public void FindClassIndexForCellValue1()
    {
        // Подготовка
        var dataProcessor = new DataEncryptor(new Workbook(), new List<List<string>>(), new List<string>(), new Dictionary<string, string>());
        var appropriateValues = new List<List<string>>
        {
            new List<string> { "10", "20", "30" }  // Допустимые значения для столбца 0
        };

        // Установка приватного поля _appropriateValues через рефлексию
        var fieldInfo = typeof(DataEncryptor).GetField("_appropriateValues", BindingFlags.NonPublic | BindingFlags.Instance);
        fieldInfo.SetValue(dataProcessor, appropriateValues);

        string cellValue = "5";  // Значение клетки, меньше минимального допустимого
        int columnIndex = 0;

        // Используем рефлексию для вызова закрытого метода
        var methodInfo = typeof(DataEncryptor).GetMethod("FindClassIndexForCellValue", BindingFlags.NonPublic | BindingFlags.Instance);
        int result = (int)methodInfo.Invoke(dataProcessor, new object[] { cellValue, columnIndex });

        // Результат
        Assert.Equal(-1, result);  // Ожидаем, что метод вернёт -1
    }

    [Fact]
    public void FindClassIndexForCellValue2()
    {
        // Подготовка
        var dataProcessor = new DataEncryptor(new Workbook(), new List<List<string>>(), new List<string>(), new Dictionary<string, string>());
        var appropriateValues = new List<List<string>>
        {
            new List<string> { "10", "20", "30", "40", "50" }  // Допустимые значения для столбца 0
        };

        // Установка приватного поля _appropriateValues через рефлексию
        var fieldInfo = typeof(DataEncryptor).GetField("_appropriateValues", BindingFlags.NonPublic | BindingFlags.Instance);
        fieldInfo.SetValue(dataProcessor, appropriateValues);

        string cellValue = "50";  // Значение клетки
        int columnIndex = 0;

        // Используем рефлексию для вызова закрытого метода
        var methodInfo = typeof(DataEncryptor).GetMethod("FindClassIndexForCellValue", BindingFlags.NonPublic | BindingFlags.Instance);
        int result = (int)methodInfo.Invoke(dataProcessor, new object[] { cellValue, columnIndex });

        // Результат
        Assert.Equal(4, result);  // Ожидаем, что метод вернёт -1
    }

    [Fact]
    public void FindClassIndexForCellValue3()
    {
        // Подготовка
        var dataProcessor = new DataEncryptor(new Workbook(), new List<List<string>>(), new List<string>(), new Dictionary<string, string>());
        var appropriateValues = new List<List<string>>
        {
            new List<string> { "10", "20", "30", "40", "50" }  // Допустимые значения для столбца 0
        };

        // Установка приватного поля _appropriateValues через рефлексию
        var fieldInfo = typeof(DataEncryptor).GetField("_appropriateValues", BindingFlags.NonPublic | BindingFlags.Instance);
        fieldInfo.SetValue(dataProcessor, appropriateValues);

        string cellValue = "45";  // Значение клетки
        int columnIndex = 0;

        // Используем рефлексию для вызова закрытого метода
        var methodInfo = typeof(DataEncryptor).GetMethod("FindClassIndexForCellValue", BindingFlags.NonPublic | BindingFlags.Instance);
        int result = (int)methodInfo.Invoke(dataProcessor, new object[] { cellValue, columnIndex });

        // Результат
        Assert.Equal(3, result);  // Ожидаем, что метод вернёт -1
    }

    /// <summary>
    /// Тест для проверки, что метод шифрования возвращает пустой список, когда нет данных.
    /// </summary>
    [Fact]
    public void DataEncryptor_GetEncryptedRecords_ShouldReturnEmptyList_WhenNoData()
    {
        // Создаем пустую рабочую книгу Excel
        var workbook = new Workbook();

        // Создаем пустой список данных
        var data = new List<List<string>>();

        // Создаем пустой словарь для шифрования
        var mappings = new Dictionary<string, string>();

        // Создаем экземпляр DataEncryptor с пустыми данными
        var encryptor = new DataEncryptor(workbook, data, new List<string>(), mappings);

        // Получаем зашифрованные записи
        var result = encryptor.GetEncryptedRecords();

        // Проверяем, что результат не null и пустой
        Assert.NotNull(result);
        Assert.Empty(result);  // Ожидаем пустой список, так как данных нет
    }





    // Тесты для класса Hero

    /// <summary>
    /// Тест для проверки, что герой корректно инициализируется.
    /// </summary>
    [Fact]
    public void Hero_ShouldInitializeCorrectly()
    {
        // Подготовка
        var hero = new Hero("John", "100", 10, "magical", 30, "Low");  // Инициализация героя

        // Проверка свойств героя
        Assert.Equal("John", hero.Name);  // Проверяем имя героя
        Assert.Equal("100", hero.Main_attribute);   // Проверяем здоровье героя
        Assert.Equal(10, hero.Damage);  // Проверяем атаку героя
        Assert.Equal("magical", hero.Attack_type); // Проверяем тип атаки
        Assert.Equal(30, hero.Move_speed); // Проверяем скорость движения
        Assert.Equal("Low", hero.Difficulty); // Проверяем уровень сложности
    }

    /// <summary>
    /// Тест для проверки, что герой корректно инициализируется с другими значениями.
    /// </summary>
    [Fact]
    public void Hero_ShouldInitializeCorrectly2()
    {
        // Подготовка данных для героя
        string name = "John";
        string mainAttribute = "Strength";
        int damage = 50;
        string attackType = "Melee";
        int moveSpeed = 300;
        string difficulty = "Medium";

        // Инициализация героя
        var hero = new Hero(name, mainAttribute, damage, attackType, moveSpeed, difficulty);

        // Проверка свойств героя
        Assert.Equal("John", hero.Name);                  // Проверяем имя героя
        Assert.Equal("Strength", hero.Main_attribute);    // Проверяем основной атрибут героя
        Assert.Equal(50, hero.Damage);                    // Проверяем урон героя
        Assert.Equal("Melee", hero.Attack_type);          // Проверяем тип атаки героя
        Assert.Equal(300, hero.Move_speed);               // Проверяем скорость передвижения героя
        Assert.Equal("Medium", hero.Difficulty);          // Проверяем сложность управления героем
    }

    /// <summary>
    /// Тест для проверки, что урон героя находится в допустимом диапазоне.
    /// </summary>
    [Fact]
    public void Hero_ShouldHaveValidDamage()
    {
        // Подготовка
        var hero = new Hero("John", "Strength", 50, "Melee", 300, "Medium");

        // Действие
        int damage = hero.Damage; // Получаем значение урона героя

        // Проверка диапазона урона
        Assert.InRange(damage, 0, 1000);  // Ожидаем, что урон находится в диапазоне от 0 до 1000
    }

    /// <summary>
    /// Тест для проверки, что скорость передвижения героя находится в допустимом диапазоне.
    /// </summary>
    [Fact]
    public void Hero_ShouldHaveValidMoveSpeed()
    {
        // Подготовка
        var hero = new Hero("John", "Agility", 45, "Ranged", 500, "Hard");

        // Действие
        int moveSpeed = hero.Move_speed; // Получаем значение скорости передвижения героя

        // Проверка диапазона скорости передвижения
        Assert.InRange(moveSpeed, 100, 600);  // Ожидаем, что скорость находится в диапазоне от 100 до 600
    }

    /// <summary>
    /// Тест для проверки работы различных типов атак у героев.
    /// </summary>
    [Fact]
    public void Hero_ShouldHandleDifferentAttackTypes()
    {
        // Подготовка
        var meleeHero = new Hero("MeleeHero", "Strength", 60, "Melee", 350, "Easy");
        var rangedHero = new Hero("RangedHero", "Agility", 40, "Ranged", 400, "Hard");

        // Проверка типа атаки у героя ближнего боя
        Assert.Equal("Melee", meleeHero.Attack_type);   // Ожидаем тип атаки "Melee"

        // Проверка типа атаки у героя дальнего боя
        Assert.Equal("Ranged", rangedHero.Attack_type); // Ожидаем тип атаки "Ranged"
    }

    /// <summary>
    /// Тест для проверки уровня сложности управления героем.
    /// </summary>
    [Fact]
    public void Hero_ShouldHaveDifficultyLevel()
    {
        // Подготовка
        var hero = new Hero("John", "Intelligence", 30, "Magic", 280, "Medium");

        // Действие
        string difficulty = hero.Difficulty; // Получаем уровень сложности

        // Проверка, что уровень сложности является допустимым
        Assert.True(difficulty == "Easy" || difficulty == "Medium" || difficulty == "Hard", "Difficulty should be valid");
    }
}