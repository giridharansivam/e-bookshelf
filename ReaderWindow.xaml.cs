using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
using Microsoft.VisualBasic;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;

using Amazon.S3;
using Amazon;
using Amazon.S3.Model;
using Syncfusion.Pdf.Graphics;
using System.Net.Http;
using System.Net.Sockets;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2;

namespace _301335487_Paramasivam__Lab_2
{
    /// <summary>
    /// Interaction logic for ReaderWindow.xaml
    /// </summary>
    public partial class ReaderWindow : Window
    {
        private const string awsAccessKey = "";
        private const string awsSecretKey = "";
        private const string bucketName = "booksdocument";
        private string keyName;
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.USEast1;
        private static IAmazonS3 s3Client;
        private string userID="";
        private string author="";
        private string title="";
        private string bookID = "";   

        public ReaderWindow(Book key,string username)
        {
            InitializeComponent();
            keyName =key.s3Url;
            this.userID = username;
            this.bookID = "book1"; // Add this line
            s3Client = new AmazonS3Client(awsAccessKey, awsSecretKey, bucketRegion);
            LoadPdfFromS3();

        }
        private async void LoadPdfFromS3()
        {
            try
            {
                // Prepare the request to get the PDF from S3
                GetObjectRequest request = new GetObjectRequest
                {
                    BucketName = bucketName,
                    Key = keyName
                };

                // Get the response from S3
                GetObjectResponse response = await s3Client.GetObjectAsync(request);

                // Copy the response stream to a memory stream
                MemoryStream documentStream = new MemoryStream();
                response.ResponseStream.CopyTo(documentStream);

                // Load the PDF into the PdfViewerControl
                documentStream.Position = 0; // Reset the stream position to the beginning
                pdfViewer.Load(documentStream);
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                MessageBox.Show($"Error encountered on server. Message:'{amazonS3Exception.Message}'");
            }
            catch (Exception e)
            {
                MessageBox.Show($"Unknown encountered on server. Message:'{e.Message}'");
            }
        }

        private async void BookmarkButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int currentPage = pdfViewer.CurrentPage;

                // Log the values before making the update call
                MessageBox.Show($"Username: {userID}, s3Url: {bookID}, CurrentPage: {currentPage}");

                // Validate the inputs
                if (string.IsNullOrEmpty(userID) || string.IsNullOrEmpty(keyName) || currentPage <= 0)
                {
                    MessageBox.Show("Invalid input values. Cannot save bookmark.");
                    return;
                }

                await DynamoDBHelper.UpdateBookCurrentPageAsync(userID, bookID, currentPage);
                MessageBox.Show("Bookmark saved successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving the bookmark: {ex.Message}");
            }
        }

    }

    

}
