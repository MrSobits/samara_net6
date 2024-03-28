namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип инженерной сети
    /// </summary>
    public enum TypeEngineerSystem
    {
        [Display("Холодное водоснабжение")]
        ConstructiveElement = 10,

        [Display("Горячее водоснабжение")]
        Engineer = 20,

        [Display("Теплоснабжение")]
        MeteringUnit = 30,

        [Display("Водоотведение")]
        WasteWater = 40,

        [Display("Электроснабжение")]
        Electro = 50,

        [Display("Газоснабжение")]
        Gas = 60
    }
}
