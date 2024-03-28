namespace Sobits.RosReg.Enums
{
    using Bars.B4.Utils;

    public enum ExtractCategory
    {
        [Display("Не указана")]
        NotSet = 0,

        [Display("ЕГРН - Пощемения")]
        EgrnRoom = 100,

        [Display("ЕГРН - Помещения юр.лица")]
        EgrnJurRoom = 101,

        [Display("ЕГРЮЛ")]
        Egrul = 200,

        [Display("ЕГРИП")]
        Egrip = 300,

        [Display("ЕГРН - Парковочное место")]
        EgrnParking = 400
    }
}