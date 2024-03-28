namespace Bars.Gkh.RegOperator.Entities.PersonalAccount
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;

    /// <summary>
    /// Операция расчета долга
    /// </summary>
    public class PersonalAccountCalcDebt : BaseEntity
    {
        /// <summary>
        /// Лицевой счет
        /// </summary>
        public virtual BasePersonalAccount PersonalAccount { get; set; }

        /// <summary>
        /// Прежний абонент
        /// </summary>
        public virtual LegalAccountOwner PreviousOwner { get; set; }

        /// <summary>
        /// Период с
        /// </summary>
        public virtual DateTime DateStart { get; set; }

        /// <summary>
        /// Период по
        /// </summary>
        public virtual DateTime DateEnd { get; set; }

        /// <summary>
        /// Номер соглашения
        /// </summary>
        public virtual string AgreementNumber { get; set; }

        /// <summary>
        /// Документ-основание
        /// </summary>
        public virtual FileInfo Document { get; set; }
    }
}