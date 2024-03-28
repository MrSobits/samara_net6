namespace Bars.GkhCr.Entities
{
    using Bars.GkhCr.Enums;

    using Gkh.Entities;

    /// <summary>
    /// История изменения вида работы объекта КР
    /// </summary>
    public class TypeWorkCrHistory : BaseGkhEntity
    {
        /// <summary>
        /// Вид работы объекта КР
        /// </summary>
        public virtual TypeWorkCr TypeWorkCr { get; set; }

        /// <summary>
        /// Тип действия для истории вида работ объекта КР
        /// </summary>
        public virtual TypeWorkCrHistoryAction TypeAction { get; set; }

        /// <summary>
        ///  Причина
        /// </summary>
        public virtual TypeWorkCrReason TypeReason { get; set; }

        /// <summary>
        /// Разрез финансирования
        /// </summary>
        public virtual FinanceSource FinanceSource { get; set; }

        /// <summary>
        /// Объем выполнения
        /// </summary>
        public virtual decimal? Volume { get; set; }

       /// <summary>
        /// Сумма расходов
        /// </summary>
        public virtual decimal? Sum { get; set; }

        /// <summary>
        ///  Год ремонта
        /// </summary>
        public virtual int? YearRepair { get; set; }

        /// <summary>
        ///  Новый год ремонта
        /// </summary>
        public virtual int? NewYearRepair { get; set; }

        /// <summary>
        ///  Имя пользователя
        /// </summary>
        public virtual string UserName { get; set; }

        /// <summary>
        /// Конструктивный элемент
        /// </summary>
        public virtual string StructElement { get; set; }
    }
}
