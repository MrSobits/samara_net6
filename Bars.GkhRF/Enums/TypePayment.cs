namespace Bars.GkhRf.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип оплаты
    /// </summary>
    public enum TypePayment
    {
        [Display("Капремонт")]
        Cr = 10,

        [Display("Найм рег. фонда")]
        HireRegFund = 20,

        [Display("Капремонт по 185 ФЗ")]
        Cr185 = 30,

        [Display("Текущий ремонт жилого здания")]
        BuildingCurrentRepair = 40,

        [Display("Ремонт сан. тех. сетей")]
        SanitaryEngineeringRepair = 50,

        [Display("Ремонт сетей центрального отопления")]
        HeatingRepair = 60,

        [Display("Ремонт сетей электроснабжения")]
        ElectricalRepair = 70,

        [Display("Ремонт жилого здания и внутридомовых сетей")]
        BuildingRepair = 80
    }
}
