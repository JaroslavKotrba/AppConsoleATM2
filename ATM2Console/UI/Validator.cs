using System;
using System.ComponentModel;

namespace ATM2Console.UI
{   // To get correct input from an user
    public static class Validator
    {
        // T can be converted to any type
        public static T Convert<T>(string prompt)
        {
            bool valid = false;
            string userInput;

            while (!valid)
            {
                userInput = Utility.GetUserInput(prompt);

                try
                {
                    var converter = TypeDescriptor.GetConverter(typeof(T));
                    if (converter != null)
                    {
                        return (T)converter.ConvertFromString(userInput);
                    } else {
                        return default;
                    }
                }

                catch
                {
                    Utility.PrintMessage("\nInvalid input. Try again!", false);
                }
            }
            return default;
        }
    }
}