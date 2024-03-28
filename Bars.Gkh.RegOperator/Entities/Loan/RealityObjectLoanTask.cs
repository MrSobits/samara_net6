namespace Bars.Gkh.RegOperator.Entities.Loan
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Tasks.Common.Entities;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Сущность связи задачи взятия займа с домом
    /// </summary>
    public class RealityObjectLoanTask : PersistentObject
    {
        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Задача взятия займа
        /// </summary>
        public virtual TaskEntry Task { get; set; }
    }
}