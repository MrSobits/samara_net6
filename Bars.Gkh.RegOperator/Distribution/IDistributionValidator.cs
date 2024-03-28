namespace Bars.Gkh.RegOperator.Distribution
{
    using Bars.B4;

    /// <summary>
    /// Валидатор распределения после выбора объектов
    /// </summary>
    public interface IDistributionValidator
    {
        /// <summary>
        /// Код
        /// </summary>
        string Code { get; }

        /// <summary>
        /// Проверка является обязательной
        /// <para>Если обязательная проверка будет неуспешной, то продолжение работы невозможно</para>
        /// </summary>
        bool IsMandatory { get; }

        /// <summary>
        /// Проверка выполняется при распределении
        /// </summary>
        bool IsApply { get; }

        /// <summary>
        /// Применить распределение
        /// </summary>
        /// <param name="baseParams"></param>
        IDataResult Validate(BaseParams baseParams);
    }
}