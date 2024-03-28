namespace Bars.Gkh.DomainService.MetaValueConstructor
{
    using Bars.B4;
    using Bars.Gkh.MetaValueConstructor.Enums;

    /// <summary>
    /// Интерфейс работы с объектами-значениями
    /// </summary>
    public interface IDataValueService
    {
        /// <summary>
        /// Тип конструктора, к которому применима реализация
        /// </summary>
        DataMetaObjectType ConstructorType { get; }

        /// <summary>
        /// Метод возвращает дерево значений с мета описанием элементов
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат запроса</returns>
        IDataResult GetMetaValues(BaseParams baseParams);

        /// <summary>
        /// Метод начинает расчёт всех элементов
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат запроса</returns>
        IDataResult CalcNow(BaseParams baseParams);

        /// <summary>
        /// Метод начинает массовый расчёт всех элементов
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат запроса</returns>
        IDataResult CalcMass(BaseParams baseParams);
    }
}