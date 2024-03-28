namespace Bars.Gkh.InspectorMobile.Api.Version1.Services
{
    using System.Threading.Tasks;

    /// <summary>
    /// Интерфейс базового CRUD-сервиса
    /// </summary>
    /// <typeparam name="TCreateModel"></typeparam>
    /// <typeparam name="TUpdateModel"></typeparam>
    public interface IBaseApiService<in TCreateModel, in TUpdateModel>
    {
        /// <summary>
        /// Создать сущность
        /// </summary>
        /// <param name="createModel">Данные для создания</param>
        /// <returns>Идентификатор созданнойсущности</returns>
        long? Create(TCreateModel createModel);

        /// <summary>
        /// Обновить сущность
        /// </summary>
        /// <param name="entityId">Идентификатор обновляемой сущности</param>
        /// <param name="updateModel">Данные для обновления</param>
        /// <returns></returns>
        long Update(long entityId, TUpdateModel updateModel);

        /// <summary>
        /// Удалить сущность
        /// </summary>
        /// <param name="entityId">Идентификатор сущности</param>
        Task DeleteAsync(long entityId);
    }
}