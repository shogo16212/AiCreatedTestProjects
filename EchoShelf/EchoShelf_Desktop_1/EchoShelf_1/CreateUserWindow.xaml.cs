using EchoShelf_1.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace EchoShelf_1
{
    /// <summary>
    /// Interaction logic for CreateUserWindow.xaml
    /// </summary>
    public partial class CreateUserWindow : Window
    {
        private DB db = new DB();
        private User user = new User();
        public CreateUserWindow()
        {
            InitializeComponent();
            if (Common.UserId == 0)
            {
                logoutButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                user = db.Users.ToList().First(a => a.UserId == Common.UserId);
            }
        }

        private void logoutButton_Click(object sender, RoutedEventArgs e)
        {
            Settings1.Default.UserId = 0;
            Settings1.Default.Save();
            new MainWindow().Show();

        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (usernameTextBox.Text.IsNullOrEmpty() || emailTextBox.Text.IsNullOrEmpty() || passBox.Password.IsNullOrEmpty())
                {
                    "Input error.".Err();
                }

                user.UserName = usernameTextBox.Text;
                user.Email = emailTextBox.Text;
                user.PasswordHash = passBox.Password;
                user.AvatarImagePath = null;
                user.IsDeleted = false;

                if (user.UserId == 0)
                {
                    user.CreatedAt = DateTime.Now;
                    user.UpdatedAt = DateTime.Now;
                    db.Users.Add(user);
                }
                else
                {
                    user.UpdatedAt = DateTime.Now;
                }

                db.SaveChanges();
                "Success".Show();
                Close();
            }
            catch (Exception ex)
            {
                ex.Message.Show();
            }
        }

        private void openButton_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog { Filter = "*.png;*.jpg|*.png;*.jpg" };
            if (ofd.ShowDialog() == true)
            {
                iconImageView.Source = new BitmapImage(new Uri(ofd.FileName));
                //var fileName = System.IO.Path.GetFileName(ofd.FileName);
                //var bytes = File.ReadAllBytes(ofd.FileName);
                //var rootPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
                //var fullPath = System.IO.Path.Combine(rootPath, fileName);

                //if (File.Exists(fullPath))
                //{

                //}
            }
        }
    }
}
