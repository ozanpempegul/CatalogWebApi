﻿using AutoMapper;
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
                createAccountResource.Password = MD5Salting(createAccountResource.Password, createAccountResource.Email.Length);

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

                resource.OldPassword = MD5Salting(resource.OldPassword, tempAccount.Email.Length);

                if (resource.OldPassword != tempAccount.Password)
                    return new BaseResponse<AccountDto>("Account_Password_Error");

                
                // Update infomation
                tempAccount.Password = MD5Salting(resource.NewPassword,tempAccount.Email.Length);
                tempAccount.LastActivity = DateTime.UtcNow;

                await UnitOfWork.CompleteAsync();

                return new BaseResponse<AccountDto>(Mapper.Map<AccountDto>(tempAccount));
            }
            catch (Exception ex)
            {
                throw new MessageResultException("Account_Updating_Error", ex);
            }
        }
        
        
        public async Task<BaseResponse<AccountDto>> SendEmail(AccountDto createAccountResource, string subject, string body)
        {

            string[] lines = File.ReadAllLines("credentials.txt");
            string email = lines[0];
            string password = lines[1];

            try
            {
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;

                // reading it from credentials.txt, password is app pasword not the actual password of the email.
                client.Credentials = new NetworkCredential(email, password);

                MailMessage msgObj = new MailMessage();
                msgObj.To.Add(createAccountResource.Email);
                msgObj.From = new MailAddress("mephistopeles11@gmail.com");
                msgObj.Subject = subject;
                msgObj.Body = body;

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

        // TO DO each password should be different in database. Use user email maybe? (Since it is unique.)
        //MD5 and Salting
        public string MD5Salting(string pwd, int emailLength)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] bytes = new byte[emailLength];
            bytes = md5.ComputeHash(Encoding.Unicode.GetBytes(pwd));
            string result = BitConverter.ToString(bytes).Replace("-", String.Empty);
            return result.ToLower();
        }
    }
}
