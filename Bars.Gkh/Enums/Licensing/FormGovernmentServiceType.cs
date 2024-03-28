namespace Bars.Gkh.Enums.Licensing
{
    using Bars.B4.Utils;

    /// <summary>
    /// Государственная услуга
    /// </summary>
    public enum FormGovernmentServiceType
    {
        /// <summary>
        /// Лицензирование
        /// </summary>
        [Display("Лицензирование")]
        Licensing = 10,

        /// <summary>
        /// Квалификационные аттестаты
        /// </summary>
        [Display("Квалификационные аттестаты")]
        QualificationCertificate = 20
    }
}