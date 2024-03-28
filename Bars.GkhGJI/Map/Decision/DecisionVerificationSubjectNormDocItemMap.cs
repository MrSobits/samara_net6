namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Маппинг для <see cref="DecisionVerificationSubjectNormDocItem"/>
    /// </summary>
	public class DecisionVerificationSubjectNormDocItemMap : BaseEntityMap<DecisionVerificationSubjectNormDocItem>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public DecisionVerificationSubjectNormDocItemMap() : 
                base("Требования НПА проверки", "GJI_DECISION_VERIFSUBJ_NORM_DOC")
        {
        }
        
        /// <summary>
        /// Мап
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.DecisionVerificationSubject, "НПА проверки приказа ГЖИ").Column("DECISION_VERIFSUBJ_ID").NotNull().Fetch();
            this.Reference(x => x.NormativeDocItem, "Пункт нормативного документа").Column("DOC_ITEM_ID").NotNull().Fetch();
        }
    }
}
