using System;
using ATM2Console.Domain.Entities;

namespace ATM2Console.UI
{
    public class AppScreen
    {
        internal static void Welcome()
        {
            // Change appearance
            Console.Title = "Barclays ATM";
            Console.ForegroundColor = ConsoleColor.Blue;

            // Introduction ASCII art
            Console.WriteLine("\n\n--------------------Welcome to Barclays--------------------\n\n");

            Console.WriteLine("                        .------._ ");
            Console.WriteLine("                  .-\"\"\"`-.<')    `-. ");
            Console.WriteLine("                 (.--. _   `._       `'---.__.-'");
            Console.WriteLine("                  `   `;-.-'         '-    ._");
            Console.WriteLine("                   .--'``  '._      - '   .");
            Console.WriteLine("                    `\"\"\"'-    `---'    ,");
            Console.WriteLine("                           `\\");
            Console.WriteLine("                             `\\      .'");
            Console.WriteLine("                               `'. ");
            Console.WriteLine("                                 `'.");

            Console.WriteLine("\n\n--------------------Welcome to Barclays--------------------\n\n");

            Console.WriteLine("Please insert your ATM card -->");
            Console.WriteLine("NOTE: Actual ATM machine will accept and validate a physical\nATM card, read the card number and validate it.");
        }

        internal static UserAccount UserLoginForm()
        {
            UserAccount tempUserAccount = new UserAccount();

            tempUserAccount.CardNumber = Validator.Convert<long>("your card number");
            tempUserAccount.CardPin = Convert.ToInt32(Utility.GetSecretInput("Enter your card PIN"));
            return tempUserAccount;
        }

        internal static void LoginProgress()
        {
            Console.WriteLine("\n\nChecking card number and PIN");
            Utility.PrintDotAnimation();
        }

        internal static void PrintLockScreen()
        {
            Console.Clear();
            Utility.PrintMessage("Your account is locked. Please go to the nearest branch to unlock your account!", false);
            Utility.PressEnterToContinue();
            Environment.Exit(1); // to exit the app
        }

        internal static void WelcomeCustomer(string fullName)
        {
            Utility.PrintMessage("Successfully logged in!\n", true);
            Console.WriteLine($"Welcome back, {fullName}.");
        }

        internal static void DisplayAppMenu()
        {
            Console.Clear();
            Console.WriteLine("---------- My ATM App Menu ----------");
            Console.WriteLine("1. Account Balance                  :");
            Console.WriteLine("2. Cash Deposit                     :");
            Console.WriteLine("3. Withdrawal                       :");
            Console.WriteLine("4. Transfer                         :");
            Console.WriteLine("5. Transactions                     :");
            Console.WriteLine("6. Logout                           :");
            Console.WriteLine("-------------------------------------");

        }

        internal static void LogoutProgress()
        {
            Utility.PrintMessage("Thank you and have a nice day!", true);
            Utility.PrintDotAnimation();
            Console.Clear();
        }

        internal const string cur = "GBP"; // Video

        internal static int SelectAmount()
        {
            Console.WriteLine("");
            Console.WriteLine(":1. 50 {0}     5. 1,000 {0}", cur);
            Console.WriteLine(":2. 100 {0}    6. 1,500 {0}", cur);
            Console.WriteLine(":3. 200 {0}    7. 2,000 {0}", cur);
            Console.WriteLine(":4. 500 {0}    8. 4,000 {0}", cur);
            Console.WriteLine(":9. Other      0. Get back");

            int selectedAmount = Validator.Convert<int>("<OPTION> number:");
            switch (selectedAmount)
            {
                case 1:
                    return 50;
                    break;
                case 2:
                    return 100;
                    break;
                case 3:
                    return 200;
                    break;
                case 4:
                    return 500;
                    break;
                case 5:
                    return 1000;
                    break;
                case 6:
                    return 1500;
                    break;
                case 7:
                    return 2000;
                    break;
                case 8:
                    return 4000;
                    break;
                case 9:
                    return 0;
                    break;
                case 0:
                    return 1;
                    break;
                default:
                    Utility.PrintMessage("Invalid input. Try again!", false);
                    return -1;
                    break;
            }
        }

        internal InternalTransfer InternalTransferForm()
        {
            var internalTransfer = new InternalTransfer();
            internalTransfer.RecipientBankAccountNumber = Validator.Convert<long>("recipient's account number:"); // long error
            internalTransfer.TransferAmount = Validator.Convert<decimal>($"amount {cur}:");
            internalTransfer.RecipientBankAccountName = Utility.GetUserInput("recipient's name:");
            return internalTransfer;
        }
    }
}