using CatalogWebApi.Base;

namespace CatalogWebApi.Service
{
    public interface ITokenManagementService
    {
        Task<BaseResponse<TokenResponse>> GenerateTokensAsync(TokenRequest loginResource, DateTime now, string userAgent);
    }
}
