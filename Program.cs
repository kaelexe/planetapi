using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using PlanetApi.Data;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

string dbHost = Environment.GetEnvironmentVariable("MYSQL_HOST") ?? "localhost";
string dbPort = Environment.GetEnvironmentVariable("MYSQL_PORT") ?? "3306";
string dbName = Environment.GetEnvironmentVariable("MYSQL_DATABASE") ?? "planetdb";
string dbUser = Environment.GetEnvironmentVariable("MYSQL_USER") ?? "planetuser";
string dbPassword = Environment.GetEnvironmentVariable("MYSQL_PASSWORD") ?? "planetpass";

string connectionString = $"Server={dbHost};Port={dbPort};Database={dbName};User={dbUser};Password={dbPassword};";

builder.Services.AddDbContext<PlanetContext>(options =>
{
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString),
        mySqlOptions =>
        {
            mySqlOptions.EnableRetryOnFailure();
        });
}, ServiceLifetime.Singleton);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
