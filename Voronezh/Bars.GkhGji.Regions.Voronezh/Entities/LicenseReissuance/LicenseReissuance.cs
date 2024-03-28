namespace Bars.GkhGji.Regions.Voronezh.Entities
{
    using System;
    using System.Collections.Generic;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Enums;
    using Gkh.Entities;

    /// <summary>
    /// Обращение о переоформлении лицензии
    /// </summary>
    public class LicenseReissuance : BaseGkhEntity, IStatefulEntity
    {
        /// <summary>
        /// Лицензиат
        /// </summary>
        public virtual Contragent Contragent { get; set; }

        /// <summary>
        /// Текущая лицензия
        /// </summary>
        public virtual ManOrgLicense ManOrgLicense { get; set; }

        /// <summary>
        /// Дата обращения
        /// </summary>
        public virtual DateTime? ReissuanceDate { get; set; }

        /// <summary>
        /// как обычно для всех сущностей с нумерацией делаю и стркоовый номер и тектовый наслучай если заходят изенить номер на маску
        /// </summary>
        public virtual string RegisterNumber { get; set; }

        /// <summary>
        /// как обычно для всех сущностей с нумерацией делаю и стркоовый номер и тектовый наслучай если заходят изенить номер на маску
        /// </summary>
        public virtual int? RegisterNum { get; set; }

        /// <summary>
        /// подтверждение гос пошлины
        /// </summary>
        public virtual string ConfirmationOfDuty { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// MessageId
        /// </summary>
        public virtual string MessageId { get; set; }

        /// <summary>
        /// Номер заявки в рпгу
        /// </summary>
        public virtual string RPGUNumber { get; set; }

        /// <summary>
        /// Получатель ответа
        /// </summary>
        public virtual string ReplyTo { get; set; }

        /// <summary>
		/// Причина отклонения
		/// </summary>
		public virtual string DeclineReason { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }
    }
}
