namespace Bars.Gkh.RegOperator.Entities
{
    using Bars.Gkh.Entities;

    using Gkh.Entities;
    using Gkh.Enums.Decisions;

    /// <summary>
    /// Спец, либо счет рег оператора. обращаем внимание на существования полей RegOperator/Contragent
    /// </summary>
    public class RealityObjectSpecialOrRegOperatorAccount : BaseImportableEntity
    {
        /// <summary>
        /// Рег оператор
        /// </summary>
        public virtual string RegOperator { get; set; }

        /// <summary>
        /// Упр организация
        /// </summary>
        public virtual Contragent Contragent { get; set; }
        
        /// <summary>
        /// Счет начислений
        /// </summary>
        public virtual RealityObjectChargeAccount RealityObjectChargeAccount { get; set; }

        /// <summary>
        /// Признак использования спец счета. Поле требуется, чтобы каждый раз не пересоздавать счет
        /// </summary>
        public virtual bool IsActive { get; set; }

        /// <summary>
        /// Тип счета
        /// </summary>
        public virtual CrFundFormationDecisionType AccountType { get; set; }
    }
}