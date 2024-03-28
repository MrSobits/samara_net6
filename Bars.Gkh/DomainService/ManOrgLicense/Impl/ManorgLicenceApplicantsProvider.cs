namespace Bars.Gkh.DomainService
{
    using System.Collections.Generic;

    using Bars.B4;

    /// <summary>
    /// Сервис для возможности формирования "Основание проверки соискателей лицензии"
    /// </summary>
    /// <remarks>Переопределен для Перми, в базовом пока не используется</remarks>
    public class ManorgLicenceApplicantsProvider : IManorgLicenceApplicantsProvider
    {
        /// <inheritdoc />
        public IEnumerable<ManorgLicenceApplicantsRule> GetRules(BaseParams baseParams)
        {
            return new[]
            {
                new ManorgLicenceApplicantsRule
                {
                    Text = "Проверка"
                }
            };
        }
    }
}