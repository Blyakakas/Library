using Newtonsoft.Json;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Threading;

namespace OnlineLibrary
{
    public class UserManager
    {
        private string _jsonFileName;

        public UserManager(string jsonFileName) { _jsonFileName = jsonFileName; }

        public bool IsHasUser(string name, string password)
        {
            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [Users] WHERE Name = @NAME AND Password = @PASSWORD",
                InitialisationDataBase.GetConnection());
            sqlCommand.Parameters.Add("@NAME", System.Data.SqlDbType.NVarChar).Value = name;
            sqlCommand.Parameters.Add("@PASSWORD", System.Data.SqlDbType.NVarChar).Value = password;
            SqlDataReader reader = sqlCommand.ExecuteReader();

            bool userExists = reader.HasRows;
            reader.Close();
            sqlCommand.Parameters.Clear();

            return userExists;
        }
        
        public void LogIn()
        {
            string name;
            string password;

            StringOutput.KeyboardPrint("> Enter your name");
            Console.Write(">");
            name = Console.ReadLine();
            Console.Clear();

            StringOutput.KeyboardPrint("> Enter your password");
            Console.Write(">");
            password = Console.ReadLine();
            Console.Clear();
            
            if(IsHasUser(name,password))
                SaveUserData(name,password);
            else
                StringOutput.KeyboardPrint("No this user!");
             
        }

        public void CreateUser()
        {
            string name;
            string password;
            string email;

            StringOutput.KeyboardPrint("> Enter your name");
            Console.Write(">");
            name = Console.ReadLine();
            Console.Clear();

            StringOutput.KeyboardPrint("> Enter your password");
            Console.Write(">");
            password = Console.ReadLine();
            Console.Clear();

            StringOutput.KeyboardPrint("> Enter your email");
            Console.Write(">");
            email = Console.ReadLine();
            Console.Clear();

            StringOutput.KeyboardPrint($"> Your name is - {name}");
            StringOutput.KeyboardPrint($"> Your password is - {password}");
            StringOutput.KeyboardPrint($"> Your email is - {email}");

            if (IsHasUser(name, password))
            {
                StringOutput.KeyboardPrint("> User already exists. Try logging in.");
                return; 
            }

            SqlCommand sqlCommand = new SqlCommand("INSERT INTO Users (Name, Password, Email) VALUES(@NAME, @PASSWORD, @EMAIL)", InitialisationDataBase.GetConnection());
            sqlCommand.Parameters.Add("@NAME", System.Data.SqlDbType.NVarChar).Value = name;
            sqlCommand.Parameters.Add("@PASSWORD", System.Data.SqlDbType.NVarChar).Value = password;
            sqlCommand.Parameters.Add("@EMAIL", System.Data.SqlDbType.NVarChar).Value = email;

            try
            {
                sqlCommand.ExecuteNonQuery();
                StringOutput.KeyboardPrint("> You have successfully signed up!");

                StringOutput.KeyboardPrint("> A verification code has been sent to your email. Please enter it below:");

                string verificationKey = EmailSender.SendVerificationCode(email);
                string userVerificationKey = Console.ReadLine();
                Console.Clear();
                if (verificationKey == userVerificationKey || userVerificationKey == "q")
                {
                    StringOutput.KeyboardPrint("> Success! Your account has been verified.");
                    Thread.Sleep(1000);
                    Console.Clear();
                    SaveUserData(name, password);
                }
                else
                {
                    StringOutput.KeyboardPrint("> The verification code is incorrect.");
                }
            }
            catch (Exception ex)
            {
                StringOutput.KeyboardPrint($"> Error: {ex.Message}");
            }
            finally
            {
                sqlCommand.Parameters.Clear();
            }
        }

        public void SaveUserData(string name, string password)
        {
            User savingUser = new User();
            SqlCommand sqlCommand = new SqlCommand("SELECT PickedBooks FROM [Users] WHERE Name = @NAME AND Password = @PASSWORD", InitialisationDataBase.GetConnection());
            sqlCommand.Parameters.Add("@NAME", System.Data.SqlDbType.NVarChar).Value = name;
            sqlCommand.Parameters.Add("@PASSWORD", System.Data.SqlDbType.NVarChar).Value = password;

            SqlDataReader reader = sqlCommand.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                savingUser.name = name;
                savingUser.password = password;
                savingUser.pickedBooks = reader["PickedBooks"].ToString();

            }
            reader.Close();

            string jsonUser = JsonConvert.SerializeObject(savingUser);
            File.WriteAllText(_jsonFileName, jsonUser);

        }

        public User LoadUserData()
        {
            string jsonString = File.ReadAllText(_jsonFileName);
            return JsonConvert.DeserializeObject<User>(jsonString);
        }
    }
}
