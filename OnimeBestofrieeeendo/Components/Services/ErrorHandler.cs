using Microsoft.JSInterop;

namespace OnimeBestofrieeeendo.Components.Services
{
    /// <summary>
    /// Simple error handler for students - handles errors in one place
    /// </summary>
    public class ErrorHandler
    {
        private readonly IJSRuntime _jsRuntime;

        public ErrorHandler(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        /// <summary>
        /// Logs error to console and shows popup to user
        /// </summary>
        public async Task HandleErrorAsync(Exception ex, string context = "")
        {
            // Log to console for developers
            var errorMessage = $"Error in {context}: {ex.Message}";
            Console.WriteLine(errorMessage);
            
            // Show user-friendly message
            await ShowUserErrorAsync("Something went wrong. Please try again.");
        }

        /// <summary>
        /// Logs error to console only (no popup)
        /// </summary>
        public static void LogError(Exception ex, string context = "")
        {
            var errorMessage = string.IsNullOrEmpty(context) 
                ? $"Error: {ex.Message}" 
                : $"Error in {context}: {ex.Message}";
            
            Console.WriteLine(errorMessage);
        }

        /// <summary>
        /// Shows error popup to user
        /// </summary>
        public async Task ShowUserErrorAsync(string message)
        {
            await _jsRuntime.InvokeVoidAsync("showFunnyPopup", "Error", message);
        }

        /// <summary>
        /// Shows success popup to user
        /// </summary>
        public async Task ShowSuccessAsync(string message)
        {
            await _jsRuntime.InvokeVoidAsync("showFunnyPopup", "Success!", message);
        }

        /// <summary>
        /// Handles database connection errors specifically
        /// </summary>
        public async Task HandleDatabaseErrorAsync(Exception ex, string operation = "")
        {
            LogError(ex, $"Database operation: {operation}");
            await ShowUserErrorAsync("Database connection failed. Please check your connection and try again.");
        }

        /// <summary>
        /// Handles service errors (like API calls)
        /// </summary>
        public async Task HandleServiceErrorAsync(Exception ex, string serviceName = "")
        {
            LogError(ex, $"Service: {serviceName}");
            await ShowUserErrorAsync("Service is temporarily unavailable. Please try again later.");
        }
    }
}
