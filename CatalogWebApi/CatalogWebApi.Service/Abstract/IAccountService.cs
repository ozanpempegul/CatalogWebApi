using CatalogWebApi.Base;
using CatalogWebApi.Data;
using CatalogWebApi.Dto;

namespace CatalogWebApi.Service
{
    public interface IAccountService : IBaseService<AccountDto, Account>
    {
        Task<BaseResponse<AccountDto>> SelfUpdateAsync(int id, AccountDto resource);
        Task<BaseResponse<AccountDto>> UpdatePasswordAsync(int id, UpdatePasswordRequest resource);
    }
}
