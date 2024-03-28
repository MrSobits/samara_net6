namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип определения акта
    /// </summary>
    public enum TypeDefinitionAct
    {
        [Display("Об отказе в возбуждении дела")]
        RefusingProsecute = 10,

        [Display("О доставлении лица для составления протокола")]
        TransportationToProtocol = 20
    }
}