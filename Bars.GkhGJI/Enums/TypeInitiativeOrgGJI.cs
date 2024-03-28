namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип инициативной организации (тоесть организации от которой пошла инициатива на документ)
    /// </summary>
    public enum TypeInitiativeOrgGji
    {
        [Display("Жилищная инспекция")]
        HousingInspection = 10,

        [Display("Суд")]
        Court = 20,

        [Display("Роспотребнадзор")]
        Rospotrebnadzor = 30
    }
}