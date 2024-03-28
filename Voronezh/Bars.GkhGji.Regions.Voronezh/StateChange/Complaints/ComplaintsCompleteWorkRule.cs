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
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
    using Bars.GkhGji.Regions.Voronezh.DomainService;
    using Bars.GkhGji.Regions.Voronezh.Tasks.SendPaymentRequest;
    using Castle.Windsor;
    using Entities;

    public class ComplaintsCompleteWorkRule : IRuleChangeStatus
    {
        /// <summary>
        /// Домен сервис <see cref="LogOperation"/>
        /// </summary>
        public IDomainService<LogOperation> LogOperationDomainService { get; set; }
        public IDomainService<SMEVComplaintsRequest> SMEVComplaintsRequestDomain { get; set; }
        public IDomainService<SMEVComplaintsRequestFile> SMEVComplaintsRequestFileDomain { get; set; }
        public IDomainService<SMEVComplaints> SMEVComplaintsDomainService { get; set; }
        public IDomainService<SMEVComplaintsExecutant> SMEVComplaintsExecutantDomain { get; set; }

        public virtual IWindsorContainer Container { get; set; }

        public IComplaintsService ComplaintsService { get; set; }

        public IFileManager FileManager { get; set; }

        public string Id
        {
            get { return "ComplaintsCompleteWorkRule"; }
        }

        public string Name { get { return "Рассмотрение завершено"; } }
        public string TypeId { get { return "gji_smev_complaints"; } }
        public string Description
        {
            get
            {
                return "При переводе статуса будет отправлен запрос о принятом решении по рассмотрению заявки на досудебное обжалование";
            }
        }

        public ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {            
            var taskManager = Container.Resolve<ITaskManager>();
            var complaint = statefulEntity as SMEVComplaints;
            string state = complaint.ComplaintState;
            if (string.IsNullOrEmpty(state))
            {
                state = "Не задано";
            }
            if (complaint.SMEVComplaintsDecision == null || string.IsNullOrEmpty(complaint.Answer))
            {
                return ValidateResult.No("На заполнены поля Решение и Обоснование принятого решения");
            }
            if (complaint.FileInfo == null)
            {
                return ValidateResult.No("Требуется файл с решением");
            }
            var complaintExecutor = SMEVComplaintsExecutantDomain.GetAll()
                  .Where(x => x.IsResponsible && x.SMEVComplaints.Id == complaint.Id).FirstOrDefault();
            KndRequest acceptRequest = new KndRequest
            {
                Item = new sendComplaintEventType
                {
                    id = complaint.ComplaintId,
                    revokeFlag = state.ToLower() == "отзыв жалобы",
                    revokeFlagSpecified = state.ToLower() == "отзыв жалобы",
                    Item = new sendComplaintEventTypeResolutionStage
                    {
                        Item = new sendComplaintEventTypeResolutionStageResult
                        {
                            signer = new userType
                            {
                                id = "6501c759763fac7767ab79a7",
                                name = "Дробышев Игорь Анатольевич"
                            },
                            resolution = complaint.SMEVComplaintsDecision.FullName,
                            reason = complaint.Answer
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
                    TypeComplainsRequest = TypeComplainsRequest.Decision,
                    TextReq = voidRequestElement.ToString(),
                    ComplaintId = complaint.Id
                };
                SMEVComplaintsRequestDomain.Save(voidRequestRequest);
                if (complaint.FileInfo != null)
                {
                    SMEVComplaintsRequestFileDomain.Save(new SMEVComplaintsRequestFile
                    {
                        FileInfo = complaint.FileInfo,
                        SMEVComplaintsRequest = voidRequestRequest,
                        SMEVFileType = SMEVFileType.RequestAttachment
                    });
                }
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
