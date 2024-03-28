namespace Bars.GkhCr.Regions.Tatarstan.DomainService
{
    using B4;

    /// <summary>
    /// Сервис для работы с элементами двора
    /// </summary>
    public interface IElementOutdoorService
    {
        /// <summary>
        /// Добавление работ
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IDataResult AddWorks(BaseParams baseParams);

        /// <summary>
        /// Удаление работы
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IDataResult DeleteWork(BaseParams baseParams);
    }
}