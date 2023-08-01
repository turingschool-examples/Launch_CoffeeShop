namespace CoffeeShopMVC.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public Customer Customer { get; set; }
        public List<Item> Items { get; set; } = new List<Item>();
    }
}
