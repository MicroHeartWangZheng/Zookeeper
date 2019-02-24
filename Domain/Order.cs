namespace Domain
{
    public class Order
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public string Goods { get; set; }

        public string Address { get; set; }

        public Custormer Custormer;
    }
}
