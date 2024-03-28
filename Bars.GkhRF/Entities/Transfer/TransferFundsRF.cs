namespace Bars.GkhRf.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Перечисление ден средств средств рег. фонда
    /// </summary>
    public class TransferFundsRf : BaseGkhEntity
    {
        /// <summary>
        /// Заявка на перечисление ден. средств
        /// </summary>
        public virtual RequestTransferRf RequestTransferRf { get; set; }

        /// <summary>
        /// Объект недвижимости
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Разновидность работы
        /// </summary>
        public virtual string WorkKind { get; set; } // Не enum так как в MonJF было около 2600 разных видов, причем многие использующихся единоразого

        /// <summary>
        /// Назначение платежа
        /// </summary>
        public virtual string PayAllocate { get; set; } // Не enum так как в MonJF было около 15000 разных видов, причем многие использующихся единоразого

        /// <summary>
        /// Лицевой счет
        /// </summary>
        public virtual string PersonalAccount { get; set; }

        /// <summary>
        /// Сумма
        /// </summary>
        public virtual decimal? Sum { get; set; }
    }
}
