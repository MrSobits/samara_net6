namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Статусы участников процесса
    /// </summary>
    public enum OrgStateRole
    {
        [Display("Осуществляет деятельность")]
        Active = 10,

        [Display("Деятельность прекращена")]
        Stopped = 20
    }
}
