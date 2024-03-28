namespace Bars.GkhGji.Regions.Chelyabinsk.StateChanges
{
    using B4.Modules.States;
    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
    using Bars.GkhGji.Regions.Chelyabinsk.Tasks.ERULSendInformationRequest;
    using Castle.Windsor;
    using System;

    public class LicenseApprovalChangeStateRule : IRuleChangeStatus
    {
        /// <summary>
        /// Домен сервис <see cref="LogOperation"/>
        /// </summary>
        public IDomainService<SMEVERULReqNumber> SMEVERULReqNumberDomainService { get; set; }


        public virtual IWindsorContainer Container { get; set; }

        public IFileManager FileManager { get; set; }

        public string Id
        {
            get { return "LicenseApprovalChangeStateRule"; }
        }

        public string Name { get { return "Отправка сведений об утверждении лицензии в ЕРУЛ"; } }
        public string TypeId { get { return "gkh_manorg_license"; } }
        public string Description
        {
            get
            {
                return "При переводе статуса будет отправлен отчет об утверждении лицензии в ЕРУЛ";
            }
        }

        public ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            var license = statefulEntity as ManOrgLicense;

            var _taskManager = Container.Resolve<ITaskManager>();
            var newRequest = new SMEVERULReqNumber
            {
                CalcDate = DateTime.Now,
                ERULRequestType = ERULRequestType.Changes,
                ManOrgLicense = license,
                RequestState = RequestState.NotFormed
            };
            SMEVERULReqNumberDomainService.Save(newRequest);

            BaseParams bParams = new BaseParams();
            bParams.Params.Add("taskId", newRequest.Id);
            _taskManager.CreateTasks(new SendERULRequestTaskProvider(Container), bParams);
            return ValidateResult.Yes();
        }


    }
}
