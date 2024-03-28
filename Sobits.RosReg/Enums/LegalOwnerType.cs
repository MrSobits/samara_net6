namespace Sobits.RosReg.Enums
{
    using Bars.B4.Utils;

    public enum LegalOwnerType
    {
        [Display("Не установлено")]
        NotSet = 0,

        [Display("Резидент РФ")]
        Resident = 100,

        [Display("Не резидент РФ")]
        NotResident = 200
        
    }
}