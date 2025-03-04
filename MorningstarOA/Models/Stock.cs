namespace MorningstarOA.Models;

public class Stock
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Ticker { get; set; } = string.Empty;
    public string Exchange { get; set; } = string.Empty;
}