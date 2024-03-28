namespace Bars.GkhGji.Regions.Chelyabinsk.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип основания проверки юр. лиц
    /// </summary>
    public enum TypeBaseOMSU
    {
        /// <summary>
        /// Не задано
        /// </summary>
        [Display("Не задано")]
        NotSet = 10,

        /// <summary>
        /// Истечение трех лет со дня государственной регистрации ЮЛ/ИП
        /// </summary>
        [Display("Истечение трех лет со дня государственной регистрации ОМСУ")]
        StateRegistrationAfter3Years = 20,

        /// <summary>
        /// Истечение трех лет со дня окончания проведения последней плановой проверки ЮЛ/ИП
        /// </summary>
        [Display("Истечение трех лет со дня окончания проведения последней плановой проверки ОМСУ")]
        LastWorkAfter3Years = 30,

        /// <summary>
        /// Истечение трех лет со дня начала осуществления ЮЛ/ИП предпринимательской деятельности
        /// </summary>
        [Display("Иное")]
        Other = 40
    }
}