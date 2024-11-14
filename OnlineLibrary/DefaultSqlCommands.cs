using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibrary
{
    internal class DefaultSqlCommands
    {
        public static bool IsAdmin(string name, string password)
        {
            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [Users] WHERE Name = @NAME AND Password = @PASSWORD AND IsAdmin = 1",
                InitialisationDataBase.GetConnection());
            sqlCommand.Parameters.Add("@NAME", System.Data.SqlDbType.NVarChar).Value = name;
            sqlCommand.Parameters.Add("@PASSWORD", System.Data.SqlDbType.NVarChar).Value = password;
            SqlDataReader reader = sqlCommand.ExecuteReader();

            bool userExists = reader.HasRows;
            reader.Close();
            sqlCommand.Parameters.Clear();

            return userExists;
        }

        public static List<string> GetUsedBooks(bool getAll = true, string name = "", string password = "")
        {
            SqlCommand sqlCommand;
            string usedBooks = "";
            List<string> result = new List<string>();

            if (getAll)
            {
                sqlCommand = new SqlCommand("SELECT Name, PickedBooks FROM[Users] WHERE PickedBooks != 'b", InitialisationDataBase.GetConnection());
            }
            else
            {
                sqlCommand = new SqlCommand("SELECT Name, PickedBooks FROM[Users] WHERE Name = @NAME AND Password = @PASSWORD", InitialisationDataBase.GetConnection());
                sqlCommand.Parameters.Add("@NAME", System.Data.SqlDbType.NVarChar).Value = name;
                sqlCommand.Parameters.Add("@PASSWORD", System.Data.SqlDbType.NVarChar).Value = password;
            }

            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

            if (sqlDataReader.HasRows)
            {
                while (sqlDataReader.Read())
                {
                    if (sqlDataReader["PickedBooks"] != DBNull.Value && sqlDataReader["Name"] != DBNull.Value)
                    {
                        string userName = sqlDataReader["Name"].ToString();
                        string pickedBooks = sqlDataReader["PickedBooks"].ToString();

                        string[] usedBookArray = pickedBooks.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        foreach (var book in usedBookArray)
                        {
                            result.Add($"Книга: {book}, Пользователь: {userName}");
                        }
                    }
                }
            }
            sqlDataReader.Close();

            return result;
        }
        
        public static void GetBookFromLibrary(string name, User user)
        {
            SqlCommand sqlCommand = new SqlCommand("UPDATE Users SET PickedBooks = PickedBooks + ' ' + @BOOK_NAME WHERE Name = @NAME AND PASSWORD = @PASSWORD", InitialisationDataBase.GetConnection());
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
            sqlCommand.Parameters.Add("BOOK_NAME", System.Data.SqlDbType.NVarChar).Value = name;
            sqlCommand.Parameters.Add("NAME", System.Data.SqlDbType.NVarChar).Value = user.name;
            sqlCommand.Parameters.Add("PASSWORD", System.Data.SqlDbType.NVarChar).Value = user.password;
            List<string> borrowedBooks = GetUsedBooks(false, user.name, user.password);
            Console.WriteLine($"You borrow {borrowedBooks[borrowedBooks.Count - 1]}");
        }
    }
}
