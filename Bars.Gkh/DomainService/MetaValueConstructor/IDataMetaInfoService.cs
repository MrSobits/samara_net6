namespace Bars.Gkh.DomainService.MetaValueConstructor
{
    using Bars.B4;

    /// <summary>
    /// Сервис для работы с мета-информацией конструкторов
    /// </summary>
    public interface IDataMetaInfoService
    {
        /// <summary>
        /// Метод возвращает дерево элементов
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат запроса</returns>
        IDataResult GetTree(BaseParams baseParams);

        /// <summary>
        /// Метод возвращает элементы верхнего уровня
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат запроса</returns>
        IDataResult GetRootElements(BaseParams baseParams);

        /// <summary>
        /// Метод возвращает все системные источники данных
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат запроса</returns>
        IDataResult GetDataFillers(BaseParams baseParams);

        /// <summary>
        /// Метод копирует реализацию конструктора из одной группы в другую
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        IDataResult CopyConstructor(BaseParams baseParams);
    }
}