namespace Bars.GkhCr.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    /// <summary>Маппинг для "Архив значений в мониторинге СМР"</summary>
    public class ArchiveMultiplyContragentSmrMap : BaseImportableEntityMap<ArchiveMultiplyContragentSmr>
    {
        
        public ArchiveMultiplyContragentSmrMap() : 
                base("Архив значений в мониторинге СМР с контрагентами", "CR_OBJ_CMP_ARCHIVE_MULTI_CONTR")
        {
        }

        protected override void Map()
        {
            Property(x => x.VolumeOfCompletion, "Объем выполнения").Column("VOLUME");
            Property(x => x.PercentOfCompletion, "Процент выполнения").Column("PERCENT");
            Property(x => x.CostSum, "Сумма расходов").Column("COST_SUM");

            Reference(x => x.TypeWorkCr, "Вид Работ").Column("TYPE_WORK_ID").Fetch();
            Reference(x => x.Contragent, "Контрагент").Column("CONTRAGENT_ID").Fetch();
        }
    }
}
