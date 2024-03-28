namespace Bars.B4.Modules.FIAS.AutoUpdater.GAR
{
    using System.Xml.Serialization;

    using Bars.B4.Modules.FIAS.AutoUpdater.Utils;
    
    /// <summary>
    /// Класс для десериализации параметров адресообразующих объектов
    /// </summary>
    [FiasEntityName("as_addr_obj_params", "PARAMS")]
    [XmlType("PARAM")]
    public class FiasAddressObjectParam : FiasParam
    {
        
    }
}