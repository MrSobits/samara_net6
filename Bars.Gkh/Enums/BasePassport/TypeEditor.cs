namespace Bars.Gkh.Enums.BasePassport
{
    using System.Xml.Serialization;
    using Bars.B4.Utils;

    /// <summary>
    /// Тип значения техпаспорта
    /// </summary>
    /// <remarks>
    /// При добавлении редактора, необходимо отредактировать методы 'getEditorConfig' и 'createRenderer' в B4.aspects.GkhPassport
    /// </remarks>
    public enum TypeEditor
    {
        [Display("Отображение")]
        [XmlEnum(Name = "Display")]
        Display = 0,

        [Display("Перечисление")]
        [XmlEnum(Name = "Enum")]
        Enum = 1,

        [Display("Значение из справочника")]
        [XmlEnum(Name = "Dict")]
        Dict = 2,

        [Display("Множественный выбор из справочника")]
        [XmlEnum(Name = "MultiDict")]
        MultiDict = 3,

        [Display("Текст")]
        [XmlEnum(Name="Text")]
        Text = 10,

        [Display("Дата")]
        [XmlEnum(Name="Date")]
        Date = 20,

        [Display("Целое")]
        [XmlEnum(Name="Int")]
        Int = 30,

        [Display("Число")]
        [XmlEnum(Name="Double")]
        Double = 40,
       
        //[Display("Число null")]
        //[XmlEnum(Name="NullableDouble")]
        //NullableDouble = 50,

        [Display("Финансовый")]
        [XmlEnum(Name="Decimal")]
        Decimal = 60,

        [Display("Логический")]
        [XmlEnum(Name="Bool")]
        Bool = 70,

        [Display("Тип шахты лифта")]
        [XmlEnum(Name="TypeLiftShaft")]
        TypeLiftShaft = 90,

        [Display("Тип подвала")]
        [XmlEnum(Name="TypeBasement")]
        TypeBasement = 100,

        [Display("Отпуск ресурсов")]
        [XmlEnum(Name="TransferResource")]
        TransferResource = 110,

        [Display("Выбор энергоэффективности")]
        [XmlEnum(Name="ChooseEnergy")]
        ChooseEnergy = 120,

        [Display("Тип отопления")]
        [XmlEnum(Name="TypeHeating")]
        TypeHeating = 130,

        [Display("Тип горячего водоснабжения")]
        [XmlEnum(Name="TypeHotWater")]
        TypeHotWater = 140,

        [Display("Тип холодного водоснабжения")]
        [XmlEnum(Name="TypeColdWater")]
        TypeColdWater = 150,

        [Display("Тип водоотведения(канализации)")]
        [XmlEnum(Name="TypeSewage")]
        TypeSewage = 160,

        [Display("Тип электроснабжения")]
        [XmlEnum(Name="TypePower")]
        TypePower = 170,

        [Display("Тип газоснабжения")]
        [XmlEnum(Name="TypeGas")]
        TypeGas = 180,

        [Display("Тип вентиляции")]
        [XmlEnum(Name="TypeVentilation")]
        TypeVentilation = 190,

        [Display("Тип водостока")]
        [XmlEnum(Name="TypeDrainage")]
        TypeDrainage = 200,

        [Display("Конструкция мусоропровода")]
        [XmlEnum(Name="ConstructionChute")]
        ConstructionChute = 210,

        [Display("Тип крыши")]
        [XmlEnum(Name="TypeRoof")]
        TypeRoof = 240,

        [Display("Тип лифта")]
        [XmlEnum(Name="TypeLift")]
        TypeLift = 250,

        [Display("Вид коммунального ресурса")]
        [XmlEnum(Name = "TypeCommResourse")]
        TypeCommResourse = 260,

        [Display("Наличие прибора учета")]
        [XmlEnum(Name = "ExistMeterDevice")]
        ExistMeterDevice = 270,

        [Display("Тип интерфейса прибора учета")]
        [XmlEnum(Name = "InterfaceType")]
        InterfaceType = 280,

        [Display("Единица измерения")]
        [XmlEnum(Name = "UnutOfMeasure")]
        UnutOfMeasure = 290,

        [Display("Пожаротушение")]
        [XmlEnum(Name = "FirefightingType")]
        FirefightingType = 300,

        [Display("Да/Нет/Не задано")]
        [XmlEnum(Name = "YesNoNotSet")]
        YesNoNotSet = 310,

        [Display("Лифт(ООИ)")]
        [XmlEnum(Name = "RealityObjectStructuralElementLift")]
        RealityObjectStructuralElementLift = 320
    }
}
