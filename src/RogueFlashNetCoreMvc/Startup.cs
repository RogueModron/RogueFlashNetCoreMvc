using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RogueFlashNetCoreMvc.Model;
using System.IO;
using RogueFlashNetCoreMvc.Support;

namespace RogueFlashNetCoreMvc
{
    public class Startup
    {
        private IConfigurationRoot configuration = null;


        public Startup()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.SetBasePath(Directory.GetCurrentDirectory());
            configurationBuilder.AddJsonFile("settings.json");
            configuration = configurationBuilder.Build();
        }


        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(LogLevel.Debug);
            loggerFactory.AddDebug(LogLevel.Debug);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            app.UseStatusCodePagesWithRedirects("/error");

            app.UseResponseCompression();
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}",
                    defaults: new {
                        controller = "Start",
                        action = "Index"
                    });
                routes.MapRoute(
                    name: "ajax",
                    template: "Ajax/{controller}/{action}",
                    defaults: new {
                        action = "Index"
                    });
            });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();

            services.AddEntityFramework();

            var connectionString = configuration["ConnectionStrings:Default"];
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

            services.AddResponseCompression(options => options.Providers.Add<GzipCompressionProvider>());
            
            services.AddMvc().AddMvcOptions(options => options.ModelMetadataDetailsProviders.Insert(0, new EmptyStringDisplayMetadataProvider()));
        }
    }
}
