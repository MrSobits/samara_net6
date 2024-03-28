namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип группы объекта общего имущества
    /// </summary>
    public enum TypeGroup
    {
        [Display("Внутридомовые инженерные системы и оборудование")]
        НouseEngineeringSystemsAndEquipment = 10,

        [Display("Несущие конструкции строения")]
        FramingsBuilding = 20,

        [Display("Ненесущие конструкции строения")]
        NotFramingsBuilding = 30,

        [Display("Помещения общего пользования")]
        CommunalFacilities = 40,

        [Display("Крыши")]
        Roof = 50,

        [Display("Территория и благоустройство")]
        GroundsAndLandscaping = 60
    }
}
