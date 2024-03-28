namespace Bars.GisIntegration.Base.Map.External.Dict.House
{
    using Bars.B4.DataAccess.ByCode;
    using Bars.GisIntegration.Base.Entities.External.Dict.House;

    using NHibernate.Mapping.ByCode;

    /// <summary>
    /// Маппинг для Bars.Ris.Contragent.Entities.CommunalSource
    /// </summary>
    public class CommunalSourceMap : BaseEntityMap<CommunalSource>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public CommunalSourceMap() :
            base("NSI_COMMUNAL_SOURCE")
        {
            //Устанавливаем схему РИС
            this.Schema("NSI");

            this.Id(x => x.Id, m =>
            {
                m.Column("COMMUNAL_SOURCE_ID");
                m.Generator(Generators.Native);
            });
            this.Map(x => x.CommunalSourceName, "COMMUNAL_SOURCE");
            this.Map(x => x.DictCode, "DICT_CODE");
            this.Map(x => x.GisGuid, "GIS_GUID");
            this.Map(x => x.CommunalSourceShortName, "COMMUNAL_SOURCE_SHORT");
            this.Map(x => x.IsPuLink, "IS_PU_LINK");
            this.References(x => x.Okei, "OKEI_ID");
            this.Map(x => x.ChangedBy, "CHANGED_BY");
            this.Map(x => x.ChangedOn, "CHANGED_ON");
        }

    }
}
