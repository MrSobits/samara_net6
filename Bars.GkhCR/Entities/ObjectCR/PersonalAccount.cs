namespace Bars.GkhCr.Entities
{
    using Bars.Gkh.Entities;
    using Bars.GkhCr.Enums;

    /// <summary>
    /// Лицевой счет
    /// </summary>
    public class PersonalAccount : BaseGkhEntity
    {
        /// <summary>
        /// Объект капитального ремонта
        /// </summary>
        public virtual ObjectCr ObjectCr { get; set; }

        /// <summary>
        /// Группа финансирования
        /// </summary>
        public virtual TypeFinanceGroup FinanceGroup { get; set; }

        /// <summary>
        /// Счет закрыт
        /// </summary>
        public virtual bool Closed { get; set; }

        /// <summary>
        /// Лицевой счет
        /// </summary>
        public virtual string Account { get; set; }
    }
}
