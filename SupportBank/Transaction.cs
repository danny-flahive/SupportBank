using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportBank
{
    struct Transaction
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        public DateTime date { get; }
        public string fromAccount { get; }
        public string toAccount { get; }
        public string description { get; }
        public decimal amount { get; }

        public Transaction(IList<string> data)
        {
            date = DateTime.Parse(data[0]);
            fromAccount = data[1];
            toAccount = data[2];
            description = data[3];
            amount = decimal.Parse(data[4]);
        }

        public override string ToString()
        {
            return $"On {date.ToShortDateString()}, {fromAccount} borrowed {amount:C} from {toAccount} for {description}\n";
        }
    }
}
