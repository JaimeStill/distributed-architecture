using App.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Platform.Broker;
using Platform.Contracts;

var services = new ServiceCollection()
    .RegisterPicsumService()
    .BuildServiceProvider();

IStreamService<IPhoto> picsumSvc =
    services.GetService<IStreamService<IPhoto>>();

string connection = args.Length > 0
    ? args.First()
    : null;

while (string.IsNullOrEmpty(connection)) {
    Console.WriteLine("Provide a connection string:");
    connection = Console.ReadLine();
}

try
{
    Console.WriteLine($"Connection: {connection}");

    var builder = new DbContextOptionsBuilder<AppDbContext>()
        .UseSqlServer(connection);

    using var db = new AppDbContext(builder.Options);
    await db.Initialize(picsumSvc);
    Console.WriteLine("The database was successfully updated");
}
catch (Exception ex)
{
    throw new Exception("An error occurred while updating the database", ex);
}
