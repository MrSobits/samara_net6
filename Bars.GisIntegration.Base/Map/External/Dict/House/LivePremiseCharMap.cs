namespace Bars.GisIntegration.Base.Map.External.Dict.House
{
    using Bars.B4.DataAccess.ByCode;
    using Bars.GisIntegration.Base.Entities.External.Dict.House;

    using NHibernate.Mapping.ByCode;

    /// <summary>
    /// Маппинг для Bars.Ris.Contragent.Entities.LivePremiseChar
    /// </summary>
    public class LivePremiseCharMap : BaseEntityMap<LivePremiseChar>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public LivePremiseCharMap() :
            base("NSI_LIVE_PREMISE_CHAR")
        {
            //Устанавливаем схему РИС
            this.Schema("NSI");

            this.Id(x => x.Id, m =>
            {
                m.Column("LIVE_PREMISE_CHAR_ID");
                m.Generator(Generators.Native);
            });
            this.Map(x => x.Value, "LIVE_PREMISE_CHAR");
            this.Map(x => x.DictCode, "DICT_CODE");
            this.Map(x => x.GisGuid, "GIS_GUID");
            this.Map(x => x.ChangedBy, "CHANGED_BY");
            this.Map(x => x.ChangedOn, "CHANGED_ON");
        }

    }
}
