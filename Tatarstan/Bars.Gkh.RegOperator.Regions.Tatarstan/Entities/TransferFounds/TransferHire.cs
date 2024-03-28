namespace Bars.Gkh.RegOperator.Regions.Tatarstan.Entities
{
    using B4.DataAccess;

    using Bars.Gkh.RegOperator.Entities;

    using GkhRf.Entities;

    /// <summary>
    /// Пречисления по найму
    /// </summary>
    public class TransferHire : BaseEntity
    {
        /// <summary>
        /// Ссылка на перечисление
        /// </summary>
        public virtual TransferRfRecord TransferRecord { get; set; }

        /// <summary>
        /// Лицевой счет
        /// </summary>
        public virtual BasePersonalAccount Account { get; set; }

        /// <summary>
        /// Перечисленная сумма
        /// </summary>
        public virtual decimal TransferredSum { get; set; }

        /// <summary>
        /// Признак Да/Нет - Перечислено или нет
        /// </summary>
        public virtual bool Transferred { get; set; }
    }
}