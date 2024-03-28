namespace Bars.Gkh.RegOperator.Entities.Owner
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Enums;

    using Gkh.Modules.ClaimWork.Entities;

    using Newtonsoft.Json;

    /// <summary>
    /// Собственник в исковом заявлении
    /// </summary>
    public class LawsuitOwnerInfo : BaseEntity
    {
        /// <summary>
        /// Наименование собственника
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Тип собственника
        /// </summary>
        public virtual PersonalAccountOwnerType OwnerType { get; set; }

        /// <summary>
        /// Лицевой счет
        /// </summary>
        public virtual BasePersonalAccount PersonalAccount { get; set; }

        /// <summary>
        /// Период с
        /// </summary>
        public virtual ChargePeriod StartPeriod { get; set; }

        /// <summary>
        /// Период по
        /// </summary>
        public virtual ChargePeriod EndPeriod { get; set; }

        /// <summary>
        /// Новая доля собственности
        /// </summary>
        [JsonIgnore]
        public virtual decimal AreaShare => this.AreaShareNumerator == 0 ? 0 : decimal.Divide(this.AreaShareNumerator, this.AreaShareDenominator);

        /// <summary>
        /// Числитель новой доли собственности
        /// </summary>
        public virtual int AreaShareNumerator { get; set; }

        /// <summary>
        /// Знаменатель новой доли собственности
        /// </summary>
        public virtual int AreaShareDenominator { get; set; }

        /// <summary>
        /// Новая задолженность по базовому тарифу
        /// </summary>
        public virtual decimal DebtBaseTariffSum { get; set; }

        /// <summary>
        /// Новая задолженность по тарифу решения
        /// </summary>
        public virtual decimal DebtDecisionTariffSum { get; set; }

        /// <summary>
        /// Новая задолженность по пени
        /// </summary>
        public virtual decimal PenaltyDebt { get; set; }

        /// <summary>
        /// Исковое зявление
        /// </summary>
        public virtual Lawsuit Lawsuit { get; set; }

        /// <summary>
        /// Совместная собственность
        /// </summary>
        public virtual bool SharedOwnership { get; set; }

        /// <summary>
        /// Несовершеннолетний
        /// </summary>
        public virtual bool Underage { get; set; }

        /// <summary>
        /// Номер заявления
        /// </summary>
        public virtual string ClaimNumber { get; set; }

        /// <summary>
        /// СНИЛС
        /// </summary>
        public virtual string SNILS { get; set; }

        /// <summary>
        /// Взыскания долга - Участок ССП
        /// </summary>
        public virtual JurInstitution JurInstitution { get; set; }
    }
}