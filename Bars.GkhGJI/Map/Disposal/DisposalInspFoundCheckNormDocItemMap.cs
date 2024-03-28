namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Маппинг для <see cref="DisposalInspFoundCheckNormDocItem"/>
    /// </summary>
	public class DisposalInspFoundCheckNormDocItemMap : BaseEntityMap<DisposalInspFoundCheckNormDocItem>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public DisposalInspFoundCheckNormDocItemMap() : 
                base("Требования НПА проверки", "GJI_DISPOSAL_INSP_FOUND_CHECK_NORM_DOC")
        {
        }
        
        /// <summary>
        /// Мап
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.DisposalInspFoundationCheck, "НПА проверки приказа ГЖИ").Column("FOUND_CHECK_ID").Fetch();
            this.Reference(x => x.NormativeDocItem, "Пункт нормативного документа").Column("DOC_ITEM_ID").NotNull().Fetch();
            this.Reference(x => x.Disposal, "Распоряжение ГЖИ").Column("DISPOSAL_ID").Fetch();
        }
    }
}
