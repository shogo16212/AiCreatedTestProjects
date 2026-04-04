using Microsoft.EntityFrameworkCore;
using ProductManagementApp_20260319.Models;
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

namespace ProductManagementApp_20260319
{
    /// <summary>
    /// Interaction logic for HistoryWindow.xaml
    /// </summary>
    public partial class HistoryWindow : Window
    {
        private DB db = new DB();
        public HistoryWindow()
        {
            InitializeComponent();

            Refresh();
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void Refresh()
        {
            dataGrid.ItemsSource = db.StockHistories.Include(a => a.Product).ToList().OrderByDescending(a => a.CreatedAt).ToList();
        }
    }
}
