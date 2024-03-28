namespace Bars.GkhGji.Regions.Tomsk.InspectionRules
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
    using Bars.GkhGji.InspectionRules;
    using Bars.GkhGji.Regions.Tomsk.Entities;

    using Castle.Windsor;

    /// <summary>
    /// Правило создание документа 'Предписание' из документа 'Распоряжение на проверку предписания' (для распоряжения на проверку предписания)
    /// </summary>
    public class TomskDisposalToPrescriptionByViolationRule : IDocumentGjiRule
    {
        #region injection
        public IWindsorContainer Container { get; set; }

        public IDomainService<Disposal> DisposalDomain { get; set; }

        public IDomainService<Prescription> PrescriptionDomain { get; set; }

        public IDomainService<ActCheckRealityObject> ActCheckRoDomain { get; set; }

        public IDomainService<ActCheckViolation> ActCheckViolDomain { get; set; }

        public IDomainService<InspectionGjiStage> InspectionStageDomain { get; set; }

        public IDomainService<InspectionGjiInspector> InspectionInspectorDomain { get; set; }

        public IDomainService<InspectionGjiRealityObject> InspectionRoDomain { get; set; }

        public IDomainService<InspectionGjiViol> InspectionViolDomain { get; set; }

        public IDomainService<DocumentGjiInspector> DocumentInspectorDomain { get; set; }

        public IDomainService<PrescriptionRealityObject> PrescriptionRoDomain { get; set; }

        public IDomainService<PrescriptionViol> PrescriptionViolDomain { get; set; }

        public IDomainService<DocumentGjiChildren> ChildrenDomain { get; set; }

        public IDomainService<InspectionGjiViolStage> ViolStageDomain { get; set; }
        #endregion

        #region Внутренние переменные
        private long[] ViolationIds { get; set; }
        #endregion

        public virtual string CodeRegion
        {
            get { return "Tomsk"; }
        }

        public virtual string Id
        {
            get { return "TomskDisposalToPrescriptionByViolationRule"; }
        }

        public virtual string Description
        {
            get { return "Правило создание документа 'Предписание' из документа 'Распоряжение на проверку предписания'"; }
        }

        public virtual string ActionUrl
        {
            get { return "B4.controller.Prescription"; }
        }

        public virtual string ResultName
        {
            get { return "Предписание"; }
        }

        public virtual TypeDocumentGji TypeDocumentInitiator
        {
            get { return TypeDocumentGji.Disposal; }
        }

        public virtual TypeDocumentGji TypeDocumentResult
        {
            get { return TypeDocumentGji.Prescription; }
        }

        // тут надо принять параметры если таковые имеютя
        public virtual void SetParams(BaseParams baseParams)
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

        public virtual IDataResult CreateDocument(DocumentGji document)
        {

            /*
             * проверяем, если у распоряжения уже есть акты проверки, то нельзя создавать предписание
             */
            if (ChildrenDomain.GetAll().Any(x => x.Parent.Id == document.Id && x.Children.TypeDocumentGji == TypeDocumentGji.ActCheck))
            {
                return new BaseDataResult(
                    false,
                    "Невозможно сформировать Предписание, так как уже сформирован акт проверки");
            }

            #region Формируем этап проверки
            var parentStage = document.Stage;
            if (parentStage != null && parentStage.Parent != null)
            {
                parentStage = parentStage.Parent;
            }

            InspectionGjiStage newStage = null;
            var currentStage = this.InspectionStageDomain.GetAll()
                                   .FirstOrDefault(x => x.Parent.Id == parentStage.Id
                                                        && x.TypeStage == TypeStage.Prescription);

            if (currentStage == null)
            {
                // Если этап не найден, то создаем новый этап
                currentStage = new InspectionGjiStage
                {
                    Inspection = document.Inspection,
                    TypeStage = TypeStage.Prescription,
                    Parent = parentStage,
                    Position = 1
                };

                var stageMaxPosition =
                    InspectionStageDomain.GetAll()
                        .Where(x => x.Inspection.Id == document.Inspection.Id)
                        .OrderByDescending(x => x.Position)
                        .FirstOrDefault();

                if (stageMaxPosition != null)
                    currentStage.Position = stageMaxPosition.Position + 1;

                newStage = currentStage;
            }

            #endregion

            #region Формируем Предписание
            var prescription = new Prescription()
            {
                Inspection = document.Inspection,
                TypeDocumentGji = TypeDocumentGji.Prescription,
                Stage = currentStage
            };
            #endregion

            #region Формируем связь с родителем
            var parentChildren = new DocumentGjiChildren
            {
                Parent = document,
                Children = prescription
            };
            #endregion

            #region Формируем Инспекторов
            var listInspectors = new List<DocumentGjiInspector>();
            var inspectorIds = DocumentInspectorDomain.GetAll()
                .Where(x => x.DocumentGji.Id == document.Id)
                .Select(x => x.Inspector.Id)
                .Distinct()
                .ToList();

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
            var listNewViol = new List<PrescriptionViol>();
            var violationList = InspectionViolDomain.GetAll().Where(x => ViolationIds.Contains(x.Id));
            
            foreach (var viol in violationList)
            {
                var newRecord = new PrescriptionViol()
                {
                    Document = prescription,
                    InspectionViolation = viol,
                    DatePlanRemoval = viol.DatePlanRemoval,
                    DateFactRemoval = viol.DateFactRemoval,
                    TypeViolationStage = TypeViolationStage.InstructionToRemove,
                    Description = viol.Description,
                    Action = viol.Action,
                    SumAmountWorkRemoval = viol.SumAmountWorkRemoval
                };

                listNewViol.Add(newRecord);
            }
            #endregion

            #region Формируем дома документа

            // Поскольку пользователи сами выбирают дома то их и переносим
            var listRo = new List<PrescriptionRealityObject>();
            var listRoViol = InspectionViolDomain.GetAll()
                                      .Where(x => ViolationIds.Contains(x.Id))
                                      .Where(x => x.RealityObject != null)
                                      .Select(x => x.RealityObject.Id)
                                      .Distinct()
                                      .ToList();

            foreach (var id in listRoViol)
            {
                var ro = new PrescriptionRealityObject
                {
                    Prescription = prescription,
                    RealityObject = new RealityObject { Id = id }
                };

                listRo.Add(ro);
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

                    listRo.ForEach(x => this.PrescriptionRoDomain.Save(x));

                    listNewViol.ForEach(x => this.PrescriptionViolDomain.Save(x));

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
            /*
             Данное правило работает для предписаний любого типа
            */

            return new BaseDataResult();
        }
    }
}
