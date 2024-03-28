namespace Bars.GisIntegration.Base.Map.External.Housing.Notif
{
    using Bars.B4.DataAccess.ByCode;
    using Bars.GisIntegration.Base.Entities.External.Housing.Notif;

    using NHibernate.Mapping.ByCode;

    /// <summary>
    /// Маппинг
    /// </summary>
    public class NotifDocMap : BaseEntityMap<NotifDoc>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public NotifDocMap() :
            base("NOTIF_DOC")
        {
            //Устанавливаем схему РИС
            this.Schema("DATA");

            this.Id(x => x.Id, m =>
            {
                m.Column("NOTIF_DOC_ID");
                m.Generator(Generators.Native);
            });
            this.References(x => x.DataSupplier, "DATA_SUPPLIER_ID");
            this.References(x => x.Notif, "NOTIF_ID");
            this.References(x => x.Attachment, "ATTACHMENT_ID");
            this.Map(x => x.ChangedBy, "CHANGED_BY");
        }
    }
}
