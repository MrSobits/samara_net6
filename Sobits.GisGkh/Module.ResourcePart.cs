namespace Sobits.GisGkh
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
            
            bundler.RegisterScriptsBundle("external-libs", new[]
                {
                    "~/libs/B4/cryptopro/jsxmlsigner.js",
                    "~/libs/B4/cryptopro/xadessigner.js"
                });
        }
    }
}