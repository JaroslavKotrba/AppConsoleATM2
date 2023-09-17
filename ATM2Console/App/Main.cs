using System;
using ATM2Console.UI;

namespace ATM2Console.App
{
	public class Init
	{
        static void Main(string[] args)
        {
            // New session
            ATMApp atmApp = new ATMApp();
            atmApp.InitializeData();
            atmApp.Run();
        }
    }
}

