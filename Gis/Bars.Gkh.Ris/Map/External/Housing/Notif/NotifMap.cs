namespace Bars.Gkh.Ris.Map.External.Housing.Notif
{
    using Bars.B4.DataAccess.ByCode;
    using Bars.Gkh.Ris.Entities.External.Housing.Notif;

    using NHibernate.Mapping.ByCode;

    /// <summary>
    /// Маппинг
    /// </summary>
    public class NotifMap : BaseEntityMap<Notif>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public NotifMap() : base("NOTIF")
        {
            //Устанавливаем схему РИС
            this.Schema("DATA");

            this.Id(x => x.Id, m =>
            {
                m.Column("NOTIF_ID");
                m.Generator(Generators.Native);
            });
            this.References(x => x.DataSupplier, "DATA_SUPPLIER_ID");
            this.References(x => x.NotifType, "NOTIF_TYPE_ID");
            this.Map(x => x.NotifTopic, "notif_topic");
            this.Map(x => x.NotifContent, "notif_content");
            this.Map(x => x.NotifFrom, "notif_from");
            this.Map(x => x.NotifTo, "notif_to");
            this.Map(x => x.IsImportant, "is_important");
            this.Map(x => x.IsAll, "is_all");
            this.Map(x => x.IsSend, "is_send");
            this.Map(x => x.IsUnlim, "is_unlim");
            this.References(x => x.WorkType, "work_type_id");
            this.Map(x => x.WorkFrom, "work_from");
            this.Map(x => x.WorkTo, "work_to");
            this.Map(x => x.IsDel, "is_del");
            this.Map(x => x.OuterId, "outer_id");
            this.Map(x => x.ChangedBy, "CHANGED_BY");
        }
    }
}
