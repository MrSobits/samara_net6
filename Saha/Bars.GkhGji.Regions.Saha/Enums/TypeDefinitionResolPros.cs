namespace Bars.GkhGji.Regions.Saha.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип определения постановления прокуратуры
    /// </summary>
    public enum TypeDefinitionResolPros
    {
        [Display("О назначении времени и места рассмотрения дела")]
        TimeAndPlaceHearing = 10,

        [Display("Об отложении рассмотрения дела")]
        PostponeCase = 30
    }
}