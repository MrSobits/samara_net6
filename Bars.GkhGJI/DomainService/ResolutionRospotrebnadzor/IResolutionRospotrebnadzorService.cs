namespace Bars.GkhGji.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Сервис для работы с постановлением Роспотребнадзора
    /// </summary>
    public interface IResolutionRospotrebnadzorService
    {
        /// <summary>
        /// Получить постановление Роспотребнадзора по ID документа
        /// </summary>
        /// <param name="documentId">ID ГЖИ документа</param>
        IDataResult GetInfo(long? documentId);

        /// <summary>
        /// Список постановлений Роспотребнадзора
        /// </summary>
        /// <param name="baseParams"></param>
        IDataResult ListView(BaseParams baseParams);

        /// <summary>
        /// Список view постановлений Роспотребнадзора
        /// </summary>
        IQueryable<ViewResolutionRospotrebnadzor> GetViewList(BaseParams baseParams);

        /// <summary>
        /// Добавить список статей закона к постановлению Роспотребнадзора
        /// </summary>
        IDataResult AddArticleLawList(BaseParams baseParams);
    }
}