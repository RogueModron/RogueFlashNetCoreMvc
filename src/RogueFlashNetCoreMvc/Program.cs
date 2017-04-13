using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace RogueFlashNetCoreMvc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var hostBuilder = new WebHostBuilder();
            hostBuilder.UseKestrel();
            hostBuilder.UseIISIntegration();
            hostBuilder.UseStartup<Startup>();

            hostBuilder.UseContentRoot(Directory.GetCurrentDirectory());

            hostBuilder.Build().Run();
        }
    }
}
