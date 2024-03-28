namespace Bars.GkhGji.Regions.Tomsk.InspectionRules
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
    using Bars.GkhGji.Regions.Tomsk.Entities;

    using Castle.Windsor;

    /// <summary>
    /// Правило создание документа 'Предписания' из документа 'Административного дела'
    /// </summary>
    public class TomskAdminCaseToPrescriptionlRule : IDocumentGjiRule
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<DocumentGjiInspector> DocumentInspectorDomain { get; set; }

        public IDomainService<Prescription> PrescriptionDomain { get; set; }

        public IDomainService<AdministrativeCase> AdminCaseDomain { get; set; }

        public IDomainService<InspectionGjiStage> InspectionStageDomain { get; set; }

        public IDomainService<DocumentGjiChildren> ChildrenDomain { get; set; }

        public IDomainService<PrescriptionViol> PrescriptionViolDomain { get; set; }

        public IDomainService<PrescriptionRealityObject> PrescriptionRoDomain { get; set; }

        public IDomainService<InspectionGjiViol> InspectionViolDomain { get; set; }

        #region Внутренние переменные
        private long[] ViolationIds { get; set; }
        #endregion

        public string CodeRegion
        {
            get { return "Tomsk"; }
        }

        public string Id
        {
            get { return "AdminCaseToPrescriptionlRule"; }
        }

        public string Description
        {
            get { return "Правило создания документа 'Предписания' из документа 'Административное дело'"; }
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
            get { return TypeDocumentGji.AdministrativeCase; }
        }

        public TypeDocumentGji TypeDocumentResult
        {
            get { return TypeDocumentGji.Prescription; }
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
            var adminCase = AdminCaseDomain.GetAll().FirstOrDefault(x => x.Id == document.Id);

            if (adminCase == null)
            {
                return new BaseDataResult(false, "Не найден документ Административного дела");
            }

            var prescription = new Prescription()
            {
                Inspection = document.Inspection,
                TypeDocumentGji = TypeDocumentGji.Prescription,
                Contragent = adminCase.Contragent,
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

            #region Формируем Инспекторов тянем их из родительского документа
            var listInspectors = new List<DocumentGjiInspector>();

            if (adminCase.Inspector != null)
            {
                listInspectors.Add(new DocumentGjiInspector
                {
                    DocumentGji = prescription,
                    Inspector = adminCase.Inspector
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

        public IDataResult ValidationRule(DocumentGji document)
        {
            return new BaseDataResult();
        }
    }
}
