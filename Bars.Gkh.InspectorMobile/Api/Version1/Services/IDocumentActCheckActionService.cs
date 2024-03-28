namespace Bars.Gkh.InspectorMobile.Api.Version1.Services
{
    using System.Collections.Generic;

    using Bars.Gkh.InspectorMobile.Api.Version1.Models.ActCheckAction;

    /// <summary>
    /// Интерфейс сервиса "Протокол действия акта"
    /// </summary>
    public interface IDocumentActCheckActionService : IBaseApiService<DocumentActCheckActionCreate, DocumentActCheckActionUpdate>
    {
        /// <summary>
        /// Получить действие акта проверки по идентификатору
        /// </summary>
        /// <param name="actionId">Идентификатор действия акта проверки</param>
        DocumentActCheckActionGet Get(long actionId);

        /// <summary>
        /// Получить список действий актов проверок по идентификаторам родительских документов
        /// </summary>
        /// <param name="parentDocumentIds">Идентификаторы родительских документов</param>
        IEnumerable<DocumentActCheckActionGet> GetList(long[] parentDocumentIds);
    }
}