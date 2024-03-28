namespace Bars.Gkh.Overhaul.Services.ServiceContracts
{ 
    using System.ServiceModel;
   using CoreWCF.Web;

    using Bars.Gkh.Overhaul.Services.DataContracts;

    public partial interface IService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetDataOfKons/{roId=null}")]
        GetDataOfKonsResponse GetDataOfKons(string roId);
    }
}