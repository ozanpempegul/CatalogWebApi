namespace CatalogWebApi.Data
{
    public class PersonRepository : GenericRepository<Person>, IPersonRepository
    {
        public PersonRepository(AppDbContext Context) : base(Context)
        {
        }

        public async Task<(IEnumerable<Person> records, int total)> GetPaginationAsync(QueryResource pagination, PersonDto filterResource)
        {
            var queryable = ConditionFilter(filterResource);

            var total = await queryable.CountAsync();

            var records = await queryable.AsNoTracking()
                .AsSplitQuery()
                .OrderByDescending(x => x.accountid)
                .Skip((pagination.Page - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            return (records, total);
        }
        private IQueryable<Person> ConditionFilter(PersonDto filterResource)
        {
            var queryable = Context.Person.AsQueryable();

            if (filterResource != null)
            {
                if (!string.IsNullOrEmpty(filterResource.accountid))
                    queryable = queryable.Where(x => x.accountid.Equals(filterResource.accountid.RemoveSpaceCharacter()));

                if (!string.IsNullOrEmpty(filterResource.FirstName))
                {
                    string fullName = filterResource.FirstName.RemoveSpaceCharacter().ToLower();
                    queryable = queryable.Where(x => x.firstname.Contains(fullName));
                }

                if (!string.IsNullOrEmpty(filterResource.LastName))
                {
                    string fullName = filterResource.LastName.RemoveSpaceCharacter().ToLower();
                    queryable = queryable.Where(x => x.lastname.Contains(fullName));
                }
            }

            return queryable;
        }

        public override async Task<Person> GetByIdAsync(int id)
        {
            return await Context.Person.AsSplitQuery().SingleOrDefaultAsync(x => x.accountid == id);
        }

        public async Task<int> TotalRecordAsync()
        {
            return await Context.Person.CountAsync();
        }
    }
}
