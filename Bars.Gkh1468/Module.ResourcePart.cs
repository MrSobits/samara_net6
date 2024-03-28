namespace Bars.Gkh1468
{
    using Bars.B4.ResourceBundling;

    public partial class Module
    {
        /// <summary>
        /// Метод регистрации бандлов
        /// </summary>
        private void RegisterBundlers()
        {
            var bundler = Container.Resolve<IResourceBundler>();
            bundler.RegisterCssBundle("b4-all", new[] { "~/content/css/gkh1468.css" });
            bundler.RegisterScriptsBundle("external-libs", new[] { "~/libs/B4/CryptoPro.js" });
        }                                                    
    }                                                        
}                                                            
