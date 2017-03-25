using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.AppService;
using HtmlAgilityPack;

namespace WebCrawler.Classes
{
    class WebCrawler
    {

        public static List<Dog> Breeds = new List<Dog>();

        public async Task GetBreedsData()
        {
            //clear breeds list if there are any records
            if (Breeds.Count > 0)
            {
                Breeds.Clear();
            }

            var url = "http://dogtime.com/dog-breeds/profiles";

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
            for (var index = 0; index < Breeds.Count; index++)
            {
                MainPage.Progress.Value = ((index + 1) / (double) Breeds.Count) * 100;

                var dog = Breeds[index];
                var profileUrl = dog.ProfileUrl;

                var httpClient = new HttpClient();
                var html = await httpClient.GetStringAsync(profileUrl);

                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(html);

                var generalInfo = htmlDocument.DocumentNode.Descendants("div")
                    .Where(node => node.GetAttributeValue("class", "")
                        .Equals("inside-box")).ToList();

                string breedGroup;
                string height;
                string weight;
                string lifeSpan; 

                try
                {
                     breedGroup = generalInfo.ElementAt(1).ChildNodes.ElementAt(2).InnerText;
                }
                catch (Exception e)
                {
                    breedGroup = "none";
                }

                try
                {
                    height = generalInfo.ElementAt(1).ChildNodes.ElementAt(5).InnerText;
                }
                catch (Exception e)
                {
                    height = "none";
                }

                try
                {
                    weight = generalInfo.ElementAt(1).ChildNodes.ElementAt(8).InnerText;
                }
                catch (Exception e)
                {
                    weight = "none";
                }

                try
                {
                    lifeSpan = generalInfo.ElementAt(1).ChildNodes.ElementAt(11).InnerText;
                }
                catch (Exception e)
                {
                    lifeSpan = "none";
                }
            }
        }



        public List<Dog> GetBreeds()
        {
            return Breeds;
        }
    }
}
