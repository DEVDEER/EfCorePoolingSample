namespace devdeer.EfCorePoolingSample.Ui.ConsoleApp
{
    using Entities;

    using Microsoft.EntityFrameworkCore;

    public class SampleContext : DbContext
    {
        #region constructors and destructors

        public SampleContext(DbContextOptions<SampleContext> options) : base(options)
        {
        }

        #endregion

        #region properties

        public DbSet<Person> People { get; set; } = null!;

        #endregion
    }
}