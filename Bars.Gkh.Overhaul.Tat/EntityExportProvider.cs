namespace Bars.Gkh.Overhaul.Tat
{
    using Gkh.Entities.CommonEstateObject;
    using ImportExport;
    using Overhaul.Entities;

    public class EntityExportProvider : IEntityExportProvider
    {
        public void FillContainer(EntityExportContainer container)
        {
            container.Add(typeof(CommonEstateObject), "ООИ");
            container.Add(typeof(GroupType), "Тип группы ООИ");
        }
    }
}