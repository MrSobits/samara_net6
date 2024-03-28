namespace Bars.Gkh.RegOperator.Entities
{
    using System;
    using System.Collections.Generic;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    /// Пакет неподтвержденных начислений
    /// </summary>
    [Serializable]
    public class UnacceptedChargePacket : BaseImportableEntity
    {
        private IList<UnacceptedCharge> _charges;

        /// <summary>
        /// Конструктор
        /// </summary>
        public UnacceptedChargePacket()
        {
            _charges = new List<UnacceptedCharge>();
        }

        /// <summary>
        /// Дата формирования пакета
        /// </summary>
        public virtual DateTime CreateDate { get; set; }

        /// <summary>
        /// Описание пакета
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Состояние пакета
        /// </summary>
        public virtual PaymentOrChargePacketState PacketState { get; set; }
        
        /// <summary>
        /// Имя (ФИО) пользователя, который произвел расчет
        /// </summary>
        public virtual string UserName { get; set; }

        /// <summary>
        /// Начисления
        /// </summary>
        public virtual IEnumerable<UnacceptedCharge> Charges
        {
            get { return _charges; }
        }
    }
}