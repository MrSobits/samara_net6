namespace Bars.GkhGji.Regions.Tatarstan.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    
    // TODO : Расскоментировать после реализации GisIntegration
    /*using Bars.GisIntegration.Base.Domain;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Tor.Entities;*/
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Disposal;

    using Castle.Windsor;

    public class TatarstanDisposalService : ITatarstanDisposalService
    {
        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public IDataResult GetDependenciesString(BaseParams baseParams)
        {
            var disposalId = baseParams.Params.GetAsId("disposalId");
            var dependenciesList = this.GetDependenciesItems(disposalId);
            var dependenciesHash = dependenciesList.Select(x => x.Type).ToHashSet();

            var refFuncs = new List<Func<string>>
            {
                () => dependenciesHash.Contains(typeof(DisposalAnnex)) ? "Приложения" : null,
                () => dependenciesHash.Contains(typeof(DisposalExpert)) ? "Эксперты" : null,
                () => dependenciesHash.Contains(typeof(DisposalProvidedDoc)) ? "Предоставляемые документы" : null,
                () => dependenciesHash.Contains(typeof(DisposalTypeSurvey)) ? "Типы обследования" : null,
                () => dependenciesHash.Contains(typeof(DisposalInspFoundationCheck)) ? "НПА проверки" : null,
                () => dependenciesHash.Contains(typeof(DisposalSurveyObjective)) ? "Задачи проверки" : null,
                () => dependenciesHash.Contains(typeof(DisposalSurveyPurpose)) ? "Цели проверки" : null,
                () => dependenciesHash.Contains(typeof(DisposalVerificationSubject)) ? "Предметы проверки" : null,
                //  () => dependenciesHash.Contains(typeof(RisTask)) ? "Запись в реестре Интеграция с ЕРП" : null,
               // () => dependenciesHash.Contains(typeof(TorTask)) ? "Запись в реестре Интеграция с ТОР КНД" : null,
            };

            var refs = refFuncs.Select(x => x()).Where(x => x != null).ToArray();
            return refs.Any() 
                ? new BaseDataResult($"При удалении данной записи произойдет удаление всех связанных объектов: {string.Join(", ", refs)}." +
                $" Вы действительно хотите удалить документ и все связанные записи?")
                : new BaseDataResult();
        }

        /// <inheritdoc />
        public List<TatarstanDisposalBeforeDeleteItem> GetDependenciesItems(long disposalId)
        {
            var annexService = this.Container.Resolve<IDomainService<DisposalAnnex>>();
            var expertService = this.Container.Resolve<IDomainService<DisposalExpert>>();
            var provDocsService = this.Container.Resolve<IDomainService<DisposalProvidedDoc>>();
            var typeServiceService = this.Container.Resolve<IDomainService<DisposalTypeSurvey>>();
            var disposalInspFoundationCheckDomain = this.Container.Resolve<IDomainService<DisposalInspFoundationCheck>>();
            var surveyObjectiveDomain = this.Container.Resolve<IDomainService<DisposalSurveyObjective>>();
            var surveyPurposeDomain = this.Container.Resolve<IDomainService<DisposalSurveyPurpose>>();
            var disposalVerificationSubjectDomain = this.Container.Resolve<IDomainService<DisposalVerificationSubject>>();
            // TODO : Расскоментировать после реализации GisIntegration
           // var taskTriggerDomain = this.Container.Resolve<IDomainService<RisTaskTrigger>>();
           /* var risTaskDomain = this.Container.Resolve<IDomainService<RisTask>>();
            var torTaskDomain = this.Container.Resolve<IDomainService<TorTask>>();*/

            using (this.Container.Using(annexService,
                expertService,
                provDocsService,
                typeServiceService,
                disposalInspFoundationCheckDomain,
                surveyObjectiveDomain,
                surveyPurposeDomain,
                disposalVerificationSubjectDomain))
               // torTaskDomain,
               // risTaskDomain,
               // taskTriggerDomain))
            {
             /*   var taskIds = risTaskDomain.GetAll()
                    .Where(x => x.DocumentGji.Id == disposalId)
                    .Select(x => x.Id).ToList();*/

                return new List<TatarstanDisposalBeforeDeleteItem>
                    {
                        {
                            new TatarstanDisposalBeforeDeleteItem(typeof(DisposalAnnex), annexService.GetAll()
                                .Where(x => x.Disposal.Id == disposalId)
                                .Select(x => x.Id).ToList(), 0)
                        },
                        {
                           new TatarstanDisposalBeforeDeleteItem( typeof(DisposalExpert), expertService.GetAll()
                               .Where(x => x.Disposal.Id == disposalId)
                               .Select(x => x.Id).ToList(), 1)
                        },
                        {
                            new TatarstanDisposalBeforeDeleteItem(typeof(DisposalProvidedDoc), provDocsService.GetAll()
                                .Where(x => x.Disposal.Id == disposalId)
                                .Select(x => x.Id).ToList(), 2)
                        },
                        {
                            new TatarstanDisposalBeforeDeleteItem(typeof(DisposalTypeSurvey), typeServiceService.GetAll()
                                .Where(x => x.Disposal.Id == disposalId)
                                .Select(x => x.Id).ToList(), 3)
                        },
                        {
                            new TatarstanDisposalBeforeDeleteItem(typeof(DisposalInspFoundationCheck), disposalInspFoundationCheckDomain.GetAll()
                                .Where(x => x.Disposal.Id == disposalId)
                                .Select(x => x.Id).ToList(), 4)
                        },
                        {
                            new TatarstanDisposalBeforeDeleteItem(typeof(DisposalSurveyObjective), surveyObjectiveDomain.GetAll()
                                .Where(x => x.Disposal.Id == disposalId)
                                .Select(x => x.Id).ToList(), 5)
                        },
                        {
                            new TatarstanDisposalBeforeDeleteItem(typeof(DisposalSurveyPurpose), surveyPurposeDomain.GetAll()
                                .Where(x => x.Disposal.Id == disposalId)
                                .Select(x => x.Id).ToList(), 6)
                        },
                        {
                            new TatarstanDisposalBeforeDeleteItem(typeof(DisposalVerificationSubject), disposalVerificationSubjectDomain.GetAll()
                                .Where(x => x.Disposal.Id == disposalId)
                                .Select(x => x.Id).ToList(), 7)
                        },
                   /*     {
                            new TatarstanDisposalBeforeDeleteItem(typeof(RisTaskTrigger), taskTriggerDomain.GetAll()
                                .Where(x => taskIds.Contains(x.Task.Id))
                                .Select(x => x.Id).ToList(), 8)
                        },
                        {
                            new TatarstanDisposalBeforeDeleteItem(typeof(RisTask), taskIds, 9)
                        },
                        {
                            new TatarstanDisposalBeforeDeleteItem(typeof(TorTask), torTaskDomain.GetAll()
                                .Where(x => x.Disposal.Id == disposalId)
                                .Select(x => x.Id).ToList(), 10)
                        }*/
                    }.Where(x => x.IdsList.Any())
                    .OrderBy(x => x.Order)
                    .ToList();
            }
        }
    }
}
