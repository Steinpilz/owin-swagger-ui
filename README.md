# Steinpilz.Owin.SwaggerUI

## Introduction

OWIN middleware to serve SwaggerUI web assets. 

Distributed as NuGet package `Steinpilz.Owin.SwaggerUI`

## Sample 

```csharp
using System;

namespace Steinpilz.Owin.SwaggerUI.ConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Microsoft.Owin.Hosting.WebApp.Start("http://localhost:5008", appBuilder =>
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
```

## FAQ

## Questions & Issues

Use built-in gitlab [issue tracker](https://github.com/Steinpilz/owin-swagger-ui/issues)

## Maintainers
@ivanbenko

## Contribution

* Setup development environment:

1. Clone the repo
2. ```.paket\paket restore``` 
3. ```build target=build```
