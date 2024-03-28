namespace Bars.GisIntegration.Base.Map.External.Dict.House
{
    using Bars.B4.DataAccess.ByCode;
    using Bars.GisIntegration.Base.Entities.External.Dict.House;

    using NHibernate.Mapping.ByCode;

    /// <summary>
    /// Маппинг для Bars.Ris.Contragent.Entities.PremiseCategory
    /// </summary>
    public class PremiseCategoryMap : BaseEntityMap<PremiseCategory>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public PremiseCategoryMap() :
            base("NSI_PREMISE_CATEGORY")
        {
            //Устанавливаем схему РИС
            this.Schema("NSI");

            this.Id(x => x.Id, m =>
            {
                m.Column("PREMISE_CATEGORY_ID");
                m.Generator(Generators.Native);
            });
            this.Map(x => x.Value, "PREMISE_CATEGORY");
            this.Map(x => x.DictCode, "DICT_CODE");
            this.Map(x => x.GisGuid, "GIS_GUID");
            this.Map(x => x.ChangedBy, "CHANGED_BY");
            this.Map(x => x.ChangedOn, "CHANGED_ON");
        }

    }
}
