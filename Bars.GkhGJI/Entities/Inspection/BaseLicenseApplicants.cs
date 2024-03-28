namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Основание проверки соискателей лицензии
    /// </summary>
    public class BaseLicenseApplicants : InspectionGji
    {
        /// <summary>
        /// Тип проверки
        /// </summary>
        public virtual InspectionGjiType InspectionType { get; set; }

        /// <summary>
        /// Форма проверки
        /// </summary>
        public virtual TypeFormInspection TypeForm { get; set; }

        /// <summary>
        /// Обращение за выдачей лицензии
        /// </summary>
        public virtual ManOrgLicenseRequest ManOrgLicenseRequest { get; set; }
    }
}