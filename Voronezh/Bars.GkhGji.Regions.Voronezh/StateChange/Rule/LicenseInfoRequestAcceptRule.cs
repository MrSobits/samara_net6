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
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.AppealCits;
    using Bars.GkhGji.Regions.Voronezh.ASDOU;
    using Bars.GkhGji.Regions.Voronezh.DomainService;
    using Castle.Windsor;
    using Entities;

    public class LicenseInfoRequestAcceptRule : IRuleChangeStatus
    {
        /// <summary>
        /// Домен сервис <see cref="LogOperation"/>
        /// </summary>
        public IDomainService<LogOperation> LogOperationDomainService { get; set; }

        public IDomainService<AppealCitsAnswer> AppealCitsAnswerDomainService { get; set; }
        public IDomainService<AppealCitsStatSubject> AppealCitsStatSubjectDomainService { get; set; }
        public IDomainService<AppealCitsExecutionType> AppealCitsExecutionTypeDomainService { get; set; }

        public virtual IWindsorContainer Container { get; set; }

        public IRPGUService RPGUService { get; set; }

        public IFileManager FileManager { get; set; }

        public string Id
        {
            get { return "LicenseInfoRequestAcceptRule"; }
        }

        public string Name { get { return "Отправка отчета об исполнении услуги"; } }
        public string TypeId { get { return "gji_license_action"; } }
        public string Description
        {
            get
            {
                return "При переводе статуса будет отправлен отчет об исполнении госуслуги";
            }
        }

        public ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            var appeal = statefulEntity as LicenseAction;
            if (!string.IsNullOrEmpty(appeal.MessageId))
            {
                if (RPGUService.SendLicActionAcceptMessage(Convert.ToInt64(statefulEntity.Id), true))
                {
                    return ValidateResult.Yes();
                }
                return ValidateResult.No("Не удалось отправить статус заявки на РПГУ");
            }
            return ValidateResult.Yes();
        }       


    }
}
