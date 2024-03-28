namespace Bars.GkhGji.Regions.Chelyabinsk.Entities
{
    using System;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Enums;

    /// <summary>
    /// Основание проверки соискателей лицензии
    /// </summary>
    public class BaseLicenseReissuance : InspectionGji
    {
        /// <summary>
        /// Форма проверки
        /// </summary>
        public virtual FormCheck FormCheck { get; set; }

        /// <summary>
        /// Обращение за переоформлением лицензии
        /// </summary>
        public virtual LicenseReissuance LicenseReissuance { get; set; }
    }
}