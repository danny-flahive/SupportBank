using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportBank
{
    class Account
    {
        public string Name;
        private List<List<string>> transactions;
        private decimal balance;

        public Account(string name)
        {
            this.Name = name;
            balance = 0;
            transactions = new List<List<string>>();
        }

        public void UpdateAccount(List<string> transaction)
        {
            if (transaction[1] == Name)
            {
                balance -= decimal.Parse(transaction[4]);
            }
            else if (transaction[2] == Name)
            {
                balance += decimal.Parse(transaction[4]);
            }
            else
            {
                //Should never be reached, here to protect against later changes to Main
                Console.WriteLine("Incorrect account");
                return;
            }
            transactions.Add(transaction);
        }

        public decimal GetBalance()
        {
            return this.balance;
        }

        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Account account = (Account)obj;
                return Name.Equals(account.Name);
            }
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (List<string> transaction in transactions)
            {
                stringBuilder.Append($"On {transaction[0]}, {transaction[1]} borrowed {transaction[4]} from " +
                    $"{transaction[2]} for {transaction[3]}\n");
            }
            return stringBuilder.ToString();
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name);
        }
    }
}
