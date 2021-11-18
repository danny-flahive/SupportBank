using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SupportBank
{
    class Program
    {
        static void Main(string[] args)
        {
            bool cont = true;
            string path = @"C:\Users\Danny.Flahive\Corndel\SupportBank\SupportBank\Transactions2014.csv";
            Dictionary<string, Account> accounts = GenerateAccounts(GetTransactionData(path));
            while (cont)
            {
                Console.WriteLine("Enter an option: ");
                string userInput = Console.ReadLine();
                if (userInput.ToLower() == "list all")
                {
                    ListAll(accounts);
                }
                else if (userInput.Substring(0, 5).ToLower() == "list ")
                {
                    ListAccount(userInput[5..], accounts);
                }
                else if (userInput.ToLower() == "quit")
                {
                    cont = false;
                }
                else
                {
                    Console.WriteLine("Command not recognised.");
                }
            }
        }

        private static void ListAccount(string name, Dictionary<string, Account> accounts)
        {
            if (accounts.ContainsKey(name))
            {
                Console.WriteLine(accounts[name].ToString());
            }
            else
            {
                Console.WriteLine("Account not found");
            }
        }

        private static void ListAll(Dictionary<string, Account> accounts)
        {
            foreach (var pair in accounts)
            {
                Console.WriteLine($"{pair.Key} has a balance of {pair.Value.GetBalance():C}");
            }
            Console.WriteLine();
        }

        private static Dictionary<string, Account> GenerateAccounts(string[] allData)
        {
            Dictionary<string, Account> accounts = new Dictionary<string, Account>();
            foreach (string transactionData in allData.Skip(1))
            {
                Transaction transaction = new Transaction(transactionData.Split(","));
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
            return accounts;
        }

        private static string[] GetTransactionData(string path)
        {
            try
            {
                return File.ReadAllLines(path);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return new string[0];
        }
    }
}
