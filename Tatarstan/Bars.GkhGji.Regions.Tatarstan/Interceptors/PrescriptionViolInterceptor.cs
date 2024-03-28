namespace Bars.GkhGji.Regions.Tatarstan.Interceptors
{
    using System;
    using B4;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.ConfigSections;
    using GkhGji.Entities;

    /// <summary>
    /// Интерцептор сущности "Этап указания к устранению нарушения в предписании" (см. <see cref="PrescriptionViol"/>)
    /// Регион: Татарстан
    /// </summary>
    public class PrescriptionViolInterceptor : GkhGji.Interceptors.PrescriptionViolInterceptor
    {
        private const int DefaultPlanRemoval = 12;

        /// <summary>
        /// Проверка плановой даты устранения
        /// </summary>
        /// <param name="entity">Сущность <see cref="PrescriptionViol"/></param>
        /// <param name="actCheckDate">Дата родительского документа</param>
        /// <returns>Результат выполнения проверки</returns>
        protected override IDataResult CheckDatePlanRemoval(PrescriptionViol entity, DateTime? actCheckDate)
        {
            var housingInspection = this.Container.GetGkhConfig<HousingInspection>();

            try
            {
                var timeForCorrectingViol = housingInspection.GeneralConfig.TimeForCorrectingViol;

                var configIsLimitDate = timeForCorrectingViol.IsLimitDate;
                var configPeriod = timeForCorrectingViol.Period;
                var configPeriodKind = timeForCorrectingViol.PeriodKind;

                if (configIsLimitDate)
                {
                    if (configPeriod == null)
                    {
                        return this.Failure(
                            $"Необходимо установить значение: \"Администрирование\\ Единые настройки приложения \\ Жилищная инспекция\\ Общие\\ Период\"");
                    }

                    if (entity.DatePlanRemoval.HasValue && actCheckDate.HasValue)
                    {
                        if (configPeriodKind == PeriodKind.Day)
                        {
                            if (entity.DatePlanRemoval.Value > actCheckDate.Value.AddDays((int)configPeriod))
                            {
                                return this.Failure($"Срок устранения нарушения не должен превышать {configPeriod} дней.");
                            }
                        }

                        if (configPeriodKind == PeriodKind.Month)
                        {
                            if (entity.DatePlanRemoval.Value > actCheckDate.Value.AddMonths((int)configPeriod))
                            {
                                return this.Failure($"Срок устранения нарушения не должен превышать {configPeriod} месяцев.");
                            }
                        }
                    }
                }
                else
                {
                    if (entity.DatePlanRemoval != null && actCheckDate != null
                        && entity.DatePlanRemoval.Value > actCheckDate.Value.AddMonths(PrescriptionViolInterceptor.DefaultPlanRemoval))
                    {
                        return this.Failure($"Срок устранения нарушения не должен превышать {PrescriptionViolInterceptor.DefaultPlanRemoval} месяцев.");
                    }
                }

                return this.Success();
            }
            finally
            {
                this.Container.Release(housingInspection);
            }
        }
    }
}