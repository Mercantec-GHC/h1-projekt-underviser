using Npgsql;
using OnimeBestofrieeeendo.Models;

namespace OnimeBestofrieeeendo.Services

{
    public partial class DbService
    {
        private readonly string _connectionString;

        public DbService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<TradeItems>> GetShopItemsAsync()
        {
            var items = new List<TradeItems>();

            const string sql = @"
                SELECT 
                    shop.shop_item_id,
                    shop.price,
                    shop.quantity,
                    items.item_name,
                    items.item_type,
                    items.rarity,
                    items.image_url
                FROM shop
                JOIN items ON shop.item_id = items.item_id
                WHERE shop.available = TRUE
                ORDER BY shop.shop_item_id;";

            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            using var cmd = new NpgsqlCommand(sql, connection);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var item = new TradeItems
                {
                    ShopItemId = reader.GetInt32(reader.GetOrdinal("shop_item_id")),
                    Price = reader.GetInt32(reader.GetOrdinal("price")),
                    Quantity = reader.GetInt32(reader.GetOrdinal("quantity")),
                    ItemName = reader.GetString(reader.GetOrdinal("item_name")),
                    ItemType = reader.GetString(reader.GetOrdinal("item_type")),
                    Rarity = reader.GetString(reader.GetOrdinal("rarity")),
                    ImageUrl = reader.IsDBNull(reader.GetOrdinal("image_url"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("image_url"))
                };

                items.Add(item);
            }

            return items;
        }


        public async Task<TradeItems?> GetItemDetailsAsync(int id)
        {
            const string sql = @"
        SELECT 
            shop.shop_item_id,
            shop.price,
            shop.quantity,
            shop.available,
            items.item_name,
            items.item_type,
            items.rarity,
            items.image_url,
            items.description
        FROM shop
        JOIN items ON shop.item_id = items.item_id
        WHERE shop.item_id = @id;";

            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            using var cmd = new NpgsqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                var item = new TradeItems
                {
                    ShopItemId = reader.GetInt32(reader.GetOrdinal("shop_item_id")),
                    Price = reader.GetInt32(reader.GetOrdinal("price")),
                    Quantity = reader.GetInt32(reader.GetOrdinal("quantity")),
                    Available = reader.GetBoolean(reader.GetOrdinal("available")),
                    ItemName = reader.GetString(reader.GetOrdinal("item_name")),
                    ItemType = reader.GetString(reader.GetOrdinal("item_type")),
                    Rarity = reader.GetString(reader.GetOrdinal("rarity")),
                    ImageUrl = reader.IsDBNull(reader.GetOrdinal("image_url"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("image_url")),
                    Description = reader.IsDBNull(reader.GetOrdinal("description"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("description"))
                };

                return item;
            }

            return null;
        }


    }
}
