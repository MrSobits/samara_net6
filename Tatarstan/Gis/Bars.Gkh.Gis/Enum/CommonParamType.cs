namespace Bars.Gkh.Gis.Enum
{
    using B4.Utils;

    public enum CommonParamType
    {
        [Display("Число, целое")]
        Integer = 10,

        [Display("Число, два знака")]
        Decimal = 20,

        [Display("Дата")]
        Date = 30,

        [Display("Вид дома")]
        TypeHouse = 40,

        [Display("Тип кровли")]
        TypeRoof = 50,

        [Display("Материал стен")]
        WallMaterial = 60,

        [Display("Материал кровли")]
        RoofingMaterial = 70,

        [Display("Серия, тип проекта")]
        TypeProject = 80,

        [Display("Система отопления")]
        HeatingSystem = 90,
    }
}