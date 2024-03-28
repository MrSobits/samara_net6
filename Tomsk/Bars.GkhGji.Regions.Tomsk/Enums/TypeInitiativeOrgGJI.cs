//В Томске понадобилось перекрыть данный енум для того чтобы были тругие переменные при выборе на клиенте

namespace Bars.GkhGji.Regions.Tomsk.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип инициативной организации (тоесть организации от которой пошла инициатива на документ)
    /// </summary>
    public enum TypeInitiativeOrgGji
    {
        [Display("Департаментом ЖКХ и государственного жилищного надзора Томской области")]
        HousingInspection = 10,

        [Display("Суд")]
        Court = 20
    }
}