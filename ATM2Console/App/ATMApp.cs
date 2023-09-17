using System;
using System.Collections.Generic; // loading to use List
using ATM2Console.UI; // loading from AppScreen.cs namespace ATM2Console.UI
using ATM2Console.Domain.Entities; // loading from UserAccount.cs namespace ATM2Console.Domain.Entities
using ATM2Console.Domain.Interfaces; // loading from IUserLogin.cs namespace ATM2Console.Domain.Interfaces
using ATM2Console.Domain.Enums; // loading from AppMenu.cs namespace ATM2Console.Domain.Enums
using System.Linq; // loading to use FirstOrDefault()
using ConsoleTables; // loading to use ConsoleTable()

namespace ATM2Console.App
{
    public class ATMApp // : IUserLogin, IUserAccountActions, ITransaction // COM + . to apply
    {
        // init Get & Set user data
        private List<UserAccount> userAccountList;
        private UserAccount selectedAccount;
        private List<Transaction> _listOfTransactions;
        private const decimal minimumKeptAmount = 50;
        private readonly AppScreen screen; // in order to load InternalTransferForm() as it is not static

        public ATMApp() // in order to load InternalTransferForm() as it is not static
        {
            screen = new AppScreen();
        }

        public void Run()
        {
            // AppScreen intro
            AppScreen.Welcome();
            Utility.PressEnterToContinue();

            // Authentication
            CheckUserCardNumAndPassword();
            AppScreen.WelcomeCustomer(selectedAccount.FullName);
            Utility.PressEnterToContinue();

            // Account operations
            bool infinite = true;
            while (infinite)
            {
                AppScreen.DisplayAppMenu();
                infinite = ProcessMenuoption(infinite);
                Utility.PressEnterToContinue();
                if (!infinite)
                {
                    break;
                }
            }
        }

        public void InitializeData()
        {
            userAccountList = new List<UserAccount>
            {
                new UserAccount{Id = 1,
                                CardNumber = 1234567890,
                                CardPin = 1234,
                                AcountNumber = 99999999,
                                FullName = "Jaroslav Kotrba",
                                AccountBalance = 10000.00m,
                                TotalLogin = 0,
                                IsLocked = false},
                new UserAccount{Id = 2,
                                CardNumber = 0987654321,
                                CardPin = 4321,
                                AcountNumber = 19951015,
                                FullName = "Anna Ondrackova",
                                AccountBalance = 103.00m,
                                TotalLogin = 0,
                                IsLocked = false},
                new UserAccount{Id = 3,
                                CardNumber = 1111111111,
                                CardPin = 0000,
                                AcountNumber = 12345678,
                                FullName = "Anna Kotrbova",
                                AccountBalance = 10.00m,
                                TotalLogin = 0,
                                IsLocked = true}

            };
            _listOfTransactions = new List<Transaction>(); // video
        }

        public void CheckUserCardNumAndPassword()
        {
            bool isCorrectLogin = false;

            while(isCorrectLogin == false)
            {
                UserAccount inputAccount = AppScreen.UserLoginForm();
                AppScreen.LoginProgress();
                foreach(UserAccount account in userAccountList)
                {
                    selectedAccount = account;
                    if (inputAccount.CardNumber.Equals(selectedAccount.CardNumber))
                    {
                        if (inputAccount.CardPin.Equals(selectedAccount.CardPin))
                        {
                            selectedAccount = account;

                            if (selectedAccount.IsLocked || selectedAccount.TotalLogin > 3)
                            {
                                AppScreen.PrintLockScreen();
                            }
                            else
                            {
                                selectedAccount.TotalLogin = 0;
                                isCorrectLogin = true;
                                break;
                            }
                        }
                    }
                }

                if (!isCorrectLogin)
                {
                    selectedAccount.TotalLogin++; // TotalLogin every round +1
                    Utility.PrintMessage("\nInvalid card number or PIN.\n", false);

                    Utility.PressEnterToContinue();
                    Console.Clear();

                    selectedAccount.IsLocked = selectedAccount.TotalLogin == 3;
                    if (selectedAccount.IsLocked)
                    {
                        AppScreen.PrintLockScreen();
                    }
                }
            }
        }

