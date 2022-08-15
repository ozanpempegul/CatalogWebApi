using CatalogWebApi.Base;
using Microsoft.EntityFrameworkCore;

namespace CatalogWebApi.Data
{
    public class AccountRepository : GenericRepository<Account>, IAccountRepository
    {
        public AccountRepository(AppDbContext context) : base(context) { }


        public async Task<Account> GetByIdAsync(int id, bool hasToken)
        {
            var queryable = Context.Account.Where(x => x.Id.Equals(id));
            return await queryable.SingleOrDefaultAsync();
        }

        public async Task<Account> ValidateCredentialsAsync(TokenRequest loginResource)
        {
            var accountStored = await Context.Account
                .Where(x => x.Email == loginResource.Email.ToLower())
                .SingleOrDefaultAsync();

            if (accountStored is null)
                return null;
            else
            {
                if(accountStored.invalidtries >= 3)
                {
                    throw new MessageResultException("account is blocked due to excessive number of tries");
                }
                else if (accountStored.isdeleted == true)
                {
                    throw new MessageResultException("account is deleted");
                }
                // Validate credential
                bool isValid = accountStored.Password.CheckingPassword(loginResource.Password);
                if (isValid)
                {
                    accountStored.GetType().GetProperty("invalidtries").SetValue(accountStored, 0);
                    return accountStored;
                }
                else
                {
                    accountStored.GetType().GetProperty("invalidtries").SetValue(accountStored, accountStored.invalidtries + 1);
                    return null;
                }
            }
        }
        public async Task<Account> GetByEmailAsync(string email)
        {
            var queryable = Context.Account.Where(x => x.Email.Equals(email));
            return await queryable.FirstOrDefaultAsync();
        }

        public async Task<Account> GetByUsernameAsync(string username)
        {
            var queryable = Context.Account.Where(x => x.UserName.Equals(username));
            return await queryable.FirstOrDefaultAsync();
        }
    }
}
