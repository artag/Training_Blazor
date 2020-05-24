using System.Drawing;
using Microsoft.Extensions.Hosting;
using Webview;

namespace BlazorDesktop
{
    internal static class HostExtensions
    {
        /// <summary>
        /// Run the host with the given webview.
        /// <para>
        ///  This runs both the main loop of the ASP .NET Core app and the main
        ///  loop of the <see cref="Webview" />. When the view is closed the
        ///  server is stopped
        /// </para>
        /// </summary>
        /// <param name="host">The host to run.</param>
        /// <param name="builder">The builder to connect to the app.</param>
        public static void RunWebview(this IHost host, WebviewBuilder builder)
        {
            host.Start();

            var content = new WebHostContent(host);
            builder.WithContent(content).Build().Run();
            host.StopAsync();
        }

        /// <summary>
        /// Run the host with a webview.
        /// <para>
        ///  This runs both the main loop of the ASP .NET Core app and the main
        ///  loop of the <see cref="Webview" />. When the view is closed the
        ///  server is stopped.
        /// </para>
        /// </summary>
        /// <param name="host">The host to run.</param>
        /// <param name="title">The title to use for the view.</param>
        /// <param name="size">The size of the view.</param>
        public static void RunWebview(this IHost host, string title = "", Size size = default)
        {
            RunWebview(host, new WebviewBuilder().WithTitle(title).WithSize(size));
        }
    }
}
