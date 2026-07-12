using SilentMoment_1.Models;
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

namespace SilentMoment_1
{
    /// <summary>
    /// Interaction logic for Window3.xaml
    /// </summary>
    public partial class Window3 : Window
    {
        private DB db = new DB();
        public Window3()
        {
            InitializeComponent();

            Refresh();
        }

        private void b1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tb1.Text.IsNullOrEmpty()) "Input error.".Err();
                if (db.Tags.ToList().Any(a => a.TagName == tb1.Text)) "Already in input.".Err();

                db.Tags.Add(new Models.Tag { TagName =  tb1.Text, CreatedAt = DateTime.Now });
                db.SaveChanges();
                Refresh();
                "Added".Err();
            }
            catch (Exception ex)
            {
                ex.Message.Show();
            }
        }

        private void Refresh()
        {
            dg1.ItemsSource = db.Tags.ToList();

        }
    }
}
