namespace Bars.GkhGji.Regions.Zabaykalye.StateChange
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    using Castle.Core.Internal;
    using Castle.Windsor;

    /// <summary>
    /// Правило "Присвоение номера обращению граждан"
    /// </summary>
    public class AppealCitsValidationNumberRule : IRuleChangeStatus
    {
        public IWindsorContainer Container { get; set; }

        public string Id { get; } = "gji_appeal_citizens_validation_number_rule";

        public string Name { get; } = "Присвоение номера обращению граждан";

        public string TypeId { get; } = "gji_appeal_citizens";

        public string Description { get; } = "Данное правило присваивает номер обращению граждан";

        /// <summary>
        /// Регулярное выражение, формат номера О-15-1 или Ж-16-2
        /// </summary>
        protected static Regex RegexValidator { get; } = new Regex(@"^[ОЖ]-\d{2}-(\d+)$", RegexOptions.Compiled);

        /// <summary>
        /// Проверить возможность перехода статуса
        /// </summary>
        /// <param name="statefulEntity">Сущность</param>
        /// <param name="oldState">Старое состояние</param>
        /// <param name="newState">Новое состояние</param>
        /// <returns>Результат проверки</returns>
        public ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            var appealCitcs = statefulEntity as AppealCits;
            if (appealCitcs != null)
            {
                if (appealCitcs.NumberGji.IsNullOrEmpty())
                {
                    return this.FillNumber(appealCitcs);
                }

                return ValidateResult.Yes();
            }

            return ValidateResult.Yes();
        }

        /// <summary>
        /// Сгенерировать номер обращения
        /// </summary>
        /// <param name="appealCits">Обращение граждан</param>
        /// <returns>Результат установки номера</returns>
        protected virtual ValidateResult FillNumber(AppealCits appealCits)
        {
            string documentTypePrefix;
            if (appealCits.TypeCorrespondent == TypeCorrespondent.CitizenHe 
                || appealCits.TypeCorrespondent == TypeCorrespondent.CitizenShe
                || appealCits.TypeCorrespondent == TypeCorrespondent.CitizenThey)
            {
                documentTypePrefix = "Ж";
            }
            else
            {
                documentTypePrefix = "О";
            }


            var year = appealCits.DateFrom?.Year;

            if (!year.HasValue)
            {
                return ValidateResult.No("Не заполнено поле \"От\"");
            }

            var appealCitsDomain = this.Container.ResolveDomain<AppealCits>();
            try
            {
                var prefix = $"{documentTypePrefix}-{year % 100}-";
                var maxNumber = appealCitsDomain.GetAll()
                    .Where(x => x.DateFrom != null && x.DateFrom.Value.Year == year.Value && x.NumberGji.StartsWith(prefix))
                    .AsEnumerable()
                    .Select(x => AppealCitsValidationNumberRule.RegexValidator.Match(x.NumberGji))
                    .Where(x => x.Success)
                    .Select(x => x.Groups[1].Value.ToInt())
                    .SafeMax(x => x);

                maxNumber++;
                appealCits.NumberGji = prefix + maxNumber;
                appealCits.Number = appealCits.NumberGji;

                return ValidateResult.Yes();
            }
            finally
            {
                this.Container.Release(appealCitsDomain);
            }
        }
    }
}
