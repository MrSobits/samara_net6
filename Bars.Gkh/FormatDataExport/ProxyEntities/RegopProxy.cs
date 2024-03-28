namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using System;

    using Bars.B4.DataModels;
    using Bars.Gkh.FormatDataExport.ProxySelectors;

    /// <summary>
    /// Региональные операторы капитального ремонта (regop.csv)
    /// </summary>
    public class RegopProxy : IHaveId
    {
        /// <inheritdoc />
        [ProxyId(typeof(ContragentProxy))]
        public long Id { get; set; }

        /// <summary>
        /// 2. Номер основания наделения полномочиями
        /// </summary>
        public string DocNumber { get; set; }

        /// <summary>
        /// 3. Дата основания наделения полномочиями
        /// </summary>
        public DateTime? DocDate { get; set; }

        /// <summary>
        /// 4. Количество штатных единиц
        /// </summary>
        public int? StaffCount { get; set; }

        /// <summary>
        /// 5. Курирующий орган государственной власти
        /// </summary>
        [ProxyId(typeof(ContragentProxy))]
        public long? ParentContragent { get; set; }

        /// <summary>
        /// 6. Наличие у организации информационной системы, автоматизирующей учет фонда КР и выставление платежных документов на уплату взносов
        /// </summary>
        public int? HasPaymentInformSystem { get; set; }

        /// <summary>
        /// 7. Наличие у субъекта РФ информационной системы, автоматизирующей планирование и формирование программы капитального ремонта
        /// </summary>
        public int? HasCrInformSystem { get; set; }

        /// <summary>
        /// 8. Модель выставления платежных документов на оплату взносов на капитальный ремонт:
        /// </summary>
        public int? PaymentModel { get; set; }

        /// <summary>
        /// 9. День месяца, до которого выставляются платежные документы для оплаты взносов на капитальный ремонт
        /// </summary>
        public int? PaymentDay { get; set; }

        /// <summary>
        /// 10. Месяц, в котором выставляются платежные документы для оплаты взносов на капитальный ремонт
        /// </summary>
        public int? PaymentMonth { get; set; }
    }
}