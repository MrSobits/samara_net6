﻿using Bars.Gkh.RegOperator.Services.DataContracts;

namespace Bars.Gkh.RegOperator.Services.ServiceContracts
{
    using System.ServiceModel;

    public partial interface IService
    {
        [OperationContract]
        [XmlSerializerFormat]
        SyncRkcResult SyncChargePaymentRkc(SyncChargePaymentRkcRecord record);
    }
}