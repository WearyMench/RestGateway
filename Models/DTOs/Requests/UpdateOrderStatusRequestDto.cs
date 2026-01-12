using System.ComponentModel.DataAnnotations;

namespace RestGateway.Models.DTOs.Requests;

/// <summary>
/// Request DTO for updating order status
/// </summary>
public class UpdateOrderStatusRequestDto
{
    /// <summary>
    /// Order identifier
    /// </summary>
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "OrderId must be greater than 0")]
    public int OrderId { get; set; }

    /// <summary>
    /// New status for the order
    /// </summary>
    [Required]
    public string NewStatus { get; set; } = string.Empty;
}
