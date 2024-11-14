using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibrary
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;
            string jsonFileName = "user.json";
            InitialisationDataBase.Init();
            UserManager userManager = new UserManager(jsonFileName);
            StringOutput.KeyboardPrint("Welcome to library!");
            while(true)
            {
                if(File.Exists(jsonFileName))
                {
                    bool isAdmin = false;
                    User currentUser = userManager.LoadUserData();
                    if(DefaultSqlCommands.IsAdmin(currentUser.name, currentUser.password))
                    {
                        StringOutput.KeyboardPrint($"Hello admin {currentUser.name}");
                        isAdmin = true;
                    }
                    else
                    {
                        StringOutput.KeyboardPrint($"Hello {currentUser.name}");
                    }
                    StringOutput.KeyboardPrint("BORROW TO BORROW THE BOOK");
                    string s = Console.ReadLine();
                    if(s == "delete")
                    {
                        File.Delete(jsonFileName);
                        Console.Clear();
                        continue;
                    }
                    if(isAdmin && s == "gotAll")
                    {
                        foreach(var i in DefaultSqlCommands.GetUsedBooks())
                            Console.WriteLine(i);
                    }
                    if(s == "borrow")
                    {
                        StringOutput.KeyboardPrint("> Write name of book");
                        string bookName = Console.ReadLine();
                        DefaultSqlCommands.GetBookFromLibrary(bookName,currentUser);
                        continue;
                    }

                }
                else
                {
                    StringOutput.KeyboardPrint("> 1 TO LOGIN");
                    StringOutput.KeyboardPrint("> 2 TO SIGN UP");
                    string input = Console.ReadLine();
                    if (input == "1")
                    {
                        Console.Clear();
                        userManager.LogIn();
                        input = "";
                        continue;
                    }
                    if (input == "2")
                    {
                        Console.Clear();
                        userManager .CreateUser();
                        input = "";
                        continue;
                    }
                }
            }
        }
    }
}
