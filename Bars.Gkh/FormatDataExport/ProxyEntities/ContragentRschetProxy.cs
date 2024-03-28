namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using System;

    using Bars.B4.DataModels;
    using Bars.Gkh.FormatDataExport.ProxySelectors;

    /// <summary>
    /// Расчетные счета
    /// </summary>
    public class ContragentRschetProxy : IHaveId
    {
        /// <summary>
        /// 1. Уникальный код контрагента
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 2. Номер расчетного счета
        /// </summary>
        public string SettlementAccount { get; set; }

        /// <summary>
        /// 3. Банк
        /// </summary>
        [ProxyId(typeof(BankProxy))]
        public long? BankContragentId { get; set; }

        /// <summary>
        /// 4. Контрагент
        /// </summary>
        [ProxyId(typeof(ContragentProxy))]
        public long? ContragentId { get; set; }

        /// <summary>
        /// 5. Корреспондентский счет
        /// </summary>
        public string CorrAccount { get; set; }

        /// <summary>
        /// 6. Дата открытия
        /// </summary>
        public DateTime? OpenDate { get; set; }

        /// <summary>
        /// 7. Дата закрытия
        /// </summary>
        public DateTime? CloseDate { get; set; }

        /// <summary>
        /// Признак филиала
        /// </summary>
        public bool IsFilial { get; set; }

        /// <summary>
        /// Идентификатор Банка контрагента
        /// </summary>
        public long? ContragentBankCreditOrgId { get; set; }

        /// <summary>
        /// Идентификатор Bars.Gkh.RegOperator.Entities.CalcAccount
        /// </summary>
        public long? CalcAccountId { get; set; }

        /// <summary>
        /// Признак счета регионального оператора
        /// </summary>
        public bool IsRegopAccount { get; set; }

        /// <summary>
        /// Тип счета регионального оператора
        /// </summary>
        public int? RegopAccountType { get; set; }
    }
}