namespace RestGateway.Models.DTOs.Responses;

/// <summary>
/// Response DTO for order creation
/// </summary>
public class CreateOrderResponseDto
{
    /// <summary>
    /// Created order identifier
    /// </summary>
    public int OrderId { get; set; }

    /// <summary>
    /// Current order status
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Order creation date and time
    /// </summary>
    public DateTime CreatedDate { get; set; }
}
