namespace RestGateway.Models.DTOs.Responses;

/// <summary>
/// Product item in order response
/// </summary>
public class ProductItemResponseDto
{
    /// <summary>
    /// Product identifier
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// Quantity of the product
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Unit price of the product
    /// </summary>
    public decimal UnitPrice { get; set; }
}
