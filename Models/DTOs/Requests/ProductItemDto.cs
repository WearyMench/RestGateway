using System.ComponentModel.DataAnnotations;

namespace RestGateway.Models.DTOs.Requests;

/// <summary>
/// Product item in an order
/// </summary>
public class ProductItemDto
{
    /// <summary>
    /// Product identifier
    /// </summary>
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "ProductId must be greater than 0")]
    public int ProductId { get; set; }

    /// <summary>
    /// Quantity of the product
    /// </summary>
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
    public int Quantity { get; set; }

    /// <summary>
    /// Unit price of the product
    /// </summary>
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "UnitPrice must be greater than 0")]
    public decimal UnitPrice { get; set; }
}
