namespace Bars.Gkh.Domain.TechPassport
{
    /// <summary>
    /// Тип компонента техпаспорта
    /// </summary>
    public enum TypeTechPassportComponent
    {
        /// <summary>Тип перекрытий</summary>
        TypeFloorType = 10,

        /// <summary>Тип системы отопления</summary>
        TypeHeatingType = 20,

        /// <summary>Тип системы горячего водоснабжения</summary>
        TypeHotWaterType = 30,

        /// <summary>Газоснабжение</summary>
        TypeGas = 40,

        /// <summary>Количество выходов на чердак</summary>
        TypeRoofEntrance = 60,

        /// <summary>Расположения узлов ввода теплоснабжения</summary>
        TypeHeatingEntrance = 70,

        /// <summary>Расположения узлов ввода ГВС</summary>
        TypeHotWaterEntrance = 80,

        /// <summary>Расположения узлов ввода ХВС</summary>
        TypeColdWaterEntrance = 90,

        /// <summary>Расположения узлов ввода электроснабжения</summary>
        TypeElectroEntrance = 100
    }
}
