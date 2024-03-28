namespace Bars.Gkh.Overhaul.Tat.Entities
{
    using B4.DataAccess;

    using Bars.Gkh.Entities;

    using Enums;
    using Enum;

    /// <summary>
    /// Базовая сущность решения собственников помещений МКД
    /// </summary>
    public class BasePropertyOwnerDecision : BaseEntity
    {
        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Протокол собственников помещений МКД
        /// </summary>
        public virtual PropertyOwnerProtocols PropertyOwnerProtocol { get; set; }

        /// <summary>
        /// Ежемесячный взнос на КР (руб./кв.м)
        /// </summary>
        public virtual decimal MonthlyPayment { get; set; }

        /// <summary>
        /// Наименование решения
        /// </summary>
        public virtual PropertyOwnerDecisionType PropertyOwnerDecisionType { get; set; }

        /// <summary>
        /// Способ формирования фонда
        /// </summary>
        public virtual MethodFormFundCr? MethodFormFund { get; set; }

         /// <summary>
        /// Организационно-правовая форма УО
        /// </summary>
        public virtual MoOrganizationForm MoOrganizationForm { get; set; }
    }
}