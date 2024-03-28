namespace Bars.GkhGji.Regions.Voronezh.StateChanges
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
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
    using Bars.GkhGji.Regions.Voronezh.ASDOU;
    using Bars.GkhGji.Regions.Voronezh.DomainService;
    using Bars.GkhGji.Regions.Voronezh.Tasks.ERULSendInformationRequest;
    using Castle.Windsor;
    using Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;

    public class LicenseTerminationChangeStateRule : IRuleChangeStatus
    {
        /// <summary>
        /// Домен сервис <see cref="LogOperation"/>
        /// </summary>
        public IDomainService<SMEVERULReqNumber> SMEVERULReqNumberDomainService { get; set; }


        public virtual IWindsorContainer Container { get; set; }

        public IRPGUService RPGUService { get; set; }

        public IFileManager FileManager { get; set; }

        public string Id
        {
            get { return "LicenseTerminationChangeStateRule"; }
        }

        public string Name { get { return "Отправка сведений о прекращении действия лицензии в ЕРУЛ"; } }
        public string TypeId { get { return "gkh_manorg_license"; } }
        public string Description
        {
            get
            {
                return "При переводе статуса будет отправлен отчет о прекращении действия лицензии в ЕРУЛ";
            }
        }

        public ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            var license = statefulEntity as ManOrgLicense;

            if (!license.DateTermination.HasValue)
            {
                return ValidateResult.No("Не заполнено поле \"Дата прекращения действия лицензии\"");
            }

            var _taskManager = Container.Resolve<ITaskManager>();
            var newRequest = new SMEVERULReqNumber
            {
                CalcDate = DateTime.Now,
                ERULRequestType = BaseChelyabinsk.Enums.ERULRequestType.Changes,
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
