namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.ActCheckAction
{
    using Bars.Gkh.BaseApiIntegration.Attributes;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Interfaces;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction;

    /// <summary>
    /// Модель получения Замечания
    /// </summary>
    public class ActCheckActionRemarkGet : BaseActCheckActionRemark
    {
        /// <summary>
        /// Уникальный идентификатор записи
        /// </summary>
        public long Id { get; set; }
    }
    
    /// <summary>
    /// Модель создания Замечания
    /// </summary>
    public class ActCheckActionRemarkCreate : BaseActCheckActionRemark
    {
    }
    
    /// <summary>
    /// Модель обновления Замечания
    /// </summary>
    public class ActCheckActionRemarkUpdate : BaseActCheckActionRemark, INestedEntityId
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [OnlyExistsEntity(typeof(ActCheckActionRemark))]
        public long? Id { get; set; }
    }
    
    /// <summary>
    /// Базовая модель Замечание
    /// </summary>
    public class BaseActCheckActionRemark
    {
        /// <summary>
        /// Замечание
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// ФИО контролируемого лица
        /// </summary>
        public string FullName { get; set; }
    }
}