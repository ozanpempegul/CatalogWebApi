using AutoMapper;
using CatalogWebApi.Base;
using CatalogWebApi.Data;
using CatalogWebApi.Dto;

namespace CatalogWebApi.Service
{
    public class ProductService : BaseService<ProductDto, Product>, IProductService
    {
        public ProductService(IProductRepository productRepository, IMapper mapper, IUnitOfWork unitOfWork) : base(productRepository, mapper, unitOfWork)
        {
            this.productRepository = productRepository;
        }

        private readonly IProductRepository productRepository;


        public override async Task<BaseResponse<ProductDto>> InsertAsync(ProductDto createProductResource)
        {
            try
            {
                // Mapping Resource to Product
                var product = Mapper.Map<ProductDto, Product>(createProductResource);
                

                await productRepository.InsertAsync(product);
                await UnitOfWork.CompleteAsync();

                // Mappping response
                var response = Mapper.Map<Product, ProductDto>(product);

                return new BaseResponse<ProductDto>(response);
            }
            catch (Exception ex)
            {
                throw new MessageResultException("Product_Saving_Error", ex);
            }
        }

        public override async Task<BaseResponse<ProductDto>> UpdateAsync(int id, ProductDto request)
        {
            try
            {
                // Validate Id is existent
                var product = await productRepository.GetByIdAsync(id);
                if (product is null)
                {
                    return new BaseResponse<ProductDto>("Product_Id_NoData");
                }

                product.Name = request.Name;
                product.CategoryId = request.CategoryId;
                product.ColorId = request.ColorId;
                product.BrandId = request.BrandId;

                productRepository.Update(product);
                await UnitOfWork.CompleteAsync();

                return new BaseResponse<ProductDto>(Mapper.Map<Product, ProductDto>(product));
            }
            catch (Exception ex)
            {
                throw new MessageResultException("Product_Saving_Error", ex);
            }
        }

      
        public async Task<PaginationResponse<IEnumerable<ProductDto>>> GetPaginationAsync(QueryResource pagination, ProductDto filterResource)
        {
            var paginationProduct = await productRepository.GetPaginationAsync(pagination, filterResource);

            // Mapping
            var tempResource = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(paginationProduct.records);

            var resource = new PaginationResponse<IEnumerable<ProductDto>>(tempResource);

            // Using extension-method for pagination
            resource.CreatePaginationResponse(pagination, paginationProduct.total);

            return resource;
        }
    }
}
