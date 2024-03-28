namespace Bars.GkhGji.Regions.Tatarstan.InspectionRules.DocumentRules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.InspectionRules;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;

    using Castle.Windsor;

    public class VisitSheetToMotivatedPresentationRule : IDocumentGjiRule
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<VisitSheetViolationInfo> VisitSheetViolationInfoDomain { get; set; }

        public IDomainService<VisitSheetViolation> VisitSheetViolationDomain { get; set; }
        
        public IDomainService<InspectionGjiStage> InspectionStageDomain { get; set; }

        public IDomainService<DocumentGjiChildren> DocumentGjiChildrenDomain { get; set; }

        public IDomainService<PreventiveActionTask> PreventiveActionTaskDomain { get; set; }

        public IDomainService<MotivatedPresentation> MotivatePresentationActionIsolated { get; set; }

        public IDomainService<MotivatedPresentationViolation> MotivatedPresentationViolationDomain { get; set; }

        public IDomainService<MotivatedPresentationRealityObject> MotivatedPresentationRealityObjectDomain { get; set; }

        /// <inheritdoc />
        public string CodeRegion => "Tat";

        /// <inheritdoc />
        public string Id => nameof(VisitSheetToMotivatedPresentationRule);

        /// <inheritdoc />
        public string Description => "Правило создания документа 'Мотивированное представление' из документа 'Лист визита по профилактическому мероприятию'";

        /// <inheritdoc />
        public string ResultName => "Мотивированное представление";

        /// <inheritdoc />
        public string ActionUrl => "B4.controller.actionisolated.motivatedpresentation.MotivatedPresentation";

        /// <inheritdoc />
        public TypeDocumentGji TypeDocumentInitiator => TypeDocumentGji.VisitSheet;

        /// <inheritdoc />
        public TypeDocumentGji TypeDocumentResult => TypeDocumentGji.MotivatedPresentation;

        /// <inheritdoc />
        public void SetParams(BaseParams baseParams)
        {
            var realityIds = baseParams.Params.GetAs("realityIds", "");

            this.RealityIds = realityIds.ToLongArray();

            if (this.RealityIds.Length == 0)
            {
                throw new Exception("Необходимо выбрать дома");
            }
        }

        /// <inheritdoc />
        public IDataResult CreateDocument(DocumentGji document)
        {
            #region Формируем этап проверки
            var parentStage = document.Stage;
            if (parentStage?.Parent != null)
            {
                parentStage = parentStage.Parent;
            }

            InspectionGjiStage newStage = null;
            var currentStage = this.InspectionStageDomain.GetAll()
                .FirstOrDefault(x => x.Parent.Id == parentStage.Id
                    && x.TypeStage == TypeStage.MotivatedPresentation);

            if (currentStage == null)
            {
                // Если этап ненайден, то создаем новый этап
                currentStage = new InspectionGjiStage
                {
                    Inspection = document.Inspection,
                    TypeStage = TypeStage.MotivatedPresentation,
                    Parent = parentStage,
                    Position = 1
                };

                var stageMaxPosition = this.InspectionStageDomain.GetAll()
                    .Where(x => x.Inspection.Id == document.Inspection.Id)
                    .OrderByDescending(x => x.Position)
                    .FirstOrDefault();

                if (stageMaxPosition != null)
                    currentStage.Position = stageMaxPosition.Position + 1;

                newStage = currentStage;
            }
            #endregion

            #region Формируем документ

            var preventiveActionTaskId = this.DocumentGjiChildrenDomain.GetAll()
                .Where(x => x.Children.Id == document.Id 
                    && x.Parent.TypeDocumentGji == TypeDocumentGji.PreventiveActionTask
                    && x.Children.TypeDocumentGji == TypeDocumentGji.VisitSheet)
                .Select(x => x.Parent.Id)
                .FirstOrDefault();

            var preventiveActionTask = this.PreventiveActionTaskDomain.Get(preventiveActionTaskId);
            
            var motivatedPresentation = new MotivatedPresentation
            {
                Inspection = document.Inspection,
                TypeDocumentGji = TypeDocumentGji.MotivatedPresentation,
                Stage = currentStage,
                DocumentYear = document.DocumentYear,
                DocumentNum = document.DocumentNum,
                CreationPlace = preventiveActionTask.ActionLocation,
                IssuedMotivatedPresentation = preventiveActionTask.TaskingInspector,
                ResponsibleExecution = preventiveActionTask.Executor
            };
            #endregion

            #region Формируем связь с родителем
            var parentChildren = new DocumentGjiChildren
            {
                Parent = document,
                Children = motivatedPresentation
            };
            #endregion

            #region Формируем нарушения
            var violationList = new List<MotivatedPresentationViolation>();
            var realityObjectList = new List<MotivatedPresentationRealityObject>(); 
            var visitSheetViolationDict = this.VisitSheetViolationInfoDomain.GetAll()
                .Where(x => x.VisitSheet.Id == document.Id)
                .Where(x => this.RealityIds.Contains(x.RealityObject.Id))
                .Join(this.VisitSheetViolationDomain.GetAll(),
                    x => x.Id,
                    y => y.ViolationInfo.Id,
                    (x, y) => new
                    {
                        x.Id,
                        x.RealityObject,
                        y.Violation
                    })
                .AsEnumerable()
                .GroupBy(x => new
                    {
                        x.Id,
                        x.RealityObject
                    },
                    (x, y) => new
                    {
                        x.RealityObject,
                        Violations = y.Where(z => z.Violation != null)
                            .DistinctBy(z => z.Violation.Id)
                            .Select(z => z.Violation)
                    })
                .ToDictionary(x => x.RealityObject, y => y.Violations);

            foreach (var group in visitSheetViolationDict)
            {
                var realityObject = new MotivatedPresentationRealityObject
                {
                    RealityObject = group.Key,
                    MotivatedPresentation = motivatedPresentation
                };
                
                realityObjectList.Add(realityObject);
                violationList.AddRange(group.Value.Select(x => new MotivatedPresentationViolation
                {
                    MotivatedPresentationRealityObject = realityObject,
                    Violation = x
                }));
            }

            #endregion
            
            #region Сохранение

            this.Container.InTransaction(() =>
            {
                if (newStage != null)
                {
                    this.InspectionStageDomain.Save(newStage);
                }

                this.MotivatePresentationActionIsolated.Save(motivatedPresentation);
                this.DocumentGjiChildrenDomain.Save(parentChildren);
                realityObjectList.ForEach(x => this.MotivatedPresentationRealityObjectDomain.Save(x));
                violationList.ForEach(x => this.MotivatedPresentationViolationDomain.Save(x));
            });
            #endregion

            return new BaseDataResult(new { documentId = motivatedPresentation.Id, inspectionId = motivatedPresentation.Inspection.Id });
        }

        /// <inheritdoc />
        public IDataResult ValidationRule(DocumentGji document)
        {
            if (this.DocumentGjiChildrenDomain.GetAll()
                .Any(
                    x =>
                        x.Parent.Id == document.Id
                        && x.Children.TypeDocumentGji == TypeDocumentGji.MotivatedPresentation))
            {
                return new BaseDataResult(false, "У документа 'Лист визита' может быть только 1 мотивированное представление");
            }
            
            // Получаем нарушения по документу
            var violationIds = this.VisitSheetViolationInfoDomain
                .GetAll()
                .Where(x => x.VisitSheet.Id == document.Id)
                .Select(x => x.Id)
                .ToList();

            // Проверяем есть хоть одна угроза в нарушениях у документа
            var hasViolThreats = this.VisitSheetViolationDomain
                .GetAll()
                .WhereIf(violationIds.Any(), x => violationIds.Contains(x.ViolationInfo.Id))
                .Any(x => x.IsThreatToLegalProtectedValues);

            if (!hasViolThreats)
            {
                return new BaseDataResult(false, "У документа 'Лист визита' отсутствуют нарушения с признаками опасности");
            }

            return new BaseDataResult();
        }
        
        private long[] RealityIds { get; set; }
    }
}