using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineLibrary
{
    internal class StringOutput
    {
        public static void KeyboardPrint(string text, int delayToWriteChar = 10, bool newLine = true)
        {
            foreach(char c in text)
            {
                Console.Write(c);
                Thread.Sleep(delayToWriteChar);
            }

            if(newLine)
                Console.WriteLine();
        }
    }
}
