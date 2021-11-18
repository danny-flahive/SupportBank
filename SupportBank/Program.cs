using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SupportBank
{
    class Program
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            CreateLogger();
            bool cont = true;
            string pathPartOne = @"C:\Users\Danny.Flahive\Corndel\SupportBank\SupportBank\Transactions2014.csv";
            string pathPartTwo = @"C:\Users\Danny.Flahive\Corndel\SupportBank\SupportBank\DodgyTransactions2015.csv";
            Dictionary<string, Account> accounts = GenerateAccounts(GetTransactionData(pathPartTwo));
            while (cont)
            {
                RunMenu(ref cont, accounts);
            }
        }

        private static void RunMenu(ref bool cont, Dictionary<string, Account> accounts)
        {
            Console.WriteLine("Enter an option: ");
            string userInput = Console.ReadLine();
            if (userInput.ToLower() == "list all")
            {
                ListAll(accounts);
            }
            else if (userInput.Length > 5 && userInput.Substring(0, 5).ToLower() == "list ")
            {
                ListAccount(userInput[5..], accounts);
            }
            else if (userInput.ToLower() == "quit")
            {
                Logger.Info("User manually exited program");
                cont = false;
            }
            else
            {
                Logger.Warn("User gave invalid command to menu");
                Console.WriteLine("Command not recognised.");
            }
        }

        private static void CreateLogger()
        {
            string logPath = @"C:\Users\Danny.Flahive\Corndel\SupportBank\SupportBank\SupportBank.log";
            var config = new LoggingConfiguration();
            var target = new FileTarget { FileName = logPath, Layout = @"${longdate} ${level} - ${logger}: ${message}" };
            config.AddTarget("File Logger", target);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, target));
            LogManager.Configuration = config;
            File.WriteAllText(logPath, "");
            Logger.Info("Program started");
        }

        private static void ListAccount(string name, Dictionary<string, Account> accounts)
        {
            Logger.Info("ListAccount chosen");
            if (accounts.ContainsKey(name))
            {
                Console.WriteLine(accounts[name].ToString());
            }
            else
            {
                Logger.Warn("Invalid account given to ListAccount");
                Console.WriteLine("Account not found");
            }
        }

        private static void ListAll(Dictionary<string, Account> accounts)
        {
            Logger.Info("Listing all accounts");
            foreach (var pair in accounts)
            {
                Console.WriteLine($"{pair.Key} has a balance of {pair.Value.GetBalance():C}");
            }
            Console.WriteLine();
        }

        private static Dictionary<string, Account> GenerateAccounts(string[] allData)
        {
            Logger.Info("Generating accounts from user data");
            Dictionary<string, Account> accounts = new Dictionary<string, Account>();
            foreach (var rowData in allData.Skip(1).Select((transactionData, index) => (transactionData, index)))
            {
                int rowNumber = rowData.index + 2; //Due to 0 indexing and skipping the first line
                Transaction transaction = GenerateTransaction(rowData.transactionData, rowNumber);
                if (transaction.toAccount == null) //Could check for any parameter being null here to show the row is invalid
                {
                    continue;
                }
                if (!accounts.ContainsKey(transaction.fromAccount))
                {
                    accounts[transaction.fromAccount] = new Account(transaction.fromAccount);
                }
                if (!accounts.ContainsKey(transaction.toAccount))
                {
                    accounts[transaction.toAccount] = new Account(transaction.toAccount);
                }
                accounts[transaction.fromAccount].UpdateAccount(transaction);
                accounts[transaction.toAccount].UpdateAccount(transaction);
            }
            Logger.Info("Accounts successfully created.");
            return accounts;
        }

        private static string[] GetTransactionData(string path)
        {
            Logger.Info("Reading data from file");
            try
            {
                return File.ReadAllLines(path);
            }
            catch (Exception e)
            {
                Logger.Fatal($"Exception thrown when reading from file with message: {e.Message}");
                Console.WriteLine(e.Message);
            }
            return new string[0];
        }

        private static Transaction GenerateTransaction(string transactionData, int row)
        {
            string errorMessage;
            try
            {
                Transaction transaction = new Transaction(transactionData.Split(","));
                return transaction;
            }
            catch (IndexOutOfRangeException e)
            {
                errorMessage = $"Not enough fields in row {row}.";
            }
            catch (FormatException e)
            {
                errorMessage = $"The data in row {row} is not properly formatted. Exception message: {e.Message}";
            }
            catch (Exception e)
            {
                errorMessage = $"Unhandled exception in row {row}. Error message: {e.Message}.";
            }
            errorMessage += $" Row {row} skipped.";
            Logger.Warn(errorMessage);
            Console.WriteLine(errorMessage);
            return new Transaction();
        }
    }
}
