﻿namespace BlazorMarkedsplads.Models
{
    public class Shop
    {
        public int ShopItemId { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public bool Available { get; set; }
        public string ItemName { get; set; }
        public string ItemType { get; set; }
        public string Rarity { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
    }

}
