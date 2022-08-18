using CatalogWebApi.Base;
using CatalogWebApi.Data;
using CatalogWebApi.Dto;

namespace CatalogWebApi.Service
{
    public interface IOfferService : IBaseService<OfferDto, Offer>
    {
        Task<BaseResponse<OfferDto>> InsertAsync(OfferDto insertResource, int offeredPrice, bool isPercent);
        Task<BaseResponse<OfferDto>> RemoveAsync(int id, int userId);
        Task<BaseResponse<IEnumerable<OfferDto>>> GetMyOffersAsync(int userId);
        Task<BaseResponse<IEnumerable<IEnumerable<OfferDto>>>> GetMyProductsOffersAsync(int userId);
        Task<BaseResponse<OfferDto>> SellAsync(int userId, int offerId);
    }
}   
