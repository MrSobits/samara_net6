namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.ActCheckAction
{
    using Bars.Gkh.BaseApiIntegration.Attributes;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Interfaces;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction.DocRequestAction;
    using Bars.GkhGji.Regions.Tatarstan.Enums;

    /// <summary>
    /// Модель получения Запрошенные сведения действия Истребование документов
    /// </summary>
    public class DocRequestActionRequestInfoGet : BaseDocRequestActionRequestInfo
    {
        /// <summary>
        /// Уникальный идентификатор записи
        /// </summary>
        public long Id { get; set; }
    }
    
    /// <summary>
    /// Модель создания Запрошенные сведения действия Истребование документов
    /// </summary>
    public class DocRequestActionRequestInfoCreate : BaseDocRequestActionRequestInfo
    {
    }
    
    /// <summary>
    /// Модель обновления Запрошенные сведения действия Истребование документов
    /// </summary>
    public class DocRequestActionRequestInfoUpdate : BaseDocRequestActionRequestInfo, INestedEntityId
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [OnlyExistsEntity(typeof(DocRequestActionRequestInfo))]
        public long? Id { get; set; }
    }
    
    /// <summary>
    /// Базовая модель Запрошенные сведения действия Истребование документов
    /// </summary>
    public class BaseDocRequestActionRequestInfo
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        ///Описание
        /// </summary>
        public RequestInfoType Description { get; set; }
    }
}