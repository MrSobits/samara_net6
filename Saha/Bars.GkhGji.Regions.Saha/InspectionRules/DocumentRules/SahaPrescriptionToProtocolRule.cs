namespace Bars.GkhGji.Regions.Saha.InspectionRules
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
    using Bars.GkhGji.InspectionRules;
    using Bars.GkhGji.Regions.Saha.Entities;

    using Castle.Windsor;

    /// <summary>
    /// Правило создание документа 'Протокола' из документа 'Предписание'
    /// </summary>
    public class SahaPrescriptionToProtocolRule : IDocumentGjiRule
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<Prescription> PrescriptionDomain { get; set; }

        public IDomainService<Protocol> ProtocolDomain { get; set; }

        public IDomainService<InspectionGjiStage> InspectionStageDomain { get; set; }
        
        public IDomainService<DocumentGjiInspector> DocumentInspectorDomain { get; set; }

        public IDomainService<ActSurveyRealityObject> ActSurveyRoDomain { get; set; }

        public IDomainService<DocumentGjiChildren> ChildrenDomain { get; set; }

        public IDomainService<InspectionGjiViol> InspectionViolDomain { get; set; }

        public IDomainService<ProtocolViolation> ProtocolViolationDomain { get; set; }

        public IDomainService<DocumentViolGroup> ViolGroupDomain { get; set; }

        public IDomainService<DocumentViolGroupLongText> ViolGroupLongTextDomain { get; set; }

        public IDomainService<DocumentViolGroupPoint> ViolGroupPointDomain { get; set; }

        #region Внутренние переменные
        private long[] ViolationGroupIds { get; set; }
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
            // В данном методе принимаем параметр violationGroupIds выбранных дома
            var ids = baseParams.Params.GetAs("violationGroupIds", "");

            ViolationGroupIds = !string.IsNullOrEmpty(ids)
                                  ? ids.Split(',').Select(id => id.ToLong()).ToArray()
                                  : new long[0];

            if (ViolationGroupIds.Length == 0)
            {
                throw new Exception("Необходимо выбрать описания");
            }
        }

        public IDataResult CreateDocument(DocumentGji document)
        {
            #region Формируем протокол
            var prescription = PrescriptionDomain.GetAll()
                                    .Where(x => x.Id == document.Id)
                                    .Select(x => new { x.Executant, x.Contragent, x.PhysicalPerson, x.PhysicalPersonInfo })
                                    .FirstOrDefault();

            if (prescription == null)
            {
                throw new Exception("Неудалось получить предписание");
            }

            var protocol = new Protocol()
            {
                Inspection = document.Inspection,
                TypeDocumentGji = TypeDocumentGji.Protocol,
                Contragent = prescription.Contragent,
                Executant = prescription.Executant,
                PhysicalPerson = prescription.PhysicalPerson,
                PhysicalPersonInfo = prescription.PhysicalPersonInfo
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
            var inspectorIds = this.DocumentInspectorDomain.GetAll().Where(x => x.DocumentGji.Id == document.Id)
                    .Select(x => x.Inspector.Id).Distinct().ToList();

            foreach (var id in inspectorIds)
            {
                listInspectors.Add(new DocumentGjiInspector
                {
                    DocumentGji = protocol,
                    Inspector = new Inspector { Id = id }
                });
            }
            #endregion

            #region Формируем нарушения (по выбранным пользователями группам)
            var violToSave = new List<ProtocolViolation>(); // Список на сохранение нарушений
            var violGroupToSave = new List<DocumentViolGroup>(); // Список на сохранение групп нарушений
            var violGroupLongTextToSave = new List<DocumentViolGroupLongText>(); // Список насохранение больших текстовых полей для групп нарушений
            var violGroupPointToSave = new List<DocumentViolGroupPoint>(); // Список на сохранение пунктов нарушений для групп нарушений 

            // получаем выбранные описания 
            var violGroups = ViolGroupDomain.GetAll()
                                .Where(x => ViolationGroupIds.Contains(x.Id));

            // получаем большой текст по выбранным описаниям
            var violGroupsLongText = ViolGroupLongTextDomain.GetAll()
                                       .Where(x => ViolationGroupIds.Contains(x.ViolGroup.Id))
                                       .GroupBy(x => x.ViolGroup.Id)
                                       .ToDictionary(x => x.Key, y => y.ToList());

            // получаем пункты нарушений по выбранным описаниям
            var violGroupsPoint = ViolGroupPointDomain.GetAll()
                                    .Where(x => ViolationGroupIds.Contains(x.ViolGroup.Id))
                                    .GroupBy(x => x.ViolGroup.Id)
                                    .ToDictionary(x => x.Key, y => y.ToList());

            foreach (var violGr in violGroups)
            {

                var newGroup = new DocumentViolGroup
                {
                    Document = protocol,
                    RealityObject = violGr.RealityObject,
                    Description = violGr.Description,
                    Action = violGr.Action,
                    DatePlanRemoval = violGr.DatePlanRemoval,
                    DateFactRemoval = violGr.DateFactRemoval
                };

                violGroupToSave.Add(newGroup);

                if (violGroupsLongText.ContainsKey(violGr.Id))
                {
                    foreach (var longText in violGroupsLongText.Get(violGr.Id))
                    {
                        violGroupLongTextToSave.Add(new DocumentViolGroupLongText
                        {
                            ViolGroup = newGroup,
                            Action = longText.Action,
                            Description = longText.Description
                        });
                    }
                }

                if (violGroupsPoint.ContainsKey(violGr.Id))
                {
                    foreach (var point in violGroupsPoint.Get(violGr.Id))
                    {
                        var newViolationStage = new ProtocolViolation
                        {
                            Document = protocol,
                            InspectionViolation = point.ViolStage.InspectionViolation,
                            DatePlanRemoval = point.ViolStage.InspectionViolation.DatePlanRemoval,
                            DateFactRemoval = point.ViolStage.InspectionViolation.DateFactRemoval,
                            TypeViolationStage = TypeViolationStage.Sentence
                        };

                        violToSave.Add(newViolationStage);

                        violGroupPointToSave.Add(new DocumentViolGroupPoint
                        {
                            ViolGroup = newGroup,
                            ViolStage = newViolationStage
                        });

                    }
                }


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

                    violGroupToSave.ForEach(x => this.ViolGroupDomain.Save(x));

                    violGroupLongTextToSave.ForEach(x => this.ViolGroupLongTextDomain.Save(x));

                    violToSave.ForEach(x => this.ProtocolViolationDomain.Save(x));

                    violGroupPointToSave.ForEach(x => this.ViolGroupPointDomain.Save(x));

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
