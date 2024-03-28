namespace Bars.GkhGji.LogMap.ActCheck
{
    using Bars.B4.Modules.NHibernateChangeLog;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class ActCheckAnnexLogMap : AuditLogMap<ActCheckAnnex>
    {
        public ActCheckAnnexLogMap()
        {
            this.Name("Акт проверки - Приложения");

            this.Description(x => x.ActCheck.DocumentNumber ?? "");

            this.MapProperty(x => x.Name, "DocumentNumber", "Номер документа");
            this.MapProperty(x => x.DocumentDate, "DocumentDate", "Дата документа");
            this.MapProperty(x => x.Description, "IssuedDefinition", "Описание");
            this.MapProperty(x => x.File.FullName, "File", "Файл");
        }
    }
}