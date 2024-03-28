namespace Bars.GkhGji.ViewModel.ResolutionRospotrebnadzor
{
    using Bars.B4;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// View Постановление Роспотребнадзора
    /// </summary>
    public class ResolutionRospotrebnadzorViewModel
        : ResolutionRospotrebnadzorViewModel<ResolutionRospotrebnadzor>
    {
    }

    /// <summary>
    /// Generic View Постановление Роспотребнадзора
    /// </summary>
    public class ResolutionRospotrebnadzorViewModel<T> : BaseViewModel<T>
        where T : ResolutionRospotrebnadzor
    {
    }
}