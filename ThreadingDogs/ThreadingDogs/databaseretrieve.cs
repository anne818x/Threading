using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadingDogs.Classes;

namespace ThreadingDogs
{
    class databaseretrieve
    {
        List<Dog> Breedname = new List<Dog>();
        public List<Dog> dogList()
        {
            try
            {
                //Connection string to the database 
                using (MySqlConnection connection = new MySqlConnection("Server = 127.0.0.1; Database = dogthreading; Uid = root; Pwd = 1234; SslMode = None;"))
                {
                    //Opens
                    connection.Open();
                    MySqlCommand getCommand = connection.CreateCommand();
                    getCommand.CommandText = "SELECT * FROM dog";
                    using (MySqlDataReader reader = getCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var dog = new Dog();
                            dog.Breed = reader.GetString("breed");
                            dog.BreedGroup = reader.GetString("breed_group");
                            dog.Height = reader.GetString("dog_height");
                            dog.Weight = reader.GetString("dog_weight");
                            dog.LifeSpan = reader.GetString("lifespan");
                            dog.Image = reader.GetString("dog_image");
                            dog.ProfileUrl = reader.GetString("link_dog");
                            Breedname.Add(dog);
                        }
                    }
                    connection.Close();
                }
            }
            catch (MySqlException)
            {
                
            }
            return Breedname;
        }

     
           
        }
}
