namespace CoffeeShopMVC.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PriceInCents { get; set; }


        public string PriceInDollars()
        {
            return $"${Math.Round(Convert.ToDouble(PriceInCents) / 100, 2)}";
        }

    }
}
