using System;

namespace Steinpilz.Owin.SwaggerUI.ConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Microsoft.Owin.Hosting.WebApp.Start("http://localhost:5008", appBuilder =>
            {
                appBuilder.UseSwaggerUI("/ui/swagger", opt => opt.UseSwaggerEndpoint("https://qdc.apps.stein-pilz.com/swagger.yaml"));
            }))
            {
                Console.ReadLine();
            }
        }
    }
}