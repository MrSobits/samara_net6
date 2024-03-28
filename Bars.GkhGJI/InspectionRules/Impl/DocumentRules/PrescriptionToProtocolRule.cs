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
    /// Правило создание документа 'Протокола' из документа 'Предписание'
    /// </summary>
    public class PrescriptionToProtocolRule : IDocumentGjiRule
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<Prescription> PrescriptionDomain { get; set; }

        public IDomainService<Protocol> ProtocolDomain { get; set; }

        public IDomainService<InspectionGjiStage> InspectionStageDomain { get; set; }
        
        public IDomainService<DocumentGjiInspector> DocumentInspectorDomain { get; set; }
        public IDocumentGjiInspectorService DocumentInspectorDomainService { get; set; }

        public IDomainService<ActSurveyRealityObject> ActSurveyRoDomain { get; set; }

        public IDomainService<DocumentGjiChildren> ChildrenDomain { get; set; }

        public IDomainService<InspectionGjiViol> InspectionViolDomain { get; set; }

        public IDomainService<ProtocolViolation> ProtocolViolationDomain { get; set; }

        #region Внутренние переменные
        private long[] ViolationIds { get; set; }
        #endregion

        public string CodeRegion
        {
            get { return "Tat"; }
        }

        public string Id
        {
            get { return "PrescriptionToProtocolRule"; }
        }

        public string Description
        {
            get { return "Правило создание документа 'Протокола' из документа 'Предписание' (по выбранным нарушениям)"; }
        }

        public string ActionUrl
        {
            get { return "B4.controller.ProtocolGji"; }
        }

        public string ResultName
        {
            get { return "Протокол"; }
        }

        public TypeDocumentGji TypeDocumentInitiator
        {
            get { return TypeDocumentGji.Prescription; }
        }

        public TypeDocumentGji TypeDocumentResult
        {
            get { return TypeDocumentGji.Protocol; }
        }

        // тут надо принять параметры если таковые имеютя
        public void SetParams(BaseParams baseParams)
        {
            // В данном методе принимаем параметр violationIds выбранных пользователем нарушений
            var violationIds = baseParams.Params.GetAs("violationIds", "");

            ViolationIds = !string.IsNullOrEmpty(violationIds)
                                  ? violationIds.Split(',').Select(id => id.ToLong()).ToArray()
                                  : new long[0];

            if (ViolationIds.Length == 0)
            {
                throw new Exception("Необходимо выбрать нарушения");
            }
        }

        public IDataResult CreateDocument(DocumentGji document)
        {
            #region Формируем протокол
            var prescription = PrescriptionDomain.GetAll()
                                    .Where(x => x.Id == document.Id)
                                    .Select(x => new
                                    {
                                        x.Executant, 
                                        x.Contragent, 
                                        x.PhysicalPerson, 
                                        x.PhysicalPersonInfo,
                                        x.DocumentYear,
                                        x.DocumentNum,
                                        x.DocumentSubNum,
                                        x.LiteralNum
                                    })
                                    .FirstOrDefault();

            if (prescription == null)
            {
                throw new Exception("Не удалось получить предписание");
            }

            var protocol = new Protocol
            {
                Inspection = document.Inspection,
                TypeDocumentGji = TypeDocumentGji.Protocol,
                Contragent = prescription.Contragent,
                Executant = prescription.Executant,
                PhysicalPerson = prescription.PhysicalPerson,
                PhysicalPersonInfo = prescription.PhysicalPersonInfo,
                DocumentYear = prescription.DocumentYear,
                DocumentNum = prescription.DocumentNum,
                DocumentSubNum = prescription.DocumentSubNum,
                LiteralNum = prescription.LiteralNum
                
            };
            #endregion

            #region Формируем этап протокола
            // Если у родительского документа есть этап у которого есть родитель
            // тогда берем именно родительский Этап (Просто для красоты в дереве, чтобы не плодить дочерние узлы)
            var parentStage = document.Stage;
            if (parentStage != null && parentStage.Parent != null)
            {
                parentStage = parentStage.Parent;
            }

            InspectionGjiStage newStage = null;

            var currentStage = InspectionStageDomain.GetAll().FirstOrDefault(x => x.Parent == parentStage && x.TypeStage == TypeStage.Protocol);

            if (currentStage == null)
            {
                // Если этап ненайден, то создаем новый этап
                currentStage = new InspectionGjiStage
                {
                    Inspection = document.Inspection,
                    TypeStage = TypeStage.Protocol,
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

            protocol.Stage = currentStage;
            #endregion

            #region Формируем связь с родителем
            var parentChildren = new DocumentGjiChildren
            {
                Parent = document,
                Children = protocol
            };
            #endregion

            #region Формируем Инспекторов тянем их из родительского документа
            var listInspectors = new List<DocumentGjiInspector>();
            var inspectorIds = DocumentInspectorDomainService.GetInspectorsByDocumentId(document.Id)
                    .Select(x => x.Inspector.Id).ToList();

            for (var i = 0; i < inspectorIds.Count; i++)
            {
                listInspectors.Add(new DocumentGjiInspector
                {
                    DocumentGji = protocol,
                    Inspector = new Inspector { Id = inspectorIds[i] },
                    Order = i
                });
            }
            #endregion

            #region Формируем нарушения (по выбранным пользователями)
            var listProtocolViol = new List<ProtocolViolation>();
            var violationList = InspectionViolDomain.GetAll().Where(x => ViolationIds.Contains(x.Id));

            foreach (var viol in violationList)
            {
                var newRecord = new ProtocolViolation
                {
                    Document = protocol,
                    InspectionViolation = viol,
                    DatePlanRemoval = viol.DatePlanRemoval,
                    DateFactRemoval = viol.DateFactRemoval,
                    TypeViolationStage = TypeViolationStage.Sentence
                };

                listProtocolViol.Add(newRecord);
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

                    this.ProtocolDomain.Save(protocol);

                    this.ChildrenDomain.Save(parentChildren);

                    listInspectors.ForEach(x => this.DocumentInspectorDomain.Save(x));

                    listProtocolViol.ForEach(x => this.ProtocolViolationDomain.Save(x));

                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                    throw;
                }
            }
            #endregion

            return new BaseDataResult(new { documentId = protocol.Id, inspectionId = document.Inspection.Id });
        }

        public IDataResult ValidationRule(DocumentGji document)
        {
            return new BaseDataResult();
        }
    }
}
