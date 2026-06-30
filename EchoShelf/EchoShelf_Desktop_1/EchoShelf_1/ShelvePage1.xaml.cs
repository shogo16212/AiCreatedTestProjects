using EchoShelf_1.Models;
using Microsoft.EntityFrameworkCore;
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
    /// Interaction logic for ShelvePage1.xaml
    /// </summary>
    public partial class ShelvePage1 : Page
    {
        private DB db = new DB();
        private Shelf selectShelf = new Shelf();    
        public ShelvePage1()
        {
            InitializeComponent();

            Refresh();
        }

        private void shelfComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var shelf = shelfComboBox.SelectedItem as Shelf;
            if (shelf == null) return;

            listBox.ItemsSource = shelf.ShelfItems.ToList();
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            new EditShelfWindow1().ShowDialog();

            Refresh();
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            var shelf = shelfComboBox.SelectedItem as Shelf;
            if (shelf == null) return;

            new EditShelfWindow1(shelf.ShelfId).ShowDialog();
            Refresh();
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Refresh()
        {
            db = new DB();
            shelfComboBox.ItemsSource = db.Shelves.Include(a => a.ShelfItems).ThenInclude(a => a.Memory).ToList().OrderBy(a => a.DisplayOrder).ToList();
            shelfComboBox.SelectedItem = db.Shelves.Include(a => a.ShelfItems).ThenInclude(a => a.Memory).ToList().OrderBy(a => a.DisplayOrder).ToList().FirstOrDefault();
        }
    }
}
