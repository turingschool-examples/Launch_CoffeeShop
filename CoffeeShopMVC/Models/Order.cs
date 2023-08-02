using CoffeeShopMVC.Controllers;

namespace CoffeeShopMVC.Models
{
    public class Order
    {
        public int Id { get; set; }
        public Customer OrderCustomer { get; set; }
        public List<Item>? ListOfItems { get; set; } = new List<Item>();
        public string DateCreated { get; set; }

       
        
        public int GetAllItemsPrice() 
        
        {
            int Total = 0;
            foreach (var item in ListOfItems) 
            {
                Total  += item.PriceInCents;
            }
            
            return Total;
        }
    }
}
