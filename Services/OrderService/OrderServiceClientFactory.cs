using System.ServiceModel;
using RestGateway.OrderService;

namespace RestGateway.Services;

/// <summary>
/// Factory for creating SOAP OrderService client instances with custom configuration
/// </summary>
public class OrderServiceClientFactory
{
    private readonly IConfiguration _configuration;

    public OrderServiceClientFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Creates an instance of the OrderService client with BasicHttpBinding configured for SOAP 1.1
    /// </summary>
    public OrderServiceClient CreateClient()
    {
        var baseUrl = _configuration["OrderService:BaseUrl"]
            ?? "http://localhost:5000/OrderService.svc";

        // Create BasicHttpBinding with explicit configuration for SOAP 1.1
        var binding = CreateBasicHttpBinding();

        // Create the endpoint address
        var endpointAddress = new EndpointAddress(baseUrl);

        // Create and return the client
        return new OrderServiceClient(binding, endpointAddress);
    }

    /// <summary>
    /// Creates a BasicHttpBinding configured for SOAP 1.1
    /// </summary>
    private BasicHttpBinding CreateBasicHttpBinding()
    {
        var binding = new BasicHttpBinding();

        // Explicit configuration for SOAP 1.1
        binding.MessageEncoding = WSMessageEncoding.Text;
        binding.TextEncoding = System.Text.Encoding.UTF8;
        binding.Security.Mode = BasicHttpSecurityMode.None;

        // Configuration limits from appsettings.json or default values
        var maxMessageSize = _configuration.GetValue<int?>("OrderService:Binding:MaxReceivedMessageSize")
            ?? int.MaxValue;
        var maxBufferSize = _configuration.GetValue<int?>("OrderService:Binding:MaxBufferSize")
            ?? int.MaxValue;

        binding.MaxReceivedMessageSize = maxMessageSize;
        binding.MaxBufferSize = maxBufferSize;
        binding.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
        binding.AllowCookies = true;

        // Timeouts from appsettings.json or default values
        var sendTimeout = _configuration.GetValue<TimeSpan?>("OrderService:Binding:SendTimeout")
            ?? TimeSpan.FromMinutes(1);
        var receiveTimeout = _configuration.GetValue<TimeSpan?>("OrderService:Binding:ReceiveTimeout")
            ?? TimeSpan.FromMinutes(10);

        binding.OpenTimeout = TimeSpan.FromMinutes(1);
        binding.CloseTimeout = TimeSpan.FromMinutes(1);
        binding.SendTimeout = sendTimeout;
        binding.ReceiveTimeout = receiveTimeout;

        return binding;
    }
}
