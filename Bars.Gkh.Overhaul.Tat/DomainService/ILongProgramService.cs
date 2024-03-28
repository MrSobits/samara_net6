namespace Bars.Gkh.Overhaul.Tat.DomainService
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Tat.Entities;

    public interface ILongProgramService
    {
        /// <summary>
        /// Сформировать 1 этап ДПКР
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат выполнения</returns>
        IDataResult MakeStage1(BaseParams baseParams);

        /// <summary>
        /// Формирование 2 этапа ДПКР
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат</returns>
        IDataResult MakeStage2(BaseParams baseParams);

        /// <summary>
        /// Создание ДПКР для опебликования
        /// </summary>
        IDataResult CreateDpkrForPublish(BaseParams baseParams);

        IDataResult GetParams(BaseParams baseParams);

        IDataResult SetPriority(BaseParams baseParams);

        /// <summary>
        /// Получение дерева ООИ с детализацией по КЭ
        /// </summary>
        /// <param name="baseParams">Содержащий st3Id - id 3 этапа ДПКР</param>
        /// <returns>Дерево</returns>
        IDataResult ListDetails(BaseParams baseParams);

        /// <summary>
        /// Получение краткой информации по 3 этапу ДПКР
        /// </summary>
        /// <param name="baseParams">Содержащий st3Id - id 3 этапа ДПКР</param>
        /// <returns>Объект</returns>
        IDataResult GetInfo(BaseParams baseParams);

        /// <summary>
        /// Получение видов работ по объекту недвижимости
        /// </summary>
        /// <param name="baseParams">Содержащий идентификатор 3 этапа ДПКР</param>
        IDataResult ListWorkTypes(BaseParams baseParams);

        /// <summary>
        /// Сохранение новой версии программы
        /// </summary>
        /// <param name="baseParams"></param>
        IDataResult MakeNewVersion(BaseParams baseParams);

        /// <summary>
        /// Обновлении суммы работы
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        IDataResult UpdateWorkSum(BaseParams baseParams);

        /// <summary>
        /// Копировать суммы из сохраненной версии
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        IDataResult CopyFromVersion(BaseParams baseParams);
    }
}