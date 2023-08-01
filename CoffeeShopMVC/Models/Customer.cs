namespace CoffeeShopMVC.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<Order> Orders { get; set; }


        public string TotalSpent()
        {
            int sum = 0;

            foreach(var order in Orders)
            {
                foreach(var item in order.Items)
                {
                    sum += item.PriceInCents;
                }
            }
            return $"${Math.Round(Convert.ToDouble(sum) / 100,2)}";
        }

        public List<string> AllItems()
        {
            var allItems = new List<Item>();

            foreach(var order in Orders)
            {
                allItems.AddRange(order.Items);
            }

            return allItems.Select(i => i.Name).ToList();
        }
    }
}
