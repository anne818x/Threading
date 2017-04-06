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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ThreadingDogs
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 

    public sealed partial class MainPage : Page
    {
        PrintManager printmgr = PrintManager.GetForCurrentView();
        PrintDocument printDoc = null;
        PrintTask task = null;
        databaseretrieve data = new databaseretrieve();
        List<Dog> liStdog;

        public MainPage()
        {
            this.InitializeComponent();
            printmgr.PrintTaskRequested += Printmgr_PrintTaskRequested;
        }
        private void Page_Load(object sender, RoutedEventArgs e)
        {
            ListofBreed();
        }

        private void Printmgr_PrintTaskRequested(PrintManager sender, PrintTaskRequestedEventArgs args)
        {
            var deferral = args.Request.GetDeferral();
            task = args.Request.CreatePrintTask("Print", OnPrintTaskSourceRequrested);
            //task.Completed += PrintTask_Completed;
            PrintTaskOptionDetails printDetailedOptions = PrintTaskOptionDetails.GetFromPrintTaskOptions(task.Options);
            deferral.Complete();
        }


        public void ListofBreed()
        {
            liStdog = data.dogList();
            foreach (Dog dog in liStdog.AsParallel())
            {
                Dogslist.Items.Add(dog.Breed);
                DogslistCompare.Items.Add(dog.Breed);
            }
        }

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
        private async void OnPrintTaskSourceRequrested(PrintTaskSourceRequestedArgs args)
        {
            var def = args.GetDeferral();
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
              () =>
              {
                  args.SetSource(printDoc?.DocumentSource);
              });
            def.Complete();
        }
        private void appbar_Printer_Click(object sender, RoutedEventArgs e)
        {
            registerPrint();
        }

        private async void registerPrint()
        {
            if (printDoc != null)
            {
                printDoc.GetPreviewPage -= OnGetPreviewPage;
                printDoc.Paginate -= PrintDic_Paginate;
                printDoc.AddPages -= PrintDic_AddPages;
            }
            this.printDoc = new PrintDocument();
            printDoc.GetPreviewPage += OnGetPreviewPage;
            printDoc.Paginate += PrintDic_Paginate;
            printDoc.AddPages += PrintDic_AddPages;
            bool showPrint = await PrintManager.ShowPrintUIAsync();
        }

        private void PrintDic_AddPages(object sender, AddPagesEventArgs e)
        {
            printDoc.AddPage(this);
            printDoc.AddPagesComplete();
        }
        private void PrintDic_Paginate(object sender, PaginateEventArgs e)
        {
            PrintTaskOptions opt = task.Options;
            PrintTaskOptionDetails printDetailedOptions = PrintTaskOptionDetails.GetFromPrintTaskOptions(e.PrintTaskOptions);
            printDoc.SetPreviewPageCount(1, PreviewPageCountType.Final);
        }
        private void OnGetPreviewPage(object sender, GetPreviewPageEventArgs e)
        {
            printDoc.SetPreviewPage(e.PageNumber, Area);
        }

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
