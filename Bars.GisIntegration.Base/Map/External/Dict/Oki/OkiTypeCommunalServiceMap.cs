namespace Bars.GisIntegration.Base.Map.External.Dict.Oki
{
    using Bars.B4.DataAccess.ByCode;
    using Bars.GisIntegration.Base.Entities.External.Dict.Oki;

    using NHibernate.Mapping.ByCode;

    /// <summary>
    /// Маппинг для Bars.Ris.Contragent.Entities.OkiTypeCommunalService
    /// </summary>
    public class OkiTypeCommunalServiceMap : BaseEntityMap<OkiTypeCommunalService>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public OkiTypeCommunalServiceMap() :
            base("OKI_TYPE_COMMUNAL_SERVICE")
        {
            //Устанавливаем схему РИС
            this.Schema("DATA");

            this.Id(x => x.Id, m =>
            {
                m.Column("OKI_TYPE_COMMUNAL_SERVICE_ID");
                m.Generator(Generators.Native);
            });
            this.References(x => x.CommunalService, "COMMUNAL_SERVICE_ID");
            this.References(x => x.OkiType, "OKI_TYPE_ID");
            this.References(x => x.DataSupplier, "DATA_SUPPLIER_ID");
            this.Map(x => x.ChangedBy, "CHANGED_BY");
            this.Map(x => x.ChangedOn, "CHANGED_ON");
        }

    }
}
