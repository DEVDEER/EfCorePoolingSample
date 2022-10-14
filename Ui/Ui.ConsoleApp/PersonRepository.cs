namespace devdeer.EfCorePoolingSample.Ui.ConsoleApp
{
    using System.Diagnostics;

    using Entities;

    using Microsoft.EntityFrameworkCore;

    public class PersonRepository
    {
        #region constructors and destructors

        public PersonRepository(IDbContextFactory<SampleContext> factory)
        {
            DbContext = factory.CreateDbContext();
        }

        #endregion

        #region methods

        public async Task ClearAsync()
        {
            await DbContext.Database.ExecuteSqlRawAsync("TRUNCATE TABLE People;");
        }

        public async Task<int> CreateItemsAsync(int amount)
        {
            for (var i = 0; i < amount; i++)
            {
                DbContext.People.Add(
                    new Person
                    {
                        Number = Guid.NewGuid()
                            .ToString("N")
                    });
            }
            return await DbContext.SaveChangesAsync();
        }

        public async Task<int> GetCountAsync()
        {
            Trace.TraceInformation($"Current context id: {ContextId}");
            return await DbContext.People.CountAsync();
        }

        #endregion

        #region properties

        public string ContextId => DbContext.ContextId.ToString();

        private SampleContext DbContext { get; }

        #endregion
    }
}