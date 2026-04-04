using ProductManagementApp_20260319.Models;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProductManagementApp_20260319
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DB db = new DB();
        public MainWindow()
        {
            InitializeComponent();

            Refresh();
        }

        private void listButton_Click(object sender, RoutedEventArgs e)
        {
            new ListWindow().ShowDialog();
            Refresh();
        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            new CreateWindow().ShowDialog();
            Refresh();

        }

        private void historyButton_Click(object sender, RoutedEventArgs e)
        {
            new HistoryWindow().ShowDialog();
            Refresh();

        }

        private void Refresh()
        {
            db = new DB();

            amountTextBlock.Text = $"Submit Products amount:{db.Products.ToList().Count()}";
            stockTextBlock.Text = $"Stock:{db.Products.ToList().Sum(a => a.Stock)}";

            dataGrid.ItemsSource = db.StockHistories.OrderByDescending(a => a.CreatedAt).ToList();
        }
    }
}