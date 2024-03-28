namespace Bars.GkhCr.Map
{
    using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    
    /// <summary>Маппинг для "Виды работ договора подряда КР"</summary>
    public class SpecialBuildContractTypeWorkMap : BaseImportableEntityMap<SpecialBuildContractTypeWork>
    {
        public SpecialBuildContractTypeWorkMap() : 
                base("Виды работ договора подряда КР", "CR_SPECIAL_BLD_CONTR_TYPE_WRK")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.Sum, "Сумма").Column("SUM");

            this.Reference(x => x.BuildContract, "Договор подряда КР").Column("BUILD_CONTRACT_ID").NotNull().Fetch();
            this.Reference(x => x.TypeWork, "Вид работ").Column("TYPE_WORK_ID").NotNull().Fetch();
        }
    }
}
