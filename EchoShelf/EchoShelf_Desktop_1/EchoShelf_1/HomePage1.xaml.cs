using EchoShelf_1.Models;
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

namespace EchoShelf_1
{
    /// <summary>
    /// Interaction logic for HomePage1.xaml
    /// </summary>
    public partial class HomePage1 : Page
    {
        private DB db = new DB();
        public HomePage1()
        {
            InitializeComponent();

            Refresh();
        }

        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            new MemoryWindow().ShowDialog();

            Refresh();
        }

        private void Refresh()
        {
            db = new DB();
            dataGrid.ItemsSource = db.Memories.ToList().OrderByDescending(a => a.MemoryDate).ToList();

        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var memory = dataGrid.SelectedItem as Memory;
            if (memory == null) return;
            
            new MemoryWindow(memory.MemoryId).ShowDialog();
            Refresh();
        }
    }
}
