namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    public enum TypeDefinitionProtocolProsecutor
    {
        [Display("О назначении времени и места рассмотрения дела")]
        TimeAndPlaceHearing = 10,

        [Display("О возвращении протокола должностному лицу")]
        ReturnProtocol = 20,

        [Display("Об отложении рассмотрения дела")]
        PostponeCase = 30,

        [Display("О приводе")]
        About = 40,

        [Display("О передаче дела на рассмотрение по подведомственности")]
        TransferCase = 50,

        // Тип нужен для Ставраполя
        [Display("Об отказе в удовлетворении ходатайства")]
        DenialPetition = 60,

        // Тип нужен для Ставраполя
        [Display("Об исправлении опечатки")]
        CorrectionMisprint = 70,

        // Тип нужен для Ставраполя, Саха
        [Display("Об истребовании сведений, необходимых для разрешения дела об административном правонарушении")]
        ReclamationInformation = 80,

        // Тип нужен для региона Саха, смоленск
        [Display("О продлении срока рассмотрения дела об административном правонарушении")]
        TermAdministrativeInfraction = 90,

        // Тип нужен для региона Тула
        [Display("Об отклонении ходатайства")]
        RequestDeviation = 100
    }
}