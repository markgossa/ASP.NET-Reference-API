using LSE.Stocks.Api.Middleware;
using LSE.Stocks.Api.ServiceCollectionExtensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LSE.Stocks.Api;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration) => Configuration = configuration;

    public static void ConfigureServices(IServiceCollection services) 
        => services.AddEndpointsApiExplorer()
            .AddSwaggerGen()
            .RegisterServices()
            .AddControllers();

    public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();
        app.UseCustomExceptionHandler();

        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}
