namespace Sobits.RosReg.Enums
{
    using Bars.B4.Utils;

    public enum OwnerType
    {
        [Display("Не определен")]
        NotSet = 0,

        [Display("Физ. лицо")]
        Physical = 10,

        [Display("Юр. лицо")]
        Legal = 20,

        [Display("Муниципальное")]
        Municipal = 30
    }
}