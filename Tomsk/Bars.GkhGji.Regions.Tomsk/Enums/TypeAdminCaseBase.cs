namespace Bars.GkhGji.Regions.Tomsk.Enums
{
    using Bars.B4.Utils;

    public enum TypeAdminCaseBase
    {
        [Display("Принято решение о возбуждении административного дела")]
        DecitionInitiate = 10,

        [Display("Выявлены нарушения в ходе визуального осмотра")]
        VisualInspection = 20,

        [Display("По результатам проверки")]
        ViolationNotRemoved = 30,

        [Display("Вынесено  постановление о прекращении адм.дела в отношении другого отв. лица")]
        ResolutionTermination = 40
    }
}