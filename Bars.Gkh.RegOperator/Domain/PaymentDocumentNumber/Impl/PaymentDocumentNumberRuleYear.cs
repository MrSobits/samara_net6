namespace Bars.Gkh.RegOperator.Domain.PaymentDocumentNumber.Impl
{
    using Bars.Gkh.ConfigSections.RegOperator.PaymentDocument;
    using Bars.Gkh.Domain.PaymentDocumentNumber;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;
    using Castle.Windsor;
    using System;

    /// <summary>
    /// Правило для добавления Inn
    /// </summary>
    internal class PaymentDocumentNumberRuleYear : PaymentDocumentNumberRuleBase
    {
        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="container">IoC контейнер</param>
        public PaymentDocumentNumberRuleYear(IWindsorContainer container) 
            : base(container)
        {
        }

        /// <summary>
        /// Описание правила
        /// </summary>
        public override string Description
        {
            get { return "Год начислений"; }
        }

        /// <summary>
        /// Настройки количество символов
        /// </summary>
        public override ColumnConfig SymbolsCountConfig
        {
            get
            {
                return new ColumnConfig
                {
                    Editable = true,
                    Visible = true
                };
            }
        }

        /// <summary>
        /// Настройки расположение символов
        /// </summary>
        public override ColumnConfig SymbolsLocationConfig
        {
            get
            {
                return new ColumnConfig
                {
                    Editable = true,
                    Visible = true
                };
            }
        }

        /// <summary>
        /// Метод построения части номера
        /// </summary>
        /// <param name="numberBuilderConfig">Конфигурация правила</param>
        /// <param name="obj">Документ на оплату</param>
        /// <returns>Часть номера</returns>
        public override string GetNumberPart(NumberBuilderConfig numberBuilderConfig, object obj)
        {
            var result = string.Empty;

            var paymentDocumentSnapshot = obj as PaymentDocumentSnapshot;
            if (paymentDocumentSnapshot != null)
            {
                var data = paymentDocumentSnapshot.Period.StartDate.ToString("yyyy");
                result = this.HandleString(numberBuilderConfig, data);
            }

            return result;
        }

        /// <summary>
        /// Метод построения примера части номера
        /// </summary>
        /// <param name="numberBuilderConfig">Конфигурация правила</param>
        /// <returns>Часть номера</returns>
        public override string GetNumberExamplePart(NumberBuilderConfig numberBuilderConfig)
        {
            return this.HandleString(numberBuilderConfig, DateTime.Now.ToString("yyyy"));
        }
    }
}
