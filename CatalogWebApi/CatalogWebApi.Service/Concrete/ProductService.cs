using AutoMapper;
using CatalogWebApi.Base;
using CatalogWebApi.Data;
using CatalogWebApi.Dto;
using Microsoft.AspNetCore.Http;

namespace CatalogWebApi.Service
{
    public class ProductService : BaseService<ProductDto, Product>, IProductService
    {
        public ProductService(IProductRepository productRepository, IMapper mapper, IUnitOfWork unitOfWork) : base(productRepository, mapper, unitOfWork)
        {
            this.productRepository = productRepository;
        }

        private readonly IProductRepository productRepository;


        public override async Task<BaseResponse<ProductDto>> GetByIdAsync(int id)
        {
            var tempProduct = await productRepository.GetByIdAsync(id);
            // Mapping Entity to Resource
            var result = Mapper.Map<Product, ProductDto>(tempProduct);

            return new BaseResponse<ProductDto>(result);
        }

        public new async Task<BaseResponse<ProductDto>> InsertAsync(ProductDto createProductResource, int userId, IFormFile? image)
        {
            try
            {
                //Mapping IFormFile to Base64String Array
                if(image is not null)
                {
                    var ms = new MemoryStream();
                    image.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    // act on the Base64 data

                    // Mapping Resource to Product               
                    var product = Mapper.Map<ProductDto, Product>(createProductResource);
                    product.Image = fileBytes;
                    product.AccountId = userId;
                    await productRepository.InsertAsync(product);
                    await UnitOfWork.CompleteAsync();

                    // Mappping response
                    var response = Mapper.Map<Product, ProductDto>(product);

                    return new BaseResponse<ProductDto>(response);
                }
                else
                {
                    // Mapping Resource to Product               
                    var product = Mapper.Map<ProductDto, Product>(createProductResource);
                    product.Image = null;
                    product.AccountId = userId;
                    await productRepository.InsertAsync(product);
                    await UnitOfWork.CompleteAsync();

                    // Mappping response
                    var response = Mapper.Map<Product, ProductDto>(product);

                    return new BaseResponse<ProductDto>(response);
                }

            }
            catch (Exception ex)
            {
                throw new MessageResultException("Product_Saving_Error", ex);
            }
        }

        public new async Task<BaseResponse<ProductDto>> UpdateAsync(int id, ProductDto request, int userId)
        {
            try
            {
                // Validate Id is existent
                var product = await productRepository.GetByIdAsync(id);
                if (product is null)
                {
                    return new BaseResponse<ProductDto>("Product_Id_NoData");
                }
                if (product.AccountId != userId)
                {
                    return new BaseResponse<ProductDto>("Product you are trying to change is not yours."); ;
                }

                product.Name = request.Name;
                product.Description = request.Description;
                product.CategoryId = request.CategoryId;
                product.ColorId = request.ColorId;
                product.BrandId = request.BrandId;
                product.IsOfferable = request.IsOfferable;
                product.IsUsed = request.IsUsed;
                product.Price = request.Price;

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
        public new async Task<BaseResponse<ProductDto>> RemoveAsync(int id, int userId)
        {
            try
            {
                // Validate Id is existent
                var tempProduct = await productRepository.GetByIdAsync(id);
                if (tempProduct is null)
                    return new BaseResponse<ProductDto>("Id_NoData");
                if (tempProduct.AccountId != userId)
                {
                    return new BaseResponse<ProductDto>("Invalid Product");
                }

                productRepository.RemoveAsync(tempProduct);
                await UnitOfWork.CompleteAsync();

                return new BaseResponse<ProductDto>(Mapper.Map<Product, ProductDto>(tempProduct));
            }
            catch (Exception ex)
            {
                throw new MessageResultException("Deleting_Error", ex);
            }
        }

        public async Task<BaseResponse<IEnumerable<ProductDto>>> GetAllByCategoryIdAsync(int categoryId)
        {
            // Get list record from DB
            var tempEntity = await productRepository.GetAllByCategoryIdAsync(categoryId);
            if (tempEntity is null)
                return new BaseResponse<IEnumerable<ProductDto>>("Id_NoData");
            // Mapping Entity to Resource
            var result = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(tempEntity);

            var resource = new PaginationResponse<IEnumerable<ProductDto>>(result);

            return resource;
        }

        public async Task<BaseResponse<IEnumerable<ProductDto>>> GetAllMyProductsAsync(int userId)
        {
            // Get list record from DB
            var tempEntity = await productRepository.GetAllMyProductsAsync(userId);
            if (tempEntity is null)
                return new BaseResponse<IEnumerable<ProductDto>>("No Data");
            // Mapping Entity to Resource
            var result = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(tempEntity);

            var resource = new PaginationResponse<IEnumerable<ProductDto>>(result);

            return resource;
        }
    }
}
