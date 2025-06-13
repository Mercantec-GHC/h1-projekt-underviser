using Npgsql;
using OnimeBestofrieeeendo.Models;

namespace OnimeBestofrieeeendo.Components.Services
{
    // Класс для работы с магазином
    public class ShopService(IConfiguration configuration)
    {
        // Строка подключения к базе данных
        private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";

        // SQL-запрос для получения товаров
        private const string LoadProductsSql = @"
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

        // SQL-запрос для списания денег у пользователя
        private const string UpdateUserBalanceSql = @"
            UPDATE users SET balance = balance - @price 
            WHERE id = @userId AND balance >= @price";

        // SQL-запрос для уменьшения количества товара
        private const string UpdateProductQuantitySql = @"
            UPDATE shop_items SET quantity = quantity - 1 
            WHERE shop_item_id = @shopItemId AND quantity > 0";

        // Получаем список товаров из базы
        public async Task<List<ShopProductView>> LoadProductsAsync()
        {
            var products = new List<ShopProductView>();

            // Открываем соединение с базой
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            // Готовим и выполняем запрос
            await using var cmd = new NpgsqlCommand(LoadProductsSql, conn);
            await using var reader = await cmd.ExecuteReaderAsync();

            // Читаем все товары по очереди
            while (await reader.ReadAsync())
            {
                products.Add(new ShopProductView
                {
                    ShopItemId = reader.GetInt32(0), // id товара в магазине
                    ItemId = reader.GetInt32(1),     // id самого предмета
                    Name = reader.GetString(2),      // название
                    ImageUrl = reader.GetString(3),  // картинка
                    Description = reader.GetString(4), // описание
                    Price = reader.GetDecimal(5),    // цена
                    Quantity = reader.GetInt32(6),   // сколько осталось
                    Rarity = reader.GetString(7)     // редкость
                });
            }

            // Возвращаем список товаров
            return products;
        }

        // Покупка товара пользователем
        public async Task<bool> BuyProductAsync(int userId, ShopProductView product)
        {
            // Открываем соединение с базой
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            // Начинаем транзакцию (чтобы всё прошло вместе)
            await using var transaction = await conn.BeginTransactionAsync();

            // 1. Списываем деньги у пользователя
            var userCmd = new NpgsqlCommand(UpdateUserBalanceSql, conn, transaction);
            userCmd.Parameters.AddWithValue("@price", product.Price);
            userCmd.Parameters.AddWithValue("@userId", userId);
            var userUpdateResult = await userCmd.ExecuteNonQueryAsync();

            if (userUpdateResult == 0)
            {
                // Если денег не хватает, отменяем всё
                await transaction.RollbackAsync();
                return false;
            }

            // 2. Уменьшаем количество товара на 1
            var productCmd = new NpgsqlCommand(UpdateProductQuantitySql, conn, transaction);
            productCmd.Parameters.AddWithValue("@shopItemId", product.ShopItemId);
            var productUpdateResult = await productCmd.ExecuteNonQueryAsync();

            if (productUpdateResult == 0)
            {
                // Если товара нет, тоже отменяем всё
                await transaction.RollbackAsync();
                return false;
            }

            // 3. Всё прошло успешно, сохраняем изменения
            await transaction.CommitAsync();
            return true;
        }
    }
}