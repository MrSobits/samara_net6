namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    public enum ImagesGroup
    {
        [Display("Изображение жилого дома")]
        PictureHouse = 10,

        [Display("До кап. ремонта")]
        BeforeOverhaul = 20,

        [Display("Захламление МОП")]
        MOPWasted = 25,

        [Display("В ходе кап. ремонта")]
        InOverhaul = 30,

        [Display("После кап. ремонта")]
        AfterOverhaul = 40,

        [Display("Фотокарточка")]
        Avatar = 50
    }
}
