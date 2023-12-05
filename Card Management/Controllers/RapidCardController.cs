using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Card_Management.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RapidCardController : ControllerBase
    {
        // Normally would prefer to put the into database
        // TODO replace with Entity Framework
        private static readonly List<RapidCard> rapidCards = new List<RapidCard>();

        [HttpPost("CreateCard")]
        public ActionResult<RapidCard> CreateCard()
        {
            RapidCard rc = new RapidCard(NewCardNumber());
            rapidCards.Add(rc);
            return Ok(rc);
        }

        private string NewCardNumber()
        {
            // There is probably a better way of doing this
            // But this will keep looking until we get a new number
            while (true)
            {
                string num = RandomDigits(15);

                //Making sure it is unique
                RapidCard? rc = rapidCards.FirstOrDefault(c => c.CardNumber == num);
                if (rc == null) return num;
            }
        }

        private string RandomDigits(int length)
        {
            var random = new Random();
            string s = string.Empty;
            for (int i = 0; i < length; i++)
                s = String.Concat(s, random.Next(10).ToString());
            return s;
        }

        [HttpPost("MakePayment")]
        public ActionResult MakePayment(string cardNumber, decimal amount)
        {
            RapidCard? rc = rapidCards.FirstOrDefault(c => c.CardNumber == cardNumber);
            if (rc == null) return NotFound("Please, verify card's number");

            rc.Balance += amount;

            return Ok(rc.Balance);
        }

        [HttpPost("GetBalence")]
        public ActionResult<string> GetBalence(string cardNumber)
        {
            RapidCard? rc = rapidCards.FirstOrDefault(c => c.CardNumber == cardNumber);
            return rc == null ? NotFound("Please, verify card's number") : Ok(rc.Balance);
        }
    }
}
