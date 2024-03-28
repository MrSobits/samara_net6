namespace Bars.GkhDi.Enums
{
    using System;
    using B4.Utils;

    /// <summary>
    /// Оборудование дома
    /// </summary>
    public enum EquipmentDi
    {
        [Display("Отсутствует")] 
        No = 10,

        [Display("Мусоропровод")] 
        Chute = 20,

        [Display("Лифт")] 
        Lift = 30,

        [Display("Лифт и мусоропровод")] 
        LiftAndChute = 40
    }
}