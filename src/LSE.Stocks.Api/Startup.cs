using LSE.Stocks.Api.Middleware;
using LSE.Stocks.Api.ServiceCollectionExtensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.IO;
using System.Reflection;

namespace LSE.Stocks.Api;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration) => Configuration = configuration;

    public void ConfigureServices(IServiceCollection services) 
        => services.AddEndpointsApiExplorer()
            .AddSwaggerGen(o => AddSwaggerDocumentation(o))
            .AddMediatorServices()
            .AddRepositories(Configuration)
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

    private static void AddSwaggerDocumentation(SwaggerGenOptions o)
    {
        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        o.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    }
}
