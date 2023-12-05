namespace Card_Management
{
    public class RapidCard
    {
        public RapidCard(string cardNumber)
        {
            CardNumber = cardNumber;
            Balance = 0;
        }
        public string? CardNumber { get; set; }
        public decimal Balance { get; set; }
    }
}
