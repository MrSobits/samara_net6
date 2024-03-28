namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип акта проверки ГЖИ
    /// </summary>
    public enum TypeActCheckGji
    {
        [Display("Акт проверки общий")]
        ActCheckGeneral = 10,

        [Display("Акт проверки на 1 дом")]
        ActCheckIndividual = 20,

        [Display("Акт проверки документа ГЖИ")]
        ActCheckDocumentGji = 30,

        [Display("Акт осмотра")]
        ActView = 40,

        [Display("Акт КНМ без взаимодействия")]
        ActActionIsolated = 50
    }
}