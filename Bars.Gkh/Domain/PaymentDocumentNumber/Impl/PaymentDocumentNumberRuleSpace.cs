namespace Bars.Gkh.Domain.PaymentDocumentNumber.Impl
{
    using Bars.Gkh.ConfigSections.RegOperator.PaymentDocument;
    using Castle.Windsor;

    /// <summary>
    /// Правило для добавления символа ' ' пробел
    /// </summary>
    internal class PaymentDocumentNumberRuleSpace: PaymentDocumentNumberRuleBase
    {
        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="container">IoC контейнер</param>
        public PaymentDocumentNumberRuleSpace(IWindsorContainer container) 
            : base(container)
        {
        }

        /// <summary>
        /// Описание правила
        /// </summary>
        public override string Description
        {
            get { return "' ' пробел"; }
        }

        /// <summary>
        /// Метод построения части номера
        /// </summary>
        /// <param name="numberBuilderConfig">Конфигурация правила</param>
        /// <param name="obj">Документ на оплату</param>
        /// <returns>Часть номера</returns>
        public override string GetNumberPart(NumberBuilderConfig numberBuilderConfig, object obj)
        {
            return " ";
        }
    }
}
