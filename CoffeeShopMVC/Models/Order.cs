namespace CoffeeShopMVC.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public Customer Customer { get; set; }
        public List<Item> Items { get; set; } = new List<Item>();

        public int TotalPrice()
        {
            int sum = 0;
            foreach (var item in Items)
            {
                sum += item.PriceInCents;
            }
            return sum;
        }
    }
}
