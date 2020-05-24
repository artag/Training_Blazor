using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Webview;

namespace BlazorDesktop
{
    internal class WebHostContent : IContent
    {
        private string _address;

        /// <summary>
        /// Get a URI for the wrapped <see cref="IWebHost" />
        /// </summary>
        /// <returns>The frist address in the server.</returns>
        public string ToUri() => _address;

        /// <summary>
        /// Create a Content Provider for the given <see cref="IWebHost" />.
        /// </summary>
        /// <param name="host">The host to connect to</param>
        public WebHostContent(IHost host)
        {
            var features = host.Services.GetRequiredService<IServer>().Features.Get<IServerAddressesFeature>();
            //var features = host.ServerFeatures.Get<IServerAddressesFeature>();
            _address = features.Addresses.FirstOrDefault();
        }
    }
}
