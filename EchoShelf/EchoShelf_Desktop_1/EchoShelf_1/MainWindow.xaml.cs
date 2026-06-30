using EchoShelf_1.Models;
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

namespace EchoShelf_1
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

            //Settings1.Default.UserId = 0;
            //Settings1.Default.Save();

            emailTextBox.Text = "admin@echoshelf.local";
            passBox.Password = "password_hash";

            Common.UserId = Settings1.Default.UserId;
            if (Common.UserId != 0)
            {
                new MenuWindow().Show();
                Close();
            }
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var user = db.Users.ToList().FirstOrDefault(a => a.Email == emailTextBox.Text && a.PasswordHash == passBox.Password);
                if (user == null) "Authentication failed.".Err();

                if (keepCheckBox.IsChecked == true)
                {
                    Settings1.Default.UserId = user.UserId;
                    Settings1.Default.Save();
                }


                Common.UserId = user.UserId;
                new MenuWindow().Show();
                Close();
            }
            catch (Exception ex)
            {
                ex.Message.Show();
            }
        }

        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            new CreateUserWindow().ShowDialog();
        }
    }
}