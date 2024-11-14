using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineLibrary
{
    public class InitialisationDataBase
    {
        private static SqlConnection _connection;

        public static void Init()
        {
            _connection = new SqlConnection(ConfigurationManager.ConnectionStrings["LibraryDB"].ConnectionString);
            try
            {
                Console.WriteLine("Trying to connecting db...");
                _connection.Open();
                Thread.Sleep(2000);
                Console.WriteLine("Success");
                Console.Clear();
            }
            catch(Exception ex) 
            {
                File.Create("exceptions.txt");
                File.WriteAllText("exceptions.txt", ex.StackTrace);
            }
        }

        public static SqlConnection GetConnection()
        {
            return _connection;
        }
    }
}
