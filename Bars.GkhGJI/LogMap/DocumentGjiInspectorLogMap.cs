namespace Bars.GkhGji.LogMap
{
    using B4.Modules.NHibernateChangeLog;
    using Entities;

    public class DocumentGjiInspectorLogMap : AuditLogMap<DocumentGjiInspector>
    {
        public DocumentGjiInspectorLogMap()
        {
            this.Name("Инспекторы документа ГЖИ");
            this.Description(x => x.DocumentGji.DocumentNumber ?? "");

            this.MapProperty(x => x.Inspector.Fio, "Inspector", "Инспектор");
        }
    }
}