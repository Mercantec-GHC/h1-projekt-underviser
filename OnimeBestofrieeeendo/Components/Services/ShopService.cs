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

        public async Task<List<ShopProductView>> LoadProductsAsync()
        {
            var products = new List<ShopProductView>();
            try
            {
                await using var conn = new NpgsqlConnection(_connectionString);
                await conn.OpenAsync();

                var sql = @"
    SELECT shop_items.shop_item_id,
           shop_items.item_id,
           products.name,
           products.image_url,
           products.description,
           shop_items.price,
           shop_items.quantity, 
           shop_items.rarity
    FROM shop_items
    JOIN products ON shop_items.item_id = products.id
    WHERE shop_items.available = true";

                await using var cmd = new NpgsqlCommand(sql, conn);
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
            }
            catch (Exception)
            {
                // Return empty list on error
            }
            return products;
        }

        public async Task<bool> BuyProductAsync(int userId, ShopProductView product)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var transaction = await conn.BeginTransactionAsync();

            try
            {
                // Обновляем баланс пользователя
                var userCmd = new NpgsqlCommand("UPDATE users SET balance = balance - @price WHERE id = @userId AND balance >= @price", conn, transaction);
                userCmd.Parameters.AddWithValue("@price", product.Price);
                userCmd.Parameters.AddWithValue("@userId", userId);
                var userUpdateResult = await userCmd.ExecuteNonQueryAsync();

                if (userUpdateResult == 0)
                {
                    // Баланса недостаточно, откатываем транзакцию
                    await transaction.RollbackAsync();
                    return false;
                }

                // Уменьшаем количество товара, если есть в наличии
                var productCmd = new NpgsqlCommand("UPDATE shop_items SET quantity = quantity - 1 WHERE shop_item_id = @shopItemId AND quantity > 0", conn, transaction);
                productCmd.Parameters.AddWithValue("@shopItemId", product.ShopItemId);
                var productUpdateResult = await productCmd.ExecuteNonQueryAsync();

                if (productUpdateResult == 0)
                {
                    // Товара нет, откатываем транзакцию
                    await transaction.RollbackAsync();
                    return false;
                }

                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }
    }
}