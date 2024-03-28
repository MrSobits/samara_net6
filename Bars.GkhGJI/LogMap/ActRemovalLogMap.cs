namespace Bars.GkhGji.LogMap
{
    using B4.Modules.NHibernateChangeLog;
    using B4.Utils;
    using Entities;

    public class ActRemovalLogMap : AuditLogMap<ActRemoval>
    {
        public ActRemovalLogMap()
        {
            this.Name("Документы ГЖИ");
            this.Description(x => x.DocumentNumber ?? "");

            this.MapProperty(x => x.Description, "PhysicalPersonInfo", "Описание");
            this.MapProperty(x => x.TypeRemoval, "TypeRemoval", "Нарушения устранены", x => x.Return(y => y.GetDisplayName()));
        }
    }
}