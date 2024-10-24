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
}