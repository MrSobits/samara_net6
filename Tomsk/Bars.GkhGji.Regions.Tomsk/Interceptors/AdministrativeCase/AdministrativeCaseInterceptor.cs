namespace Bars.GkhGji.Regions.Tomsk.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tomsk.Entities;
    using Bars.GkhGji.Regions.Tomsk.Entities.Inspection;

    public class AdministrativeCaseInterceptor : EmptyDomainInterceptor<AdministrativeCase>
    {
        public override IDataResult BeforeCreateAction(IDomainService<AdministrativeCase> service, AdministrativeCase entity)
        {
            // Перед сохранением меняем присваиваем статус начальный к документу
            var stateProvider = Container.Resolve<IStateProvider>();
            stateProvider.SetDefaultState(entity);

            return this.Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<AdministrativeCase> service, AdministrativeCase entity)
        {
            var serviceStage = Container.Resolve<IDomainService<InspectionGjiStage>>();
            var childrenDomain = Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var annexDomain = Container.Resolve<IDomainService<AdministrativeCaseAnnex>>();
            var articleLawDomain = Container.Resolve<IDomainService<AdministrativeCaseArticleLaw>>();
            var docDomain = Container.Resolve<IDomainService<AdministrativeCaseDoc>>();
            var requirementDomain = Container.Resolve<IDomainService<Requirement>>();
            var providedDocDomain = Container.Resolve<IDomainService<AdministrativeCaseProvidedDoc>>();
            var descriptionDomain = Container.Resolve<IDomainService<AdministrativeCaseDescription>>();
            var violationDomain = Container.Resolve<IDomainService<AdministrativeCaseViolation>>();

            try
            {
                if (childrenDomain.GetAll().Count(x => x.Parent.Id == entity.Id) > 0)
                {
                    return this.Failure("Административное дело имеет дочерние документы.");
                }

                var refFuncs =
                    new List<Func<long, string>>
                    {
                        id => annexDomain.GetAll().Any(x => x.AdministrativeCase.Id == id) ? "Приложения" : null,
                        id => articleLawDomain.GetAll().Any(x => x.AdministrativeCase.Id == id) ? "Статьи закона" : null,
                        id => docDomain.GetAll().Any(x => x.AdministrativeCase.Id == id) ? "Документы" : null,
                        id => requirementDomain.GetAll().Any(x => x.Document.Id == id) ? "Требования" : null,
                        id => providedDocDomain.GetAll().Any(x => x.AdministrativeCase.Id == id) ? "Предоставляемые документы" : null
                    };

                var refs = refFuncs.Select(x => x(entity.Id)).Where(x => x != null).ToArray();

                var message = string.Empty;

                if (refs.Length > 0)
                {
                    message = refs.Aggregate(message, (current, str) => current + string.Format(" {0}; ", str));
                    message = string.Format("Существуют связанные записи в следующих таблицах: {0}", message);
                    return Failure(message);
                }

                // Удаляем все записи В таблице ParentChildren связанные с этим документом
                // проверяем есть ли дочерние документы у акта проверки
                var childrenIds = childrenDomain.GetAll()
                    .Where(x => x.Children.Id == entity.Id)
                    .Select(x => x.Id)
                    .ToList();

                foreach (var childId in childrenIds)
                {
                    childrenDomain.Delete(childId);
                }

                // Удаляем все дочерние Нарушения
                var violationIds = violationDomain.GetAll().Where(x => x.Document.Id == entity.Id)
                    .Select(x => x.Id).ToList();

                foreach (var violId in violationIds)
                {
                    violationDomain.Delete(violId);
                }

                entity.Stage = serviceStage.Load(entity.Stage.Id);

                var description = descriptionDomain.GetAll().FirstOrDefault(x => x.AdministrativeCase.Id == entity.Id);
                if (description != null)
                {
                    descriptionDomain.Delete(description.Id);
                }

                return this.Success();
            }
            finally 
            {
                Container.Release(violationDomain);
                Container.Release(serviceStage);
                Container.Release(childrenDomain);
                Container.Release(annexDomain);
                Container.Release(articleLawDomain);
                Container.Release(requirementDomain);
                Container.Release(providedDocDomain);
                Container.Release(descriptionDomain);
                Container.Release(docDomain);
            }
        }

        public override IDataResult AfterDeleteAction(IDomainService<AdministrativeCase> service, AdministrativeCase entity)
        {
            // поулчаем все этапы у которых нет ни одног одокумента
            var domainServiceStage = Container.Resolve<IDomainService<InspectionGjiStage>>();
            var domainServiceDocument = Container.Resolve<IDomainService<DocumentGji>>();
            try
            {
                var stage = entity.Stage;

                if (!domainServiceDocument.GetAll().Any(x => x.Stage.Id == stage.Id))
                {
                    // Если в этапе нет документов то удаляем этап
                    domainServiceStage.Delete(stage.Id);
                }

                return this.Success();
            }
            finally 
            {
                Container.Release(domainServiceStage);
                Container.Release(domainServiceDocument);
            }
        }

        public override IDataResult AfterCreateAction(IDomainService<AdministrativeCase> service, AdministrativeCase entity)
        {
            var primaryBaseStatementAppealCitsDomainService = Container.Resolve<IDomainService<PrimaryBaseStatementAppealCits>>();

            try
            {
                var primaryAppealCitsData = primaryBaseStatementAppealCitsDomainService.GetAll()
                .Where(x => x.BaseStatementAppealCits.Inspection.Id == entity.Inspection.Id)
                .Select(x => new
                {
                    x.BaseStatementAppealCits.Inspection.Contragent,
                    x.BaseStatementAppealCits.AppealCits.Executant,
                    x.ObjectCreateDate
                })
                .OrderByDescending(x => x.ObjectCreateDate)
                .FirstOrDefault();

                if (primaryAppealCitsData != null)
                {
                    if (primaryAppealCitsData.Executant != null)
                    {
                        entity.Inspector = primaryAppealCitsData.Executant;
                    }

                    if (primaryAppealCitsData.Contragent != null)
                    {
                        entity.Contragent = primaryAppealCitsData.Contragent;
                    }
                }

                return Success();
            }
            finally 
            {
                Container.Release(primaryBaseStatementAppealCitsDomainService);
            }
        }
    }
}