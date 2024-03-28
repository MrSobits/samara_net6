namespace Bars.GkhGji.Regions.Habarovsk.StateChanges
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Xml.Linq;
    using System.Xml.Serialization;
    using B4.DataAccess;
    using B4.Modules.States;
    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.AppealCits;
    using Bars.GkhGji.Regions.Habarovsk.ASDOU;
    using Bars.GkhGji.Regions.Habarovsk.DomainService;
    using Bars.GkhGji.Regions.Habarovsk.Tasks.SendPaymentRequest;
    using Castle.Windsor;
    using Entities;

    public class ProtocolOSPRequestRule : IRuleChangeStatus
    {

        public virtual IWindsorContainer Container { get; set; }

        public string Id
        {
            get { return "ProtocolOSPRequestRule"; }
        }

        public string Name { get { return "Уведомление заявителя"; } }
        public string TypeId { get { return "oss_request"; } }
        public string Description
        {
            get
            {
                return "При переводе статуса заявителю будет отправлено уведомление о смене статуса заявления";
            }
        }

        public ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            var taskManager = Container.Resolve<ITaskManager>();
            var complaint = statefulEntity as ProtocolOSPRequest;
            if (!string.IsNullOrEmpty(newState.Description) && !string.IsNullOrEmpty(complaint.Email))
            {
                EmailSender emailSender = EmailSender.Instance;
                emailSender.Send(complaint.Email, "Рассмотрение заявления", newState.Description, null);
            }
          

            return ValidateResult.Yes();

        }    

    }
}
