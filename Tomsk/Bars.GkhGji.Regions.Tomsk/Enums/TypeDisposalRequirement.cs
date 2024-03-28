namespace Bars.GkhGji.Regions.Tomsk.Enums
{
    using B4.Utils;

    /// <summary>
    /// Тип требования
    /// </summary>
    public enum TypeRequirement
    {
        [Display("Требование на проверку")]
        RequirementOnCheck = 10,

        [Display("Требование на предоставление информации")]
        RequirementOnInfoProvision = 20,

        [Display("Требование на проверку предписания")]
        RequirementOnPrescriptionCheck = 30,

        [Display("Требование на протокол")]
        RequirementOnProtocol = 40
    }
}