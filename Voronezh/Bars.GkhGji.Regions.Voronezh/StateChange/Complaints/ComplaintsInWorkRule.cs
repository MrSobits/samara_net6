namespace Bars.GkhGji.Regions.Voronezh.StateChange
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
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums;
    using Bars.GkhGji.Regions.Voronezh.DomainService;
    using Bars.GkhGji.Regions.Voronezh.Tasks.SendPaymentRequest;
    using Castle.Windsor;
    using Entities;

    public class ComplaintsInWorkRule : IRuleChangeStatus
    {
        /// <summary>
        /// Домен сервис <see cref="LogOperation"/>
        /// </summary>
        public IDomainService<LogOperation> LogOperationDomainService { get; set; }
        public IDomainService<SMEVComplaintsRequest> SMEVComplaintsRequestDomain { get; set; }
        public IDomainService<SMEVComplaints> SMEVComplaintsDomainService { get; set; }
        public IDomainService<SMEVComplaintsExecutant> SMEVComplaintsExecutantDomain { get; set; }

        public virtual IWindsorContainer Container { get; set; }

        public IComplaintsService ComplaintsService { get; set; }

        public IFileManager FileManager { get; set; }

        public string Id
        {
            get { return "ComplaintsInWorkRule"; }
        }

        public string Name { get { return "Проверка"; } }
        public string TypeId { get { return "gji_smev_complaints"; } }
        public string Description
        {
            get
            {
                return "При переводе статуса будет отправлен запрос о приеме на проверку заявки на досудебное обжалование";
            }
        }

        public ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            var taskManager = Container.Resolve<ITaskManager>();
            var complaint = statefulEntity as SMEVComplaints;

            var complaintExecutor = SMEVComplaintsExecutantDomain.GetAll()
                  .Where(x => x.IsResponsible && x.SMEVComplaints.Id == complaint.Id).FirstOrDefault();
            KndRequest acceptRequest = new KndRequest
            {
                Item = new sendComplaintEventType
                {
                    id = complaint.ComplaintId,
                    Item = new sendComplaintEventTypeAssignStage
                    {
                        Item = new voidType
                        {
                           
                        }
                    },
                    eventTime = DateTime.Now,
                    unit = new unitType
                    {
                        id = "68566872-7cc6-ea4b-0f69-eac0cf88819f",
                        Value = "ГОСУДАРСТВЕННАЯ ЖИЛИЩНАЯ ИНСПЕКЦИЯ ВОРОНЕЖСКОЙ ОБЛАСТИ"
                    }
                }
            };

            if (acceptRequest != null)
            {
                var voidRequestElement = ToXElement<KndRequest>(acceptRequest);
                SMEVComplaintsRequest voidRequestRequest = new SMEVComplaintsRequest
                {
                    CalcDate = DateTime.Now,
                    TypeComplainsRequest = TypeComplainsRequest.Void,
                    TextReq = voidRequestElement.ToString(),
                    ComplaintId = complaint.Id
                };
                SMEVComplaintsRequestDomain.Save(voidRequestRequest);
                var baseParams = new BaseParams();

                if (!baseParams.Params.ContainsKey("taskId"))
                    baseParams.Params.Add("taskId", voidRequestRequest.Id.ToString());

                taskManager.CreateTasks(new SendComplaintsCustomRequestTaskProvider(Container), baseParams);
            }


            return ValidateResult.Yes();
        }

        private XElement ToXElement<T>(object obj)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (TextWriter streamWriter = new StreamWriter(memoryStream))
                {
                    var xmlSerializer = new XmlSerializer(typeof(T));
                    xmlSerializer.Serialize(streamWriter, obj);
                    return XElement.Parse(Encoding.UTF8.GetString(memoryStream.ToArray()));
                }
            }
        }

    }
}
