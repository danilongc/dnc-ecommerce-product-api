using System.Reflection;
using DNCEcommerceApi;
using DNCEcommerceApi.Data;
using DNCEcommerceApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "DNCEcommerceAPi", Version = "v1" });
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });

        //Configure DB Connection
        string? mysqlDbConnectionString = builder.Configuration.GetConnectionString("MysqlDbConnectionString");
        builder.Services.AddDbContext<AppDbContext>(opts => opts.UseMySql(mysqlDbConnectionString, ServerVersion.AutoDetect(mysqlDbConnectionString)));

        builder.Services.AddScoped<IProductService, ProductService>();
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        var logger = builder.Logging.Services.BuildServiceProvider().GetRequiredService<ILogger<Program>>();
        app.UseMiddleware<ExceptionMiddleware>(logger);

        app.UseAuthorization();

        app.MapControllers();

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.Migrate();
        }

        app.Run();
    }
}