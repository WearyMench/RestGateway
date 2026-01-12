using System.ComponentModel.DataAnnotations;

namespace RestGateway.Models.DTOs.Requests;

/// <summary>
/// Address information
/// </summary>
public class AddressDto
{
    /// <summary>
    /// Street address
    /// </summary>
    [Required]
    [StringLength(200, ErrorMessage = "Street cannot exceed 200 characters")]
    public string Street { get; set; } = string.Empty;

    /// <summary>
    /// City name
    /// </summary>
    [Required]
    [StringLength(100, ErrorMessage = "City cannot exceed 100 characters")]
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// State or province
    /// </summary>
    [Required]
    [StringLength(100, ErrorMessage = "State cannot exceed 100 characters")]
    public string State { get; set; } = string.Empty;

    /// <summary>
    /// ZIP or postal code
    /// </summary>
    [Required]
    [StringLength(20, ErrorMessage = "ZipCode cannot exceed 20 characters")]
    public string ZipCode { get; set; } = string.Empty;

    /// <summary>
    /// Country name
    /// </summary>
    [Required]
    [StringLength(100, ErrorMessage = "Country cannot exceed 100 characters")]
    public string Country { get; set; } = string.Empty;
}
