namespace Bars.Gkh1468
{
    using Entities;
    using Gkh.ImportExport;

    public class EntityExportContainer : IEntityExportProvider
    {
        public void FillContainer(Gkh.ImportExport.EntityExportContainer container)
        {
            container.Add(typeof(MetaAttribute), "Атрибут структуры паспорта 1468");
            container.Add(typeof(Part), "Раздел структуры паспорта 1468");
            container.Add(typeof(PassportStruct), "Cтруктура паспорта 1468");
        }
    }
}