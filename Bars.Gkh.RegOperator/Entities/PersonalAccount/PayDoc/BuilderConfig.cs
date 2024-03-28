namespace Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    /// Настройки источников для документов на оплату
    /// </summary>
    public class BuilderConfig : BaseEntity
    {
        /// <summary>
        /// Тип платежного документа (физ лицо, юр лицо, юр лицо с реестром)
        /// </summary>
        public virtual PaymentDocumentType PaymentDocumentType { get; set; }

        /// <summary>
        /// Код настройки через точку (полный путь)
        /// </summary>
        public virtual string Path { get; set; }

        /// <summary>
        /// Включен ли источник
        /// </summary>
        public virtual bool Enabled { get; set; }
    }
}