namespace Bars.GkhCr.Regions.Tatarstan.Navigation
{
    using Bars.B4;
    using Bars.B4.Navigation;

    public class NavigationProvider : BaseMainMenuProvider
    {
        /// <inheritdoc />
        public override void Init(MenuItem root)
        {
            var outdoorProgram = root.Add("Капитальный ремонт").Add("Программы благоустройства дворов");
            outdoorProgram.Add("Программы благоустройства дворов", "outdoorprogram").AddRequiredPermission("GkhCr.OutdoorProgram.View").WithIcon("programmCr"); ;
            outdoorProgram.Add("Объекты программ благоустройства дворов", "objectoutdoorcr").AddRequiredPermission("GkhCr.ObjectOutdoorCr.View").WithIcon("objectCr");

            var dictCr = root.Add("Справочники").Add("Капитальный ремонт");
            dictCr.Add("Виды работ по благоустройству дворов", "workrealityobjectoutdoor").AddRequiredPermission("GkhCr.Dict.WorkRealityObjectOutdoor.View");
            dictCr.Add("Элементы двора", "elementoutdoor").AddRequiredPermission("GkhCr.Dict.ElementOutdoor.View");
        }
    }
}
