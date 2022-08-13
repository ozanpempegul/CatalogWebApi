using AutoMapper;
using CatalogWebApi.Base;
using CatalogWebApi.Data;
using CatalogWebApi.Dto;

namespace CatalogWebApi.Service
{
    public class AccountService : BaseService<AccountDto, Account>, IAccountService
    {
        private readonly IAccountRepository accountRepository;

        public AccountService(IAccountRepository accountRepository, IMapper mapper, IUnitOfWork unitOfWork) : base(accountRepository, mapper, unitOfWork)
        {
            this.accountRepository = accountRepository;
        }
        public override async Task<BaseResponse<AccountDto>> InsertAsync(AccountDto createAccountResource)
        {
            try
            {
                // Mapping Resource to Account
                var tempAccount = Mapper.Map<AccountDto, Account>(createAccountResource);

                await accountRepository.InsertAsync(tempAccount);
                await UnitOfWork.CompleteAsync();

                tempAccount.LastActivity = DateTime.UtcNow;

                return new BaseResponse<AccountDto>(Mapper.Map<Account, AccountDto>(tempAccount));
            }
            catch (Exception ex)
            {
                throw new MessageResultException("Account_Saving_Error", ex);
            }
        }

       
        public async Task<BaseResponse<AccountDto>> SelfUpdateAsync(int id, AccountDto resource)
        {
            try
            {
                var tempAccount = await accountRepository.GetByIdAsync(id);

                // Update infomation
                Mapper.Map(resource, tempAccount);
                accountRepository.Update(tempAccount);

                tempAccount.LastActivity = DateTime.UtcNow;

                await UnitOfWork.CompleteAsync();

                return new BaseResponse<AccountDto>(Mapper.Map<AccountDto>(tempAccount));
            }
            catch (Exception ex)
            {
                throw new MessageResultException("Account_Updating_Error", ex);
            }
        }

        public async Task<BaseResponse<AccountDto>> UpdatePasswordAsync(int id, UpdatePasswordRequest resource)
        {
            try
            {
                // Validate Id is existent?
                var tempAccount = await accountRepository.GetByIdAsync(id, hasToken: true);
                if (tempAccount is null)
                    return new BaseResponse<AccountDto>("Account_NoData");
                if (!tempAccount.Password.CheckingPassword(resource.OldPassword))
                    return new BaseResponse<AccountDto>("Account_Password_Error");

                // Update infomation
                tempAccount.Password = resource.NewPassword;
                tempAccount.LastActivity = DateTime.UtcNow;

                await UnitOfWork.CompleteAsync();

                return new BaseResponse<AccountDto>(Mapper.Map<AccountDto>(tempAccount));
            }
            catch (Exception ex)
            {
                throw new MessageResultException("Account_Updating_Error", ex);
            }
        }
     
    }
}
