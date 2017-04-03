using MySql.Data.MySqlClient;

//using Microsoft.EntityFrameworkCore;


namespace WebCrawler.Classes
{
    class DatabaseUpload
    {
        public static bool InsertToDb()
        {
            try
            {
                using (
                    MySqlConnection connection =
                        new MySqlConnection("Server=127.0.0.1;Database=dogthreading;Uid=root;Pwd=1234;SslMode=None;"))
                {
                    connection.Open();
                    foreach (var breed in WebCrawler.ExportBreeds)
                    {
                        MySqlCommand insertCommand = connection.CreateCommand();
                        insertCommand.CommandText =
                            "INSERT INTO dog( breed, dog_image, breed_group, dog_height, dog_weight, lifespan, link_dog)VALUES( @breed, @dog_image, @breed_group, @dog_height, @dog_weight, @lifespan, @link_dog)";
                        // insertCommand.Parameters.AddWithValue("@id_dog", 1);
                        insertCommand.Parameters.AddWithValue("@breed", breed.Breed);
                        insertCommand.Parameters.AddWithValue("@dog_image", breed.Image);
                        insertCommand.Parameters.AddWithValue("@breed_group", breed.BreedGroup);
                        insertCommand.Parameters.AddWithValue("@dog_height", breed.Height);
                        insertCommand.Parameters.AddWithValue("@dog_weight", breed.Weight);
                        insertCommand.Parameters.AddWithValue("@lifespan", breed.LifeSpan);
                        insertCommand.Parameters.AddWithValue("@link_dog", breed.ProfileUrl);
                        insertCommand.ExecuteNonQuery();
                    }

                }
                return true;
            }
            catch (MySqlException)
            {
                // Don't forget to handle it 
                return false;
            }
        }
    }
}