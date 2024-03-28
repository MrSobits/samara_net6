namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using System;

    using Bars.B4.DataModels;
    using Bars.Gkh.FormatDataExport.ProxySelectors;

    /// <summary>
    /// Оплата по договорам на  выполнение работ по  капитальному ремонту(paydogov.csv)
    /// </summary>
    public class PayDogovProxy : IHaveId
    {
        /// <summary>
        /// 1. Уникальный код работы/услуги
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 2. Код договора на выполнение работ по капитальному ремонту
        /// </summary>
        [ProxyId(typeof(DogovorPkrProxy))]
        public long? ContractId { get; set; }

        /// <summary>
        /// 3. Статус
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// 4. Вид оплаты
        /// </summary>
        public int? PaymentType { get; set; }

        /// <summary>
        /// 5. Получатель
        /// </summary>
        [ProxyId(typeof(ContragentProxy))]
        public long? ContragentRecipient { get; set; }

        /// <summary>
        /// 6. Плательщик
        /// </summary>
        [ProxyId(typeof(ContragentProxy))]
        public long? ContragentPayer { get; set; }

        /// <summary>
        /// 7. Дата оплаты
        /// </summary>
        public DateTime? PaymentDate { get; set; }

        /// <summary>
        /// 8. Сумма оплаты
        /// </summary>
        public decimal? PaymentSum { get; set; }

        #region PAYDOGOVWORK
        /// <summary>
        /// PAYDOGOVWORK 3. Работа по дому
        /// </summary>
        [ProxyId(typeof(WorkDogovProxy))]
        public long? WorkId{ get; set; }

        /// <summary>
        /// PAYDOGOVWORK 4. Сумма оплаты за счет средств собственников
        /// </summary>
        public decimal? OwnerSum{ get; set; }

        /// <summary>
        /// PAYDOGOVWORK 5. Сумма оплаты за счет средств поддержки
        /// </summary>
        public decimal? SupportSum{ get; set; }
        #endregion
    }
}