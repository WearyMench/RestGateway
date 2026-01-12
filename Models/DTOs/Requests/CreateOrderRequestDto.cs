using System.ComponentModel.DataAnnotations;

namespace RestGateway.Models.DTOs.Requests;

/// <summary>
/// Request DTO for creating a new order
/// </summary>
public class CreateOrderRequestDto
{
    /// <summary>
    /// Client identifier
    /// </summary>
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "ClientId must be greater than 0")]
    public int ClientId { get; set; }

    /// <summary>
    /// List of products in the order
    /// </summary>
    [Required]
    [MinLength(1, ErrorMessage = "At least one product is required")]
    public List<ProductItemDto> Products { get; set; } = new();

    /// <summary>
    /// Delivery address
    /// </summary>
    [Required]
    public AddressDto Address { get; set; } = new();
}
