using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using ThreadingDogs.Classes;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Printing;
using Windows.Graphics.Printing.OptionDetails;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Printing;

namespace ThreadingDogs
{
    /// <summary>
    /// Page where you can print, select the listview 
    /// </summary>
    /// 

    public sealed partial class MainPage : Page
    {
        PrintManager printmgr = PrintManager.GetForCurrentView();
        PrintDocument printDoc = null;
        PrintTask task = null;
        //get the data from the database file
        databaseretrieve data = new databaseretrieve();
        List<Dog> liStdog;

        public MainPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Onloading of the page, it loads all of the dog breed names into the listview
        /// </summary>
        /// 
        private void Page_Load(object sender, RoutedEventArgs e)
        {
            ListofBreed();
        }

        /// <summary>
        /// generates list of breed names for comparing listview and selecting one dog.
        /// </summary>
        /// 
        public void ListofBreed()
        {
            liStdog = data.dogList();
            foreach (Dog dog in liStdog.AsParallel())
            {
                Dogslist.Items.Add(dog.Breed);
                DogslistCompare.Items.Add(dog.Breed);
            }
        }

        /// <summary>
        /// Displays list of breed names, user selects a dog, dog info is displayed
        /// with PLINQ
        /// </summary>
        /// 
        private void Dogslist_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            liStdog = data.dogList();
            foreach (var dog in liStdog.AsParallel())
            {
                if (Dogslist.SelectedItem.ToString() == dog.Breed)
                {
                    breed.Text = "Breed: " + dog.Breed;
                    imagedog.Source = new BitmapImage(new Uri(dog.Image, UriKind.Absolute));
                    breedgroup.Text = "Breed Group: " + dog.BreedGroup;
                    dogheight.Text = "Height: " + dog.Height;
                    dogweight.Text = "Weight: " + dog.Weight;
                    life.Text = "LifeSpan: " + dog.LifeSpan;
                }
            }
        }

        /// <summary>
        /// Listview only allows two clicks and displays the data on either side of the listview
        /// With PLINQ 
        /// </summary>
        /// 
        private void DogslistCompare_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            liStdog = data.dogList();
            if (this.DogslistCompare.SelectedItems.Count > 2)
            {
                this.DogslistCompare.SelectedItems.RemoveAt(0);
            }
            foreach (var dog in liStdog.AsParallel())
            {
                Object[] selectedDogs = DogslistCompare.SelectedItems.ToArray();

                if (selectedDogs.Length > 0)
                {
                    if (selectedDogs[0].ToString() == dog.Breed)
                    {
                        breed1.Text = "Breed: " + dog.Breed;
                        imagedog1.Source = new BitmapImage(new Uri(dog.Image, UriKind.Absolute));
                        breedgroup1.Text = "Breed Group: " + dog.BreedGroup;
                        dogheight1.Text = "Height: " + dog.Height;
                        dogweight1.Text = "Weight: " + dog.Weight;
                        life1.Text = "LifeSpan: " + dog.LifeSpan;
                    }
                }
                if (selectedDogs.Length > 1)
                {
                    if (selectedDogs[1].ToString() == dog.Breed)
                    {
                        breed2.Text = "Breed: " + dog.Breed;
                        imagedog2.Source = new BitmapImage(new Uri(dog.Image, UriKind.Absolute));
                        breedgroup2.Text = "Breed Group: " + dog.BreedGroup;
                        dogheight2.Text = "Height: " + dog.Height;
                        dogweight2.Text = "Weight: " + dog.Weight;
                        life2.Text = "LifeSpan: " + dog.LifeSpan;
                    }
                }
            }
        }
        
        /// <summary>
        /// Requesting the windows UI to be able to open the print function
        /// </summary>
        /// 
        private async void OnPrintTaskSourceRequested(PrintTaskSourceRequestedArgs args)
        {
            var def = args.GetDeferral();
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
              () =>
              {
                  args.SetSource(printDoc?.DocumentSource);
              });
            def.Complete();
        }

        /// <summary>
        /// register the document to be printed, to the printer
        /// and open printer dialogue
        /// </summary>
        /// 
        private void appbar_Printer_Click(object sender, RoutedEventArgs e)
        {
            registerPrint();
        }

        /// <summary>
        /// register the page that will be printed to the printer
        /// and open printer dialogue
        /// </summary>
        /// 
        private async void registerPrint()
        {
            this.printDoc = new PrintDocument();
            printDoc.GetPreviewPage += OnGetPreviewPage;
            printDoc.Paginate += PrintDoc_Paginate;
            printDoc.AddPages += PrintDoc_AddPages;
            printmgr.PrintTaskRequested += Printmgr_PrintTaskRequested;
            bool showPrint = await PrintManager.ShowPrintUIAsync();
        }
      
        /// <summary>
        /// creates page, of the current page that the application is on
        /// </summary>
        /// 
        private void PrintDoc_AddPages(object sender, AddPagesEventArgs e)
        {
            printDoc.AddPage(this);
            printDoc.AddPagesComplete();
        }

        /// <summary>
        /// retrieves print options and to set the page count to 1
        /// </summary>
        /// 
        private void PrintDoc_Paginate(object sender, PaginateEventArgs e)
        {
            printDoc.SetPreviewPageCount(1, PreviewPageCountType.Final);
        }

        /// <summary>
        /// Requests to do a print task
        /// </summary>
        /// 
        private void Printmgr_PrintTaskRequested(PrintManager sender, PrintTaskRequestedEventArgs args)
        {
            
            task = args.Request.CreatePrintTask("Print", OnPrintTaskSourceRequested);
        }

        /// <summary>
        /// gets the preview of grid view, so its possible to see the what will be printed
        /// </summary>
        /// 
        private void OnGetPreviewPage(object sender, GetPreviewPageEventArgs e)
        {
           printDoc.SetPreviewPage(e.PageNumber, Area);
        }
        /// <summary>
        /// These methods are toggles to make the listview collapse
        /// </summary>
        /// 
        private void selectDogComBtn_Click(object sender, RoutedEventArgs e)
        {
            toggleListviewView(DogslistCompare);
        }

        private void selectDogBtn_Click(object sender, RoutedEventArgs e)
        {
            toggleListviewView(Dogslist);
        }

        private void toggleListviewView(ListView lw)
        {
            if (lw.Visibility == Visibility.Collapsed)
            {
                lw.Visibility = Visibility.Visible;
            }
            else if (lw.Visibility == Visibility.Visible)
            {
                lw.Visibility = Visibility.Collapsed;
            }

        }
    }
}
