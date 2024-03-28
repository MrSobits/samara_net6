namespace Bars.Gkh.Domain.PaymentDocumentNumber.Impl
{
    using Bars.Gkh.ConfigSections.RegOperator.PaymentDocument;
    using Castle.Windsor;

    /// <summary>
    /// Правило для добавления символа '/' слэш
    /// </summary>
    internal class PaymentDocumentNumberRuleSlash: PaymentDocumentNumberRuleBase
    {
        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="container">IoC контейнер</param>
        public PaymentDocumentNumberRuleSlash(IWindsorContainer container) 
            : base(container)
        {
        }

        /// <summary>
        /// Описание правила
        /// </summary>
        public override string Description
        {
            get { return @"'/' слэш"; }
        }

        /// <summary>
        /// Метод построения части номера
        /// </summary>
        /// <param name="numberBuilderConfig">Конфигурация правила</param>
        /// <param name="obj">Документ на оплату</param>
        /// <returns>Часть номера</returns>
        public override string GetNumberPart(NumberBuilderConfig numberBuilderConfig, object obj)
        {
            return @"/";
        }
    }
}
