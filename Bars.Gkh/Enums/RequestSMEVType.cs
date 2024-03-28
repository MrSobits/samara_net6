namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип запроса в СМЭВ
    /// </summary>
    public enum RequestSMEVType
    {
        /// <summary>Запрос МВД</summary>
        [Display("Запрос МВД")]
        MVDRequest = 10,

        /// <summary>Запрос ЕГРЮЛ</summary>
        [Display("Запрос ЕГРЮЛ")]
        EGRULRequest = 20,

        /// <summary>Запрос ЕГРИП</summary>
        [Display("Запрос ЕГРИП")]
        EGRIPRequest = 30,

        /// <summary>Запрос ЕГРН</summary>
        [Display("Запрос ЕГРН")]
        EGRNRequest = 40,

        /// <summary>Не задано</summary>
        [Display("Не задано")]
        NotSet = 0,


    }
}