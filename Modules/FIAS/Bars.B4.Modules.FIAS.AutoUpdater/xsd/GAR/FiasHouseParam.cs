namespace Bars.B4.Modules.FIAS.AutoUpdater.GAR
{
    using System.Xml.Serialization;

    using Bars.B4.Modules.FIAS.AutoUpdater.Utils;
    
    /// <summary>
    /// Класс для десериализации параметров домов ФИАС
    /// </summary>
    [FiasEntityName("as_houses_params", "PARAMS")]
    [XmlType("PARAM")]
    public class FiasHouseParam : FiasParam
    {
        
    }
}