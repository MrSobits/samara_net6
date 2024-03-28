namespace Bars.GisIntegration.Base.Map.External.Dict.Oki
{
    using Bars.B4.DataAccess.ByCode;
    using Bars.GisIntegration.Base.Entities.External.Dict.Oki;

    using NHibernate.Mapping.ByCode;

    /// <summary>
    /// Маппинг для Bars.Ris.Contragent.Entities.CommunalService
    /// </summary>
    public class ExtCommunalServiceMap : BaseEntityMap<ExtCommunalService>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public ExtCommunalServiceMap() :
            base("NSI_COMMUNAL_SERVICE")
        {
            //Устанавливаем схему РИС
            this.Schema("NSI");

            this.Id(x => x.Id, m =>
            {
                m.Column("COMMUNAL_SERVICE_ID");
                m.Generator(Generators.Native);
            });
            this.Map(x => x.Value, "COMMUNAL_SERVICE");
            this.Map(x => x.DictCode, "DICT_CODE");
            this.Map(x => x.GisGuid, "GIS_GUID");
            this.Map(x => x.ChangedBy, "CHANGED_BY");
            this.Map(x => x.ChangedOn, "CHANGED_ON");
        }

    }
}
