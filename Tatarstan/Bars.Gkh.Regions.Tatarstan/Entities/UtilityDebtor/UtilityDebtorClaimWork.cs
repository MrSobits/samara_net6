namespace Bars.Gkh.Regions.Tatarstan.Entities.UtilityDebtor
{
    using Bars.Gkh.Enums;
    using Bars.Gkh.Modules.ClaimWork.Entities;

    /// <summary>
    /// Задолженность по оплате ЖКУ
    /// </summary>
    public class UtilityDebtorClaimWork : BaseClaimWork
    {
        /// <summary>
        /// Абонент
        /// </summary>
        public virtual string AccountOwner { get; set; }

        /// <summary>
        /// Тип абонента
        /// </summary>
        public virtual OwnerType OwnerType { get; set; } 

        /// <summary>
        /// Сумма долга
        /// </summary>
        public virtual decimal? ChargeDebt { get; set; }

        /// <summary>
        /// Сумма долга по пени
        /// </summary>
        public virtual decimal? PenaltyDebt { get; set; }

        /// <summary>
        /// Номер лс
        /// </summary>
        public virtual string PersonalAccountNum { get; set; }

        /// <summary>
        /// Статус лс
        /// </summary>
        public virtual string PersonalAccountState { get; set; }
    }
}