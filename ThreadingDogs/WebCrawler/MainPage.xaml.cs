using System.Collections.Generic;
using System.IO;
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

        public static ProgressBar Progress;
    

        public MainPage()
        {
            this.InitializeComponent();
            Progress = BarProgress;
            crawler = new Classes.WebCrawler(this);

       
        }

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

        private async void GetAddData_Click(object sender, RoutedEventArgs e)
        {
            TextLog.Text += "Downloading additional information..." + "\r\n";

            var task = crawler.GetUniqueBreedInfo();
            await task;

            TextLog.Text += "Done!";
        }

        private void saveToDb_Click(object sender, RoutedEventArgs e)
        {
            DatabaseUpload.inert();
        }

        public void ChangeTextBoxValue(string message)
        {
            TextLog.Text += "\r\n" + message;
        }

      
           

        }


    }

