using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeeCalculator
{
    // I am probably doing this wrong, or at least slopily.  Would love to see it done properly
    public sealed class UniversalFeesExchange
    {
        private static UniversalFeesExchange instance = null;
        private static readonly object padlock = new object();
        private static readonly Random Rnd = new Random();
        public readonly decimal Fee = (decimal)(Rnd.NextDouble() * 2);

        UniversalFeesExchange()
        {
        }

        public static UniversalFeesExchange Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new UniversalFeesExchange();
                    }
                    return instance;
                }
            }
        }
    }
}
