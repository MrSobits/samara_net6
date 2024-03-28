/// <mapping-converter-backup>
/// namespace Bars.GkhRf.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhRf.Entities;
/// 
///     public class ViewPaymentMap : PersistentObjectMap<ViewPayment>
///     {
///         public ViewPaymentMap()
///             : base("VIEW_RF_PAYMENT")
///         {
///             Map(x => x.ChargePopulationCr, "CR_CHARGE");
///             Map(x => x.ChargePopulationHireRf, "HIRE_RF_CHARGE");
///             Map(x => x.ChargePopulationCr185, "CR185_CHARGE");
///             Map(x => x.ChargePopulationBldRepair, "BLD_REPAIR_CHARGE");
///             Map(x => x.PaidPopulationCr, "CR_PAID");
///             Map(x => x.PaidPopulationHireRf, "HIRE_RF_PAID");
///             Map(x => x.PaidPopulationCr185, "CR185_PAID");
///             Map(x => x.PaidPopulationBldRepair, "BLD_REPAIR_PAID");
///             Map(x => x.MunicipalityId, "MU_ID");
///             Map(x => x.MunicipalityName, "MU_NAME");
///             Map(x => x.Address, "ADDRESS");
///             Map(x => x.RealityObjectId, "RO_ID");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhRf.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhRf.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhRf.Entities.ViewPayment"</summary>
    public class ViewPaymentMap : PersistentObjectMap<ViewPayment>
    {
        
        public ViewPaymentMap() : 
                base("Bars.GkhRf.Entities.ViewPayment", "VIEW_RF_PAYMENT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ChargePopulationCr, "Начислено населению, тип оплаты КР").Column("CR_CHARGE");
            Property(x => x.ChargePopulationHireRf, "Начислено населению, тип оплаты найм Рег.Фонда").Column("HIRE_RF_CHARGE");
            Property(x => x.ChargePopulationCr185, "Начислено населению, тип оплаты Кап. Ремонт по 185 ФЗ").Column("CR185_CHARGE");
            Property(x => x.ChargePopulationBldRepair, "Начислено населению, тип оплаты ремонт жилого здания").Column("BLD_REPAIR_CHARGE");
            Property(x => x.PaidPopulationCr, "Оплачено населением, тип оплаты Кап. Рем.").Column("CR_PAID");
            Property(x => x.PaidPopulationHireRf, "Оплачено населением, тип оплаты найм Рег.Фонда").Column("HIRE_RF_PAID");
            Property(x => x.PaidPopulationCr185, "Оплачено населением, тип оплаты Кап. Ремонт по 185 ФЗ").Column("CR185_PAID");
            Property(x => x.PaidPopulationBldRepair, "Оплачено населением, тип оплаты ремонт жилого здания").Column("BLD_REPAIR_PAID");
            Property(x => x.MunicipalityId, "Идентификатор муниципального образования").Column("MU_ID");
            Property(x => x.MunicipalityName, "Наименование муниципального образования").Column("MU_NAME");
            Property(x => x.Address, "Адрес жилого дома").Column("ADDRESS");
            Property(x => x.RealityObjectId, "Идентификатор жилого дома").Column("RO_ID");
        }
    }
}
