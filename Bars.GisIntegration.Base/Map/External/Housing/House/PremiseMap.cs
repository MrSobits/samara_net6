namespace Bars.GisIntegration.Base.Map.External.Housing.House
{
    using Bars.B4.DataAccess.ByCode;
    using Bars.GisIntegration.Base.Entities.External.Housing.House;

    using NHibernate.Mapping.ByCode;

    /// <summary>
    /// Маппинг для Bars.Ris.Contragent.Entities.Premise
    /// </summary>
    public class PremiseMap : BaseEntityMap<Premise>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public PremiseMap() :
            base("GF_PREMISE")
        {
            //Устанавливаем схему РИС
            this.Schema("DATA");

            this.Id(x => x.Id, m =>
            {
                m.Column("PREMISE_ID");
                m.Generator(Generators.Native);
            });
            this.References(x => x.DataSupplier, "DATA_SUPPLIER_ID");
            this.References(x => x.PremiseCategory, "PREMISE_CATEGORY_ID");
            this.Map(x => x.PremiseGuid, "PREMISE_GUID");
            this.Map(x => x.PremiseNumber, "PREMISE_NUMBER");
            this.Map(x => x.CadastrNumber, "CADASTR_NUMBER");
            this.Map(x => x.IsGknNone, "IS_GKN_NONE");
            this.Map(x => x.Floor, "FLOOR");
            this.Map(x => x.EndDate, "END_DATE");
            this.Map(x => x.TotalSquare, "TOTAL_SQUARE");
            this.References(x => x.Porch, "PORCH_ID");
            this.References(x => x.House, "HOUSE_ID");
            this.References(x => x.LivePremiseChar, "LIVE_PREMISE_CHAR_ID");
            this.Map(x => x.EgrpNumber, "EGRP_NUMBER");
            this.Map(x => x.LiveSquare, "LIVE_SQUARE");
            this.Map(x => x.NonliveOtherChar, "NONLIVE_OTHER_CHAR");
            this.Map(x => x.IsCommonProperty, "IS_COMMON_NONLIVE");
            this.Map(x => x.ChangedBy, "CHANGED_BY");
            this.Map(x => x.ChangedOn, "CHANGED_ON");
        }

    }
}
