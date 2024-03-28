namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;


    /// <summary>Маппинг для "Решение судебных участков"</summary>
    public class SPAccOwnerRealityObjectMap : BaseEntityMap<SPAccOwnerRealityObject>
    {

        public SPAccOwnerRealityObjectMap() :
                base("Дома на спецсчетах", "GJI_SPEC_ACC_OWNER_RO")
        {
        }

        protected override void Map()
        {
            Reference(x => x.SpecialAccountOwner, "OWNER_ID").Column("OWNER_ID").Fetch();
            Reference(x => x.RealityObject, "МКД").Column("RO_ID").Fetch();
            Reference(x => x.CreditOrg, "БАНК").Column("BANK_ID").Fetch();
            Property(x => x.DateEnd, "Дата закрытия").Column("DATE_END");
            Property(x => x.DateStart, "Дата открытия").Column("DATE_START");
            Property(x => x.SpacAccNumber, "Номер спецсчета").Column("ACC_NUM");
         
        }
    }
}
