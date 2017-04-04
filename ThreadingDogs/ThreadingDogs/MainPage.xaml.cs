using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using ThreadingDogs.Classes;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ThreadingDogs
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        databaseretrieve data = new databaseretrieve();
        List<Dog> liStdog;

        public MainPage()
        {
            this.InitializeComponent();
            
        }

        private void Page_Load(object sender, RoutedEventArgs e)
        {
            ListofBreed();
        }

        public void ListofBreed()
        {
            liStdog = data.dogList();
            foreach (Dog dog in liStdog)
            {
                Dogslist.Items.Add(dog.Breed);
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void button_Click_1(object sender, RoutedEventArgs e)
        {
            
            
        }
        private void Dogslist_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            liStdog = data.dogList();
            foreach (var dog in liStdog)
            {
                if (Dogslist.SelectedItem.ToString() == dog.Breed)
                {
                    breed.Text = "Breed: " + dog.Breed;
                    breedgroup.Text = "Breed Group: " + dog.BreedGroup;
                    dogheight.Text = "Height: " + dog.Height;
                    dogweight.Text = "Weight: " + dog.Weight;
                    life.Text = "LifeSpan: " + dog.LifeSpan;
                }
            }

        }
    }
}
