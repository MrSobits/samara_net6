namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    public enum ActionLevelNormativeAct
    {
        /// <summary>
        /// На региональном уровне
        /// </summary>
        [Display("На региональном уровне")]
        Regional = 0,

        /// <summary>
        /// На муниципальном уровне
        /// </summary>
        [Display("На муниципальном уровне")]
        Municipal = 1,

        /// <summary>
        /// На региональном и муниципальном уровнях
        /// </summary>
        [Display("На региональном и муниципальном уровнях")]
        RegionalAndMunicipal = 2
    }
}