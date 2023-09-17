using System;
using System.Text; // to load class StringBuilder()
using System.Globalization; // to load class CultureInfo()

namespace ATM2Console.UI
{
    public static class Utility
    {
        public static string GetSecretInput(string prompt)
        {
            bool isPrompt = true;
            string asterics = "";

            StringBuilder input = new StringBuilder();

            while (true)
            {
                if (isPrompt)
                    Console.WriteLine(prompt);
                isPrompt = false; // error need to be set as false
                ConsoleKeyInfo inputKey = Console.ReadKey(true);

                if (inputKey.Key == ConsoleKey.Enter)
                {
                    if(input.Length == 4)
                    {
                        break;
                    } else {
                        PrintMessage("\nPlease enter 4 digits.", false);
                        isPrompt = true;
                        input.Clear();
                        continue; // error need to be set as continue
                    }
                }

                if (inputKey.Key == ConsoleKey.Backspace && input.Length > 0)
                {
                    input.Remove(input.Length - 1, 1);
                    Console.Write("\b \b"); // Move the cursor back, print a space, and move the cursor back again
                }
                else if (inputKey.Key != ConsoleKey.Backspace) {
                    input.Append(inputKey.KeyChar);
                    Console.Write(asterics + "*");
                }
            }
            return input.ToString();
        }

        public static void PrintMessage(string msg, bool success = true)
        {
            // Changing background color
            if(success)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            } else {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            Console.WriteLine(msg);
            Console.ForegroundColor = ConsoleColor.Blue;
        }

        public static string GetUserInput(string prompt)
        {
            // Tasks for a user to enter
            Console.WriteLine($"\nEnter {prompt}");
            return Console.ReadLine();
        }

        public static void PrintDotAnimation(int timer=10)
        {
            for (int i = 0; i < timer; i++)
            {
                Console.Write(".");
                Thread.Sleep(200);
            }
            Console.Clear();
        }

        public static void PressEnterToContinue()
        {
            // Press Enter
            Console.WriteLine("\nPress Enter to continue...\n");

            // Wait before closing
            Console.ReadLine();
        }

        // format GBP
        private static CultureInfo culture = new CultureInfo("en-GB");

        public static string FormatAmount(decimal amt)
        {
            return String.Format(culture, "{0:C2}", amt);
        }

        // transaction ID
        private static long tranId;

        public static long GetTransactionId()
        {
            return ++tranId;
        }
    }
}