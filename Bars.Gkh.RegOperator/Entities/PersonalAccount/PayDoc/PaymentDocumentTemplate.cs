namespace Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Шаблон документа на оплату по периоду
    /// </summary>
    public class PaymentDocumentTemplate : BaseEntity
    {
        /// <summary>
        /// Период начисления
        /// </summary>
        public virtual ChargePeriod Period { get; set; }

        /// <summary>
        /// Код шаблона
        /// </summary>
        public virtual string TemplateCode { get; set; }

        /// <summary>
        /// Шаблон
        /// </summary>
        public virtual byte[] Template { get; set; }
    }
}