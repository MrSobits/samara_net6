namespace Bars.Gkh.Gis
{
    using B4.ResourceBundling;

    public partial class Module
    {
        /// <summary>
        /// Метод регистрации бандлов
        /// </summary>
        private void RegisterBundlers()
        {
            var bundler = Container.Resolve<IResourceBundler>();
            bundler.RegisterCssBundle("b4-all", new[]
                {
                    "~/content/css/b4GisMain.css",
                    "~/content/css/b4KP60Protocol.css"
                });
            bundler.RegisterScriptsBundle("external-libs", new[]
                {
                    "~/libs/B4/JSTreeGraph.js",
                    "~/libs/d3/d3.min.js",
#if !DEBUG
                "~/libs/Piwik/analytics.js"
#endif
                });
        }
    }
}