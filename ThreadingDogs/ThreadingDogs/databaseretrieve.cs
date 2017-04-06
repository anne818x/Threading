using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using ThreadingDogs.Classes;

namespace ThreadingDogs
{

    class databaseretrieve
    {
        //The list to put the breeds into
        List<Dog> Breedname = new List<Dog>();

        /// <summary>
        /// Retrieves all the dog data out of the database, and puts it into a list
        /// which is connected to the mainpage.
        /// </summary>
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
                    //Query to select all columns out of the database
                    getCommand.CommandText = "SELECT * FROM dog";
                    //Reads the data 
                    using (MySqlDataReader reader = getCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var dog = new Dog();
                            //getting data from each column from the database
                            dog.Breed = reader.GetString("breed");
                            dog.BreedGroup = reader.GetString("breed_group");
                            dog.Height = reader.GetString("dog_height");
                            dog.Weight = reader.GetString("dog_weight");
                            dog.LifeSpan = reader.GetString("lifespan");
                            dog.Image = reader.GetString("dog_image");
                            dog.ProfileUrl = reader.GetString("link_dog");
                            //adds the data it retireved from dog into the list
                            Breedname.Add(dog);
                        }
                    }
                    //closes connection
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
