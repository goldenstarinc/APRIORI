using Aspose.Cells;
using HeroesLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessor
{
    /// <summary>
    /// Класс, отвечающий за чтение данных героев
    /// Для обработки данных использует библиотеку Aspose.Cells
    /// </summary>
    public class HeroRepository
    {
        // Файл Excel с данными о героях
        private Workbook _workbook;
        public HeroRepository(string filePath)
        {
            Workbook workbook = new Workbook(filePath);
        }

        /// <summary>
        /// Считывает героев из данного Excel файла
        /// </summary>
        /// <returns>Список объектов типа Hero</returns>
        private List<Hero> ReadHeroes()
        {
            List<Hero> heroes = new List<Hero>();

            Worksheet worksheet = _workbook.Worksheets[0];

            // Получаем диапазон данных
            Cells cells = worksheet.Cells;

            for (int i = 1; i <= cells.MaxDataRow; i++)
            {
                // Читаем значения из каждой ячейки строки
                string name = cells[i, 0].StringValue;
                string mainAttribute = cells[i, 1].StringValue;
                int damage = cells[i, 2].IntValue;
                string attackType = cells[i, 3].StringValue;
                int moveSpeed = cells[i, 4].IntValue;
                string difficulty = cells[i, 5].StringValue;

                // Создаем объект Hero и добавляем его в список
                var hero = new Hero(name, mainAttribute, damage, attackType, moveSpeed, difficulty);
                heroes.Add(hero);
            }

            return heroes;
        }
    }
}
