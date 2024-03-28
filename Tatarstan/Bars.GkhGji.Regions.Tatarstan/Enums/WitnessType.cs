namespace Bars.GkhGji.Regions.Tatarstan.Enums
{
    using Bars.B4.Utils;

    public enum WitnessType
    {
        /// <summary>
        /// Свидетель
        /// </summary>
        [Display("Свидетель")]
        Witness = 1,

        /// <summary>
        /// Потерпевший
        /// </summary>
        [Display("Потерпевший")]
        Victim = 2,

        /// <summary>
        /// Представитель потерпевшего
        /// </summary>
        [Display("Представитель потерпевшего")]
        DelegateVictim = 3,

        /// <summary>
        /// Понятой
        /// </summary>
        [Display("Понятой")]
        Eyewitness = 4
    }
}
