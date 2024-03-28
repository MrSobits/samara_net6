namespace Bars.Gkh.RegOperator.Regions.Tyumen
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
            bundler.RegisterCssBundle("b4-all", new[]
                {
                    "~/libs/content/css/IconButton.css"
                });
        }                                                    
    }                                                        
}                                                            
