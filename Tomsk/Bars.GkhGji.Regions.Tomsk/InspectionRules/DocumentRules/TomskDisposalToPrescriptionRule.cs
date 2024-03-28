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
    /// Правило создание документа 'Предписание' из документа 'Распоряжение' (для основного распоряжения)
    /// </summary>
    public class TomskDisposalToPrescriptionRule : IDocumentGjiRule
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

        public IDomainService<DocumentGjiInspector> DocumentInspectorDomain { get; set; }

        public IDomainService<PrescriptionRealityObject> PrescriptionRoDomain { get; set; }

        public IDomainService<DocumentGjiChildren> ChildrenDomain { get; set; }

        public IDomainService<InspectionGjiViolStage> ViolStageDomain { get; set; }
        #endregion

        #region Внутренние переменные
        private long[] RealityObjectIds { get; set; }
        #endregion

        public virtual string CodeRegion
        {
            get { return "Tomsk"; }
        }

        public virtual string Id
        {
            get { return "TomskDisposalToPrescriptionRule"; }
        }

        public virtual string Description
        {
            get { return "Правило создание документа 'Предписание' из документа 'Распоряжение'"; }
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
            // В данном методе принимаем параметр Id выбранных пользователем домов
            var realityIds = baseParams.Params.GetAs("realityIds", "");

            RealityObjectIds = !string.IsNullOrEmpty(realityIds)
                                  ? realityIds.Split(',').Select(id => id.ToLong()).ToArray()
                                  : new long[0];

            if (RealityObjectIds.Length == 0)
            {
                throw new Exception("Необходимо выбрать дома");
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

            #region Формируем дома документа

            // Поскольку пользователи сами выбирают дома то их и переносим
            var listRo = new List<PrescriptionRealityObject>();

            foreach (var id in RealityObjectIds)
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
             если Приказ на проверку предписания - то данное правило уже нельзя сформировать
             поскольку необходимо выбирать не из домов а по нарушениям
            */

            var disposal = DisposalDomain.GetAll().FirstOrDefault(x => x.Id == document.Id);

            if (disposal.TypeDisposal != TypeDisposalGji.Base)
            {
                return new BaseDataResult(false, "Данное правило работает только для основного приказа");
            }

            return new BaseDataResult();
        }
    }
}
