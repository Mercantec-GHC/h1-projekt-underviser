# ErrorHandler - Упрощенная обработка ошибок


`ErrorHandler` is a service that helps you handle errors in one place instead of writing the same code everywhere.

**Теперь (просто):**
```csharp
try
{
    // code
}
catch (Exception ex)
{
    await ErrorHandler.HandleErrorAsync(ex, "LoadUserProfile");
}
```



В компонентах (Razor страницы)

```csharp
@inject ErrorHandler ErrorHandler

// В методах:
try
{
    await SomeService.DoSomething();
    await ErrorHandler.ShowSuccessAsync("Success!");
}
catch (Exception ex)
{
    await ErrorHandler.HandleErrorAsync(ex, "DoSomething");
}
```

### 2. В сервисах

```csharp
public async Task SaveData(Data data)
{
    try
    {
        // save data
    }
    catch (Exception ex)
    {
        ErrorHandler.LogError(ex, "SaveData");
        throw; // we pass the error
    }
}
```

## ErrorHandler Methods

### `HandleErrorAsync(Exception ex, string context)`
- Logs the error to the console  
- Shows a user-friendly message

### `ShowSuccessAsync(string message)`
- Shows a success message

### `ShowUserErrorAsync(string message)`
- Shows a user error message

### `LogError(Exception ex, string context)` (static)
- Logs to the console only, no popup

### `HandleDatabaseErrorAsync(Exception ex, string operation)`
- Specifically for database errors

### `HandleServiceErrorAsync(Exception ex, string serviceName)`
- Specifically for service errors

