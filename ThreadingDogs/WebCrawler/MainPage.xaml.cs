using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading;
using System.Threading.Tasks;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WebCrawler
{
    /// <summary>
    /// The main screen where all logs and buttons are located.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Classes.WebCrawler crawler = new Classes.WebCrawler();

        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void getBreeds_Click_1(object sender, RoutedEventArgs e)
        {
            var task = crawler.GetBreedsData();
            await task;

            await crawler.GetUniqueBreedInfo();

            var breeds = crawler.GetBreeds();

            foreach (var dog in breeds)
            {
                DogsLog.Items.Add(DogsLog.Items != null ? dog.Breed : "No data was found. Please try again.");
            }
        }

    }
}
