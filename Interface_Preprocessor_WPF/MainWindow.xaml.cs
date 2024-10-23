using Microsoft.Win32;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using DataProcessor;
using System.Windows.Media.Animation;

namespace Interface_Preprocessor_WPF
{

    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new StartPage());
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            string pageTag = button.Tag.ToString();
            switch (pageTag)
            {
                case "Page1":
                    MainFrame.Navigate(new SelectPage());
                    break;
                case "Page2":
                    MainFrame.Navigate(new ViewPage());
                    break;
                case "Page3":
                    if (SharedData.Instance.MetaFile != null)
                    {
                        MainFrame.Navigate(new RuleConstructionPage());
                    }
                    else
                    {
                        CustomMessageBox customMessageBox = new CustomMessageBox("Ошибка: Для работы с данным окном требуется загрузить все необходимые файлы.");
                        customMessageBox.ShowDialog();
                        MainFrame.Navigate(new SelectPage());
                    }
                    break;
                case "Page4":
                    MainFrame.Navigate(new InfoPage());
                    break;
            }
        }

        private void MenuArea_MouseEnter(object sender, MouseEventArgs e)
        {
            Storyboard showMenu = (Storyboard)this.FindResource("ShowMenu");
            showMenu.Begin();
        }

        private void MenuBorder_MouseLeave(object sender, MouseEventArgs e)
        {
            Storyboard hideMenu = (Storyboard)this.FindResource("HideMenu");
            hideMenu.Begin();
        }
    }
}
