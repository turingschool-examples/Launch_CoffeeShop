namespace CoffeeShopMVC.Models
{
    public class Order
    {
        public int Id { get; set; }
        public Customer OrderCustomer { get; set; }
        public List<Item> ListOfItems { get; set; }
        public string DateCreated { get; set; }
    }
}
