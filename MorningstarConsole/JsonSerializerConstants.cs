using System.Text.Json;

namespace MorningstarConsole;

public static class JsonSerializerConstants
{
    public static JsonSerializerOptions CaseInsensitive => new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
    };
}