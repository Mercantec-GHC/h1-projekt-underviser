// File: OnimeBestofrieeeendo/Components/Services/ShopService.cs
using Npgsql;
using OnimeBestofrieeeendo.Models;

namespace OnimeBestofrieeeendo.Components.Services
{
    public class ShopService
{
    private readonly string _connectionString;

    public ShopService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
    }

    private async Task<NpgsqlConnection> OpenConnectionAsync()
    {
        var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();
        return conn;
    }

    public async Task<List<ShopProductView>> LoadProductsAsync()
    {
        var products = new List<ShopProductView>();
        await using var conn = await OpenConnectionAsync();

        var cmd = new NpgsqlCommand(@"
            SELECT shop_items.shop_item_id, shop_items.item_id,
                   products.name, products.image_url, products.description,
                   shop_items.price, shop_items.quantity, shop_items.rarity
            FROM shop_items
            JOIN products ON shop_items.item_id = products.id
            WHERE shop_items.available = true", conn);

        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            products.Add(new ShopProductView
            {
                ShopItemId = reader.GetInt32(0),
                ItemId = reader.GetInt32(1),
                Name = reader.GetString(2),
                ImageUrl = reader.GetString(3),
                Description = reader.GetString(4),
                Price = reader.GetDecimal(5),
                Quantity = reader.GetInt32(6),
                Rarity = reader.GetString(7)
            });
        }

        return products;
    }

    public async Task<bool> BuyProductAsync(int userId, ShopProductView product)
    {
        await using var conn = await OpenConnectionAsync();

        var cmd = new NpgsqlCommand(@"
            UPDATE users SET balance = balance - @price WHERE id = @userId;
            UPDATE shop_items SET quantity = quantity - 1 WHERE shop_item_id = @shopItemId;
        ", conn);

        cmd.Parameters.AddWithValue("@price", product.Price);
        cmd.Parameters.AddWithValue("@userId", userId);
        cmd.Parameters.AddWithValue("@shopItemId", product.ShopItemId);

        try
        {
            var result = await cmd.ExecuteNonQueryAsync();
            return result > 1; // обе команды выполнились
        }
        catch (Exception)
        {
            return false;
        }
    }
}
}