namespace Bars.GkhGji.Regions.Tatarstan.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип запрошенных сведений
    /// </summary>
    public enum RequestInfoType
    {
        /// <summary>
        /// Документ
        /// </summary>
        [Display("Документ")]
        Document = 1,

        /// <summary>
        /// Фотоматериалы
        /// </summary>
        [Display("Фотоматериалы")]
        PhotoMaterials = 2,

        /// <summary>
        /// Видеоматериалы
        /// </summary>
        [Display("Видеоматериалы")]
        VideoMaterials = 3
    }
}