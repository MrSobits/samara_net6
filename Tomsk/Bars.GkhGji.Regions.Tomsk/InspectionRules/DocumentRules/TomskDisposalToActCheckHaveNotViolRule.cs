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
    /// Правило создание документа 'Акта нарушения не выявлены' из документа 'Приказ'
    /// </summary>
    public class TomskDisposalToActCheckHaveNotViolRule : IDocumentGjiRule
    {
        #region injection
        public IWindsorContainer Container { get; set; }

        public IDomainService<Disposal> DisposalDomain { get; set; }

        public IDomainService<ActCheck> ActCheckDomain { get; set; }

        public IDomainService<ActRemoval> ActRemovalDomain { get; set; }

        public IDomainService<InspectionGjiStage> InspectionStageDomain { get; set; }

        public IDomainService<InspectionGjiInspector> InspectionInspectorDomain { get; set; }

        public IDomainService<InspectionGjiRealityObject> InspectionRoDomain { get; set; }

        public IDomainService<DocumentGjiInspector> DocumentInspectorDomain { get; set; }

        public IDomainService<ActCheckRealityObject> ActCheckRoDomain { get; set; }

        public IDomainService<PrescriptionRealityObject> PrescriptionRoDomain { get; set; }

        public IDomainService<DocumentGjiChildren> ChildrenDomain { get; set; }

        public IDomainService<RealityObject> RoDomain { get; set; }

        public IDomainService<InspectionGjiViolStage> ViolStageDomain { get; set; }

        public IDomainService<ActCheckVerificationResult> ActCheckVerifiDomain { get; set; }

        #endregion

        public virtual string CodeRegion
        {
            get { return "Tomsk"; }
        }

        public virtual string Id
        {
            get { return "TomskDisposalToActCheckHaveNotViolRule"; }
        }

        public virtual string Description
        {
            get { return "Правило создания документа 'Акт (нарушения не выявлены)' из документа 'Приказ'"; }
        }

        public virtual string ActionUrl
        {
            get { return "B4.controller.ActCheck"; }
        }

        public virtual string ResultName
        {
            get { return "Акт (нарушения не выявлены)"; }
        }

        public virtual TypeDocumentGji TypeDocumentInitiator
        {
            get { return TypeDocumentGji.Disposal; }
        }

        public virtual TypeDocumentGji TypeDocumentResult
        {
            get { return TypeDocumentGji.ActCheck; }
        }

        // тут надо принять параметры если таковые имеютя
        public virtual void SetParams(BaseParams baseParams)
        {
            // Не ожидаем никаких параметров
        }

        public virtual IDataResult CreateDocument(DocumentGji document)
        {


            // Поулчаем наличие других уже созданных документов из распоряжения
            var docConcurents = ChildrenDomain.GetAll().Where(x => x.Parent.Id == document.Id).Select(x => x.Children).ToList();

            if (docConcurents.Count() > 0)
            {
                if (docConcurents.Any( x => x.TypeDocumentGji == TypeDocumentGji.ActCheck ))
                {
                    return new BaseDataResult(false, "Невозможно сформировать Акт (нарушения не выявлены), так как уже сформирован акт проверки");
                }

                if (docConcurents.Any(x => x.TypeDocumentGji == TypeDocumentGji.Prescription))
                {
                    return new BaseDataResult(false, "Невозможно сформировать Акт (нарушения не выявлены), так как уже сформировано предписание");
                }
            }

            // Поулчаем само распоряжение
            var disposal = DisposalDomain.FirstOrDefault(x => x.Id == document.Id);
            
            #region Формируем этап проверки
            var parentStage = document.Stage;
            if (parentStage != null && parentStage.Parent != null)
            {
                parentStage = parentStage.Parent;
            }

            InspectionGjiStage newStage = null;
            var currentStage = this.InspectionStageDomain.GetAll()
                                   .FirstOrDefault(x => x.Parent.Id == parentStage.Id
                                                        && x.TypeStage == TypeStage.ActCheck);

            if (currentStage == null)
            {
                // Если этап не найден, то создаем новый этап
                currentStage = new InspectionGjiStage
                {
                    Inspection = document.Inspection,
                    TypeStage = TypeStage.ActCheck,
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

            #region Формируем Акт Проверки
            var actCheck = new ActCheck
            {
                Inspection = document.Inspection,
                TypeActCheck = TypeActCheckGji.ActCheckIndividual,
                TypeDocumentGji = TypeDocumentGji.ActCheck,
                Stage = currentStage,
                DocumentDate = disposal.DateEnd
            };
            #endregion

            #region Проставляем признак Ход проверки - нарушение не выявлено
            var verify = new ActCheckVerificationResult
            {
                ActCheck = actCheck,
                TypeVerificationResult = Bars.GkhGji.Regions.Tomsk.Enums.TypeVerificationResult.Type40
            };
            #endregion

            #region Формируем связь с родителем
            var parentChildren = new DocumentGjiChildren
            {
                Parent = document,
                Children = actCheck
            };
            #endregion

            #region переносим дома либо из основания либо из родительского распоряжения
            // тут необходимо поулчить родителя распоряжения 
            // Если Акт (нарушения не выявлены) сформирован из Основного распоряжения, то дома берем из основания проверки
            // Если акт сформирован из распоряжения на проверку предписания (тоетс ьу него есть родительское предписание) то дома берем из него
            // Если акт сформирован по новой проверки, тоесть у него еть родительский акт , тогда дома берем из родителського акта
            var parent = ChildrenDomain.GetAll().Where(x => x.Children.Id == document.Id).Select(x => x.Parent).FirstOrDefault();
            var listIds = new List<long>();

            if (parent == null)
            {
                // Если родительского документа нет то берем из основания проверки
                listIds = InspectionRoDomain.GetAll()
                            .Where(x => x.Inspection.Id == document.Inspection.Id)
                            .Select(x => x.RealityObject.Id)
                            .Distinct()
                            .ToList();
            }
            else if (parent.TypeDocumentGji == TypeDocumentGji.Prescription)
            {
                // если распоряжение сформировано из предписания то берем дома оттуда
                listIds = PrescriptionRoDomain.GetAll()
                            .Where(x => x.Prescription.Id == parent.Id)
                            .Select(x => x.RealityObject.Id)
                            .Distinct()
                            .ToList();
            }
            else if (parent.TypeDocumentGji == TypeDocumentGji.ActCheck)
            {
                // если распоряжение сформировано из прердписания то берем дома оттуда
                listIds = ActCheckRoDomain.GetAll()
                            .Where(x => x.ActCheck.Id == parent.Id)
                            .Select(x => x.RealityObject.Id)
                            .Distinct()
                            .ToList();
            }
            
            var listRo = new List<ActCheckRealityObject>();

            foreach (var roId in listIds)
            {
                listRo.Add(new ActCheckRealityObject
                               {
                                   RealityObject = new RealityObject { Id = roId },
                                   ActCheck = actCheck,
                                   HaveViolation = YesNoNotSet.No
                               });
            }
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
                    DocumentGji = actCheck,
                    Inspector = new Inspector { Id = id }
                });
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

                    this.ActCheckDomain.Save(actCheck);

                    this.ActCheckVerifiDomain.Save(verify);

                    this.ChildrenDomain.Save(parentChildren);

                    listRo.ForEach(x => ActCheckRoDomain.Save(x));

                    listInspectors.ForEach(x => this.DocumentInspectorDomain.Save(x));

                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                    throw;
                }
            }
            #endregion

            return new BaseDataResult(new { documentId = actCheck.Id, inspectionId = actCheck.Inspection.Id });
        }

        public virtual IDataResult ValidationRule(DocumentGji document)
        {
            
            return new BaseDataResult();
        }
    }
}
