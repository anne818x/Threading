using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler.Classes
{
    interface ICrawlerInterface
    {
        Task GetBreedsData();

        Task GetUniqueBreedInfo();

        List<Dog> GetBreeds();

        int GetBreedsCount();

        List<Dog> GetAllDogs();
    }
}
