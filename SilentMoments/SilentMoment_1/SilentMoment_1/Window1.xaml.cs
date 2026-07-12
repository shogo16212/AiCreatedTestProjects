using Microsoft.Win32;
using SilentMoment_1.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private DB db = new DB();
        private byte[]? bytes = null;
        private ObservableCollection<Tag> tags = new ObservableCollection<Tag>();
        public Window1()
        {
            InitializeComponent();

            c2.ItemsSource = db.Places.ToList();
            c2.SelectedItem = db.Places.ToList().FirstOrDefault();
            c3.ItemsSource = db.Tags.ToList();
            c3.SelectedItem = db.Tags.ToList().FirstOrDefault();


            var numbers = Enumerable.Range(1, 10).ToList().Select(a =>a .ToString()).ToList();
            c1.ItemsSource = numbers;
            c1.SelectedItem = numbers.FirstOrDefault();

            lb1.ItemsSource = tags;
        }

        private void b1_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog { Filter = "*.png;*.jpg|*.png;*.jpg"};
            if (ofd.ShowDialog() == true)
            {
                
            }
        }

        private void b3_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void b4_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var level = c1.SelectedItem as string;
                var place = c2.SelectedItem as Place;
                if (level == null || place == null || tb1.Text.IsNullOrEmpty() || tb2.Text.IsNullOrEmpty()) "Input error.".Err();

                db.QuietMoments.Add(new QuietMoment { Title = tb1.Text, QuietLevel = int.Parse(level), Memo = tb2.Text, PhotoUrl = null, PlaceId = place.PlaceId, RecordedAt = DateTime.Now });
                db.SaveChanges();
                "Submited.".Show();
                Close();
            }
            catch (Exception ex)
            {
                ex.Message.Show();
            }
        }

        private void b2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var tag = db.Tags.FirstOrDefault();
                if (tag == null) return;

                if (tags.Any(a => a.TagId == tag.TagId))
                {
                    "Already in selected this tag.".Err();
                }

                tags.Add(tag);

            }
            catch (Exception ex)
            {
                ex.Message.Show();
            }
        }
    }
}
