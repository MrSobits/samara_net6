namespace Bars.GkhGji.InspectionRules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    using Castle.Windsor;

    /// <summary>
    /// Правило создание документа 'Предостережение' из документа 'Акт без взаимодействия'
    /// </summary>
    public class ActIsolatedToWarningDocRule : IDocumentGjiRule
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<InspectionGjiStage> InspectionStageDomain { get; set; }
        
        public IDomainService<DocumentGjiInspector> DocumentInspectorDomain { get; set; }

        public IDomainService<InspectionGjiInspector> InspectionInspectorDomain { get; set; }

        public IDomainService<WarningDoc> WarningDocDomain { get; set; }

        public IDomainService<WarningDocRealObj> WarningDocRealObjDomain { get; set; }

        public IDomainService<ActIsolatedRealObj> ActIsolatedRealObjDomain { get; set; }

        public IDomainService<DocumentGjiChildren> ChildrenDomain { get; set; }

        public string CodeRegion => "Tat";

        public string Id => "ActIsolatedToWarningDocRule";

        public string Description => "Правило создание документа 'Предостережение' из документа 'Акт без взаимодействия'";

        public string ActionUrl => "B4.controller.WarningDoc";

        public string ResultName => "Предостережение";

        public TypeDocumentGji TypeDocumentInitiator => TypeDocumentGji.ActIsolated;

        public TypeDocumentGji TypeDocumentResult => TypeDocumentGji.WarningDoc;

        public List<WarningDocRealObj> listRo = new List<WarningDocRealObj>();

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

            var currentStage = this.InspectionStageDomain.GetAll()
                .FirstOrDefault(x => x.Parent == parentStage && x.TypeStage == TypeStage.WarningDoc);

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
                var stageMaxPosition = this.InspectionStageDomain.GetAll().Where(x => x.Inspection.Id == document.Inspection.Id)
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

            if (document.TypeDocumentGji == TypeDocumentGji.ActIsolated)
            {
                // Добавляем дома из акта без взаимодейства по которому создаем предострежение
                this.listRo = this.AddRealityObjects(document, documentGji);
            }

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

                    if (this.listRo.Count > 0)
                    {
                        this.listRo.ForEach(x => this.WarningDocRealObjDomain.Save(x));
                    }

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
            var result = this.ChildrenDomain.GetAll()
                .Any(x => x.Parent.Id == document.Id);

            if (document.Inspection.TypeBase != TypeBase.GjiWarning || result)
            {
                return new BaseDataResult(false, "");
            }

            return new BaseDataResult();
        }

        private List<WarningDocRealObj> AddRealityObjects(DocumentGji document, WarningDoc documentGji)
        {
            var roIds = this.ActIsolatedRealObjDomain.GetAll().Where(x => x.ActIsolated.Id == document.Id).Select(x => x.RealityObject.Id).ToHashSet();

            foreach (var id in roIds)
            {
                var warningDocRo = new WarningDocRealObj
                {
                    WarningDoc = documentGji,
                    RealityObject = new RealityObject { Id = id },
                };

                this.listRo.Add(warningDocRo);
            }

            return this.listRo;
        }
    }
}
