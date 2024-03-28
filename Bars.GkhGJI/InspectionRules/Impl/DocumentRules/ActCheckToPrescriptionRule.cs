using Bars.GkhGji.DomainService;

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
    /// Правило создание документа 'Предписания' из документа 'Акт проверки'
    /// </summary>
    public class ActCheckToPrescriptionRule : IDocumentGjiRule
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<Prescription> PrescriptionDomain { get; set; }

        public IDomainService<InspectionGjiStage> InspectionStageDomain { get; set; }
        
        public IDomainService<DocumentGjiInspector> DocumentInspectorDomain { get; set; }

        public IDocumentGjiInspectorService DocumentInspectorService { get; set; }

        public IDomainService<ActSurveyRealityObject> ActSurveyRoDomain { get; set; }

        public IDomainService<DocumentGjiChildren> ChildrenDomain { get; set; }

        public IDomainService<InspectionGjiViol> InspectionViolDomain { get; set; }

        public IDomainService<ViolationActionsRemovGji> ViolationActionDomain { get; set; }

        public IDomainService<PrescriptionViol> PrescriptionViolDomain { get; set; }

        #region Внутренние переменные
        protected long[] ViolationIds { get; set; }
        #endregion

        public string CodeRegion
        {
            get { return "Tat"; }
        }

        public string Id
        {
            get { return "ActCheckToPrescriptionRule"; }
        }

        public string Description
        {
            get { return "Правило создание документа 'Предписания' из документа 'Акт проверки' (по выбранным нарушениям)"; }
        }

        public string ActionUrl
        {
            get { return "B4.controller.Prescription"; }
        }

        public string ResultName
        {
            get { return "Предписание"; }
        }

        public TypeDocumentGji TypeDocumentInitiator
        {
            get { return TypeDocumentGji.ActCheck; }
        }

        public TypeDocumentGji TypeDocumentResult
        {
            get { return TypeDocumentGji.Prescription; }
        }

        // тут надо принять параметры если таковые имеютя
        public void SetParams(BaseParams baseParams)
        {
            // В данном методе принимаем параметр violationIds выбранных дома
            var violationIds = baseParams.Params.GetAs("violationIds", "");

            ViolationIds = !string.IsNullOrEmpty(violationIds)
                                  ? violationIds.Split(',').Select(id => id.ToLong()).ToArray()
                                  : new long[0];

            if (ViolationIds.Length == 0)
            {
                throw new Exception("Необходимо выбрать нарушения");
            }
        }

        public virtual IDataResult CreateDocument(DocumentGji document)
        {
            #region Формируем предписание
            var prescription = new Prescription()
            {
                Inspection = document.Inspection,
                TypeDocumentGji = TypeDocumentGji.Prescription,
                Contragent = document.Inspection.Contragent,
                IsFamiliar = PrescriptionFamiliar.No
            };
            #endregion

            #region Формируем этап проверки
            // Если у родительского документа есть этап у которого есть родитель
            // тогда берем именно родительский Этап (Просто для красоты в дереве, чтобы не плодить дочерние узлы)
            var parentStage = document.Stage;
            if (parentStage != null && parentStage.Parent != null)
            {
                parentStage = parentStage.Parent;
            }

            InspectionGjiStage newStage = null;

            var currentStage = InspectionStageDomain.GetAll().FirstOrDefault(x => x.Parent == parentStage && x.TypeStage == TypeStage.Prescription);

            if (currentStage == null)
            {
                // Если этап ненайден, то создаем новый этап
                currentStage = new InspectionGjiStage
                {
                    Inspection = document.Inspection,
                    TypeStage = TypeStage.Prescription,
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

            prescription.Stage = currentStage;
            #endregion

            #region формируем связь с родителем
            var parentChildren = new DocumentGjiChildren
            {
                Parent = document,
                Children = prescription
            };
            #endregion

            #region Формируем Инспекторов тянем их из родительского документа
            var listInspectors = new List<DocumentGjiInspector>();
            var inspectorIds = DocumentInspectorService.GetInspectorsByDocumentId(document.Id)
                    .Select(x => x.Inspector.Id).ToList();

            for(var i = 0; i < inspectorIds.Count; i++)
            {
                listInspectors.Add(new DocumentGjiInspector
                {
                    DocumentGji = prescription,
                    Inspector = new Inspector { Id = inspectorIds[i] },
                    Order = i
                });
            }
            #endregion

            #region Формируем нарушения (по выбранным пользователями)
            var listPrescriptionViol = new List<PrescriptionViol>();
            var violationList = InspectionViolDomain.GetAll().Where(x => ViolationIds.Contains(x.Id));
            
            foreach (var viol in violationList)
            {
                var newRecord = new PrescriptionViol
                {
                    Document = prescription,
                    InspectionViolation = viol,
                    DatePlanRemoval = viol.DatePlanRemoval,
                    DateFactRemoval = viol.DateFactRemoval,
                    TypeViolationStage = TypeViolationStage.InstructionToRemove
                };

                // Пробуем по Справочнику нарушений получить Мероприятие по устранению
                newRecord.Action = ViolationActionDomain.GetAll()
                        .Where(x => x.ViolationGji.Id == viol.Violation.Id)
                        .Select(x => x.ActionsRemovViol.Name)
                        .FirstOrDefault();

                listPrescriptionViol.Add(newRecord);
            }
            #endregion

            #region Сохранение
            using (var tr = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    if (newStage != null)
                    {
                        this.InspectionStageDomain.Save(newStage);
                    }

                    this.PrescriptionDomain.Save(prescription);

                    this.ChildrenDomain.Save(parentChildren);

                    listInspectors.ForEach(x => this.DocumentInspectorDomain.Save(x));

                    listPrescriptionViol.ForEach(x => this.PrescriptionViolDomain.Save(x));

                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                    throw;
                }
            }
            #endregion

            return new BaseDataResult(new { documentId = prescription.Id, inspectionId = document.Inspection.Id });
        }

        public virtual IDataResult ValidationRule(DocumentGji document)
        {
            if (document.Inspection.TypeBase == TypeBase.GjiWarning)
            {
                return new BaseDataResult(false, "");
            }

            return new BaseDataResult();
        }
    }
}
