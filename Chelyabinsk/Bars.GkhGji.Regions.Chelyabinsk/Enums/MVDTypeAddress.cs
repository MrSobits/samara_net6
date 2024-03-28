namespace Bars.GkhGji.Regions.Chelyabinsk.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип адреса
    /// </summary>
    public enum MVDTypeAddress
    {
        /// <summary>
        /// Место рождения 000
        /// </summary>
        [Display("Не указывать в запросе")]
        NotSet = 10,

        /// <summary>
        /// Место рождения 000
        /// </summary>
        [Display("Место рождения")]
        BirthPlace = 20,

        /// <summary>
        /// Место жительства 200
        /// </summary>
        [Display("Место жительства")]
        LivingPlace = 30,

        /// <summary>
        /// Место пребывания 201
        /// </summary>
        [Display("Место пребывания")]
        FactPlace = 40,

        /// <summary>
        /// Иное 202
        /// </summary>
        [Display("Иное")]
        Other = 50,

    }
}