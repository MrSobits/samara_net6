namespace Bars.GkhGji.Regions.Tatarstan.InspectionRules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Analytics.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.InspectionRules;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;
    using Bars.GkhGji.Regions.Tatarstan.Enums;

    using Castle.Windsor;

    public class ActActionToMotivatedPresentationRule : IDocumentGjiRule
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<InspectionGjiStage> InspectionStageDomain { get; set; }

        public IDomainService<ActCheckRealityObject> ActCheckRoDomain { get; set; }

        public IDomainService<TaskActionIsolated> TaskDomain { get; set; }

        public IDomainService<MotivatedPresentation> MotivatedPresentationDomain { get; set; }

        public IDomainService<DocumentGjiChildren> ChildrenDomain { get; set; }

        public IDomainService<MotivatedPresentationRealityObject> DocumentRealityObjDomain { get; set; }

        public IDomainService<MotivatedPresentationViolation> DocumentViolDomain { get; set; }

        public IDomainService<ActCheckViolation> ActCheckViolDomain { get; set; }

        public IDomainService<ActCheck> ActCheckDomain { get; set; }
        
        public IDomainService<DocumentGjiInspector> DocumentInspectorDomain { get; set; }

        /// <inheritdoc />
        public string CodeRegion => "Tat";

        /// <inheritdoc />
        public string Id => nameof(ActActionToMotivatedPresentationRule);

        /// <inheritdoc />
        public string Description => "Правило создания документа 'Мотивированное представление' из документа 'Акт по КНМ без взаимодействия с контролируемыми лицами'";

        /// <inheritdoc />
        public string ResultName => "Мотивированное представление";

        /// <inheritdoc />
        public string ActionUrl => "B4.controller.actionisolated.motivatedpresentation.MotivatedPresentation";

        /// <inheritdoc />
        public TypeDocumentGji TypeDocumentInitiator => TypeDocumentGji.ActActionIsolated;

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
            var act = this.ActCheckDomain.Get(document.Id);
            
            var motivatedPresentation = new MotivatedPresentation
            {
                Inspection = document.Inspection,
                TypeDocumentGji = TypeDocumentGji.MotivatedPresentation,
                Stage = currentStage,
                DocumentDate = document.DocumentDate,
                DocumentYear = document.DocumentYear,
                DocumentNum = document.DocumentNum,
                CreationPlace = act.DocumentPlaceFias
            };
            #endregion

            #region Формируем инспекторов (Берем из Акта)

            var inspectorList = this.DocumentInspectorDomain.GetAll()
                .Where(x => x.DocumentGji.Id == document.Id)
                .Select(x => new DocumentGjiInspector
                {
                    DocumentGji = motivatedPresentation,
                    Inspector = new Inspector
                    {
                        Id = x.Inspector.Id
                    }
                })
                .ToList();
            
            #endregion
            
            #region Формируем связь с родителем
            var parentChildren = new DocumentGjiChildren
            {
                Parent = document,
                Children = motivatedPresentation
            };
            #endregion

            #region Формируем нарушения
            var listRealityObjects = new List<MotivatedPresentationRealityObject>();
            var listViolations = new List<MotivatedPresentationViolation>();

            var violationList = this.ActCheckViolDomain.GetAll()
                .Where(x => x.Document.Id == document.Id &&
                    x.InspectionViolation.Inspection.Id == document.Inspection.Id &&
                    this.RealityIds.Contains(x.ActObject.RealityObject.Id))
                .AsEnumerable()
                .GroupBy(x => x.ActObject.RealityObject)
                .ToDictionary(x => x.Key, y => y.Select(z => z.InspectionViolation.Violation));
            
            foreach (var group in violationList)
            {
                var realityObj = new MotivatedPresentationRealityObject
                {
                    MotivatedPresentation = motivatedPresentation,
                    RealityObject = group.Key
                };

                listRealityObjects.Add(realityObj);
                listViolations.AddRange(group.Value.Select(x => new MotivatedPresentationViolation
                {
                    MotivatedPresentationRealityObject = realityObj,
                    Violation = x
                }));
            }
            #endregion

            #region Сохранение
            using (var tr = this.Container.Resolve<IDataTransaction>())
            {
                if (newStage != null)
                {
                    this.InspectionStageDomain.Save(newStage);
                }

                this.MotivatedPresentationDomain.Save(motivatedPresentation);
                this.ChildrenDomain.Save(parentChildren);
                inspectorList.ForEach(x => this.DocumentInspectorDomain.Save(x));
                listRealityObjects.ForEach(x => this.DocumentRealityObjDomain.Save(x));
                listViolations.ForEach(x => this.DocumentViolDomain.Save(x));

                tr.Commit();
            }
            #endregion

            return new BaseDataResult(new { documentId = motivatedPresentation.Id, inspectionId = motivatedPresentation.Inspection.Id });
        }

        /// <inheritdoc />
        public IDataResult ValidationRule(DocumentGji document)
        {
            if (this.ChildrenDomain.GetAll()
                .Any(
                    x =>
                        x.Parent.Id == document.Id
                        && x.Children.TypeDocumentGji == TypeDocumentGji.MotivatedPresentation))
            {
                return new BaseDataResult(false, "У акта может быть только 1 мотивированное представление");
            }

            if (!this.ActCheckRoDomain.GetAll().Any(x => x.ActCheck.Id == document.Id && x.HaveViolation == YesNoNotSet.Yes))
            {
                return new BaseDataResult(false, "Нет нарушений");
            }

            var taskId = this.ChildrenDomain.GetAll()
                .Where(x => x.Children.Id == document.Id && x.Parent.TypeDocumentGji == TypeDocumentGji.TaskActionIsolated)
                .Select(x => x.Parent.Id)
                .SingleOrDefault();

            var taskKind = this.TaskDomain.Get(taskId)?.KindAction;

            if (taskKind == KindAction.Survey)
            {
                var actHasProtocol = this.ChildrenDomain.GetAll()
                    .Any(x => x.Parent.Id == document.Id 
                        && x.Children.TypeDocumentGji == TypeDocumentGji.Protocol);

                if (actHasProtocol)
                {
                    return new BaseDataResult(false, "У акта имеется сформированный дочерний документ Протокол");
                }
            }

            return new BaseDataResult();
        }

        private long[] RealityIds { get; set; }
    }
}
