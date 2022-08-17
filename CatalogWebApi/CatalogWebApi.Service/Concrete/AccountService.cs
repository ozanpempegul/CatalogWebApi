using AutoMapper;
using CatalogWebApi.Base;
using CatalogWebApi.Data;
using CatalogWebApi.Dto;

namespace CatalogWebApi.Service
{
    public class AccountService : BaseService<AccountDto, Account>, IAccountService
    {
        private readonly IAccountRepository accountRepository;
        private readonly MD5AndSaltingService _mD5AndSaltingService;
        public AccountService(IAccountRepository accountRepository, IMapper mapper, IUnitOfWork unitOfWork, MD5AndSaltingService mD5AndSaltingService) : base(accountRepository, mapper, unitOfWork)
        {
            this.accountRepository = accountRepository;
            this._mD5AndSaltingService = mD5AndSaltingService;
        }
        public override async Task<BaseResponse<AccountDto>> InsertAsync(AccountDto createAccountResource)
        {
            // Email Validation
            var existingEmail = await accountRepository.GetByEmailAsync(createAccountResource.Email);
            if (existingEmail != null)
            {
                throw new MessageResultException("There is another account registered to this email");
            }

            // Username Validation
            var existingUsername = await accountRepository.GetByUsernameAsync(createAccountResource.UserName);
            if (existingUsername != null)
            {
                throw new MessageResultException("Username is already in use");
            }

            try
            {
                
                //MD5andSalting
                createAccountResource.Password = _mD5AndSaltingService.MD5Salting(createAccountResource.Password, createAccountResource.Email);

                // Mapping Resource to Account
                var tempAccount = Mapper.Map<AccountDto, Account>(createAccountResource);
                tempAccount.LastActivity = DateTime.UtcNow;
                await accountRepository.InsertAsync(tempAccount);
                await UnitOfWork.CompleteAsync();

                

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

                resource.OldPassword = _mD5AndSaltingService.MD5Salting(resource.OldPassword, tempAccount.Email);

                if (resource.OldPassword != tempAccount.Password)
                    return new BaseResponse<AccountDto>("Account_Password_Error");

                
                // Update infomation
                tempAccount.Password = _mD5AndSaltingService.MD5Salting(resource.NewPassword,tempAccount.Email);
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
