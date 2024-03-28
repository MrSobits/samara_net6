namespace Bars.Gkh.Regions.Tatarstan.Entities.Fssp.CourtOrderGku
{
    using System;

    using Bars.B4.DataAccess;

    /// <summary>
    /// Делопроизводство
    /// </summary>
    public class Litigation : BaseEntity
    {
        /// <summary>
        /// Подразделение ОСП
        /// </summary>
        public virtual string JurInstitution { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual string State { get; set; }

        /// <summary>
        /// Регистрационный номер ИП
        /// </summary>
        public virtual string IndEntrRegistrationNumber { get; set; }

        /// <summary>
        /// Должник
        /// </summary>
        public virtual string Debtor { get; set; }

        /// <summary>
        /// Адрес должника (ФССП)
        /// </summary>
        public virtual FsspAddress DebtorFsspAddress { get; set; }

        /// <summary>
        /// Внешний Id
        /// </summary>
        public virtual long OuterId { get; set; }
        
        /// <summary>
        /// Дата создания ИП
        /// </summary>
        public virtual DateTime? EntrepreneurCreateDate { get; set; }
        
        /// <summary>
        /// Сумма задолженности ИП
        /// </summary>
        public virtual decimal?  EntrepreneurDebtSum { get; set; }
    }
}