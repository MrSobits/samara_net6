namespace Bars.GkhGji.LogMap.Resolution
{
    using B4.Modules.NHibernateChangeLog;
    using B4.Utils;
    using Entities;

    public class ResolutionDisputeLogMap : AuditLogMap<ResolutionDispute>
    {
        public ResolutionDisputeLogMap()
        {
            this.Name("Постановление - Оспаривания");
            this.Description(x => x.Resolution.DocumentNumber ?? "");

            this.MapProperty(x => x.Appeal, "Appeal", "Постановление обжаловано", x => x.Return(y => y).GetDisplayName());
            this.MapProperty(x => x.ProsecutionProtest, "ProsecutionProtest", "Протест прокуратуры");
            this.MapProperty(x => x.Court.Name, "Court", "Вид суда");
            this.MapProperty(x => x.Instance.Name, "Instance", "Инстанция");
            this.MapProperty(x => x.Lawyer.Fio, "Lawyer", "Юрист");
            this.MapProperty(x => x.CourtVerdict.Name, "CourtVerdict", "Решение суда");
            this.MapProperty(x => x.DocumentNum, "DocumentNum", "Номер документа", x => x.Return(y => y ?? ""));
            this.MapProperty(x => x.DocumentDate, "DocumentDate", "Дата документа");
            this.MapProperty(x => x.File.FullName, "File", "Файл");
            this.MapProperty(x => x.Description, "Description", "Описание");
        }
    }
}