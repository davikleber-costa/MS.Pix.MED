using MS.Pix.MED.Integration.Jdpi.Request;
using MS.Pix.MED.Integration.Jdpi.Response;

namespace MS.Pix.MED.Integration.Jdpi.Services;

public interface IJdpiIntegrationService
{
    Task<bool> AuthenticateAsync();
    Task<string> SendToJdpiAsync(object requestData, string endpoint, CancellationToken cancellationToken = default);
    Task<TResponse> QueryJdpiAsync<TResponse>(string endpoint, object? queryParameters = null, CancellationToken cancellationToken = default);
    Task<string> ProcessRefundAsync(object refundData, CancellationToken cancellationToken = default);
    Task<IEnumerable<object>> ListRefundsAsync(object queryParameters, CancellationToken cancellationToken = default);
}