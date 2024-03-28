namespace Bars.Gkh.Entities
{
    using System;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Запрос в СМЭВ соискателя лицензии
    /// </summary>
    public class ManOrgRequestSMEV : BaseEntity
    {
        /// <summary>
        /// Заявка на лицензию
        /// </summary>
        public virtual ManOrgLicenseRequest LicRequest { get; set; }

        /// <summary>
        /// Тип запроса
        /// </summary>
        public virtual RequestSMEVType RequestSMEVType { get; set; }

        /// <summary>
        /// ИД запроса
        /// </summary>
        public virtual long? RequestId { get; set; }

        /// <summary>
        /// Дата запроса
        /// </summary>
        public virtual DateTime Date { get; set; }

        /// <summary>
        /// Автор запроса
        /// </summary>
        public virtual Inspector Inspector { get; set; }

        /// <summary>
        /// Тип запроса
        /// </summary>
        public virtual SMEVRequestState SMEVRequestState { get; set; }


    }
}