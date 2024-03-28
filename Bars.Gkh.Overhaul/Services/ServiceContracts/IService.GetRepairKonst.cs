namespace Bars.Gkh.Overhaul.Services.ServiceContracts
{
    using System.ServiceModel;
    using Bars.Gkh.Overhaul.Services.DataContracts;

    using CoreWCF.Web;

    public partial interface IService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetRepairKonst/{roId=null}")]
        GetRepairKonstResponse GetRepairKonst(string roId);
    }
}