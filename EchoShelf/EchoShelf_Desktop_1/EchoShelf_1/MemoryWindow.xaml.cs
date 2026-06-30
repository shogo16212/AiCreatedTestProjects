using EchoShelf_1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Microsoft.Win32;
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

namespace EchoShelf_1
{
    /// <summary>
    /// Interaction logic for MemoryWindow.xaml
    /// </summary>
    public partial class MemoryWindow : Window
    {
        private DB db = new DB();
        private Memory memory = new Memory();
        private ObservableCollection<Tag> tags = new ObservableCollection<Tag>();
        public MemoryWindow(int memoryId = 0)
        {
            InitializeComponent();

            categoryComboBox.ItemsSource = db.Categories.ToList();
            categoryComboBox.SelectedItem = db.Categories.ToList().FirstOrDefault();

            tagsComboBox.ItemsSource = db.Tags.ToList();
            tagsComboBox.SelectedItem = db.Tags.ToList().FirstOrDefault();

            tagListBox.ItemsSource = tags;

            if (memoryId != 0)
            {
                memory = db.Memories.Include(a => a.MemoryTags).ThenInclude(a => a.Tag).ToList().First(a => a.MemoryId == memoryId);

                titleTextBox.Text = memory.Title;
                categoryComboBox.SelectedItem = db.Categories.ToList().FirstOrDefault(a => a.CategoryId == memory.CategoryId);
                memoryDatePicker.SelectedDate = DateTime.Parse(memory.MemoryDate.ToString("yyyy-MM-dd"));
                memory.MemoryTags.ToList().ForEach(a => tags.Add(a.Tag));
                episodeTextBox.Text = memory.Episode;
                favoriteCheckBox.IsChecked = memory.IsFavorite;

                addShelveButton.Visibility = Visibility.Visible;
                removeButton.Visibility = Visibility.Visible;
            }
        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var category = categoryComboBox.SelectedItem as Category;
                if (titleTextBox.Text.IsNullOrEmpty() || category == null || memoryDatePicker.SelectedDate == null || episodeTextBox.Text.IsNullOrEmpty() || !tags.Any())
                {
                    "Input error.".Err();
                }

                if (titleTextBox.Text.Length > 100 || episodeTextBox.Text.Length > 1000) "Cannot be input title length more than 100 and \r\n episode length more than 10000.".Err();

                var date = memoryDatePicker.SelectedDate.Value;

                if (date.Date >= DateTime.Now.Date) "Cannot be select date after today.".Err();



                memory.UserId = Common.UserId;
                memory.CategoryId = category.CategoryId;
                memory.Title = titleTextBox.Text;
                memory.Episode = episodeTextBox.Text;
                memory.MemoryDate = DateOnly.FromDateTime(date);
                memory.IsFavorite = favoriteCheckBox.IsChecked.Value;
                memory.CreatedAt = DateTime.Now;
                memory.UpdatedAt = DateTime.Now;

                if (memory.MemoryId == 0)
                {
                    db.Memories.Add(memory);
                }
                db.SaveChanges();

                db.MemoryTags.RemoveRange(memory.MemoryTags);
                db.SaveChanges();

                tags.ToList().ForEach(a => db.MemoryTags.Add(new MemoryTag { MemoryId = memory.MemoryId, TagId = a.TagId, CreatedAt = DateTime.Now }));
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

        private void openButton_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog { Filter = "*.png;*.jpg|*.png;*.jpg" };
            if (ofd.ShowDialog() == true)
            {
                iconImageView.Source = new BitmapImage(new Uri(ofd.FileName));
            }
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var tag = tagsComboBox.SelectedItem as Tag;
                if (tag == null) return;

                if (tags.Any(a => a.TagId == tag.TagId))
                {
                    "Already in used.".Err();
                }

                tags.Add(tag);
            }
            catch (Exception ex)
            {
                ex.Message.Show();
            }
        }

        private void removeButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Remove this memory?", "Confirm", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                db.MemoryTags.RemoveRange(memory.MemoryTags);
                db.SaveChanges();
                db.Memories.Remove(memory);
                db.SaveChanges();
                "Removed".Show();
                Close();
            }
        }

        private void addButton_Click_1(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Add this memory to shelf?", "Confirm", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                db.ShelfItems.Add(new ShelfItem() { ShelfId = 1, MemoryId = memory.MemoryId, DisplayOrder = db.ShelfItems.ToList().Where(a => a.ShelfId == 1).Count() + 1, CreatedAt = DateTime.Now });
                db.SaveChanges();
                "Success".Show();
                Close();
            }

        }
    }
}
