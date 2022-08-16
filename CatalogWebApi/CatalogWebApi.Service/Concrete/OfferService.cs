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

            if (!productRepository.GetIsOfferable(insertResource.ProductId))
            {
                throw new MessageResultException("Product is not offerable");
            }
            var product = await productRepository.GetByIdAsync(insertResource.ProductId);

            if (isPercent)
            {
                if (insertResource.OfferedPrice >= 100)
                {
                    throw new MessageResultException("You cannot offer more than its price.");
                }
                insertResource.OfferedPrice = product.Price * insertResource.OfferedPrice / 100;
            }
            else
            {
                if (insertResource.OfferedPrice >= product.Price)
                {
                    throw new MessageResultException("You cannot offer more than its price.");
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
                    throw new MessageResultException("Offer is not yours");
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
    }
}
