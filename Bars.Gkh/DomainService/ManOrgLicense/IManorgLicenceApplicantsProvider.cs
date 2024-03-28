namespace Bars.Gkh.DomainService
{
    using System;
    using System.Collections.Generic;

    using Bars.B4;

    /// <summary>
    /// Интерфейс для возможности формирования "Основание проверки соискателей лицензии"
    /// </summary>
    public interface IManorgLicenceApplicantsProvider
    {
        /// <summary>
        /// Получение правил формирования дкоументов на основе типа документа
        /// </summary>
        IEnumerable<ManorgLicenceApplicantsRule> GetRules(BaseParams baseParams);
    }

    /// <summary>
    /// Пункт меню "Сформировать проверку"
    /// </summary>
    public class ManorgLicenceApplicantsRule
    {
        /// <summary>
        /// Наименование пункта
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Параметры создания документа
        /// </summary>
        public IDictionary<string, object> ExtraParams { get; set; }

        /// <inheritdoc />
        public bool Equals(ManorgLicenceApplicantsRule other)
        {
            return this.Text == other?.Text;
        }
    }
}