namespace Bars.Gkh.RegOperator.Entities.Period
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.RegOperator.DomainService.Period;

    public class PeriodCloseCheck : BaseEntity
    {
        /// <summary>
        /// Просто код на отображение
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Отображаемое имя проверки
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Системный код проверки <see cref="IPeriodCloseChecker.Code"/>
        /// </summary>
        public virtual string Impl { get; set; }

        /// <summary>
        /// Обязательность
        /// </summary>
        public virtual bool IsCritical { get; set; }
    }
}