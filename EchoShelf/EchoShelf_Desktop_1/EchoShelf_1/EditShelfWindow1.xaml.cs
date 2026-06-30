using EchoShelf_1.Models;
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
using System.Windows.Shapes;

namespace EchoShelf_1
{
    /// <summary>
    /// Interaction logic for EditShelfWindow1.xaml
    /// </summary>
    public partial class EditShelfWindow1 : Window
    {
        private DB db = new DB();
        private Shelf shelf = new Shelf();
        public EditShelfWindow1(int shelfId = 0)
        {
            InitializeComponent();

            if (shelfId != 0)
            {
                shelf = db.Shelves.ToList().First(a => a.ShelfId == shelfId);

                shelfTextBox.Text = shelf.ShelfName;
                descriptionTextBox.Text = shelf.Description;
                orderTextBox.Text = shelf.DisplayOrder.ToString();
            }
        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (shelfTextBox.Text.IsNullOrEmpty() || descriptionTextBox.Text.IsNullOrEmpty()|| orderTextBox.Text.IsNullOrEmpty())
                {
                    "Input error.".Err();
                }

                if (!int.TryParse(orderTextBox.Text, out var order))
                {
                    "Must be input number to Order".Err();
                }

                if (shelf.ShelfId != 0)
                {

                }
                if (db.Shelves.ToList().Any(a => (shelf.ShelfId == 0 ? true : a.ShelfId != shelf.ShelfId) && a.DisplayOrder == order))
                {
                    "Already in used this order number.".Err();
                }

                shelf.UserId = Common.UserId;
                shelf.ShelfName = shelfTextBox.Text;
                shelf.Description = descriptionTextBox.Text;
                shelf.DisplayOrder = order;
                shelf.UpdatedAt = DateTime.Now;
                shelf.IsDeleted = false;

                if (shelf.ShelfId == 0)
                {
                    shelf.CreatedAt = DateTime.Now;
                    db.Shelves.Add(shelf);
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

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
