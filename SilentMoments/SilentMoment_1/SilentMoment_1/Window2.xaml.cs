using Microsoft.EntityFrameworkCore;
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
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        private DB db = new DB();
        public Window2()
        {
            InitializeComponent();

            var places = db.Places.ToList();
            places.Insert(0,new Place { PlaceId = 0, PlaceName = "All" });
            c2.ItemsSource = places;
            c2.SelectedItem = places.FirstOrDefault();
        }

        private void c2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh();
        }

        private void d1_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh();

        }

        private void d2_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh();

        }

        private List<QuietMoment> Filter(List<QuietMoment> moments, int placeId, DateTime? from, DateTime? to)
        {
            if (placeId != 0)
            {
                return Filter(moments.Where(a => a.PlaceId == placeId).ToList(), 0, from, to);
            }
            if (from!= null)
            {
                return Filter(moments.Where(a => a.RecordedAt.Date >= from.Value.Date).ToList(), 0, null, to);
            }
            if (to != null)
            {
                return Filter(moments.Where(a => a.RecordedAt.Date <= to.Value.Date).ToList(), 0, null, null);
            }
            return moments;
        }

        private void Refresh()
        {
            try
            {
                var place = c2.SelectedItem as Place;
                if (place == null)
                {
                    "Plase select all parameter.".Err();
                }

                var from = d1.SelectedDate;
                var to = d2.SelectedDate;

                if (from != null && to != null && from >= to)
                {
                    "Please select from less than and equal to.".Err();
                }

                var moments = Filter(db.QuietMoments.Include(a => a.Place).ToList(), place.PlaceId, d1.SelectedDate, d2.SelectedDate);
                dg1.ItemsSource = moments;
            }
            catch (Exception ex)
            {
                ex.Message.Show();
            }
        }
    }
}