        private bool ProcessMenuoption(bool _infinite)
        {
            switch (Validator.Convert<int>("a <MENU> option:"))
            {
                case (int)AppMenu.CheckBalance:
                    CheckBalance();
                    break;
                case (int)AppMenu.PlaceDeposit:
                    PlaceDeposit();
                    break;
                case (int)AppMenu.MakeWithdrawal:
                    MakeWithDrawal();
                    break;
                case (int)AppMenu.InternalTransfer:
                    var internalTransfer = screen.InternalTransferForm();
                    ProcessInternalTransfer(internalTransfer);
                    break;
                case (int)AppMenu.ViewTransaction:
                    ViewTransaction();
                    break;
                case (int)AppMenu.Logout:
                    AppScreen.LogoutProgress();
                    Utility.PrintMessage("You have successfully logged out!", true);
                    _infinite = false;
                    // Run();
                    break;
                default:
                    Utility.PrintMessage("Invalid option!", false);
                    break;
            }
            return _infinite;
        }

        public void CheckBalance()
        {
            Utility.PrintMessage($"Your account balance as of now: {Utility.FormatAmount(selectedAccount.AccountBalance)}", true);
        }

        public void PlaceDeposit()
        {
            // The core
            // var transaction_amt = Validator.Convert<int>("amount:");
            // selectedAccount.AccountBalance += transaction_amt;
            // CheckBalance();

            Console.WriteLine("\nOnly 50 and 100 GBP notes allowed.");
            var transaction_amt = Validator.Convert<int>($"amount {AppScreen.cur}:");
            Console.WriteLine("\nChecking and counting bank notes.");
            Utility.PrintDotAnimation();
            Console.WriteLine("");

            // some guard clause
            if (transaction_amt <= 0)
            {
                Utility.PrintMessage("Amount needs to be greater than zero. Try again!", false);
                return;
            }

            if (transaction_amt % 50 != 0)
            {
                Utility.PrintMessage($"Enter deposit amount in multiples of 50 or 100. Try again!", false);
                return;
            }

            if (PreviewBankNotesCount(transaction_amt) == false)
            {
                Utility.PrintMessage($"You have cancelled your action.", false);
                return;
            }

            // bind transactions details to transaction object
            InsertTransaction(selectedAccount.Id, TransactionType.Deposit, transaction_amt, "");

            // update account balance
            selectedAccount.AccountBalance += transaction_amt;

            // new amount
            CheckBalance();
        }

        public void MakeWithDrawal()
        {
            var transaction_amt = 0;
            int selectedAmount = AppScreen.SelectAmount();

            // get output of SelectAmount()
            if (selectedAmount == -1)
            {
                MakeWithDrawal();
                return;
            }
            else if (selectedAmount == 1)
            {
                return;
            }
            else if (selectedAmount != 0)
            {
                transaction_amt = selectedAmount;
            }
            else
            {
                transaction_amt = Validator.Convert<int>($"amount {AppScreen.cur}:");
            }

            // input validation
            if (transaction_amt <= 0)
            {
                Utility.PrintMessage("Amount needs to be greater than zero. Try again!", false);
                return;
            }
            if (transaction_amt % 50 != 0) // the rest != 0
            {
                Utility.PrintMessage("You can only withdraw amount in multiples of 50 or 100 GBP. Try again!", false);
                return;
            }

            // business logic validation
            if (transaction_amt > selectedAccount.AccountBalance)
            {
                Utility.PrintMessage($"Withdrawal failed. Your balance is too low to withdraw " +
                    $"{Utility.FormatAmount(transaction_amt)}", false);
                return;
            }
            if ((selectedAccount.AccountBalance - transaction_amt) < minimumKeptAmount)
            {
                Utility.PrintMessage($"Withdrawal failed. Your account needs to have " +
                    $"minimum of {Utility.FormatAmount(minimumKeptAmount)}!", false);
                return;
            }

            // bind withdrawal details to transaction object
            InsertTransaction(selectedAccount.Id, TransactionType.Withdrawal, -transaction_amt, "");
            selectedAccount.AccountBalance -= transaction_amt;
            Utility.PrintMessage($"You have successfully withdrawn " + $"{Utility.FormatAmount(transaction_amt)}", true);
        }

        private bool PreviewBankNotesCount(int amount)
        {
            int hundredNotesCount = amount / 100;
            int fiftyNotesCount = (amount % 100) / 50;

            Console.WriteLine("Summary");
            Console.WriteLine("-----");
            Console.WriteLine($"{AppScreen.cur} 100 X {hundredNotesCount} = {100 * hundredNotesCount}");
            Console.WriteLine($"{AppScreen.cur} 50 X {fiftyNotesCount} = {50 * fiftyNotesCount}");
            Console.WriteLine($"Total amount: {Utility.FormatAmount(amount)}");

            int opt = Validator.Convert<int>("1 to confirm");
            return opt.Equals(1);
        }

