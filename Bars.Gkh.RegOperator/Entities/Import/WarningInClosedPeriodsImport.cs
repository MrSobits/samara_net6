namespace Bars.Gkh.RegOperator.Entities.Import
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Tasks.Common.Entities;

    /// <summary>
    /// Предупреждение при импорте в закрытый период
    /// </summary>
    public class WarningInClosedPeriodsImport : BaseEntity
    {
        /// <summary>
        /// Заголовок
        /// </summary>
        public virtual string Title { get; set; }

        /// <summary>
        /// Сообщение
        /// </summary>
        public virtual string Message { get; set; }

        /// <summary>
        /// Задача, которая разбирала импорт, на сервере вычислений
        /// </summary>        
        public virtual TaskEntry Task { get; set; }
    }
}
