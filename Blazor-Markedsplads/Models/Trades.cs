namespace BlazorMarkedsplads.Models
{
    public class Trades
    {
        public int Trade_id { get; set; }
        public int Seller_id { get; set; }
        public int Buyer_id { get; set; }
        public int Item_id { get; set; }
        public int price { get; set; }
        public DateTime TradeDate { get; set; } = DateTime.Today;
    }
}
