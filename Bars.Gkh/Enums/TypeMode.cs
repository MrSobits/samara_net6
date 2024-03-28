namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Код раздела
    /// </summary>
    public enum TypeMode
    {
        [Display("Режим работы")]
        WorkMode = 10,

        [Display("Прием граждан")]
        ReceptionCitizens = 20,

        [Display("Работа диспетчерской службы")]
        DispatcherWork = 30,

        [Display("Прием юр. лиц")]
        ReceptionJurPerson = 40
    }
}
