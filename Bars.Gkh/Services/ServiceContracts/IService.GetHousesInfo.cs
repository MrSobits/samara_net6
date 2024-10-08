﻿namespace Bars.Gkh.Services.ServiceContracts
{
    using System.ServiceModel;

    using Bars.Gkh.Services.DataContracts.HousesInfo;

    using CoreWCF.Web;

    public partial interface IService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetHousesInfo/{houseIds}")]
        GetHousesInfoResponse GetHousesInfo(string houseIds);
    }
}