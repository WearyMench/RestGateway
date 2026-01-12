using RestGateway.Models.DTOs.Requests;
using RestGateway.Models.DTOs.Responses;
using RestGateway.OrderService;

namespace RestGateway.Mappings;

/// <summary>
/// Mapper for converting between REST DTOs and SOAP contracts
/// </summary>
public static class OrderMapper
{
    /// <summary>
    /// Maps CreateOrderRequestDto to SOAP CreateOrderRequest
    /// </summary>
    public static CreateOrderRequest ToSoapRequest(this CreateOrderRequestDto dto)
    {
        return new CreateOrderRequest
        {
            ClientId = dto.ClientId,
            Products = dto.Products.Select(p => new ProductItem
            {
                ProductId = p.ProductId,
                Quantity = p.Quantity,
                UnitPrice = p.UnitPrice
            }).ToArray(),
            Address = new Address
            {
                Street = dto.Address.Street,
                City = dto.Address.City,
                State = dto.Address.State,
                ZipCode = dto.Address.ZipCode,
                Country = dto.Address.Country
            }
        };
    }

    /// <summary>
    /// Maps SOAP CreateOrderResponse to CreateOrderResponseDto
    /// </summary>
    public static CreateOrderResponseDto ToDto(this CreateOrderResponse response)
    {
        return new CreateOrderResponseDto
        {
            OrderId = response.OrderId,
            Status = response.Status.ToString(),
            CreatedDate = response.CreatedDate
        };
    }

    /// <summary>
    /// Maps SOAP Order to OrderDetailsResponseDto
    /// </summary>
    public static OrderDetailsResponseDto ToDto(this Order order)
    {
        return new OrderDetailsResponseDto
        {
            OrderId = order.OrderId,
            ClientId = order.ClientId,
            Products = order.Products?.Select(p => new ProductItemResponseDto
            {
                ProductId = p.ProductId,
                Quantity = p.Quantity,
                UnitPrice = p.UnitPrice
            }).ToList() ?? new List<ProductItemResponseDto>(),
            Address = order.Address != null ? new AddressResponseDto
            {
                Street = order.Address.Street ?? string.Empty,
                City = order.Address.City ?? string.Empty,
                State = order.Address.State ?? string.Empty,
                ZipCode = order.Address.ZipCode ?? string.Empty,
                Country = order.Address.Country ?? string.Empty
            } : new AddressResponseDto(),
            Status = order.Status.ToString(),
            CreatedDate = order.CreatedDate,
            FinancialDetails = new OrderFinancialDetailsDto
            {
                Subtotal = order.Subtotal,
                Taxes = order.Taxes,
                Discount = order.Discount,
                Shipping = order.Shipping,
                Total = order.Total
            }
        };
    }

    /// <summary>
    /// Maps SOAP GetOrderDetailsResponse to OrderDetailsResponseDto
    /// </summary>
    public static OrderDetailsResponseDto ToDto(this GetOrderDetailsResponse response)
    {
        return response.Order?.ToDto() ?? new OrderDetailsResponseDto();
    }

    /// <summary>
    /// Maps SOAP CalculateOrderTotalResponse to CalculateOrderTotalResponseDto
    /// </summary>
    public static CalculateOrderTotalResponseDto ToDto(this CalculateOrderTotalResponse response, int orderId)
    {
        return new CalculateOrderTotalResponseDto
        {
            OrderId = orderId,
            FinancialDetails = new OrderFinancialDetailsDto
            {
                Subtotal = response.Subtotal,
                Taxes = response.Taxes,
                Discount = response.Discount,
                Shipping = response.Shipping,
                Total = response.Total
            }
        };
    }

    /// <summary>
    /// Maps SOAP UpdateOrderStatusResponse to UpdateOrderStatusResponseDto
    /// </summary>
    public static UpdateOrderStatusResponseDto ToDto(this UpdateOrderStatusResponse response)
    {
        return new UpdateOrderStatusResponseDto
        {
            OrderId = response.OrderId,
            PreviousStatus = response.PreviousStatus.ToString(),
            NewStatus = response.NewStatus.ToString(),
            Success = response.Success
        };
    }

    /// <summary>
    /// Converts string status to OrderStatus enum
    /// </summary>
    public static OrderStatus ToOrderStatus(this string status)
    {
        return status.ToUpper() switch
        {
            "CREATED" => OrderStatus.CREATED,
            "PAID" => OrderStatus.PAID,
            "SHIPPED" => OrderStatus.SHIPPED,
            "DELIVERED" => OrderStatus.DELIVERED,
            _ => throw new ArgumentException($"Invalid order status: {status}", nameof(status))
        };
    }
}
