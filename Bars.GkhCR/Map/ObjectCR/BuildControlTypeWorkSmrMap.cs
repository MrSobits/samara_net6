namespace Bars.GkhCr.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    /// <summary>Маппинг для "Архив значений в мониторинге СМР"</summary>
    public class BuildControlTypeWorkSmrMap : BaseImportableEntityMap<BuildControlTypeWorkSmr>
    {
        
        public BuildControlTypeWorkSmrMap() : 
                base("Стройконтроль работы объекта КР", "CR_OBJ_CMP_BUILD_CONTR")
        {
        }

        protected override void Map()
        {
            Property(x => x.VolumeOfCompletion, "Объем выполнения").Column("VOLUME");
            Property(x => x.PercentOfCompletion, "Процент выполнения").Column("PERCENT");
            Property(x => x.Latitude, "Широта").Column("LATITUDE");
            Property(x => x.Longitude, "Долгота").Column("LONGITUDE");
            Property(x => x.CostSum, "Сумма расходов").Column("COST_SUM");
            Property(x => x.Description, "Примечание").Column("DESCRIPTION");
            Property(x => x.MonitoringDate, "Дата контроля").Column("MONITORING_DATE");
            Property(x => x.DeadlineMissed, "Срыв сроков").Column("DEADLINE_MISSED");
            Reference(x => x.TypeWorkCrAddWork, "Этап работы").Column("STAGE_ID").Fetch();
            Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
            Reference(x => x.TypeWorkCr, "Вид Работ").Column("TYPE_WORK_ID").NotNull().Fetch();
            Reference(x => x.Contragent, "Контрагент").Column("CONTRAGENT_ID").Fetch();
            Reference(x => x.Controller, "Контрагент СК").Column("CONTRAGENT_SK_ID");
        }
    }
}
