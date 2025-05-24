using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;

namespace Data;

public class EntityCreationContextDesignTimeFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    private readonly IConfiguration _configuration;

    public EntityCreationContextDesignTimeFactory()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var parentDirectory = Directory.GetParent(currentDirectory)?.FullName;

        if (parentDirectory == null)
        {
            throw new InvalidOperationException("Parent directory could not be determined.");
        }

        var basePath = Path.Combine(parentDirectory, "Api");
        var builder = new ConfigurationBuilder().AddJsonFile(Path.Combine(basePath, "appsettings.json"), optional: false, reloadOnChange: true);
        _configuration = builder.Build();
    }

    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var migrationAssembly = typeof(ApplicationDbContext).Assembly.GetName().Name;
        DbContextOptionsBuilder<ApplicationDbContext> builder = new();
        _ = builder.UseSqlServer(
            connectionString: _configuration.GetConnectionString("DefaultConnection"),
            options => options.MigrationsAssembly(migrationAssembly).MigrationsHistoryTable(HistoryRepository.DefaultTableName, "app")
        );
        return new ApplicationDbContext(builder.Options);
    }
}
