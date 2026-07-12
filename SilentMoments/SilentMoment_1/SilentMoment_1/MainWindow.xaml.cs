using Microsoft.EntityFrameworkCore;
using SilentMoment_1.Models;
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

namespace SilentMoment_1
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

        private void b1_Click(object sender, RoutedEventArgs e)
        {
            new Window1().ShowDialog();
            Refresh();
        }

        private void Refresh()
        {
            db = new DB();

            var moments = db.QuietMoments.Include(a => a.Place).ToList().Where(a => a.RecordedAt.Date == DateTime.Now.Date).ToList();
            var average = 0.0;
            if (moments.Any())
            {
                average = moments.Average(a => a.QuietLevel);
            }
            l1.Content = average;
            l2.Content = moments.Count();
            dg1.ItemsSource = moments;
        }

        private void b2_Click(object sender, RoutedEventArgs e)
        {
            new Window2().ShowDialog();
            Refresh();
        }

        private void b3_Click(object sender, RoutedEventArgs e)
        {
            new Window3().ShowDialog();
            Refresh();
        }

        private void b4_Click(object sender, RoutedEventArgs e)
        {
            new Window4().ShowDialog();
            Refresh();

        }
    }
}