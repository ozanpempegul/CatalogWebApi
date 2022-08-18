using AutoMapper;
using CatalogWebApi.Base;
using CatalogWebApi.Data;
using CatalogWebApi.Dto;

namespace CatalogWebApi.Service
{
    public class OfferService : BaseService<OfferDto, Offer>, IOfferService
    {
        private readonly IOfferRepository offerRepository;
        private readonly IProductRepository productRepository;

        public OfferService(IOfferRepository offerRepository, IMapper mapper, IUnitOfWork unitOfWork, IProductRepository productRepository) : base(offerRepository, mapper, unitOfWork)
        {
            this.offerRepository = offerRepository;
            this.productRepository = productRepository;
        }

        public new async Task<BaseResponse<OfferDto>> InsertAsync(OfferDto insertResource, int offeredPrice, bool isPercent)
        {

            if (!await productRepository.GetIsOfferable(insertResource.ProductId))
            {
                return new BaseResponse<OfferDto>("Product is not offerable");
            }
            var product = await productRepository.GetByIdAsync(insertResource.ProductId);

            if (isPercent)
            {
                if (offeredPrice >= 100)
                {
                    return new BaseResponse<OfferDto>("You cannot offer more than its price.");
                }
                insertResource.OfferedPrice = product.Price * insertResource.OfferedPrice / 100;
            }
            else
            {
                if (offeredPrice >= product.Price)
                {
                    return new BaseResponse<OfferDto>("You cannot offer more than its price.");
                }
                insertResource.OfferedPrice = offeredPrice;
            }
            try
            {
                // Mapping Resource to Entity
                var tempEntity = Mapper.Map<OfferDto, Offer>(insertResource);

                await offerRepository.InsertAsync(tempEntity);
                await UnitOfWork.CompleteAsync();

                return new BaseResponse<OfferDto>(Mapper.Map<Offer, OfferDto>(tempEntity));
            }
            catch (Exception ex)
            {
                throw new MessageResultException("Saving_Error", ex);
            }
        }

        public async Task<BaseResponse<OfferDto>> RemoveAsync(int id, int userId)
        {
            try
            {
                // Validate Id is existent
                var tempOffer = await offerRepository.GetByIdAsync(id);
                if (tempOffer is null)
                    return new BaseResponse<OfferDto>("Id_NoData");

                if (tempOffer.BidderId != userId)
                {
                    return new BaseResponse<OfferDto>("Offer is not yours");
                }

                offerRepository.RemoveAsync(tempOffer);
                await UnitOfWork.CompleteAsync();

                return new BaseResponse<OfferDto>(Mapper.Map<Offer, OfferDto>(tempOffer));
            }
            catch (Exception ex)
            {
                throw new MessageResultException("Deleting_Error", ex);
            }
        }

        public async Task<BaseResponse<IEnumerable<OfferDto>>> GetMyOffersAsync(int userId)
        {
            try
            {
                // Validate Id is existent
                var tempOffer = await offerRepository.GetByBidderId(userId);
                if (tempOffer is null)
                    return new BaseResponse<IEnumerable<OfferDto>>("Id_NoData");

                return new BaseResponse<IEnumerable<OfferDto>>(Mapper.Map<IEnumerable<Offer>, IEnumerable<OfferDto>>(tempOffer));
            }
            catch (Exception ex)
            {
                throw new MessageResultException("Listing_Error", ex);
            }
        }

        public async Task<BaseResponse<IEnumerable<IEnumerable<OfferDto>>>> GetMyProductsOffersAsync(int userId)
        {

            try
            {
                List<IEnumerable<Offer>> tempOffersAll = new();
                var myProducts = await productRepository.GetAllMyProductsAsync(userId);

                if (myProducts is null)
                    return new BaseResponse<IEnumerable<IEnumerable<OfferDto>>>("NoData");

                foreach (var product in myProducts)
                {
                    var tempOffersByProductId = await offerRepository.GetByProductId(product.Id);
                    tempOffersAll.Add(tempOffersByProductId);
                };

                if (tempOffersAll is null)
                    return new BaseResponse<IEnumerable<IEnumerable<OfferDto>>>("Id_NoData");

                return new BaseResponse<IEnumerable<IEnumerable<OfferDto>>>(Mapper.Map<IEnumerable<IEnumerable<Offer>>, IEnumerable<IEnumerable<OfferDto>>>(tempOffersAll));
            }
            catch (Exception ex)
            {
                throw new MessageResultException("Listing_Error", ex);
            }
        }

        public async Task<BaseResponse<OfferDto>> SellAsync(int userId, int offerId)
        {
            try
            {
                // Validate Id is existent
                var tempOffer = await offerRepository.GetByIdAsync(offerId);
                if (tempOffer is null)
                    return new BaseResponse<OfferDto>("Id_NoData");

                var tempProduct = await productRepository.GetByIdAsync(tempOffer.ProductId);

                if (userId != tempProduct.AccountId)
                {
                    return new BaseResponse<OfferDto>("Product is not yours");
                }

                productRepository.SellAsync(tempProduct);                
                var tempOffers = await offerRepository.GetByProductId(tempOffer.ProductId);
                foreach (var offer in tempOffers)
                {
                    if (offer.Id != offerId)
                    {
                        offerRepository.RemoveAsync(offer);
                        await UnitOfWork.CompleteAsync();
                    }                        
                }
                await UnitOfWork.CompleteAsync();

                return new BaseResponse<OfferDto>(Mapper.Map<Offer, OfferDto>(tempOffer));
            }
            catch (Exception ex)
            {
                throw new MessageResultException("Selling_Error", ex);
            }
        }
    }
}
