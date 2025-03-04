using Microsoft.AspNetCore.Mvc;
using MorningstarOA.Models;

namespace MorningstarOA.Handlers;

public static class SearchHandlers
{
    public static IResult Search(
        [FromQuery] string query, 
        [FromServices] StockCache cache, 
        [FromServices] ILogger<SearchLogging> logger)
    {
        logger.LogInformation("Search query: {query}", query);
        
        if (string.IsNullOrWhiteSpace(query))
        {
            logger.LogInformation("Returning all stocks because query was empty");
            return Results.Ok(cache);
        }
        
        var filteredStocks = cache.Stocks
            .Where(s => s.Name.Contains(query, StringComparison.OrdinalIgnoreCase)
                || s.Ticker.Contains(query, StringComparison.OrdinalIgnoreCase)
                || s.Exchange.Contains(query, StringComparison.OrdinalIgnoreCase))
            .ToList();
        
        logger.LogInformation("Returning {count} stocks for {query}", filteredStocks.Count, query);
        
        return Results.Ok(filteredStocks);
    }
}
// Non-static class for logger categorization
public class SearchLogging {}
