using System.ServiceModel;
using RestGateway.Exceptions;
using RestGateway.Mappings;
using RestGateway.Models.DTOs.Requests;
using RestGateway.Models.DTOs.Responses;
using RestGateway.OrderService;
using Microsoft.Extensions.Logging;

namespace RestGateway.Services;

/// <summary>
/// Service implementation for order operations
/// </summary>
public class OrderService : IOrderService
{
    private readonly OrderServiceClientFactory _clientFactory;
    private readonly ILogger<OrderService> _logger;

    public OrderService(OrderServiceClientFactory clientFactory, ILogger<OrderService> logger)
    {
        _clientFactory = clientFactory;
        _logger = logger;
    }

    public async Task<CreateOrderResponseDto> CreateOrderAsync(CreateOrderRequestDto request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating order for client {ClientId}", request.ClientId);

        var client = _clientFactory.CreateClient();
        try
        {
            var soapRequest = request.ToSoapRequest();
            var soapResponse = await client.CreateOrderAsync(soapRequest);

            _logger.LogInformation("Order created successfully. OrderId: {OrderId}", soapResponse.OrderId);

            return soapResponse.ToDto();
        }
        catch (FaultException ex)
        {
            _logger.LogWarning(ex, "SOAP fault occurred while creating order for client {ClientId}: {Message}", 
                request.ClientId, ex.Message);
            
            // Check if this is a business validation error (quantity, price, etc.)
            if (IsBusinessValidationError(ex.Message))
            {
                throw new BusinessValidationException(ex.Message, ex);
            }
            
            // Otherwise, it's a technical error
            throw new InvalidOperationException($"SOAP service error: {ex.Message}", ex);
        }
        catch (CommunicationException ex)
        {
            _logger.LogError(ex, "Communication error occurred while creating order for client {ClientId}", request.ClientId);
            throw new InvalidOperationException("Unable to communicate with order service", ex);
        }
        catch (TimeoutException ex)
        {
            _logger.LogError(ex, "Timeout occurred while creating order for client {ClientId}", request.ClientId);
            throw new InvalidOperationException("Request to order service timed out", ex);
        }
        finally
        {
            CloseClientSafely(client);
        }
    }

    public async Task<OrderDetailsResponseDto> GetOrderDetailsAsync(int orderId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving order details for OrderId: {OrderId}", orderId);

        var client = _clientFactory.CreateClient();
        try
        {
            var soapRequest = new GetOrderDetailsRequest { OrderId = orderId };
            var soapResponse = await client.GetOrderDetailsAsync(soapRequest);

            _logger.LogInformation("Order details retrieved successfully for OrderId: {OrderId}", orderId);

            return soapResponse.ToDto();
        }
        catch (FaultException ex)
        {
            _logger.LogWarning(ex, "SOAP fault occurred while retrieving order details for OrderId: {OrderId}: {Message}", 
                orderId, ex.Message);
            
            if (IsBusinessValidationError(ex.Message))
            {
                throw new BusinessValidationException(ex.Message, ex);
            }
            
            throw new InvalidOperationException($"SOAP service error: {ex.Message}", ex);
        }
        catch (CommunicationException ex)
        {
            _logger.LogError(ex, "Communication error occurred while retrieving order details for OrderId: {OrderId}", orderId);
            throw new InvalidOperationException("Unable to communicate with order service", ex);
        }
        catch (TimeoutException ex)
        {
            _logger.LogError(ex, "Timeout occurred while retrieving order details for OrderId: {OrderId}", orderId);
            throw new InvalidOperationException("Request to order service timed out", ex);
        }
        finally
        {
            CloseClientSafely(client);
        }
    }

