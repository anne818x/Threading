using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Foundation;
using MySql.Data.MySqlClient;

//using Microsoft.EntityFrameworkCore;


namespace WebCrawler.Classes
{
    class DatabaseUpload
    {
        private static MainPage _page;
        private ICrawlerInterface crawler;

        public DatabaseUpload(MainPage page)
        {
            _page = page;
            crawler = new WebCrawlerPurina(null);
        }

        public void InsertToDb(List<Dog> exportBreeds)
        {
            try
            {
                using (
                    MySqlConnection connection =
                        new MySqlConnection("Server=127.0.0.1;Database=dogthreading;Uid=root;Pwd=1234;SslMode=None;"))
                {
                    connection.Open();

                    //delete old content in the database
                    MySqlCommand truncateCommand = connection.CreateCommand();
                    truncateCommand.CommandText = "TRUNCATE TABLE dog";
                    truncateCommand.ExecuteNonQuery();

                    //save data to database
                    for (var index = 0; index < exportBreeds.Count; index++)
                    {
                        var breed = exportBreeds[index];
                        //_page.Progress.Value = ((index + 1) / (double) exportBreeds.Count) * 100;

                        MySqlCommand insertCommand = connection.CreateCommand();
                        insertCommand.CommandText =
                            "INSERT INTO dog( breed, dog_image, breed_group, dog_height, dog_weight, lifespan, link_dog, description)VALUES( @breed, @dog_image, @breed_group, @dog_height, @dog_weight, @lifespan, @link_dog, @description)";

                        insertCommand.Parameters.AddWithValue("@breed", breed.Breed);
                        insertCommand.Parameters.AddWithValue("@dog_image", breed.Image);
                        insertCommand.Parameters.AddWithValue("@breed_group", breed.BreedGroup);
                        insertCommand.Parameters.AddWithValue("@dog_height", breed.Height);
                        insertCommand.Parameters.AddWithValue("@dog_weight", breed.Weight);
                        insertCommand.Parameters.AddWithValue("@lifespan", breed.LifeSpan);
                        insertCommand.Parameters.AddWithValue("@link_dog", breed.ProfileUrl);
                        insertCommand.Parameters.AddWithValue("@description", breed.Description);
                        insertCommand.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            catch (MySqlException)
            {
                _page.ChangeTextBoxValue("An exeption appeared durring saving. Data is still saved in DB.");
            }
        }
    }
}