using ApiResponse.Controllers;
using ApiResponse.Core.Abstractions;
using ApiResponse.Infrastructure;

namespace ApiResponse
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.AddCassandraContext("responses");

            builder.Services.AddScoped<IResponsesDbService, CassandraService>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            #pragma warning disable ASP0014
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.Run();
        }
    }
}
