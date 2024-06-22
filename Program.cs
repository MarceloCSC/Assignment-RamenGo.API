using RamenGo.API.Models;
using RamenGo.API.Repositories;
using RamenGo.API.Services;

namespace RamenGo.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            IConfigurationSection configSection = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppConfig");

            builder.Services.Configure<AppConfig>(configSection);

            builder.Services.AddSingleton<IRepository<Broth>, MockBrothRepository>();
            builder.Services.AddSingleton<IRepository<Protein>, MockProteinRepository>();
            builder.Services.AddSingleton<IRepository<Order>, MockOrderRepository>();
            builder.Services.AddSingleton<IOrdersService, OrdersService>();

            builder.Services.AddMvc(options => options.SuppressAsyncSuffixInActionNames = false);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(options => options.AddPolicy(name: "AllowAnyOrigins",
                                                                  policy => 
                                                                  {
                                                                      policy.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
                                                                  }));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseCors("AllowAnyOrigins");

            app.UseAuthorization();

            app.MapControllers();
        

            app.Run();
        }
    }
}