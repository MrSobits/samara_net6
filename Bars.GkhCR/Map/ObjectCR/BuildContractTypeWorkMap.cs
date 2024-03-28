namespace Bars.GkhCr.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    
    /// <summary>Маппинг для "Виды работ договора подряда КР"</summary>
    public class BuildContractTypeWorkMap : BaseImportableEntityMap<BuildContractTypeWork>
    {
        
        public BuildContractTypeWorkMap() : 
                base("Виды работ договора подряда КР", "CR_BLD_CONTR_TYPE_WRK")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.BuildContract, "Договор подряда КР").Column("BUILD_CONTRACT_ID").NotNull().Fetch();
            Reference(x => x.TypeWork, "Вид работ").Column("TYPE_WORK_ID").NotNull().Fetch();
            Property(x => x.Sum, "Сумма").Column("SUM");
        }
    }
}
