namespace Bars.Gkh.Domain.ParameterVersioning
{
    using B4;

    /// <summary>
    /// Сервис для сохранения версии параметра
    /// </summary>
    public interface IVersionedEntityService
    {
        /// <summary>
        /// Сохранение версионируемого параметра на основе пользовательского ввода
        /// </summary>
        /// <param name="baseParams">Параметры, пришедгие с клиента</param>
        /// <param name="updateEntity">Обновить ли объект в бд</param>
        IDataResult SaveParameterVersion(BaseParams baseParams, bool updateEntity = true);

        /// <summary>
        /// Получение списка версионируемых параметров
        /// </summary>
        IDataResult ListChanges(StoreLoadParams storeLoadParams);
    }
}