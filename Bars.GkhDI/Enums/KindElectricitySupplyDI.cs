namespace Bars.GkhDi.Enums
{
    using B4.Utils;

    public enum KindElectricitySupplyDi : byte
    {
        [Display("Без электроплит (электроотопительных установок)")]
        NoElectricCooker = 10,

        [Display("С электроплитами (электроотопительными установками)")]
        WithElectricCooker = 20
    }
}
