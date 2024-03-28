namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип кровли
    /// </summary>
    public enum TypeRoof
    {
        [Display("Не задано")]
        None = 0,

        [Display("Плоская")]
        Plane = 10,

        [Display("Скатная")]
        Pitched = 20,

        [Display("Односкатная")]
        Shed = 30,

        [Display("Двускатная")]
        Gable = 40,

        [Display("Вальмовая")]
        Hipped = 50,

        [Display("Полувальмовая")]
        HalfHipped = 60,

        [Display("Вальмовая сложной формы")]
        ComplexHipped = 70,

        [Display("Шатровая")]
        Hip = 80
    }
}
