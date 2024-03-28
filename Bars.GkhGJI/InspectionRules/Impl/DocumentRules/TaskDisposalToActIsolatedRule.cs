namespace Bars.GkhGji.InspectionRules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    using Castle.Windsor;

    /// <summary>
    /// Правило создание документа 'Акт без взаимодействия' из документа 'Задача'
    /// </summary>
    public class TaskDisposalToActIsolatedRule : IDocumentGjiRule
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<Disposal> DisposalDomain { get; set; }

        public IDomainService<ActIsolated> ActIsolatedDomain { get; set; }

        public IDomainService<InspectionGjiStage> InspectionStageDomain { get; set; }

        public IDomainService<InspectionGjiInspector> InspectionInspectorDomain { get; set; }

        public IDomainService<InspectionGjiRealityObject> InspectionRoDomain { get; set; }

        public IDomainService<DocumentGjiInspector> DocumentInspectorDomain { get; set; }

        public IDomainService<ActIsolatedRealObj> ActIsolatedRoDomain { get; set; }

        public IDomainService<DocumentGjiChildren> ChildrenDomain { get; set; }

        public IDomainService<RealityObject> RoDomain { get; set; }
        
        private long[] RealityObjectIds { get; set; }

        public virtual string CodeRegion => "Tat";

        public virtual string Id => "TaskDisposalToActIsolatedRule";

        public virtual string Description => "Правило создание документа 'Акт без взаимодействия' из документа 'Задание'";

        public virtual string ActionUrl => "B4.controller.ActIsolated";

        public virtual string ResultName => "Акт без взаимодействия";

        public virtual TypeDocumentGji TypeDocumentInitiator => TypeDocumentGji.TaskDisposal;

        public virtual TypeDocumentGji TypeDocumentResult => TypeDocumentGji.ActIsolated;

        public virtual void SetParams(BaseParams baseParams)
        {
            // В данном методе принимаем параметр Id выбранных пользователем домов
            var realityIds = baseParams.Params.GetAs("realityIds", "");

            this.RealityObjectIds = !string.IsNullOrEmpty(realityIds)
                ? realityIds.Split(',').Select(id => id.ToLong()).ToArray()
                : new long[0];

            if (this.RealityObjectIds.Length == 0)
            {
                throw new Exception("Необходимо выбрать дома");
            }
        }

        public virtual IDataResult CreateDocument(DocumentGji document)
        {
            var actIsolated = new ActIsolated
            {
                Inspection = document.Inspection,
                TypeDocumentGji = TypeDocumentGji.ActIsolated,
                DocumentNum = document.Inspection.InspectionNum
            };

            #region Формируем этап проверки
            var parentStage = document.Stage;
            if (parentStage != null && parentStage.Parent != null)
            {
                parentStage = parentStage.Parent;
            }

            InspectionGjiStage newStage = null;
            var currentStage = this.InspectionStageDomain.GetAll()
                .FirstOrDefault(x => x.Parent.Id == parentStage.Id && x.TypeStage == TypeStage.ActIsolated);

            if (currentStage == null)
            {
                // Если этап не найден, то создаем новый этап
                currentStage = new InspectionGjiStage
                {
                    Inspection = document.Inspection,
                    TypeStage = TypeStage.ActIsolated,
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

            actIsolated.Stage = currentStage;

            #endregion

            #region Формируем связь с родителем
            var parentChildren = new DocumentGjiChildren
            {
                Parent = document,
                Children = actIsolated
            };
            #endregion

            #region Формируем Инспекторов
            var listInspectors = new List<DocumentGjiInspector>();
            var inspectorIds = this.DocumentInspectorDomain.GetAll()
                .Where(x => x.DocumentGji.Id == document.Id)
                .Select(x => x.Inspector.Id)
                .Distinct()
                .ToList();

            foreach (var id in inspectorIds)
            {
                listInspectors.Add(new DocumentGjiInspector
                {
                    DocumentGji = actIsolated,
                    Inspector = new Inspector { Id = id }
                });
            }
            #endregion

            #region Формируем дома акта

            // Поскольку пользователи сами выбирают дома то их и переносим
            var listRo = new List<ActIsolatedRealObj>();
            
            foreach (var id in this.RealityObjectIds)
            {
                var actRo = new ActIsolatedRealObj
                {
                    ActIsolated = actIsolated,
                    RealityObject = new RealityObject { Id = id },
                    HaveViolation = YesNoNotSet.NotSet
                };

                listRo.Add(actRo);
            }

            actIsolated.Area = this.RoDomain.GetAll().Where(x => this.RealityObjectIds.Contains(x.Id)).Sum(x => x.AreaMkd);
            #endregion
            
            using (var tr = this.Container.Resolve<IDataTransaction>())
            {
                try
                {
                    if (newStage != null)
                    {
                        this.InspectionStageDomain.Save(newStage);
                    }

                    this.ActIsolatedDomain.Save(actIsolated);

                    this.ChildrenDomain.Save(parentChildren);

                    listInspectors.ForEach(x => this.DocumentInspectorDomain.Save(x));

                    listRo.ForEach(x => this.ActIsolatedRoDomain.Save(x));

                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                    throw;
                }
            }

            return new BaseDataResult(new { documentId = actIsolated.Id, inspectionId = actIsolated.Inspection.Id });
        }

        public virtual IDataResult ValidationRule(DocumentGji document)
        {
            if (document != null)
            {
                var disposal = this.DisposalDomain.FirstOrDefault(x => x.Id == document.Id);

                if (disposal == null)
                {
                    return new BaseDataResult(false, $"Не удалось получить задание {document.Id}");
                }
            }

            return new BaseDataResult();
        }
    }
}
