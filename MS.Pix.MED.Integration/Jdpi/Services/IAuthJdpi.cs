using MS.Pix.MED.Integration.Jdpi.Request;
using MS.Pix.MED.Integration.Jdpi.Response;
using Refit;

namespace MS.Pix.MED.Integration.Jdpi.Services;

public interface IAuthJdpi
{
    [Headers("Content-Type: application/x-www-form-urlencoded")]
    [Post("/connect/token")]
    Task<TokenResponse> GetTokenAsync([Body(BodySerializationMethod.UrlEncoded)] TokenRequest request);
}