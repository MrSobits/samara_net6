namespace Bars.Gkh.Enums
{
    using B4.Utils;

    /// <summary>
    /// Тип контрагента (для 1468)
    /// </summary>
    public enum ContragentType
    {
        [Display("Не задано")]
        NotSet = 0,
        
        [Display("ПР")]
        Pr = 10,

        [Display("УО")]
        Uo = 20,

        [Display("ПКУ")]
        Pku = 30,

        [Display("ПЖУ")]
        Pzu = 40
    }
}