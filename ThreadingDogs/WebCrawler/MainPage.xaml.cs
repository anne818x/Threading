using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WebCrawler
{
    /// <summary>
    /// The main screen where all logs and buttons are located.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Classes.WebCrawler crawler = new Classes.WebCrawler();

        public static ProgressBar Progress;

        public MainPage()
        {
            this.InitializeComponent();
            Progress = BarProgress;
        }

        private async void GetBreeds_Click_1(object sender, RoutedEventArgs e)
        {
            var task = crawler.GetBreedsData();
            await task;

            var task2 = crawler.GetUniqueBreedInfo();
            await task2;

            IEnumerable<Classes.Dog> breeds = crawler.GetBreeds();

            foreach (var dog in breeds)
            {
                DogsLog.Items.Add(DogsLog.Items != null ? dog.Breed : "No data was found. Please try again.");
            }
        }

    }
}