    public async Task<CalculateOrderTotalResponseDto> CalculateOrderTotalAsync(int orderId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Calculating order total for OrderId: {OrderId}", orderId);

        var client = _clientFactory.CreateClient();
        try
        {
            var soapRequest = new CalculateOrderTotalRequest { OrderId = orderId };
            var soapResponse = await client.CalculateOrderTotalAsync(soapRequest);

            _logger.LogInformation("Order total calculated successfully for OrderId: {OrderId}", orderId);

            return soapResponse.ToDto(orderId);
        }
        catch (FaultException ex)
        {
            _logger.LogWarning(ex, "SOAP fault occurred while calculating order total for OrderId: {OrderId}: {Message}", 
                orderId, ex.Message);
            
            if (IsBusinessValidationError(ex.Message))
            {
                throw new BusinessValidationException(ex.Message, ex);
            }
            
            throw new InvalidOperationException($"SOAP service error: {ex.Message}", ex);
        }
        catch (CommunicationException ex)
        {
            _logger.LogError(ex, "Communication error occurred while calculating order total for OrderId: {OrderId}", orderId);
            throw new InvalidOperationException("Unable to communicate with order service", ex);
        }
        catch (TimeoutException ex)
        {
            _logger.LogError(ex, "Timeout occurred while calculating order total for OrderId: {OrderId}", orderId);
            throw new InvalidOperationException("Request to order service timed out", ex);
        }
        finally
        {
            CloseClientSafely(client);
        }
    }

    public async Task<UpdateOrderStatusResponseDto> UpdateOrderStatusAsync(UpdateOrderStatusRequestDto request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating order status for OrderId: {OrderId} to {NewStatus}", request.OrderId, request.NewStatus);

        var client = _clientFactory.CreateClient();
        try
        {
            var soapRequest = new UpdateOrderStatusRequest
            {
                OrderId = request.OrderId,
                NewStatus = request.NewStatus.ToOrderStatus()
            };

            var soapResponse = await client.UpdateOrderStatusAsync(soapRequest);

            _logger.LogInformation("Order status updated successfully for OrderId: {OrderId}. Previous: {PreviousStatus}, New: {NewStatus}",
                request.OrderId, soapResponse.PreviousStatus, soapResponse.NewStatus);

            return soapResponse.ToDto();
        }
        catch (FaultException ex)
        {
            _logger.LogWarning(ex, "SOAP fault occurred while updating order status for OrderId: {OrderId}: {Message}", 
                request.OrderId, ex.Message);
            
            if (IsBusinessValidationError(ex.Message))
            {
                throw new BusinessValidationException(ex.Message, ex);
            }
            
            throw new InvalidOperationException($"SOAP service error: {ex.Message}", ex);
        }
        catch (CommunicationException ex)
        {
            _logger.LogError(ex, "Communication error occurred while updating order status for OrderId: {OrderId}", request.OrderId);
            throw new InvalidOperationException("Unable to communicate with order service", ex);
        }
        catch (TimeoutException ex)
        {
            _logger.LogError(ex, "Timeout occurred while updating order status for OrderId: {OrderId}", request.OrderId);
            throw new InvalidOperationException("Request to order service timed out", ex);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Invalid order status provided: {Status}", request.NewStatus);
            throw;
        }
        finally
        {
            CloseClientSafely(client);
        }
    }

    private void CloseClientSafely(OrderServiceClient client)
    {
        try
        {
            if (client.State != CommunicationState.Faulted)
            {
                client.Close();
            }
            else
            {
                client.Abort();
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error closing SOAP client");
            client.Abort();
        }
    }

    /// <summary>
    /// Determines if a SOAP fault message indicates a business validation error
    /// </summary>
    private static bool IsBusinessValidationError(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            return false;

        // Common business validation error patterns
        var validationKeywords = new[]
        {
            "cannot exceed",
            "must be",
            "required",
            "invalid",
            "not found",
            "not allowed",
            "minimum",
            "maximum",
            "range",
            "quantity",
            "price",
            "status",
            "order"
        };

        var lowerMessage = message.ToLowerInvariant();
        return validationKeywords.Any(keyword => lowerMessage.Contains(keyword));
    }
}
