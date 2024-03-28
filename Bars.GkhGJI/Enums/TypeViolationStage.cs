namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Этап нарушения проверки ГЖИ
    /// </summary>
    public enum TypeViolationStage
    {
        [Display("Выявление нарушения")]
        Detection = 10,

        [Display("Указание к устранению нарушения")]
        InstructionToRemove = 20,

        [Display("Устранение нарушения")]
        Removal = 30,

        [Display("Наказание за неустранение нарушения")]
        Sentence = 40
    }
}