namespace Bars.GkhCr.Enums
{
    using B4.Utils;

    /// <summary>
    /// Причина изменения вида работ из объекта КР
    /// </summary>
    public enum TypeWorkCrReason
    {
        [Display("Не указано")]
        NotSet = 0,

        [Display("Не требуется ремонт в рамках Долгосрочной программы")]
        NotRequiredDpkr = 10,

        [Display("Перенос срока ремонта на более поздний срок")]
        NewYear = 20,

        [Display("Не требуется ремонт в рамках текущей Краткосрочной программы")]
        NotRequiredShortProgram = 30
    }
}
