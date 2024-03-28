namespace Bars.Gkh.ConfigSections.Cr.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Форма дефектоной ведомости
    /// </summary>
    public enum TypeDefectListView
    {
        [Display("С данными из ДПКР")]
        WithOverhaulData = 0,

        [Display("Без данных из ДПКР")]
        WithoutOverhaulData = 10
    }
}
