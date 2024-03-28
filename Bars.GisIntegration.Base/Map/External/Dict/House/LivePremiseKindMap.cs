namespace Bars.GisIntegration.Base.Map.External.Dict.House
{
    using Bars.B4.DataAccess.ByCode;
    using Bars.GisIntegration.Base.Entities.External.Dict.House;

    using NHibernate.Mapping.ByCode;

    /// <summary>
    /// Маппинг для Bars.Ris.Contragent.Entities.LivePremiseKind
    /// </summary>
    public class LivePremiseKindMap : BaseEntityMap<LivePremiseKind>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public LivePremiseKindMap() :
            base("NSI_LIVE_PREMISE_KIND")
        {
            //Устанавливаем схему РИС
            this.Schema("NSI");

            this.Id(x => x.Id, m =>
            {
                m.Column("LIVE_PREMISE_KIND_ID");
                m.Generator(Generators.Native);
            });
            this.Map(x => x.Value, "LIVE_PREMISE_KIND");
            this.Map(x => x.DictCode, "DICT_CODE");
            this.Map(x => x.GisGuid, "GIS_GUID");
            this.Map(x => x.ChangedBy, "CHANGED_BY");
            this.Map(x => x.ChangedOn, "CHANGED_ON");
        }

    }
}
