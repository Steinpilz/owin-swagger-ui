using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Owin;
using Steinpilz.Owin.WebAssets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steinpilz.Owin.SwaggerUI
{
    public static class AppBuilderExtensions
    {
        public static IAppBuilder UseSwaggerUI(
            this IAppBuilder appBuilder, 
            string mountPath, 
            Action<SwaggerUIOptions> configuration = null)
        {
            var options = new SwaggerUIOptions();
            configuration?.Invoke(options);

            return appBuilder
                .UseWebAssets(mountPath, opt => opt
                    .AddWebAssetProcessor(new WebAssetProcessor(options.SwaggerEndpoint))
                    .UseFileSystem(new EmbeddedResourceFileSystem(
                        typeof(AppBuilderExtensions).Assembly,
                        "Steinpilz.Owin.SwaggerUI.assets"
                        ))
                );
        }
    }

    public class SwaggerUIOptions
    {
        public Func<IOwinRequest, string> SwaggerEndpoint { get; private set; } = req => "swagger.json";

        public SwaggerUIOptions UseSwaggerEndpoint(string endpointUrl)
        {
            SwaggerEndpoint = req => endpointUrl;
            return this;
        }

        public SwaggerUIOptions UseSwaggerEndpoint(Func<IOwinRequest, string> endpointUrlFunc)
        {
            SwaggerEndpoint = endpointUrlFunc;
            return this;
        }
    }

    class WebAssetProcessor : IWebAssetProcessor
    {
        private readonly Func<IOwinRequest, string> endpointUrlFunc;

        public WebAssetProcessor(Func<IOwinRequest, string> endpointUrl)
        {
            this.endpointUrlFunc = endpointUrl;
        }

        public async Task<WebAsset> ProcessAsync(WebAsset webAsset, IOwinRequest request)
        {
            var endpointUrl = this.endpointUrlFunc?.Invoke(request) ?? ""; 

            // take care about reverse proxy hosting scenarios
            var virtualFolder = request.PathBase.Value;
            var stripPath = request.Headers["X-Forwarded-Strip"] ?? "";
            var prefix = request.Headers["X-Forwarded-Prefix"] ?? "";

            if (virtualFolder.StartsWith(stripPath, StringComparison.OrdinalIgnoreCase))
                virtualFolder = virtualFolder.Remove(0, stripPath.Length);

            if (!virtualFolder.StartsWith("/"))
                virtualFolder += "/";

            var baseHref = prefix + virtualFolder;
            if (!baseHref.EndsWith("/"))
                baseHref += "/";

            return webAsset.WithNewContent(await webAsset.Content.ReplaceAsync(
                new[] {
                    ("{BASE_HREF}", baseHref),
                    ("{ENDPOINT_URL}", endpointUrl),
                }).ConfigureAwait(false));
        }
    }
}
