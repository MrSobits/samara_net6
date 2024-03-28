namespace Bars.GkhGji.Regions.BaseChelyabinsk
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
                    "~/content/css/b4GjiChelyabinsk.css"
                });
        }
    }
}