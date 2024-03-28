namespace Bars.Gkh.InspectorMobile.Api.Version1.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <typeparam name="TGetModel">Модель получения данных</typeparam>
    /// <typeparam name="TCreateModel">Модель создания данных</typeparam>
    /// <typeparam name="TUpdateModel">Модель обновления данных</typeparam>
    /// <typeparam name="TQuery">Тип параметров запроса</typeparam>
    public interface IDocumentWithParentService<TGetModel, in TCreateModel, in TUpdateModel, in TQuery>
        : IBaseApiService<TCreateModel, TUpdateModel>
    {
        /// <summary>
        /// Получить документ по идентификатору документа
        /// </summary>
        /// <param name="documentId">Идентификатор документа</param>
        TGetModel Get(long documentId);

        /// <summary>
        /// Получить документ по идентификатору документа
        /// </summary>
        /// <param name="documentId">Идентификатор документа</param>
        /// <returns></returns>
        Task<TGetModel> GetAsync(long documentId);

        /// <summary>
        /// Получить список документов по идентификаторам родительских документов
        /// </summary>
        /// <param name="parentDocumentIds">Идентификаторы родительских документов</param>
        IEnumerable<TGetModel> GetList(long[] parentDocumentIds);
        
        /// <summary>
        /// Получить список документов по идентификаторам родительских документов
        /// </summary>
        /// <param name="parentDocumentIds">Идентификаторы родительских документов</param>
        Task<IEnumerable<TGetModel>> GetListAsync(long[] parentDocumentIds);
        
        /// <summary>
        /// Получить список документов по параметрам запроса
        /// </summary>
        /// <param name="queryParams">Параметры запроса</param>
        IEnumerable<TGetModel> GetListByQuery(TQuery queryParams);
    }
}