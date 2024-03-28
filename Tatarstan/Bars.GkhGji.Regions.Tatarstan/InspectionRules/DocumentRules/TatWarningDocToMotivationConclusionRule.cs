namespace Bars.GkhGji.Regions.Tatarstan.InspectionRules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.InspectionRules;

    using Castle.Windsor;

    /// <summary>
    /// Правило создание документа 'Мотивировочное заключение' из документа 'Предостережение'
    /// </summary>
    public class TatWarningDocToMotivationConclusionRule : IDocumentGjiRule
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<InspectionGjiStage> InspectionStageDomain { get; set; }
        
        public IDomainService<DocumentGjiInspector> DocumentInspectorDomain { get; set; }

        public IDomainService<InspectionGjiInspector> InspectionInspectorDomain { get; set; }

        public IDomainService<MotivationConclusion> DocumentDomain { get; set; }

        public IDomainService<DocumentGjiChildren> ChildrenDomain { get; set; }

        public string CodeRegion => "Tat";

        public string Id => "TatWarningDocToMotivationConclusionRule";

        public string Description => "Правило создание документа 'Мотивировочное заключение' из документа 'Предостережение'";

        public string ActionUrl => "B4.controller.MotivationConclusion";

        public string ResultName => "Мотивировочное заключение";

        public TypeDocumentGji TypeDocumentInitiator => TypeDocumentGji.WarningDoc;

        public TypeDocumentGji TypeDocumentResult => TypeDocumentGji.MotivationConclusion;

        public void SetParams(BaseParams baseParams)
        {
        }

        public virtual IDataResult CreateDocument(DocumentGji document)
        {
            var warningDoc = document as WarningDoc;

            if (warningDoc == null)
            {
                return BaseDataResult.Error($"Не найдено предостережение с идентификатором '{0}'");
            }

            var documentGji = new MotivationConclusion
            {
                Inspection = warningDoc.Inspection,
                DocumentNum = warningDoc.DocumentNum,
                DocumentDate = DateTime.Today,
                BaseDocument = warningDoc,
                Autor = warningDoc.Autor,
                Executant = warningDoc.Executant
            };

            var parentStage = warningDoc.Stage;
            if (parentStage?.Parent != null)
            {
                parentStage = parentStage.Parent;
            }

            InspectionGjiStage newStage = null;

            var currentStage = this.InspectionStageDomain.GetAll()
                .FirstOrDefault(x => x.Parent == parentStage && x.TypeStage == TypeStage.MotivationConclusion);

            if (currentStage == null)
            {
                // Если этап ненайден, то создаем новый этап
                currentStage = new InspectionGjiStage
                {
                    Inspection = warningDoc.Inspection,
                    TypeStage = TypeStage.MotivationConclusion,
                    Parent = parentStage,
                    Position = 1
                };
                var stageMaxPosition = this.InspectionStageDomain.GetAll()
                    .Where(x => x.Inspection.Id == warningDoc.Inspection.Id)
                    .OrderByDescending(x => x.Position)
                    .FirstOrDefault();

                if (stageMaxPosition != null)
                {
                    currentStage.Position = stageMaxPosition.Position + 1;
                }

                // Фиксируем новый этап чтобы потом незабыть сохранить 
                newStage = currentStage;
            }

            documentGji.Stage = currentStage;

            var listInspectors = new List<DocumentGjiInspector>();
            var inspectorIds = this.InspectionInspectorDomain.GetAll()
                .Where(x => x.Inspection.Id == warningDoc.Inspection.Id)
                .Select(x => x.Inspector.Id)
                .Distinct()
                .ToList();

            foreach (var inspector in inspectorIds)
            {
                var newInspector = new DocumentGjiInspector
                {
                    DocumentGji = documentGji,
                    Inspector = new Inspector { Id = inspector }
                };

                listInspectors.Add(newInspector);
            }

            var parentChildren = new DocumentGjiChildren
            {
                Parent = warningDoc,
                Children = documentGji
            };

            this.Container.InTransaction(() =>
            {
                if (newStage != null)
                {
                    this.InspectionStageDomain.Save(newStage);
                }

                this.DocumentDomain.Save(documentGji);

                this.ChildrenDomain.Save(parentChildren);

                listInspectors.ForEach(this.DocumentInspectorDomain.Save);
            });

            return new BaseDataResult(new { documentId = documentGji.Id, inspectionId = documentGji.Inspection.Id });
        }

        public virtual IDataResult ValidationRule(DocumentGji document)
        {
            // по одному предостережению формируем только 1 мотивировочное заключение
            var result = this.DocumentDomain
                .GetAll()
                .Any(x => x.BaseDocument.Id == document.Id);

            if (document.Inspection.TypeBase != TypeBase.GjiWarning || result)
            {
                return new BaseDataResult(false, "Для предостережения уже создано мотивировочное заключение");
            }

            return new BaseDataResult();
        }
    }
}
