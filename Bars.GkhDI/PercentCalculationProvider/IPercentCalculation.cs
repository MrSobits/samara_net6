namespace Bars.GkhDi.PercentCalculationProvider
{
    using Bars.B4;
    using Bars.GkhDi.Entities;

    /// <summary>
    /// Интерфейс калькулятора расчёта процетов раскрытия информации
    /// </summary>
    public interface IPercentCalculation
    {
        /// <summary>
        /// Проверить возможность подсчёта процетов в периоде
        /// </summary>
        /// <param name="periodDi">Период раскрытия информациии</param>
        /// <returns>Возможность расчёта</returns>
        bool CheckByPeriod(PeriodDi periodDi);

        /// <summary>
        /// Массовый расчёт процентов
        /// </summary>
        /// <param name="periodId">Период раскрытия информациии</param>
        /// <param name="muIds">Муниницпальный район</param>
        /// <returns>Результат операции</returns>
        IDataResult MassCalculate(PeriodDi periodId, long[] muIds);

        /// <summary>
        /// Массовый расчёт процентов
        /// </summary>
        /// <param name="disclosureInfo">Управляющая организации в периоде раскрытия информации</param>
        /// <returns>Результат операции</returns>
        IDataResult Calculate(DisclosureInfo disclosureInfo);
    }
}