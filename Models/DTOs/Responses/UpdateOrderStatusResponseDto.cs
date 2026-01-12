namespace RestGateway.Models.DTOs.Responses;

/// <summary>
/// Response DTO for order status update
/// </summary>
public class UpdateOrderStatusResponseDto
{
    /// <summary>
    /// Order identifier
    /// </summary>
    public int OrderId { get; set; }

    /// <summary>
    /// Previous order status
    /// </summary>
    public string PreviousStatus { get; set; } = string.Empty;

    /// <summary>
    /// New order status
    /// </summary>
    public string NewStatus { get; set; } = string.Empty;

    /// <summary>
    /// Indicates if the update was successful
    /// </summary>
    public bool Success { get; set; }
}
