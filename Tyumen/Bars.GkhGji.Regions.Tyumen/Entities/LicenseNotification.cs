namespace Bars.GkhGji.Regions.Tyumen.Entities
{
    using System;
    using System.Collections.Generic;

    using Bars.Gkh.Enums;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tyumen.Enums;
    using Gkh.Entities;

    /// <summary>
    /// Извещение
    /// </summary>
    public class LicenseNotification : BaseGkhEntity
    {
        /// <summary>
        /// Лицензиат
        /// </summary>
        public virtual ManOrgContractRealityObject ManagingOrgRealityObject { get; set; }

        /// <summary>
        /// Орган местного самоуправления
        /// </summary>
        public virtual LocalGovernment LocalGovernment { get; set; }

        /// <summary>
        /// Дата направления извещения в орган местного самоуправления
        /// </summary>
        public virtual DateTime? NoticeOMSSendDate { get; set; }

        /// <summary>
        /// Номер регистрации
        /// </summary>
        public virtual string RegistredNumber { get; set; }

        /// <summary>
        /// Дата получения
        /// </summary>
        public virtual DateTime? NoticeResivedDate { get; set; }

        /// <summary>
        /// Номер извещения
        /// </summary>
        public virtual string LicenseNotificationNumber { get; set; }

        /// <summary>
        /// Результат
        /// </summary>
        public virtual OMSNoticeResult OMSNoticeResult { get; set; }

        /// <summary>
        /// Номер документа о результате решения органа местного самоуправления
        /// </summary>
        public virtual string OMSNoticeResultNumber { get; set; }

        /// <summary>
        /// Дата документа о результате решения органа местного самоуправления
        /// </summary>
        public virtual DateTime? OMSNoticeResultDate { get; set; }

        /// <summary>
        /// Комментарий
        /// </summary>
        public virtual string Comment { get; set; }

        /// <summary>
        /// Действующая УК
        /// </summary>
        public virtual Contragent Contragent { get; set; }

        /// <summary>
        /// Дата начала управления дома, дествующей УК
        /// </summary>
        public virtual DateTime? MoDateStart { get; set; }
    }
}
