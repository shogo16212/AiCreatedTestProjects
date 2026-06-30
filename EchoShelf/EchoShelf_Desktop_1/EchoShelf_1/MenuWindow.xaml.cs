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

namespace EchoShelf_1
{
    /// <summary>
    /// Interaction logic for MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        public MenuWindow()
        {
            InitializeComponent();

            frame.Content = new HomePage1();
        }

        private void homeButton_Click(object sender, RoutedEventArgs e)
        {
            frame.Content = new HomePage1();

        }

        private void shelveButton_Click(object sender, RoutedEventArgs e)
        {
            frame.Content = new ShelvePage1();

        }

        private void analysisButton_Click(object sender, RoutedEventArgs e)
        {
            frame.Content = new AnalysisPage1();

        }

        private void profileButton_Click(object sender, RoutedEventArgs e)
        {
            frame.Content = new ProfilePage1();

        }
    }
}
