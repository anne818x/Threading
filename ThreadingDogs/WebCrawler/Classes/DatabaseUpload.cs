using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace WebCrawler.Classes
{
    class DatabaseUpload
    {
        public static void InsertIntoDatabase()
        {
            using (SqliteConnection db = new SqliteConnection("Filename=dogDatabase.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                foreach (var breed in WebCrawler.ExportBreeds)
                {
                    //Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText =
                        "INSERT INTO dog VALUES (dog_ID, breed, dog_image, breed_group, dog_height, dog_weight, lifespan, link_dog);";

                    insertCommand.Parameters.AddWithValue("dog_ID"    , 1);
                    insertCommand.Parameters.AddWithValue("breed"     , breed.Breed);
                    insertCommand.Parameters.AddWithValue("dog_image" , breed.Image);
                    insertCommand.Parameters.AddWithValue("dog_group" , breed.BreedGroup);
                    insertCommand.Parameters.AddWithValue("dog_height", breed.Height);
                    insertCommand.Parameters.AddWithValue("dog_weight", breed.Weight);
                    insertCommand.Parameters.AddWithValue("lifespan"  , breed.LifeSpan);
                    insertCommand.Parameters.AddWithValue("link_dog"  , breed.ProfileUrl);

                    try
                    {
                        insertCommand.ExecuteNonQuery();
                    }
                    catch (SqliteException error)
                    {
                        //Handle error
                        return;
                    }
                }
                db.Close();
                //Output.ItemsSource = Grab_Entries();
            }
        }
    }
}