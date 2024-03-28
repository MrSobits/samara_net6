using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Bars.Gkh.DeloIntegration.Wcf.Services
{
    using System.ServiceModel.Web;
    using Bars.Gkh.DeloIntegration.Wcf.Contracts;

    using Castle.Windsor;

    [ServiceContract]
    public interface IDeloService
    {
        IWindsorContainer Container { get; set; }

        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "SetAppeals")]
        DeloResult[] SetAppeals(DeloAppealList appeals);

        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "SetAppeal")]
        DeloResult SetAppeal(DeloAppeal appeal);
        
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetAnswers")]
        GetAnswerResponse GetAnswers();

        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "UpdateAnswers")]
        UpdateAnswerResponse UpdateAnswers(AnswerList answers);
         
    }
}
