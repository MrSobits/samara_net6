﻿namespace Bars.Gkh.Services.ServiceContracts
{
    using System.ServiceModel;

    using Bars.Gkh.Services.DataContracts.GetHousesPhotoInfo;

    using CoreWCF.Web;

    public partial interface IService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetHousesPhotoInfo/{houseIds}")]
        GetHousesPhotoInfoResponse GetHousesPhotoInfo(string houseIds);
    }
}