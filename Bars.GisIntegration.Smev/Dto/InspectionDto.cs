namespace Bars.GisIntegration.Smev.Dto
{
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class InspectionDto : InspectionGji
    {
        /// <summary>
        /// Инн
        /// </summary>
        public string OrganizationInn { get; set; }
            
        /// <summary>
        /// Наименование
        /// </summary>
        public string OrganizationName { get; set; }
            
        /// <summary>
        /// Форма проверки
        /// </summary>
        public TypeFormInspection TypeForm { get; set; }
    }
}