using Microsoft.AspNetCore.Mvc;
using RestGateway.Models.DTOs.Requests;
using RestGateway.Models.DTOs.Responses;
using RestGateway.Services;
using System.ComponentModel.DataAnnotations;

namespace RestGateway.Controllers.V1;

/// <summary>
/// Orders API Controller (v1)
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(IOrderService orderService, ILogger<OrdersController> logger)
    {
        _orderService = orderService;
        _logger = logger;
    }

    /// <summary>
    /// Creates a new order
    /// </summary>
    /// <param name="request">Order creation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created order information</returns>
    /// <response code="201">Order created successfully</response>
    /// <response code="400">Invalid request data</response>
    /// <response code="502">Error communicating with order service</response>
    [HttpPost]
    [ProducesResponseType(typeof(CreateOrderResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponseDto), StatusCodes.Status502BadGateway)]
    public async Task<ActionResult<CreateOrderResponseDto>> CreateOrder(
        [FromBody] CreateOrderRequestDto request,
        CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var response = await _orderService.CreateOrderAsync(request, cancellationToken);
        return CreatedAtAction(
            nameof(GetOrderDetails),
            new { orderId = response.OrderId, version = "1.0" },
            response);
    }

    /// <summary>
    /// Gets order details by order ID
    /// </summary>
    /// <param name="orderId">Order identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Order details</returns>
    /// <response code="200">Order details retrieved successfully</response>
    /// <response code="404">Order not found</response>
    /// <response code="502">Error communicating with order service</response>
    [HttpGet("{orderId}")]
    [ProducesResponseType(typeof(OrderDetailsResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponseDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponseDto), StatusCodes.Status502BadGateway)]
    public async Task<ActionResult<OrderDetailsResponseDto>> GetOrderDetails(
        [FromRoute, Range(1, int.MaxValue)] int orderId,
        CancellationToken cancellationToken = default)
    {
        var response = await _orderService.GetOrderDetailsAsync(orderId, cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Calculates the total for an order
    /// </summary>
    /// <param name="orderId">Order identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Order total calculation</returns>
    /// <response code="200">Order total calculated successfully</response>
    /// <response code="404">Order not found</response>
    /// <response code="502">Error communicating with order service</response>
    [HttpGet("{orderId}/total")]
    [ProducesResponseType(typeof(CalculateOrderTotalResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponseDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponseDto), StatusCodes.Status502BadGateway)]
    public async Task<ActionResult<CalculateOrderTotalResponseDto>> CalculateOrderTotal(
        [FromRoute, Range(1, int.MaxValue)] int orderId,
        CancellationToken cancellationToken = default)
    {
        var response = await _orderService.CalculateOrderTotalAsync(orderId, cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Updates the status of an order
    /// </summary>
    /// <param name="orderId">Order identifier</param>
    /// <param name="request">Status update request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Status update result</returns>
    /// <response code="200">Order status updated successfully</response>
    /// <response code="400">Invalid request data</response>
    /// <response code="404">Order not found</response>
    /// <response code="502">Error communicating with order service</response>
    [HttpPut("{orderId}/status")]
    [ProducesResponseType(typeof(UpdateOrderStatusResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponseDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponseDto), StatusCodes.Status502BadGateway)]
    public async Task<ActionResult<UpdateOrderStatusResponseDto>> UpdateOrderStatus(
        [FromRoute, Range(1, int.MaxValue)] int orderId,
        [FromBody] UpdateOrderStatusRequestDto request,
        CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (request.OrderId != orderId)
        {
            ModelState.AddModelError(nameof(request.OrderId), "OrderId in route must match OrderId in body");
            return BadRequest(ModelState);
        }

        var response = await _orderService.UpdateOrderStatusAsync(request, cancellationToken);
        return Ok(response);
    }
}
