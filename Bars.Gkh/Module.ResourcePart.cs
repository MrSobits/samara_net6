namespace Bars.Gkh
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
                    "~/content/css/b4GkhMain.css"
                });

            bundler.RegisterScriptsBundle("external-libs", new[]
                {
                    "~/libs/Yandex/YandexMap.js",
                    "~/libs/SignalR/signalr.min.js",
                    "~/libs/Gkh/Init.js",
                    "~/libs/Gkh/Config.js",
                    "~/libs/B4/AjaxRequestOverrides.js",
                    "~/libs/B4/ToolTip.js",
                    "~/libs/B4/DateFix.js",
                    "~/libs/B4/WindowOverride.js",
                    "~/libs/B4/TabPanelOverride.js",
                    "~/libs/B4/ComboBoxOverride.js",
                    "~/libs/B4/ux/Highchart/highstock.js",
                    "~/libs/B4/ux/Highchart/highcharts-more.js",
                    "~/libs/B4/ux/Highchart/modules/solid-gauge.js",
                    "~/libs/B4/ux/Highchart/modules/variable-pie.js",
                });
        }
    }
}