        public void InsertTransaction(long _UserBankAccountId, TransactionType _tranType, decimal _tranAmount, string _desc)
        {
            // create a new transaction object
            var transaction = new Transaction()
            {
                TransactionId = Utility.GetTransactionId(),
                UserBankAccountId = _UserBankAccountId,
                TransactionDate = DateTime.Now,
                TransactionType = _tranType,
                TransactionAmount = _tranAmount,
                Description = _desc
            };

            // add transaction object to the list
            _listOfTransactions.Add(transaction);
        }

        private void ProcessInternalTransfer(InternalTransfer internalTransfer)
        {
            if (internalTransfer.TransferAmount <= 0)
            {
                Utility.PrintMessage("Amount needs to be more than zero. Try again!", false);
                return;
            }

            // check sender's account balance
            if (internalTransfer.TransferAmount > selectedAccount.AccountBalance)
            {
                Utility.PrintMessage($"Transfer failed. You do not hava enough balance " +
                    $"to transfer {Utility.FormatAmount(internalTransfer.TransferAmount)}", false);
                return;
            }

            // check the minimum kept amount
            if ((selectedAccount.AccountBalance - internalTransfer.TransferAmount) < minimumKeptAmount)
            {
                Utility.PrintMessage($"Transfer failed. Your account needs to have minimum " +
                    $" {Utility.FormatAmount(minimumKeptAmount)}", false);
                return;
            }

            // check reciever's account number is valid
            var selectedBankAccountReciever = (from userAcc in userAccountList
                                               where userAcc.AcountNumber == internalTransfer.RecipientBankAccountNumber
                                               select userAcc).FirstOrDefault();

            // bank account is not in our db
            if (selectedBankAccountReciever == null)
            {
                Utility.PrintMessage("Transfer failed. Recipient's bank account NUMBER is not valid.", false);
                return;
            }

            // check receiver's name
            if (selectedBankAccountReciever.FullName != internalTransfer.RecipientBankAccountName)
            {
                Utility.PrintMessage("Transfer failed. Recipient's bank account NAME does not match.", false);
                return;
            }

            // add transaction to transactions sender-records
            InsertTransaction(selectedAccount.Id, TransactionType.Transfer, -internalTransfer.TransferAmount, "Transfered " +
                $"to {selectedBankAccountReciever.AcountNumber} ({selectedBankAccountReciever.FullName})");

            // update sender's account balance
            selectedAccount.AccountBalance -= internalTransfer.TransferAmount;

            // add transaction to transactions reciever-records
            InsertTransaction(selectedBankAccountReciever.Id, TransactionType.Transfer, internalTransfer.TransferAmount, "Transfered from " +
                $"{selectedAccount.AcountNumber}({selectedAccount.FullName})");

            // update reciever's account balance
            selectedBankAccountReciever.AccountBalance += internalTransfer.TransferAmount;

            // print success message
            Utility.PrintMessage($"You have successfully transfered " +
                $"{Utility.FormatAmount(internalTransfer.TransferAmount)} to " +
                $"{internalTransfer.RecipientBankAccountName}", true);
        }

        public void ViewTransaction()
        {
            var filteredTransactionList = _listOfTransactions.Where(t => t.UserBankAccountId == selectedAccount.Id).ToList();

            // check if there is a transaction
            if(filteredTransactionList.Count <= 0)
            {
                Utility.PrintMessage("You have no transaction yet.", true);
            }
            // adding ConsoleTables (right click on Dependencies -> Manage NuGet Packages... -> search ConsoleTables -> Add Package
            else
            {
                var table = new ConsoleTable("Id", "Transaction Date", "Type", "Description", "Amount " + AppScreen.cur);
                foreach(var tran in filteredTransactionList)
                {
                    table.AddRow(tran.TransactionId, tran.TransactionDate, tran.TransactionType, tran.Description, tran.TransactionAmount);
                }
                table.Options.EnableCount = false; // will be using TransactionId for counting, automatic count is disabled
                table.Write();
                Utility.PrintMessage($"You have {filteredTransactionList.Count} transaction(s)", true);
            }
        }
    }
}