namespace MS.Pix.MED.Integration.Jdpi.Configuration;

public class JdpiConfiguration
{
    public const string SectionName = "JdpiApi";

    public string BaseUrl { get; set; } = string.Empty;

    public string AuthUrl { get; set; } = string.Empty;

    public string ClientId { get; set; } = string.Empty;

    public string ClientSecret { get; set; } = string.Empty;

    public string? Scope { get; set; }

    public int TimeoutSeconds { get; set; } = 30;

    public bool EnableTokenCache { get; set; } = true;

    public int TokenCacheMinutes { get; set; } = 50;

    public Dictionary<string, string>? DefaultHeaders { get; set; }
}