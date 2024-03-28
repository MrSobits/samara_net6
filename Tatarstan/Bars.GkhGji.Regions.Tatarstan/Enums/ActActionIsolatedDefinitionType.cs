namespace Bars.GkhGji.Regions.Tatarstan.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип определения
    /// </summary>
    public enum ActActionIsolatedDefinitionType
    {
        /// <summary>
        /// Об отказе в возбуждении дела
        /// </summary>
        [Display("Об отказе в возбуждении дела")]
        RefusalToInitiateProceedings = 1,
        
        /// <summary>
        /// О доставлении лица для составления протокола
        /// </summary>
        [Display("О доставлении лица для составления протокола")]
        PersonDeliveryForDrawingUpProtocol = 2
    }
}