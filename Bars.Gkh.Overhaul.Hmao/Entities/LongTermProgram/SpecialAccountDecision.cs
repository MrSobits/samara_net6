namespace Bars.Gkh.Overhaul.Hmao.Entities
{
    using System;

    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.Gkh.Overhaul.Hmao.Enum;

    /// <summary>
    /// Решение собственников помещений МКД (при формирования фонда КР на спец.счете)
    /// </summary>
    public class SpecialAccountDecision : BasePropertyOwnerDecision
    {
        /// <summary>
        /// Тип организации-владельца
        /// </summary>
        public virtual TypeOrganization TypeOrganization { get; set; }

        ///// <summary>
        ///// Региональный оператор
        ///// </summary>
        //public virtual RegOperator RegOperator { get; set; }

        /// <summary>
        /// Управляющая организация
        /// </summary>
        public virtual ManagingOrganization ManagingOrganization { get; set; }

        /// <summary>
        /// Номер счета
        /// </summary>
        public virtual string AccountNumber { get; set; }

        /// <summary>
        /// Дата открытия
        /// </summary>
        public virtual DateTime? OpenDate { get; set; }

        /// <summary>
        /// Дата закрытия
        /// </summary>
        public virtual DateTime? CloseDate { get; set; }

        /// <summary>
        /// Файл справки банка
        /// </summary>
        public virtual FileInfo BankHelpFile { get; set; }

        /// <summary>
        /// Кредитная организация
        /// </summary>
        public virtual CreditOrg CreditOrg { get; set; }
    }
}