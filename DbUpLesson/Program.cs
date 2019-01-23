using DbUp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using Dapper.Contrib.Extensions;

namespace DbUpLesson
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DapperConnection"].ConnectionString;

            MigrateDatabase(connectionString);

            Car car = new Car
            {
                Mark = "Nissan",
                Model = "GT-R",
                Color = "Green"
            };

            Car car2 = new Car
            {
                Mark = "Lambo",
                Model = "Aventodor",
                Color = "White"
            };

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Insert(car2);

            var cars = connection.GetAll<Car>();
            foreach(var item in cars)
            {
                Console.WriteLine(item.Mark);
            }
        }

        private static void MigrateDatabase(string connectionString)
        {
            

            EnsureDatabase.For.SqlDatabase(connectionString);
        

            var upgrader =
                DeployChanges.To
                    .SqlDatabase(connectionString)
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                    .LogToTrace()
                    .Build();

            var result = upgrader.PerformUpgrade();  
        }
    }
}
