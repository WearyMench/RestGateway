namespace RestGateway.Models.DTOs.Responses;

/// <summary>
/// Response DTO for order total calculation
/// </summary>
public class CalculateOrderTotalResponseDto
{
    /// <summary>
    /// Order identifier
    /// </summary>
    public int OrderId { get; set; }

    /// <summary>
    /// Financial breakdown
    /// </summary>
    public OrderFinancialDetailsDto FinancialDetails { get; set; } = new();
}
