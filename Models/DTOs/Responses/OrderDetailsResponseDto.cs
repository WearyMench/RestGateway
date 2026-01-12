namespace RestGateway.Models.DTOs.Responses;

/// <summary>
/// Response DTO for order details
/// </summary>
public class OrderDetailsResponseDto
{
    /// <summary>
    /// Order identifier
    /// </summary>
    public int OrderId { get; set; }

    /// <summary>
    /// Client identifier
    /// </summary>
    public int ClientId { get; set; }

    /// <summary>
    /// List of products in the order
    /// </summary>
    public List<ProductItemResponseDto> Products { get; set; } = new();

    /// <summary>
    /// Delivery address
    /// </summary>
    public AddressResponseDto Address { get; set; } = new();

    /// <summary>
    /// Current order status
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Order creation date and time
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Order financial details
    /// </summary>
    public OrderFinancialDetailsDto FinancialDetails { get; set; } = new();
}
