namespace Bars.Gkh.RegOperator.Services.ServiceContracts
{
    using System.Collections.Generic;
    using System.ServiceModel;
    using Bars.Gkh.RegOperator.Services.DataContracts.GetOwnerInfo;

    using CoreWCF.Web;

    public partial interface IService
    {

        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetOwnerInfo/{accNum},{email},{isSelected}")]
        bool GetOwnerInfo(List<string> accNum, string email, bool isSelected);
    }
}
