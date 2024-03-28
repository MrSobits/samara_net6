namespace Bars.Gkh.Services.ServiceContracts
{
    using System.IO;
    using System.ServiceModel;

    using CoreWCF.Web;

    public partial interface IService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetManagementSysExport/{periodStart}")]
        Stream GetManagementSysExport(string periodStart);
    }
}
