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
    /// Interaction logic for CreateWindow.xaml
    /// </summary>
    public partial class CreateWindow : Window
    {
        private DB db = new DB();
        private Product product = new Product();
        public CreateWindow(int productId = 0)
        {
            InitializeComponent();

            if (productId != 0)
            {
                product = db.Products.Find(productId);
                nameTextBox.Text = product.ProductName;
                stockTextBox.Text = product.Stock.ToString();
                priceTextBox.Text = product.Price.ToString();
            }
        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (new string[] { nameTextBox.Text, stockTextBox.Text, priceTextBox.Text }.Any(a => a.IsNullOrEmpty()))
                {
                    "Input error.".Show();
                }

                var stockResult = int.TryParse(stockTextBox.Text, out int stock);
                var priceResult = int.TryParse(priceTextBox.Text, out int price);

                if (!stockResult || !priceResult)
                {
                    "Please input number stock and hprice".Err();
                }

                product.ProductName = nameTextBox.Text;
                product.Price = price;
                product.Stock = stock;

                var history = new StockHistory
                {
                    Amount = stock,
                    ActionType = "編集",
                    CreatedAt = DateTime.Now,
                    Memo = ""
                };

                if (product.ProductId == 0)
                {
                    product.CreatedAt = DateTime.Now;
                    db.Products.Add(product);
                    history.ActionType = "登録";
                    history.Memo = "初期登録";
                }
                db.SaveChanges();

                history.ProductId = product.ProductId;
                
                db.StockHistories.Add(history);
                db.SaveChanges();

                "Success".Show();
                Close();
            }
            catch (Exception ex)
            {
                ex.Message.Show();
            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
