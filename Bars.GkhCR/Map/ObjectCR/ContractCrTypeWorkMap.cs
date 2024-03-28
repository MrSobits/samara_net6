namespace Bars.GkhCr.Map
{
    using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;

    /// <summary>Маппинг для "Виды работ договора подряда КР"</summary>
    public class ContractCrTypeWorkMap : BaseImportableEntityMap<ContractCrTypeWork>
    {
        public ContractCrTypeWorkMap()
            :base("Виды работ договора подряда КР", "CR_CONTR_CR_TYPE_WRK")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.ContractCr, "Договор на услуги").Column("CONTRACT_CR_ID").NotNull().Fetch();
            this.Reference(x => x.TypeWork, "Вид работ").Column("TYPE_WORK_ID").NotNull().Fetch();
            this.Property(x => x.Sum, "Сумма").Column("SUM");
        }
    }
}