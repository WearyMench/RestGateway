using RestGateway.Models.DTOs.Requests;
using RestGateway.Models.DTOs.Responses;

namespace RestGateway.Services;

/// <summary>
/// Service interface for order operations
/// </summary>
public interface IOrderService
{
    /// <summary>
    /// Creates a new order
    /// </summary>
    Task<CreateOrderResponseDto> CreateOrderAsync(CreateOrderRequestDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets order details by order ID
    /// </summary>
    Task<OrderDetailsResponseDto> GetOrderDetailsAsync(int orderId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Calculates the total for an order
    /// </summary>
    Task<CalculateOrderTotalResponseDto> CalculateOrderTotalAsync(int orderId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the status of an order
    /// </summary>
    Task<UpdateOrderStatusResponseDto> UpdateOrderStatusAsync(UpdateOrderStatusRequestDto request, CancellationToken cancellationToken = default);
}
