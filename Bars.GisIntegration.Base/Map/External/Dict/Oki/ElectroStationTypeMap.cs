namespace Bars.GisIntegration.Base.Map.External.Dict.Oki
{
    using Bars.B4.DataAccess.ByCode;

    using Entities.External.Dict.Oki;

    using NHibernate.Mapping.ByCode;

    /// <summary>
    /// Маппинг для Bars.Ris.Contragent.Entities.ElectroStationType
    /// </summary>
    public class ElectroStationTypeMap : BaseEntityMap<ElectroStationType>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public ElectroStationTypeMap() :
            base("NSI_ELECTRO_STATION_TYPE")
        {
            //Устанавливаем схему РИС
            this.Schema("NSI");

            this.Id(x => x.Id, m =>
            {
                m.Column("ELECTRO_STATION_TYPE_ID");
                m.Generator(Generators.Native);
            });
            this.Map(x => x.Value, "ELECTRO_STATION_TYPE");
            this.Map(x => x.DictCode, "DICT_CODE");
            this.Map(x => x.GisGuid, "GIS_GUID");
            this.Map(x => x.ChangedBy, "CHANGED_BY");
            this.Map(x => x.ChangedOn, "CHANGED_ON");
        }

    }
}
