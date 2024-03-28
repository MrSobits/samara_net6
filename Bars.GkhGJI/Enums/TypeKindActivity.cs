namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип вида деятельности
    /// </summary>
    public enum TypeKindActivity
    {
        [Display("Деятельность по управлению МКД")]
        ManagmentMkd = 10,

        [Display("Деятельность по обслуживанию МКД")]
        ServiceMkd = 20,

        [Display("Деятельность по управлению и обслуживанию МКД")]
        ManagmentAndServiceMkd = 30
    }
}