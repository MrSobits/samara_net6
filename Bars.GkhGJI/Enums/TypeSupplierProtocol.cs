namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип РСО составившей протокол
    /// </summary>
    public enum TypeSupplierProtocol
    {
        /// <summary>
        /// Не задано
        /// </summary>
        [Display("Не задано")]
        NonSet = 0,

        /// <summary>
        /// Газоснабжающая организация
        /// </summary>
        [Display("Газоснабжающая организация")]
        Gas = 10,

        /// <summary>
        ///Электроснабжающая организация
        /// </summary>
        [Display("Электроснабжающая организация")]
        Electro = 20,

        /// <summary>
        /// Водоснабщаюшая организация
        /// </summary>
        [Display("Водоснабщаюшая организация")]
        Water = 30,

        /// <summary>
        /// Теплоснабжающая организация
        /// </summary>
        [Display("Теплоснабжающая организация")]
        Heat = 40,

    }
}