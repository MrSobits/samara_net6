namespace Bars.GkhCr.LogMap
{
    using Bars.B4.Modules.NHibernateChangeLog;
    using Bars.B4.Utils;
    using Bars.GkhCr.Entities;

    public class DocumentWorkCrLogMap : AuditLogMap<DocumentWorkCr>
    {
        public DocumentWorkCrLogMap()
        {
            Name("Мониторинг СМР - Документы");

            Description(x => x.ReturnSafe(y => y.ObjectCr.RealityObject.Address));

            MapProperty(x => x.Contragent, "Contragent", "Контрагент", x => x.Return(y => y.Name));
            MapProperty(x => x.DocumentName, "DocumentName", "Документ");
            MapProperty(x => x.DocumentNum, "DocumentNum", "Номер документа");
            MapProperty(x => x.DateFrom, "DateFrom", "Дата документа");
            MapProperty(x => x.File, "File", "Файл", x => x.Return(y => y.Name));
            MapProperty(x => x.Description, "Description", "Описание");
        }
    }

}
