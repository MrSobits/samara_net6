namespace Bars.GisIntegration.Base.Map.External.Housing.Notif
{
    using Bars.B4.DataAccess.ByCode;
    using Bars.GisIntegration.Base.Entities.External.Housing.Notif;

    using NHibernate.Mapping.ByCode;

    /// <summary>
    /// Маппинг
    /// </summary>
    public class NotifAddressMap : BaseEntityMap<NotifAddress>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public NotifAddressMap() :
            base("NOTIF_ADDRESS")
        {
            //Устанавливаем схему
            this.Schema("DATA");

            this.Id(x => x.Id, m =>
            {
                m.Column("NOTIF_ADDRESS_ID");
                m.Generator(Generators.Native);
            });
            this.References(x => x.DataSupplier, "DATA_SUPPLIER_ID");
            this.References(x => x.Notif, "NOTIF_ID");
            this.References(x => x.House, "HOUSE_ID");
            this.References(x => x.Premise, "PREMISE_ID");
            this.Map(x => x.ChangedBy, "CHANGED_BY");
        }
    }
}
