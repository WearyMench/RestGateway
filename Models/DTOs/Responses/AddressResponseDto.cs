namespace RestGateway.Models.DTOs.Responses;

/// <summary>
/// Address information in response
/// </summary>
public class AddressResponseDto
{
    /// <summary>
    /// Street address
    /// </summary>
    public string Street { get; set; } = string.Empty;

    /// <summary>
    /// City name
    /// </summary>
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// State or province
    /// </summary>
    public string State { get; set; } = string.Empty;

    /// <summary>
    /// ZIP or postal code
    /// </summary>
    public string ZipCode { get; set; } = string.Empty;

    /// <summary>
    /// Country name
    /// </summary>
    public string Country { get; set; } = string.Empty;
}
