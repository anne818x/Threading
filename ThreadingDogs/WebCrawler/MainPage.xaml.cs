using System.Collections.Generic;
using System.IO;
using Windows.UI.ViewManagement;
using WebCrawler.Classes;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WebCrawler
{
    /// <summary>
    /// The main screen where all logs and buttons are located.
    /// </summary>
    public partial class MainPage : Page
    {
        private Classes.WebCrawler crawler;
        private Classes.DatabaseUpload dbUpload;

        public static ProgressBar Progress;

        public MainPage()
        {
            this.InitializeComponent();
            Progress = BarProgress;
            crawler = new Classes.WebCrawler(this);
            dbUpload = new Classes.DatabaseUpload(this);
        }

        /// <summary>
        /// Method gathering breed names, links to profiles and images.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void GetBreeds_Click_1(object sender, RoutedEventArgs e)
        {
            var task = crawler.GetBreedsData();
            TextLog.Text += "Start getting breeds from internet..." + "\r\n";
            await task;
            TextLog.Text += "Done!" + "\r\n" + crawler.GetBreedsCount() + " breeds found" + "\r\n";

            IEnumerable<Dog> breeds = crawler.GetBreeds();

            foreach (var dog in breeds)
            {
                DogsLog.Items.Add(dog != null ? dog.Breed : "No data was found. Please try again.");
            }
        }

        /// <summary>
        /// Method gathering additional information for each breed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void GetAddData_Click(object sender, RoutedEventArgs e)
        {
            SaveToDb.IsEnabled = false;
            TextLog.Text += "Downloading additional information..." + "\r\n";

            var task = crawler.GetUniqueBreedInfo();
            await task;

            TextLog.Text += "Done!";
            SaveToDb.IsEnabled = true;
        }

        /// <summary>
        /// Button to save breeds to DB
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void saveToDb_Click(object sender, RoutedEventArgs e)
        {
            GetAddData.IsEnabled = false;
            TextLog.Text += "\r\n" + "Saving to database";
            var taskDb = dbUpload.InsertToDb();
            await taskDb;
            GetAddData.IsEnabled = true;
        }

        /// <summary>
        /// Input information into the message box
        /// </summary>
        /// <param name="message"></param>
        public void ChangeTextBoxValue(string message)
        {
            TextLog.Text += "\r\n" + message;
        }
    }
}

