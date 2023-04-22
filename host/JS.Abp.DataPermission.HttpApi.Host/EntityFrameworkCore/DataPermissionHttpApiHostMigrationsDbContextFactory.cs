using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace JS.Abp.DataPermission.EntityFrameworkCore;

public class DataPermissionHttpApiHostMigrationsDbContextFactory : IDesignTimeDbContextFactory<DataPermissionHttpApiHostMigrationsDbContext>
{
    public DataPermissionHttpApiHostMigrationsDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<DataPermissionHttpApiHostMigrationsDbContext>()
            .UseSqlServer(configuration.GetConnectionString("DataPermission"));

        return new DataPermissionHttpApiHostMigrationsDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
