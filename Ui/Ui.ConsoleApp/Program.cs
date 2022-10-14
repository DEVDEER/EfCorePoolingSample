using System.Diagnostics;

using devdeer.EfCorePoolingSample.Ui.ConsoleApp;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args);
builder.ConfigureServices(
    services =>
    {
        services.AddPooledDbContextFactory<SampleContext>(
            (svc, b) =>
            {
                var configuration = svc.GetRequiredService<IConfiguration>();
                var connectionString = configuration.GetConnectionString("Sample");
                b.UseSqlServer(
                    connectionString,
                    o =>
                    {
                        o.EnableRetryOnFailure();
                        o.CommandTimeout(300);
                    });
            });
        services.AddTransient<PersonRepository>();
    });
using (var host = builder.Build())
{
    // check for db
    using (var ctx = host.Services.GetRequiredService<IDbContextFactory<SampleContext>>()
               .CreateDbContext())
    {
        if (ctx.Database.GetPendingMigrations()
            .Any())
        {
            Console.WriteLine("Database migration is beeing applied...");
            ctx.Database.Migrate();
            Console.WriteLine("Done");
        }
    }
    var tasks = new List<Task>();
    var overallWatch = new Stopwatch();
    overallWatch.Start();
    Enumerable.Range(1, 100)
        .ToList()
        .ForEach(
            _ => tasks.Add(
                Task.Run(
                    async () =>
                    {
                        var repo = host.Services.GetRequiredService<PersonRepository>();
                        var watch = new Stopwatch();
                        watch.Start();
                        var amount = await repo.CreateItemsAsync(1000);
                        watch.Stop();
                        Console.WriteLine($"Created {amount} items in {watch.Elapsed}.");
                    })));
    Task.WaitAll(tasks.ToArray());
    overallWatch.Stop();
    var repo = host.Services.GetRequiredService<PersonRepository>();
    var amount = await repo.GetCountAsync();
    Console.WriteLine($"{amount} people are present. Operation took {overallWatch.Elapsed}");
    await repo.ClearAsync();
    Console.WriteLine("People table was deleted.");
}
Console.WriteLine("Program finished.");