using EchoShelf_1.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
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
    /// Interaction logic for ProfilePage1.xaml
    /// </summary>
    public partial class ProfilePage1 : Page
    {
        private DB db = new DB();
        private User user = new User();
        public ProfilePage1()
        {
            InitializeComponent();

            user = db.Users.ToList().First(a => a.UserId == Common.UserId);

            usernameTextBox.Text = user.UserName;
            emailTextBox.Text = user.Email;
        }

        private void openButton_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog { Filter = "*.png;*.jpg|*.png;*.jpg" };
            if (ofd.ShowDialog() == true)
            {
                imageView.Source = new BitmapImage(new Uri(ofd.FileName));
                //var fileName = System.IO.Path.GetFileName(ofd.FileName);
                //var bytes = File.ReadAllBytes(ofd.FileName);
                //var rootPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
                //var fullPath = System.IO.Path.Combine(rootPath, fileName);

                //if (File.Exists(fullPath))
                //{

                //}
            }

        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (usernameTextBox.Text.IsNullOrEmpty() || emailTextBox.Text.IsNullOrEmpty()) "Input error.".Err();


                user.UserName = usernameTextBox.Text;
                user.Email = emailTextBox.Text;

                db.SaveChanges();

                "Saved".Show();
            }
            catch (Exception ex)
            {
                ex.Message.Show();
            }
        }

        private void logoutButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Do you logout?", "Confirm", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                //new MainWindow().Show();
                //var windows = App.Current.Windows;
                //foreach (var window in windows)
                //{
                //    if (window.GetType() != typeof(MainWindow))
                //    {
                //        (window as Window).Close();
                //    }
                //}
            }
        }
    }
}
