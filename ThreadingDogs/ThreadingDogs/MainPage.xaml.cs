﻿using System;
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
using Windows.UI.Xaml.Media.Imaging;
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
                DogslistCompare.Items.Add(dog.Breed);
            }
        }
        
        private void Dogslist_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            liStdog = data.dogList();
            foreach (var dog in liStdog)
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
         
            foreach (var dog in liStdog)
            {
                //if (DogslistCompare.SelectedItems.Count == 1)
                //{

                //    breed1.Text = "Breed: " + dog.Breed;
                //    imagedog1.Source = new BitmapImage(new Uri(dog.Image, UriKind.Absolute));
                //    breedgroup1.Text = "Breed Group: " + dog.BreedGroup;
                //    dogheight1.Text = "Height: " + dog.Height;
                //    dogweight1.Text = "Weight: " + dog.Weight;
                //    life1.Text = "LifeSpan: " + dog.LifeSpan;
                //}
                //else if(DogslistCompare.SelectedItems.Count == 2)
                //{
                //    Object[] itemz = DogslistCompare.SelectedItems.ToArray();

                //    Dog dog2 = (Dog)itemz[1];

                //    breed2.Text = "Breed: " + dog2.Breed;
                //    imagedog2.Source = new BitmapImage(new Uri(dog.Image, UriKind.Absolute));
                //    breedgroup2.Text = "Breed Group: " + dog.BreedGroup;
                //    dogheight2.Text = "Height: " + dog.Height;
                //    dogweight2.Text = "Weight: " + dog.Weight;
                //    life2.Text = "LifeSpan: " + dog.LifeSpan;
                //}

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
                if (selectedDogs.Length > 1) {
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
    }
}
