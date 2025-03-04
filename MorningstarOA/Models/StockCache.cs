namespace MorningstarOA.Models;

public class StockCache
{
    public IEnumerable<Stock> Stocks { get; set; } = new List<Stock>();
}