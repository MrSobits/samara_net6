namespace Bars.Gkh.RegOperator.Entities.Import
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Tasks.Common.Entities;
    using Gkh.Entities;

    /// <summary>
    /// Шапка импорта в закрытый период
    /// </summary>
    /// <remarks>
    /// Необходима для возможности запустить импорт повторно.
    /// В ней содержатся исходный параметр импорта: период.
    /// 
    /// Связь с журналом импортов осуществляется через Task (задачу, которая разбирала импорт на сервере вычислений).
    /// Почему не через идентификатор записи журнала импорта?
    /// Дело в том, что в обработчике импорта (например, PaymentsToClosedPeriodsImport) нет возможности получить идентификатор записи журнала,
    /// т.к. на момент запуска записи в журнале ещё нет. Запись делается в базовом классе, в самом конце процесса.
    /// Зато можно получить Task.
    /// И если потом записать Task в журнал, то получится связь между шапкой и журналом.
    /// </remarks>
    public class HeaderOfClosedPeriodsImport : BaseEntity
    {
        /// <summary>
        /// Задача, которая разбирала импорт, на сервере вычислений
        /// </summary>        
        public virtual TaskEntry Task { get; set; }

        /// <summary>
        /// Период
        /// </summary>
        public virtual ChargePeriod Period { get; set; }
    }
}
