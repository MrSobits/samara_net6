namespace Bars.GkhGji.Regions.Voronezh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип адреса проверяемого лица
    /// </summary>
    public enum PublicPropertyLevel
    {

        /// <summary>
        /// Pапрос в отношении имущества субъекта России
        /// </summary>
        [Display("запрос в отношении имущества субъекта России")]
        Subj = 20,

        /// <summary>
        /// запрос в отношении имущества муниципального образования
        /// </summary>
        [Display("Запрос в отношении имущества муниципального образования")]
        Mun = 30

    }
}