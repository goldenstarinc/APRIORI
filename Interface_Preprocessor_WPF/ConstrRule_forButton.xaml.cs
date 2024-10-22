//using DataProcessor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Interface_Preprocessor_WPF
{
    /// <summary>
    /// Логика взаимодействия для ConstrRule_forButton.xaml
    /// </summary>
    public partial class ConstrRule_forButton : Window
    {
        private string[] parameters;
        //public ConstrRule_forButton(ExcelFile File)
        //{
        //    InitializeComponent();

        //    parameters = new string[File.PropertyNames.Count];
        //    for (int i = 0; i < File.PropertyNames.Count; i++)
        //    {
        //        parameters[i] = File.PropertyNames[i];
        //    }
        //    CreateParameterTiles();
        //}

        private List<Border> selectedTiles = new List<Border>();  // Хранит выбранные плитки

        private void CreateParameterTiles()
        {
            if (parameters == null || parameters.Length == 0)
            {
                MessageBox.Show("Параметры не заданы или пусты.");
                return;
            }

            // Очищаем все элементы внутри WrapPanel на случай, если они уже есть
            parametersPanel.Children.Clear();

            // Добавляем плитки для каждого параметра
            foreach (string param in parameters)
            {
                // Создаём Border как плитку
                Border tile = new Border
                {
                    Width = 150,      // Задаём ширину плитки
                    Height = 50,     // Задаём высоту плитки
                    Margin = new Thickness(10),  // Отступы между плитками
                    CornerRadius = new CornerRadius(10),  // Скруглённые углы плитки
                    BorderBrush = new SolidColorBrush(Colors.AliceBlue), // Цвет рамки плитки
                    BorderThickness = new Thickness(2),  // Толщина рамки
                    Background = new SolidColorBrush(Colors.LightCyan) // Цвет фона
                };

                // Внутри плитки размещаем TextBlock с параметром
                TextBlock textBlock = new TextBlock
                {
                    Text = param,  // Текст параметра
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    FontSize = 16,
                    FontWeight = FontWeights.Bold,
                    TextWrapping = TextWrapping.Wrap,
                    TextAlignment = TextAlignment.Center
                };

                // Добавляем TextBlock внутрь Border
                tile.Child = textBlock;

                // Добавляем обработчик нажатия на плитку
                tile.MouseLeftButtonDown += (s, e) => OnTileClick(tile);

                // Добавляем плитку в WrapPanel
                parametersPanel.Children.Add(tile);
            }
        }

        private void OnTileClick(Border tile)
        {
            // Если плитка уже выбрана, снимаем выбор
            if (selectedTiles.Contains(tile))
            {
                selectedTiles.Remove(tile);
                tile.Background = new SolidColorBrush(Colors.LightCyan);  // Устанавливаем цвет по умолчанию
            }
            else
            {
                // Выбираем плитку
                selectedTiles.Add(tile);
                tile.Background = new SolidColorBrush(Colors.LightGreen);  // Подсвечиваем выбранную плитку
            }
        }

        private void Do_Button_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
