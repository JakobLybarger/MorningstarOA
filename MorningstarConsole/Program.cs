
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using MorningstarConsole;

const string BaseUrl = "http://localhost:5221/";
using var client = new HttpClient();

Console.WriteLine("Input Username: ");
var email = Console.ReadLine();
Console.WriteLine("Input Password: ");
var password = Console.ReadLine();

var request = new 
{
    email,
    password
};

LoginResponse? loginResponse = null;
try
{
    var json = JsonSerializer.Serialize(request);
    var content = new StringContent(json, Encoding.UTF8, "application/json");
    var response = await client.PostAsync(BaseUrl + "login", content);
    var responseString = await response.Content.ReadAsStringAsync();
    if (!response.IsSuccessStatusCode)
    {
        throw new Exception(responseString);
    }
    
    loginResponse = JsonSerializer.Deserialize<LoginResponse>(responseString, JsonSerializerConstants.CaseInsensitive)
        ?? throw new Exception("Error deserializing login response");
}
catch (Exception ex) {
    Console.WriteLine("Login Failed: " + ex.Message);
    Environment.Exit(1);
}

Console.WriteLine("Login Success!\n");

client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResponse?.Token);
while (true)
{
    Console.WriteLine("Input your query:");
    var query = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(query))
    {
        Console.WriteLine("Please Enter a valid query");
        continue;
    }

    try {
        var response = await client.GetAsync($"{BaseUrl}search?query={query}");
        var responseContent = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(responseContent);
        }
        
        var stocks = JsonSerializer.Deserialize<List<Stock>>(responseContent, JsonSerializerConstants.CaseInsensitive);
        PrintStocks(stocks);
    }
    catch (Exception ex)
    {
        Console.WriteLine("An exception occured: " + ex.Message);
        Environment.Exit(1);
    }
}


void PrintStocks(List<Stock> stocks)
{
    Console.WriteLine("Result:");
    foreach (var stock in stocks)
    {
        Console.WriteLine($"{stock.Name}|{stock.Ticker}");
    }

    Console.WriteLine();
}