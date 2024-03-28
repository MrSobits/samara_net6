namespace Bars.GkhGji.InspectionRules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    using Castle.Windsor;

    /// <summary>
    /// Правило создание документа 'Предостережение' из документа 'Акт проверки'
    /// </summary>
    public class TatActCheckToWarningDocRule : IDocumentGjiRule
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<InspectionGjiStage> InspectionStageDomain { get; set; }
        
        public IDomainService<DocumentGjiInspector> DocumentInspectorDomain { get; set; }

        public IDomainService<InspectionGjiInspector> InspectionInspectorDomain { get; set; }

        public IDomainService<WarningDoc> WarningDocDomain { get; set; }

        public IDomainService<DocumentGjiChildren> ChildrenDomain { get; set; }

        public string CodeRegion
        {
            get { return "Tat"; }
        }

        public string Id
        {
            get { return "TatActCheckToWarningDocRule"; }
        }

        public string Description
        {
            get { return "Правило создание документа 'Предостережение' из документа 'Акт проверки' (по выбранным нарушениям)"; }
        }

        public string ActionUrl
        {
            get { return "B4.controller.WarningDoc"; }
        }

        public string ResultName
        {
            get { return "Предостережение"; }
        }

        public TypeDocumentGji TypeDocumentInitiator
        {
            get { return TypeDocumentGji.ActCheck; }
        }

        public TypeDocumentGji TypeDocumentResult
        {
            get { return TypeDocumentGji.WarningDoc; }
        }
        
        public void SetParams(BaseParams baseParams)
        {
        }

        public virtual IDataResult CreateDocument(DocumentGji document)
        {
            var documentGji = new WarningDoc
            {
                Inspection = document.Inspection,
                TypeDocumentGji = TypeDocumentGji.WarningDoc,
                DocumentNum = document.Inspection.InspectionNum,
                DocumentDate = DateTime.Today
            };

            var parentStage = document.Stage;
            if (parentStage != null && parentStage.Parent != null)
            {
                parentStage = parentStage.Parent;
            }

            InspectionGjiStage newStage = null;

            var currentStage = InspectionStageDomain.GetAll().FirstOrDefault(x => x.Parent == parentStage && x.TypeStage == TypeStage.WarningDoc);

            if (currentStage == null)
            {
                // Если этап ненайден, то создаем новый этап
                currentStage = new InspectionGjiStage
                {
                    Inspection = document.Inspection,
                    TypeStage = TypeStage.WarningDoc,
                    Parent = parentStage,
                    Position = 1
                };
                var stageMaxPosition = InspectionStageDomain.GetAll().Where(x => x.Inspection.Id == document.Inspection.Id)
                    .OrderByDescending(x => x.Position).FirstOrDefault();

                if (stageMaxPosition != null)
                {
                    currentStage.Position = stageMaxPosition.Position + 1;
                }

                // Фиксируем новый этап чтобы потом незабыть сохранить 
                newStage = currentStage;
            }

            documentGji.Stage = currentStage;

            var listInspectors = new List<DocumentGjiInspector>();
            var inspectorIds = this.InspectionInspectorDomain.GetAll().Where(x => x.Inspection.Id == document.Inspection.Id)
                .Select(x => x.Inspector.Id).Distinct().ToList();

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
                Parent = document,
                Children = documentGji
            };

            using (var tr = this.Container.Resolve<IDataTransaction>())
            {
                try
                {
                    if (newStage != null)
                    {
                        this.InspectionStageDomain.Save(newStage);
                    }

                    this.WarningDocDomain.Save(documentGji);

                    this.ChildrenDomain.Save(parentChildren);

                    listInspectors.ForEach(x => this.DocumentInspectorDomain.Save(x));

                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                    throw;
                }
            }

            return new BaseDataResult(new { documentId = documentGji.Id, inspectionId = documentGji.Inspection.Id });
        }

        public virtual IDataResult ValidationRule(DocumentGji document)
        {
            var result = this.InspectionStageDomain.GetAll()
                .Where(x => x.Inspection.Id == document.Inspection.Id)
                .Any(x => x.TypeStage == TypeStage.WarningDoc);

            if (document.Inspection.TypeBase != TypeBase.GjiWarning || result)
            {
                return new BaseDataResult(false, "");
            }

            return new BaseDataResult();
        }
    }
}
