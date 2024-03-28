namespace Bars.GisIntegration.Base.Map.External.Housing.Oki
{
    using Bars.B4.DataAccess.ByCode;
    using Bars.GisIntegration.Base.Entities.External.Housing.OKI;

    using NHibernate.Mapping.ByCode;

    /// <summary>
    /// Маппинг для Bars.Ris.Contragent.Entities.OkiCommunalService
    /// </summary>
    public class OkiCommunalServiceMap : BaseEntityMap<OkiCommunalService>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public OkiCommunalServiceMap() :
            base("OKI_OBJECT_COMMUNAL_SERVICE")
        {
            //Устанавливаем схему РИС
            this.Schema("DATA");

            this.Id(x => x.Id, m =>
            {
                m.Column("OKI_OBJECT_COMMUNAL_SERVICE_ID");
                m.Generator(Generators.Native);
            });
            this.References(x => x.DataSupplier, "DATA_SUPPLIER_ID");
            this.References(x => x.OkiObject, "OKI_OBJECT_ID");
            this.References(x => x.CommunalService, "COMMUNAL_SERVICE_ID");
            this.Map(x => x.ChangedBy, "CHANGED_BY");
            this.Map(x => x.ChangedOn, "CHANGED_ON");
        }

    }
}
