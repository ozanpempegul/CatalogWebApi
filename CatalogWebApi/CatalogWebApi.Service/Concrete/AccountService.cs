using AutoMapper;
using CatalogWebApi.Base;
using CatalogWebApi.Data;
using CatalogWebApi.Dto;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;

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
                string MD5Salting(string pwd)
                {
                    MD5 md5 = new MD5CryptoServiceProvider();
                    byte[] bytes = md5.ComputeHash(Encoding.Unicode.GetBytes(pwd));
                    string result = BitConverter.ToString(bytes).Replace("-", String.Empty);
                    return result.ToLower();
                }
                createAccountResource.Password = MD5Salting(createAccountResource.Password);

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

        public async Task<BaseResponse<AccountDto>> SendEmail(AccountDto createAccountResource)
        {
            try
            {
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("mephistopeles11@gmail.com", "otetltwcpvlcrtwx"); // TO DO read it from a file, this is app pasword not the actual password.
                MailMessage msgObj = new MailMessage();
                msgObj.To.Add(createAccountResource.Email);
                msgObj.From = new MailAddress("mephistopeles11@gmail.com");
                msgObj.Subject = "Registration";
                msgObj.Body = "Successfully Registered";

                client.Send(msgObj);
                var tempAccount = Mapper.Map<AccountDto, Account>(createAccountResource);
                await UnitOfWork.CompleteAsync();
                return new BaseResponse<AccountDto>(Mapper.Map<Account, AccountDto>(tempAccount));
            }
            catch(Exception ex)
            {
                throw new MessageResultException("Sending_Email_Failed", ex);
            }

        }
    }
}
