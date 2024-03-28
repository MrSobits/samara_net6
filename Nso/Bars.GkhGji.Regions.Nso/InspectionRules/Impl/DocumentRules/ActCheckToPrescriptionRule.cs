using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bars.B4;
using Bars.B4.DataAccess;
using Bars.Gkh.Entities;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Enums;
using Bars.GkhGji.Regions.Nso.Entities;

namespace Bars.GkhGji.Regions.Nso.InspectionRules
{
    public class ActCheckToPrescriptionRule : Bars.GkhGji.InspectionRules.ActCheckToPrescriptionRule
    {
        public IDomainService<NsoActCheck> NsoActCheckDomain { get; set; }

        public IDomainService<NsoPrescription> NsoPrescriptionDomain { get; set; }

        public override IDataResult CreateDocument(DocumentGji document)
        {

            var actCheck = NsoActCheckDomain.FirstOrDefault(x => x.Id == document.Id);

            #region Формируем предписание
            var prescription = new NsoPrescription()
            {
                Inspection = document.Inspection,
                TypeDocumentGji = TypeDocumentGji.Prescription,
                Contragent = document.Inspection.Contragent,
                IsFamiliar = PrescriptionFamiliar.No,
                DocumentPlace = actCheck.DocumentPlace
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
            var inspectorIds = this.DocumentInspectorDomain.GetAll().Where(x => x.DocumentGji.Id == document.Id)
                    .Select(x => x.Inspector.Id).Distinct().ToList();

            foreach (var id in inspectorIds)
            {
                listInspectors.Add(new DocumentGjiInspector
                {
                    DocumentGji = prescription,
                    Inspector = new Inspector { Id = id }
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

                    this.NsoPrescriptionDomain.Save(prescription);

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
    }

}
