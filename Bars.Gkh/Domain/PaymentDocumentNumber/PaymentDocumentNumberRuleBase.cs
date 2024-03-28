namespace Bars.Gkh.Domain.PaymentDocumentNumber
{
    using Bars.Gkh.ConfigSections.RegOperator.PaymentDocument;
    using Bars.Gkh.Enums;
    using Castle.Windsor;
    using System;

    /// <summary>
    /// Интерфейс правила заполнения номера документа на оплату
    /// </summary>
    public abstract class PaymentDocumentNumberRuleBase
    {
        private IWindsorContainer container;
        private string name;
        /// <summary>
        /// Конструктор класса
        /// </summary>
        protected PaymentDocumentNumberRuleBase(IWindsorContainer container)
        {
            this.container = container;
        }

        /// <summary>
        /// Наименование правила (тип)
        /// </summary>
        public string Name
        {
            get
            {
                return this.name ?? (this.name = this.GetType().FullName);
            }
        }

        /// <summary>
        /// Описание правила
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// Обязательное ли правило
        /// </summary>
        public virtual bool IsRequired
        {
            get { return false; }
        }

        /// <summary>
        /// Конфигурация правила по умолчанию
        /// </summary>
        public virtual NumberBuilderConfig DefaultConfig
        {
            get
            {
                return new NumberBuilderConfig
                {
                    Name = this.Name,
                    SymbolsLocation = SymbolsLocation.All,
                    SymbolsCount = 0
                };
            }
        }

        /// <summary>
        /// Настройки количество символов
        /// </summary>
        public virtual ColumnConfig SymbolsCountConfig
        {
            get
            {
                return new ColumnConfig
                {
                    Editable = false,
                    Visible = false
                };
            }
        }

        /// <summary>
        /// Настройки расположение символов
        /// </summary>
        public virtual ColumnConfig SymbolsLocationConfig
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
        public abstract string GetNumberPart(NumberBuilderConfig numberBuilderConfig, object obj);

        /// <summary>
        /// Метод построения примера части номера
        /// </summary>
        /// <param name="numberBuilderConfig">Конфигурация правила</param>
        /// <returns>Часть номера</returns>
        public virtual string GetNumberExamplePart(NumberBuilderConfig numberBuilderConfig)
        {
            return this.GetNumberPart(numberBuilderConfig, null);
        }
        /// <summary>
        /// Обрезать часть номера в соответвстии с конфигурацией
        /// </summary>
        /// <param name="numberBuilderConfig">Конфигурация правила</param>
        /// <param name="numberPart">Часть номера</param>
        /// <returns>Обработанная строка</returns>
        protected string HandleString(NumberBuilderConfig numberBuilderConfig, string numberPart)
        {
            string result;

            if (string.IsNullOrEmpty(numberPart))
            {
                result = string.Empty;
            }
            else if (numberBuilderConfig.SymbolsLocation == SymbolsLocation.All)
            {
                result = numberPart;
            }
            else if (numberBuilderConfig.SymbolsLocation == SymbolsLocation.Start)
            {
                var startIndex = 0;
                var count = Math.Min(numberPart.Length, numberBuilderConfig.SymbolsCount);
                result = numberPart.Substring(startIndex, count);
            }
            else
            {
                var startIndex = Math.Max(numberPart.Length - numberBuilderConfig.SymbolsCount, 0);
                var count = numberPart.Length - startIndex;
                result = numberPart.Substring(startIndex, count);
            }

            return result;
        }
    }

    public class ColumnConfig
    {
        public bool Editable { get; set; }
        public bool Visible { get; set; }
    }
}