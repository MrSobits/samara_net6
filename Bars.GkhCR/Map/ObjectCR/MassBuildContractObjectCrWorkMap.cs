
namespace Bars.GkhCr.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    
    /// <summary>Маппинг для "Виды работ договора подряда КР"</summary>
    public class MassBuildContractObjectCrWorkMap : BaseEntityMap<MassBuildContractObjectCrWork>
    {
        
        public MassBuildContractObjectCrWorkMap() : 
                base("Работа объекта КР договора подряда КР", "CR_MASSBC_OBJ_CR_WORK")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.MassBuildContractObjectCr, "Договор подряда КР").Column("MASS_BC_OBJCR_ID").NotNull().Fetch();
            Reference(x => x.Work, "Вид работ").Column("WORK_ID").NotNull().Fetch();
            Property(x => x.Sum, "Сумма").Column("SUM");
        }
    }
}
