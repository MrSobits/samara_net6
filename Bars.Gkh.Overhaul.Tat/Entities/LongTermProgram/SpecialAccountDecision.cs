namespace Bars.Gkh.Overhaul.Tat.Entities
{
    using System;

    using Bars.B4.Modules.FIAS;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Modules.RegOperator.Entities.RegOperator;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Tat.Enum;
    using Bars.Gkh.RegOperator.Entities;

    /// <summary>
    /// Решение собственников помещений МКД (при формирования фонда КР на спец.счете)
    /// </summary>
    public class SpecialAccountDecision : BasePropertyOwnerDecision
    {
        /// <summary>
        /// Тип организации-владельца
        /// </summary>
        public virtual TypeOrganization TypeOrganization { get; set; }

        /// <summary>
        /// Региональный оператор
        /// </summary>
        public virtual RegOperator RegOperator { get; set; }

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

        /// <summary>
        /// Почтовый адрес кредитной организации
        /// </summary>
        public virtual FiasAddress MailingAddress { get; set; }

        /// <summary>
        /// Инн кредитной организации
        /// </summary>
        public virtual string Inn { get; set; }

        /// <summary>
        /// Кпп кредитной организации
        /// </summary>
        public virtual string Kpp { get; set; }

        /// <summary>
        /// Огрн кредитной организации
        /// </summary>
        public virtual string Ogrn { get; set; }

        /// <summary>
        /// Окпо кредитной организации
        /// </summary>
        public virtual string Okpo { get; set; }

        /// <summary>
        /// Бик кредитной организации
        /// </summary>
        public virtual string Bik { get; set; }

        /// <summary>
        /// Корр. счет кредитной организации
        /// </summary>
        public virtual string CorrAccount { get; set; }
    }
}