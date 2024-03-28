namespace Bars.GkhGji.Regions.Chelyabinsk.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Сущность для работы с сервисом истории запросов
    /// </summary>
    public class SmevHistory : BaseEntity
    {
        /// <summary>
        /// Идентификатор запроса лицензии
        /// </summary>
        public virtual long RequestId { get; set; }

        /// <summary>
        /// Тип Action
        /// </summary>
        public virtual string ActionCode { get; set; }

        /// <summary>
        /// Тип запроса лицензии
        /// </summary>
        public virtual LicenseRequestType LicenseRequestType { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual string Status { get; set; }

        /// <summary>
        /// Guid
        /// </summary>
        public virtual string Guid { get; set; }

        /// <summary>
        /// UniqId
        /// </summary>
        public virtual string UniqId { get; set; }

        /// <summary>
        /// InnerId
        /// </summary>
        public virtual string InnerId { get; set; }

        /// <summary>
        /// ExtActionId
        /// </summary>
        public virtual string ExtActionId { get; set; }

        /// <summary>
        /// SocId
        /// </summary>
        public virtual string SocId { get; set; }
    }
}