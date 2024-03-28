namespace Bars.Gkh.Reforma.Domain
{
    using Bars.B4;

    /// <summary>
    /// Сервис управления интеграцией с Реформой ЖКХ
    /// </summary>
    public interface ISyncService
    {
        /// <summary>
        /// Получение параметров
        /// </summary>
        /// <returns>Параметры</returns>
        IDataResult GetParams();

        /// <summary>
        /// Сохранение параметров
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат сохранения</returns>
        IDataResult SaveParams(BaseParams baseParams);

        /// <summary>
        /// Запустить интеграцию немедля
        /// </summary>
        /// <returns>Результат</returns>
        IDataResult RunNow();

        /// <summary>
        /// Получение информации о жилом доме
        /// </summary>
        /// <param name="id">Идентификатор жилого дома</param>
        /// <returns>Информация</returns>
        IDataResult GetRobjectDetails(long id);

        /// <summary>
        /// Получение информации об УО
        /// </summary>
        /// <param name="id">Идентификатор УО</param>
        /// <returns>Информация</returns>
        IDataResult GetManOrgDetails(long id);
    }
}