
namespace Bars.GkhCr.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    
    /// <summary>Маппинг для "Виды работ договора подряда КР"</summary>
    public class MassBuildContractWorkMap : BaseImportableEntityMap<MassBuildContractWork>
    {
        
        public MassBuildContractWorkMap() : 
                base("Виды работ договора подряда КР", "CR_MASS_BLD_CONTR_TYPE_WRK")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.MassBuildContract, "Договор подряда КР").Column("MASS_BC_ID").NotNull().Fetch();
            Reference(x => x.Work, "Вид работ").Column("WORK_ID").NotNull().Fetch();
            Property(x => x.Sum, "Сумма").Column("SUM");
        }
    }
}
