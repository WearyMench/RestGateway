namespace RestGateway.Models.DTOs.Responses;

/// <summary>
/// Financial details of an order
/// </summary>
public class OrderFinancialDetailsDto
{
    /// <summary>
    /// Subtotal amount
    /// </summary>
    public decimal Subtotal { get; set; }

    /// <summary>
    /// Taxes amount
    /// </summary>
    public decimal Taxes { get; set; }

    /// <summary>
    /// Discount amount
    /// </summary>
    public decimal Discount { get; set; }

    /// <summary>
    /// Shipping cost
    /// </summary>
    public decimal Shipping { get; set; }

    /// <summary>
    /// Total amount
    /// </summary>
    public decimal Total { get; set; }
}
