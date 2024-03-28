namespace Bars.GkhGji.Regions.Tomsk.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tomsk.Entities.Inspection;

    using Entities;

    public class ActVisualnterceptor : EmptyDomainInterceptor<ActVisual>
    {
        public IDomainService<DocumentGjiChildren> ChildrenDomain { get; set; }

        public IDomainService<DocumentGjiInspector> DocumentGjiInspectorDomainService { get; set; }

        public IDomainService<PrimaryBaseStatementAppealCits> PrimaryBaseStatementAppealCitsDomainService { get; set; }

        public override IDataResult BeforeCreateAction(IDomainService<ActVisual> service, ActVisual entity)
        {
            if (!entity.DocumentDate.HasValue)
            {
                entity.DocumentDate = DateTime.Now.Date;
            }

            // Перед сохранением меняем присваиваем статус Черновик к документу
            var stateProvider = Container.Resolve<IStateProvider>();
            stateProvider.SetDefaultState(entity);

            return Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<ActVisual> service, ActVisual entity)
        {
            if (ChildrenDomain.GetAll().Any(x => x.Parent.Id == entity.Id))
            {
                return Failure("Акт визуального осмотра имеет дочерние документы.");
            }

            // Удаляем все записи В таблице ParentChildren связанные с этим документом
            // проверяем есть ли дочерние документы у акта проверки
            var childrenIds = ChildrenDomain.GetAll()
                .Where(x => x.Children.Id == entity.Id)
                .Select(x => x.Id)
                .ToList();

            foreach (var childId in childrenIds)
            {
                ChildrenDomain.Delete(childId);
            }

            DocumentGjiInspectorDomainService.GetAll()
                .Where(x => x.DocumentGji.Id == entity.Id)
                .Select(x => x.Id)
                .ForEach(x => DocumentGjiInspectorDomainService.Delete(x));

            return Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<ActVisual> service, ActVisual entity)
        {
            // поулчаем все этапы у которых нет ни одног одокумента
            var domainServiceStage = Container.Resolve<IDomainService<InspectionGjiStage>>();
            var domainServiceDocument = Container.Resolve<IDomainService<DocumentGji>>();
            var stageId = entity.Stage.Id;

            if (!domainServiceDocument.GetAll().Any(x => x.Stage.Id == stageId))
            {
                // Если в этапе нет документов то удаляем этап
                domainServiceStage.Delete(stageId);
            }

            return Success();
        }

        public override IDataResult AfterCreateAction(IDomainService<ActVisual> service, ActVisual entity)
        {
            var primaryAppealCitsExecutant = PrimaryBaseStatementAppealCitsDomainService.GetAll()
                .Where(x => x.BaseStatementAppealCits.Inspection.Id == entity.Inspection.Id)
                .Select(x => x.BaseStatementAppealCits.AppealCits.Executant)
                .OrderByDescending(x => x.ObjectCreateDate)
                .FirstOrDefault();

            if (primaryAppealCitsExecutant != null)
            {
                DocumentGjiInspectorDomainService.Save(new DocumentGjiInspector { DocumentGji = entity, Inspector = primaryAppealCitsExecutant });
            }

            return Success();
        }
    }
}