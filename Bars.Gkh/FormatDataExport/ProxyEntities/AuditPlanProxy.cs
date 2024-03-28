namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using System;

    using Bars.B4.DataModels;
    using Bars.Gkh.FormatDataExport.ProxySelectors;

    /// <summary>
    /// Планы проверок
    /// </summary>
    public class AuditPlanProxy : IHaveId
    {
        /// <inheritdoc />
        public long Id { get; set; }

        /// <summary>
        /// 2. Признак подписания плана проверок для публикации в ГИС ЖКХ.
        /// </summary>
        public int? IsPlanSigned { get; set; }

        /// <summary>
        /// 3. Проверяющая организация
        /// </summary>
        [ProxyId(typeof(GjiProxy))]
        public long? ContragentInspectorId { get; set; }

        /// <summary>
        /// 4. Год плана проверок
        /// </summary>
        public int? PlanYear { get; set; }

        /// <summary>
        /// 5. Дата утверждения плана проверок
        /// </summary>
        public DateTime? AcceptPlanDate { get; set; }

        /// <summary>
        /// 6. Дополнительная информация
        /// </summary>
        public string AdditionalInfo { get; set; }

        /// <summary>
        /// 7. Статус плана
        /// </summary>
        public int? State { get; set; }

        /// <summary>
        /// 8. Не должен быть зарегистрирован в едином реестре проверок
        /// </summary>
        public int? IsNotRegistred { get; set; }

        /// <summary>
        /// 9. Регистрационный номер плана в Едином реестре проверок
        /// </summary>
        public int? RegistrationNumber { get; set; }
    }
}