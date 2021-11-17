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
            string path = @"C:\Users\Danny.Flahive\Corndel\SupportBank\SupportBank\Transactions2014.csv";
            string[] transactions = File.ReadAllLines(path);
            //string[] headers = transactions[0].Split(",");
            Dictionary<string, decimal> balances = new Dictionary<string, decimal>();
            foreach (string transaction in transactions.Skip(1))
            {
                string[] transactionBreakdown = transaction.Split(",");
                DateTime date = DateTime.Parse(transactionBreakdown[0]);
                string fromPerson = transactionBreakdown[1];
                string toPerson = transactionBreakdown[2];
                string detail = transactionBreakdown[3];
                decimal amount = decimal.Parse(transactionBreakdown[4]);
                balances[fromPerson] = balances.ContainsKey(fromPerson) ? balances[fromPerson] - amount : 0 - amount;
                balances[toPerson] = balances.ContainsKey(toPerson) ? balances[toPerson] + amount : 0 + amount;
            }
            foreach (var pair in balances)
            { 
                Console.WriteLine($"{pair.Key} has a balance of {pair.Value:C}");
            }
        }
    }
}
