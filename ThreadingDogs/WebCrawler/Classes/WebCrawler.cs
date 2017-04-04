using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace WebCrawler.Classes
{
    class WebCrawler
    {
        private static List<Dog> Breeds = new List<Dog>();
        private static List<Dog> ExportBreeds = new List<Dog>();
        private readonly MainPage _page;

        public WebCrawler(MainPage page)
        {
            this._page = page;
        }

        public async Task GetBreedsData()
        {
            //clear breeds list if there are any records
            if (Breeds.Count > 0)
            {
                Breeds.Clear();
            }

            const string url = "http://dogtime.com/dog-breeds/profiles";

            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var divs = htmlDocument.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "")
                    .Equals("group-list-item")).ToList();

            for (var index = 0; index < divs.Count; index++)
            {
                //Progress bar increment
                MainPage.Progress.Value = ((index + 1) / (double) divs.Count) * 100;
                await Task.Delay(10);

                var div = divs[index];
                var dog = new Dog()
                {
                    Breed = div.Descendants("h2").FirstOrDefault().InnerText,
                    ProfileUrl = div.FirstChild.Attributes.ElementAt(1).Value,
                    Image = div.FirstChild.ChildNodes.ElementAt(0).Attributes.ElementAt(1).Value
                };
                Breeds.Add(dog);
            }
        }

        public async Task GetUniqueBreedInfo()
        {
            try
            {
                for (var index = 0; index < Breeds.Count; index++)
                {
                    //Dog model containing full information for each breed
                    var completeDog = new Dog();

                    //Progress Bar counter
                    MainPage.Progress.Value = ((index + 1) / (double) Breeds.Count) * 100;
                    var dog = Breeds[index];

                    //basic data about breed
                    completeDog.Breed = dog.Breed;
                    completeDog.Image = dog.Image;
                    completeDog.ProfileUrl = dog.ProfileUrl;

                    //Request to get html page for each dog
                    var httpClient = new HttpClient();
                    var html = await httpClient.GetStringAsync(dog.ProfileUrl);

                    var htmlDocument = new HtmlDocument();
                    htmlDocument.LoadHtml(html);

                    var generalInfo = htmlDocument.DocumentNode.Descendants("div")
                        .Where(node => node.GetAttributeValue("class", "")
                            .Equals("inside-box")).ToList();

                    //getting additional information for each dog
                    try
                    {
                        completeDog.BreedGroup = generalInfo.ElementAt(1).ChildNodes.ElementAt(2).InnerText;
                    }
                    catch (Exception e)
                    {
                        completeDog.BreedGroup = "no information";
                    }

                    try
                    {
                        completeDog.Height = generalInfo.ElementAt(1).ChildNodes.ElementAt(5).InnerText;
                    }
                    catch (Exception e)
                    {
                        completeDog.Height = "no information";
                    }

                    try
                    {
                        completeDog.Weight = generalInfo.ElementAt(1).ChildNodes.ElementAt(8).InnerText;
                    }
                    catch (Exception e)
                    {
                        completeDog.Weight = "no information";
                    }

                    try
                    {
                        completeDog.LifeSpan = generalInfo.ElementAt(1).ChildNodes.ElementAt(11).InnerText;
                    }
                    catch (Exception e)
                    {
                        completeDog.LifeSpan = "no information";
                    }
                    ExportBreeds.Add(completeDog);
                }
            }
            catch (Exception e)
            {
                _page.ChangeTextBoxValue("Exeption with getting breeds!");
            }
        }


        public List<Dog> GetBreeds()
        {
            return Breeds;
        }

        public int GetBreedsCount()
        {
            return Breeds.Count;
        }

        public List<Dog> GetAllDogs()
        {
            return ExportBreeds;
        }
    }
}
