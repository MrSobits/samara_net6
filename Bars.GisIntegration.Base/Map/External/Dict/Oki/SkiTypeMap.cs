namespace Bars.GisIntegration.Base.Map.External.Dict.Oki
{
    using Bars.B4.DataAccess.ByCode;
    using Bars.GisIntegration.Base.Entities.External.Dict.Oki;

    using NHibernate.Mapping.ByCode;

    /// <summary>
    /// Маппинг для Bars.Ris.Contragent.Entities.SkiType
    /// </summary>
    public class SkiTypeMap : BaseEntityMap<SkiType>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public SkiTypeMap() :
            base("NSI_SKI_TYPE")
        {
            //Устанавливаем схему РИС
            this.Schema("NSI");

            this.Id(x => x.Id, m =>
            {
                m.Column("SKI_TYPE_ID");
                m.Generator(Generators.Native);
            });
            this.Map(x => x.SkiTypeName, "SKI_TYPE");
            this.Map(x => x.SkiTypeShortName, "SKI_TYPE_SHORT");
            this.Map(x => x.DictCode, "DICT_CODE");
            this.References(x => x.Okei, "OKEI_ID");
            this.References(x => x.CommunalSource, "COMMUNAL_SOURCE_ID");
            this.Map(x => x.GisGuid, "GIS_GUID");
            this.Map(x => x.ChangedBy, "CHANGED_BY");
            this.Map(x => x.ChangedOn, "CHANGED_ON");
        }

    }
}
