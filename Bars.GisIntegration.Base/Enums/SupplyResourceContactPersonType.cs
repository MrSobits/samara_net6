namespace Bars.GisIntegration.Base.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Лицо, являющееся стороной договора с поставщиком ресурсов
    /// </summary>
    public enum SupplyResourceContactPersonType
    {
        /// <summary>
        /// Представитель собственников
        /// </summary>
        [Display("Представитель собственников")]
        RepresentativeOwners = 10,

        /// <summary>
        /// Собственник
        /// </summary>
        [Display("Собственник")]
        Owner = 20,

        /// <summary>
        /// Арендатор нежилых помещений
        /// </summary>
        [Display("Арендатор нежилых помещений")]
        TenantNonResidentialRoom = 30
    }
}
