using System;

namespace Steinpilz.Owin.SwaggerUI.ConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Microsoft.Owin.Hosting.WebApp.Start("http://localhost:5008/owin", appBuilder =>
            {
                appBuilder.UseSwaggerUI(
                    "/ui/swagger", 
                    opt => opt.UseSwaggerEndpoint("http://petstore.swagger.io/v2/swagger.json")
                    );
            }))
            {
                Console.ReadLine();
            }
        }
    }
}