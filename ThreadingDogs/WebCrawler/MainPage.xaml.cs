using System;
using System.Collections;
using System.Collections.Generic;
using WebCrawler.Classes;
using Windows.System.Threading;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WebCrawler
{
    /// <summary>
    /// The main screen where all logs and buttons are located.
    /// </summary>
    public partial class MainPage : Page
    {
        private ICrawlerInterface Crawler;
        private ICrawlerInterface WebCrawlerPurina;
        private DatabaseUpload dbUpload;
        private List<Dog> uploadBreeds;

        public ProgressBar Progress;

        public MainPage()
        {
            this.InitializeComponent();
            Progress = BarProgress;
            Crawler = new Classes.WebCrawler(this);
            WebCrawlerPurina = new WebCrawlerPurina(this);
            dbUpload = new DatabaseUpload(this);
        }

        /// <summary>
        /// Method gathering breed names, links to profiles and images.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void GetBreeds_Click_1(object sender, RoutedEventArgs e)
        {
            IEnumerable<Dog> breeds;
            TextLog.Text += "Start getting breeds from internet..." + "\r\n";
            DogsLog.Items.Clear();
            if (WebsiteSelect.SelectionBoxItem.ToString() == "Dogtime.com")
            {
                var task = Crawler.GetBreedsData();
                await task;

                TextLog.Text += "Done!" + "\r\n" + Crawler.GetBreedsCount() + " breeds found" + "\r\n";

                breeds = Crawler.GetBreeds();

                PrintBreedsToConsole(breeds);

            }
            else if (WebsiteSelect.SelectionBoxItem.ToString() == "Purina.com")
            {
                var task = WebCrawlerPurina.GetBreedsData();
                await task;

                TextLog.Text += "Done!" + "\r\n" + WebCrawlerPurina.GetBreedsCount() + " breeds found" + "\r\n";

                breeds = WebCrawlerPurina.GetBreeds();

                PrintBreedsToConsole(breeds);
            }
        }

        /// <summary>
        /// Method that prints all breeds in a collection to the ListView
        /// </summary>
        /// <param name="breeds"></param>
        public void PrintBreedsToConsole(IEnumerable<Dog> breeds)
        {
            if (breeds != null)
            {
                foreach (var dog in breeds)
                {
                    DogsLog.Items.Add(dog != null ? dog.Breed : "No data was found. Please try again.");
                }
            }
            else
            {
                ChangeTextBoxValue("No data");
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

            if (WebsiteSelect.SelectionBoxItem.ToString() == "Dogtime.com")
            {
                var task = Crawler.GetUniqueBreedInfo();
                await task;
            }
            else if (WebsiteSelect.SelectionBoxItem.ToString() == "Purina.com")
            {
                var task = WebCrawlerPurina.GetUniqueBreedInfo();
                await task;
            }

            TextLog.Text += "\r\n Done!";
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
            if (WebsiteSelect.SelectionBoxItem.ToString() == "Dogtime.com")
            {
                uploadBreeds = Crawler.GetAllDogs();
                dbUpload.InsertToDb(uploadBreeds);
            }
            else if(WebsiteSelect.SelectionBoxItem.ToString() == "Purina.com")
            {
                uploadBreeds = WebCrawlerPurina.GetAllDogs();
                dbUpload.InsertToDb(uploadBreeds);
            }
            GetAddData.IsEnabled = true;
            TextLog.Text += "\r\n" + "Saving to database completed";
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

