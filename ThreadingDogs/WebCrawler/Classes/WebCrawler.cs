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
            if (Breeds.Count>0)
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

            foreach (var div in divs)
            {
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
            foreach (var dog in Breeds)
            {
                var dogProfile = dog.ProfileUrl;

                var httpClient = new HttpClient();
                var html = await httpClient.GetStringAsync(dogProfile);

                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(html);

                var generalInfo= htmlDocument.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("inside-box")).ToList();

                var breedGroup = generalInfo.ElementAt(1).ChildNodes.ElementAt(2).InnerText;
                var height = generalInfo.ElementAt(1).ChildNodes.ElementAt(5).InnerText;
                var weight = generalInfo.ElementAt(1).ChildNodes.ElementAt(8).InnerText;
                var lifeSpan = generalInfo.ElementAt(1).ChildNodes.ElementAt(11).InnerText;
            }
        }

        public List<Dog> GetBreeds()
        {
            return Breeds;
        }
    }
}
