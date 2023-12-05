using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace FeeCalculator
{
    public partial class Service1 : ServiceBase
    {
        // Normally would prefer to put the into database
        // TODO replace with Entity Framework
        private static readonly List<RapidCard> rapidCards = new List<RapidCard>();
        private static decimal fee = 2;  // got to start somewhere, went with 2

        private System.Timers.Timer _timer;

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            //one hour in milliseconds 60 minutes in hour, 60 seconds in minute, 1000 milliseconds in a second
            _timer = new System.Timers.Timer(60 * 60 * 1000); 

            //Will now run every hour
            _timer.Elapsed += _timer_Elapsed;
            _timer.Start();
        }

        void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //gets the new fee
            decimal newMultiplier = UniversalFeesExchange.Instance.Fee;

            //To get the new fee, multiply by the UFE new fee
            fee *= newMultiplier;

            //Since fee is not changing, lets multithread this
            Parallel.ForEach(rapidCards, card =>
            {
                card.Balance += fee;
            });
        }

        protected override void OnStop()
        {
        }
    }
}
