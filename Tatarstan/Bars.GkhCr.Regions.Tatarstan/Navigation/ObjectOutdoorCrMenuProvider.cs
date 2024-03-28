namespace Bars.GkhCr.Regions.Tatarstan.Navigation
{
    using Bars.B4;
    using Bars.GkhCr.Regions.Tatarstan.Entities;
    using Bars.GkhCr.Regions.Tatarstan.Entities.ObjectOutdoorCr;

    public class ObjectOutdoorCrMenuProvider : INavigationProvider
    { /// <inheritdoc />
        public string Key => nameof(ObjectOutdoorCr);

        /// <inheritdoc />
        public string Description => "Меню объекта программы благоустройства";

        /// <inheritdoc />
        public void Init(MenuItem root)
        {
            root.Add("Паспорт объекта", "objectoutdoorcredit/{0}/edit").AddRequiredPermission("GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrEdit.View").WithIcon("icon-book-addresses");
            root.Add("Виды работ", "objectoutdoorcredit/{0}/typeworkrealityobjectoutdoor").AddRequiredPermission("GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrTypeWork.View").WithIcon("icon-wrench-orange");
        }
    }
}
