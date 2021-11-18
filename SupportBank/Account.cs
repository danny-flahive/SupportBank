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
        private List<Transaction> transactions;
        private decimal balance;

        public Account(string name)
        {
            this.Name = name;
            balance = 0;
            transactions = new List<Transaction>();
        }

        public void UpdateAccount(Transaction transaction)
        {
            if (transaction.fromAccount == Name)
            {
                balance -= transaction.amount;
            }
            else if (transaction.toAccount == Name)
            {
                balance += transaction.amount;
            }
            else
            {
                //Should never be reached, here to protect against later changes to Main
                Console.WriteLine("Incorrect Account");
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
                //Assuming name is a unique identifier in this context, this is fine to use
                //Would need to change in the case that names were no longer unique
                Account account = (Account)obj;
                return Name.Equals(account.Name);
            }
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (Transaction transaction in transactions)
            {
                stringBuilder.Append(transaction.ToString());
            }
            stringBuilder.Append($"Overall, {Name} {(balance > 0 ? $"is owed {balance:C}" : $"owes {(balance * -1):C}")}\n");
            return stringBuilder.ToString();
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name);
        }
    }
}
