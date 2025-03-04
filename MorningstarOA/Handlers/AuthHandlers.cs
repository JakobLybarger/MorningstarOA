using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MorningstarOA.Models;

namespace MorningstarOA.Handlers;

public static class AuthHandlers
{
    public static IResult Login(
        [FromBody] LoginRequest request, 
        [FromServices] TokenGenerator tokenGenerator,
        [FromServices] IOptions<AuthOptions> authOptions,
        [FromServices] ILogger<AuthLogging> logger)
    {
        logger.LogInformation("Login attempt for user: {Email}", request.Email);

        if (request.Email != "admin@morningstar.com" || request.Password != "password")
        {
            logger.LogWarning("Failed login attempt for user: {Email}", request.Email);
            return Results.Unauthorized();
        }

        logger.LogInformation("Successful login for user: {Email}", request.Email);
        return Results.Ok(new
        {
            token = tokenGenerator.GenerateToken(request.Email, authOptions.Value)
        });
    }
}

// Non-static class for logger categorization
public class AuthLogging {}