using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace WebCrawler.Classes
{
    internal class WebCrawlerPurina : ICrawlerInterface
    {
        private static readonly List<Dog> Breeds = new List<Dog>();
        private static readonly List<Dog> ExportBreeds = new List<Dog>();
        private readonly MainPage _page;

        public WebCrawlerPurina(MainPage page)
        {
            _page = page;
        }

        public async Task GetBreedsData()
        {
            if (Breeds.Count > 0)
                Breeds.Clear();

            const string url = "https://www.purina.com/dogs/dog-breeds";

            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);
            var clearString = string.Join("", Regex.Split(html, @"(?:\r\n|\n|\r)"));

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(clearString);

            var divs = htmlDocument.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "")
                    .Contains("asset asset-stacked asset-mosaic")).ToList();

            for (var index = 0; index < divs.Count; index++)
            {
                //Progress bar increment
                _page.Progress.Value = (index + 1) / (double) divs.Count * 100;
                await Task.Delay(10);

                var div = divs[index];

                var breed = div.InnerText.Replace("<!-- end .asset-bd -->", " ").Replace(" ", "");
                var link = string.Concat(breed.Select(x => char.IsUpper(x) ? "-" + x : x.ToString())).TrimStart(' ');
                var profileUrl = "https://www.purina.com/dogs/dog-breeds/" + link.Remove(0, 1);

                var dog = new Dog
                {
                    Breed = string.Concat(breed.Select(x => char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' '),
                    ProfileUrl = profileUrl
                };
                Breeds.Add(dog);
            }
        }

        public async Task GetUniqueBreedInfo()
        {
            for (var index = 0; index < Breeds.Count; index++)
            {
                //Dog model containing full information for each breed
                var completeDog = new Dog();

                //Progress Bar counter
                _page.Progress.Value = (index + 1) / (double) Breeds.Count * 100;
                var dog = Breeds[index];

                //basic data about breed
                completeDog.Breed = dog.Breed;
                completeDog.ProfileUrl = dog.ProfileUrl;

                try
                {
                    //Request to get html page for each dog
                    var httpClient = new HttpClient();
                    var html = await httpClient.GetStringAsync(dog.ProfileUrl);

                    var clearString = string.Join("", Regex.Split(html, @"(?:\r\n|\n|\r)"));

                    var htmlDocument = new HtmlDocument();
                    htmlDocument.LoadHtml(clearString);

                    var image = htmlDocument.DocumentNode.Descendants("img")
                        .Where(node => node.GetAttributeValue("id", "")
                            .Equals("ContentPlaceHolderDefault_BodyContent_ctl00_BreedImage")).ToList();
                    var dogImage = image.ElementAt(0).Attributes.ElementAt(4).Value;
                    completeDog.Image = "https://www.purina.com" + dogImage;

                    var generalInfo = htmlDocument.DocumentNode.Descendants("ul")
                        .Where(node => node.GetAttributeValue("class", "")
                            .Equals("stat-list")).ToList();

                    var descrition = htmlDocument.DocumentNode.Descendants("p")
                        .Where(node => node.GetAttributeValue("class", "")
                            .Equals("breed-content")).ToList();

                    //getting additional information for each dog
                    try
                    {
                        var groupString = generalInfo.ElementAt(0).ChildNodes.ElementAt(1).InnerText;
                        var group = groupString.Replace("Size", "").Replace(" ", "");
                        completeDog.BreedGroup = group + " Dogs";
                    }
                    catch (Exception e)
                    {
                        completeDog.BreedGroup = "no information";
                    }

                    try
                    {
                        var heightString = generalInfo.ElementAt(0).ChildNodes.ElementAt(3).InnerText;
                        var height = heightString.Replace("Height", "").Remove(0, 95);
                        completeDog.Height = height;
                    }
                    catch (Exception e)
                    {
                        completeDog.Height = "no information";
                    }

                    try
                    {
                        var weightString = generalInfo.ElementAt(0).ChildNodes.ElementAt(5).InnerText;
                        var weight = weightString.Replace("Weight", "").Remove(0, 95);
                        completeDog.Weight = weight;
                    }
                    catch (Exception e)
                    {
                        completeDog.Weight = "no information";
                    }

                    try
                    {
                        completeDog.LifeSpan = "no information";
                    }
                    catch (Exception e)
                    {
                        completeDog.LifeSpan = "no information";
                    }
                    try
                    {
                        var description = descrition.ElementAt(0).InnerText;
                        completeDog.Description = description;
                    }
                    catch (Exception e)
                    {
                        completeDog.Description = "no information";
                    }

                    ExportBreeds.Add(completeDog);
                }
                catch (Exception e)
                {
                    _page.ChangeTextBoxValue("Exeption with gettin a breed!");
                }
            }
        }

        public List<Dog> GetBreeds()
        {
            return Breeds;
        }

        public int GetBreedsCount()
        {
            return Breeds.Count();
        }

        public List<Dog> GetAllDogs()
        {
            return ExportBreeds;
        }
    }
}