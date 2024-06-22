using Amazon.S3.Model;
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

namespace _301335487_Paramasivam__Lab_2
{
    /// <summary>
    /// Interaction logic for Bookshelf.xaml
    /// </summary>
    public partial class Bookshelf : Window
    {
        List<Book> books;
        string username;
        string bookID;
        public Bookshelf(string username)
        {
            InitializeComponent();
            this.username = username;
            LoadBooksForUser(username);
        }

        private void BooksListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void BooksListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (BooksListBox.SelectedItem != null)
            {
                var selectedItem = BooksListBox.SelectedItem as Book;

                var pdfViewerWindow = new ReaderWindow(selectedItem,username);
                pdfViewerWindow.Show();
            }
            
            else
            {
                MessageBox.Show("Please select a PDF.");
            }
        }
        private async void LoadBooksForUser(string username)
        {
            books = await DynamoDBHelper.GetBooksForUserAsync(username);
            BooksListBox.ItemsSource = books;

        }
    }
}
