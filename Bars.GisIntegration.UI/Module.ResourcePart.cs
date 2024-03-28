namespace Bars.GisIntegration.UI
{
    using Bars.B4.ResourceBundling;

    public partial class Module
    {
        /// <summary>
        /// Метод регистрации бандлов
        /// </summary>
        private void RegisterBundlers()
        {
            var bundler = this.Container.Resolve<IResourceBundler>();

            bundler.RegisterCssBundle("b4-all", new[]
                {
                    "~/content/css/risMain.css"
                });
#if !DEBUG
            bundler.RegisterScriptsBundle("external-libs", new[]
                {
                    "~/libs/B4/cryptopro/jsxmlsigner.js",
                    "~/libs/B4/cryptopro/xadessigner.js"
                });
#endif
        }
    }
}