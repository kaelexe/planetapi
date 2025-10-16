using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using PlanetApi.Models;

namespace PlanetApi.Data;

public class PlanetContext : DbContext
{
    public PlanetContext(DbContextOptions<PlanetContext> options) : base(options) { }

    public DbSet<TaskItem> Tasks { get; set; }
}

// Design-time factory for migrations
public class PlanetContextFactory : IDesignTimeDbContextFactory<PlanetContext>
{
    public PlanetContext CreateDbContext(string[] args)
    {
        // Load environment variables for design-time
        Env.Load();

        string dbHost = Environment.GetEnvironmentVariable("MYSQL_HOST") ?? "localhost";
        string dbPort = Environment.GetEnvironmentVariable("MYSQL_PORT") ?? "3306";
        string dbName = Environment.GetEnvironmentVariable("MYSQL_DATABASE") ?? "planetdb";
        string dbUser = Environment.GetEnvironmentVariable("MYSQL_USER") ?? "planetuser";
        string dbPassword = Environment.GetEnvironmentVariable("MYSQL_PASSWORD") ?? "planetpass";

        string connectionString = $"Server={dbHost};Port={dbPort};Database={dbName};User={dbUser};Password={dbPassword};";

        var optionsBuilder = new DbContextOptionsBuilder<PlanetContext>();
        optionsBuilder.UseMySql(
            connectionString,
            ServerVersion.AutoDetect(connectionString),
            mySqlOptions => mySqlOptions.EnableRetryOnFailure()
        );

        return new PlanetContext(optionsBuilder.Options);
    }
}
