namespace Bars.Gkh.RegOperator.Map.Import
{
    using System;
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities.Import;

    /// <summary>
    /// Маппинг для сущности "Шапка импорта в закрытый период"
    /// </summary>
    public class HeaderOfClosedPeriodsImportMap : BaseEntityMap<HeaderOfClosedPeriodsImport>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public HeaderOfClosedPeriodsImportMap() :
            base("Шапка импорта в закрытый период", "REGOP_HEADER_OF_CLOSED_PERIODS_IMPORT")
        {
        }

        /// <summary>
        /// Задать маппинг
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.Task, "Задача, которая разбирала импорт, на сервере вычислений").Column("TASK_ID").Fetch();
            this.Reference(x => x.Period, "Период").Column("PERIOD_ID").Fetch();
        }
    }
}
