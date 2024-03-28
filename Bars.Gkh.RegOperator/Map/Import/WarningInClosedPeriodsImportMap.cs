namespace Bars.Gkh.RegOperator.Map.Import
{
    using System;
    using B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities.Import;

    /// <summary>
    /// Маппинг для сущности "Предупреждение при импорте в закрытый период"
    /// </summary>
    public class WarningInClosedPeriodsImportMap : BaseEntityMap<WarningInClosedPeriodsImport> 
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public WarningInClosedPeriodsImportMap() : 
            base("Предупреждение при импорте в закрытый период", "REGOP_WARNING_IN_CLOSED_PERIODS_IMPORT")
        {
        }

        /// <summary>
        /// Задать маппинг
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.Title, "Заголовок").Column("TITLE");
            this.Property(x => x.Message, "Сообщение").Column("MESSAGE");
            this.Reference(x => x.Task, "Задача, которая разбирала импорт, на сервере вычислений").Column("TASK_ID").Fetch();
        }
    }
}
