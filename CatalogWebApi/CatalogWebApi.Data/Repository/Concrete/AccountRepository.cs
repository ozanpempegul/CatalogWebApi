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

        public async Task<int> TotalRecordAsync() => await Context.Account.CountAsync();

        public async Task<Account> ValidateCredentialsAsync(TokenRequest loginResource)
        {
            var accountStored = await Context.Account
                .Where(x => x.UserName == loginResource.UserName.ToLower())
                .SingleOrDefaultAsync();

            if (accountStored is null)
                return null;
            else
            {
                // Validate credential
                bool isValid = accountStored.Password.CheckingPassword(loginResource.Password);
                if (isValid)
                    return accountStored;
                else
                    return null;
            }
        }


    }
}
