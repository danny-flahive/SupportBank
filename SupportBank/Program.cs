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
            string[] transactions = GetTransactionData(path);
            Dictionary<string, Account> accounts = GenerateAccounts(transactions);
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
        }

        private static Dictionary<string, Account> GenerateAccounts(string[] data)
        {
            Dictionary<string, Account> accounts = new Dictionary<string, Account>();
            foreach (string transaction in data.Skip(1))
            {
                List<string> transactionBreakdown = transaction.Split(",").ToList();
                string fromPerson = transactionBreakdown[1];
                string toPerson = transactionBreakdown[2];
                if (!accounts.ContainsKey(fromPerson))
                {
                    accounts[fromPerson] = new Account(fromPerson);
                }
                if (!accounts.ContainsKey(toPerson))
                {
                    accounts[toPerson] = new Account(toPerson);
                }
                accounts[fromPerson].UpdateAccount(transactionBreakdown);
                accounts[toPerson].UpdateAccount(transactionBreakdown);
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
                Environment.Exit(0);
            }
            return new string[0];
        }
    }
}
