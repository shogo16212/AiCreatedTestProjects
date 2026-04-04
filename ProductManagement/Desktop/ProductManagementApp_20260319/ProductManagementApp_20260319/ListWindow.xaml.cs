using Microsoft.EntityFrameworkCore;
using ProductManagementApp_20260319.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
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
    /// Interaction logic for ListWindow.xaml
    /// </summary>
    public partial class ListWindow : Window
    {
        private DB db = new DB();
        public ListWindow()
        {
            InitializeComponent();

            Refresh();
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var data = dataGrid.SelectedItem as dynamic;
                if (data == null) "Please select product.".Err();

                new CreateWindow(data.Id).ShowDialog();
                Refresh();
            }
            catch (Exception ex)
            {
                ex.Message.Show();
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var data = dataGrid.SelectedItem as dynamic;
                if (data == null) "Please select product.".Err();

                if (MessageBox.Show("Delete this product?", "Confirm", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    db.StockHistories.Add(new StockHistory
                    {
                        ProductId = data.Id,
                        ActionType = "削除",
                        Amount = 0,
                        Memo = "削除",
                        CreatedAt = DateTime.Now,
                    });
                    db.SaveChanges();
                }
                Refresh();
            }
            catch (Exception ex)
            {
                ex.Message.Show();
            }

        }

        private void Refresh()
        {
            db = new DB();

            var products = Filter(db.Products.Include(a => a.StockHistories).ToList(), searchTextBox.Text);
            dataGrid.ItemsSource = products.Where(a => a.StockHistories.All(b => b.ActionType != "削除")).Select(a => new
            {
                Id = a.ProductId,
                a.ProductName,
                a.Price,
                a.Stock,
                a.CreatedAt,
                LastUpdateDate = a.StockHistories?.OrderByDescending(b => b.CreatedAt).FirstOrDefault()?.CreatedAt ?? a.CreatedAt
            });
        }

        private List<Product> Filter(List<Product> products, string content)
        {
            if (content != "")
            {
                return Filter(products.Where(a => a.ProductName.Contains(content)).ToList(), "");
            }

            return products;
        }
    }
}
