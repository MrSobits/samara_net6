namespace Bars.Gkh.RegOperator.Domain.PaymentDocumentNumber.Impl
{
    using Bars.B4.Utils;
    using Bars.Gkh.ConfigSections.RegOperator.PaymentDocument;
    using Bars.Gkh.Domain.PaymentDocumentNumber;
    using DataProviders.Meta;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;
    using Castle.Windsor;

    /// <summary>
    /// Правило для добавления порядковый номер документа
    /// </summary>
    internal class PaymentDocumentNumberRuleNumber : PaymentDocumentNumberRuleBase
    {
        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="container">IoC контейнер</param>
        public PaymentDocumentNumberRuleNumber(IWindsorContainer container) 
            : base(container)
        {
        }

        /// <summary>
        /// Описание правила
        /// </summary>
        public override string Description
        {
            get { return "Порядковый номер"; }
        }

        /// <summary>
        /// Обязательное ли правило
        /// </summary>
        public override bool IsRequired
        {
            get { return true; }
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
                    Editable = false,
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
                result = paymentDocumentSnapshot.ConvertTo<InvoiceInfo>().Return(x => x.ПорядковыйНомерВГоду).ToStr() ?? "";
                if (result.Length < numberBuilderConfig.SymbolsCount)
                {
                    result = result.PadLeft(numberBuilderConfig.SymbolsCount, '0');
                }
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
            var result = "5146";
            if (result.Length < numberBuilderConfig.SymbolsCount)
            {
                result = result.PadLeft(numberBuilderConfig.SymbolsCount, '0');
            }
            return result;
        }
    }
}
