namespace RestGateway.Models.DTOs.Responses;

/// <summary>
/// Standard API error response
/// </summary>
public class ApiErrorResponseDto
{
    /// <summary>
    /// Error type or code
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Error title
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// HTTP status code
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// Detailed error message
    /// </summary>
    public string Detail { get; set; } = string.Empty;

    /// <summary>
    /// Request trace identifier
    /// </summary>
    public string? TraceId { get; set; }

    /// <summary>
    /// Additional error details
    /// </summary>
    public Dictionary<string, object>? Extensions { get; set; }
}
