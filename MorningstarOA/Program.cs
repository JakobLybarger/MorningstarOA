using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MorningstarOA;
using MorningstarOA.Handlers;
using MorningstarOA.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.AddEventSourceLogger();

var json = File.ReadAllText("data.json");
var stocks = new StockCache { Stocks = JsonSerializer.Deserialize<List<Stock>>(json,
    new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
    }) ?? new List<Stock>() };
builder.Services.AddSingleton(stocks); // Thinking of this in a similar sense to dbContext on how I consume it
    
builder.Services.AddSingleton<TokenGenerator>();

builder.Services.AddOptions<AuthOptions>()
    .Bind(builder.Configuration.GetSection("Jwt"));

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(x =>
    {
        x.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateIssuerSigningKey = true,
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = true
        };
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/login", AuthHandlers.Login)
    .WithName("Login")
    .WithOpenApi();

app.MapGet("/search", SearchHandlers.Search)
    .RequireAuthorization()
    .WithName("Search")
    .WithOpenApi();

app.Run();
