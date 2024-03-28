namespace Bars.Gkh.Regions.Tatarstan.Navigation
{
    using Bars.B4;
    using Bars.Gkh.Navigation;

    public class RealityObjectOutdoorMenuProvider : INavigationProvider
    {
        public string Key => RealityObjectOutdoorMenuKey.Key;

        public string Description => RealityObjectOutdoorMenuKey.Description;

        /// <inheritdoc />
        public void Init(MenuItem root)
        {
            root.Add("Фото-архив", "realityobjectoutdooredit/{0}/image").AddRequiredPermission("Gkh.RealityObjectOutdoor.Register.Image.View").WithIcon("icon-camera");
            root.Add("Элементы двора до благоустройства", "realityobjectoutdooredit/{0}/element").AddRequiredPermission("Gkh.RealityObjectOutdoor.Register.Element.View").WithIcon("icon-table-gear");
        }
    }
}
