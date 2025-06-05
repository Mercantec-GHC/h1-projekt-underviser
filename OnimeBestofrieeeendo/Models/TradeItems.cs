namespace OnimeBestofrieeeendo.Models
{
    public class TradeItems

    {
        public int ShopItemId { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public bool Available { get; set; } = true;
        public string ItemName { get; set; }
        public string ItemType { get; set; }
        public string Rarity { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
    }

}
