namespace Bars.Gkh.Navigation
{
    using Bars.B4;
    using Bars.Gkh.Entities.RealityObj;

    public class RealityObjectOutdoorMenuKey
    {
        public static string Key => nameof(RealityObjectOutdoor);

        public static string Description => "Меню карточки двора";
    }

    public class RealityObjectOutdoorMenuProvider : INavigationProvider
    {

        /// <inheritdoc />
        public string Key => RealityObjectOutdoorMenuKey.Key;

        /// <inheritdoc />
        public string Description => RealityObjectOutdoorMenuKey.Description;

        /// <inheritdoc />
        public void Init(MenuItem root)
        {
            root.Add("Общие сведения", "realityobjectoutdooredit/{0}/edit").AddRequiredPermission("Gkh.RealityObjectOutdoor.View");
        }
    }
}
