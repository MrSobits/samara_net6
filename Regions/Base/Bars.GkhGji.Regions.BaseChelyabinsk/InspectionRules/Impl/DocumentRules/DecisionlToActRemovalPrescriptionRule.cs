﻿namespace Bars.GkhGji.Regions.BaseChelyabinsk.InspectionRules.Impl.DocumentRules
{
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
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActRemoval;

    using Castle.Windsor;

    /// <summary>
    /// Правило создание документа 'Акт проверки предписания' из документа 'Распоряжение'
    /// </summary>
    public class DecisionToActRemovalPrescriptionRule : IDocumentGjiRule
    {
        #region injection
        public IWindsorContainer Container { get; set; }

        #endregion

        public virtual string CodeRegion
        {
            get { return "Tat"; }
        }

        public virtual string Id
        {
            get { return "DecisionToActRemovalPrescriptionRule"; }
        }

        public virtual string Description
        {
            get { return "Правило создание документа 'Акт проверки предписания' из документа 'Решение'"; }
        }

        public virtual string ActionUrl
        {
            get { return "B4.controller.ActRemoval"; }
        }

        public virtual string ResultName
        {
            get { return "Акт проверки предписания"; }
        }

        public virtual TypeDocumentGji TypeDocumentInitiator
        {
            get { return TypeDocumentGji.Decision; }
        }

        public virtual TypeDocumentGji TypeDocumentResult
        {
            get { return TypeDocumentGji.ActRemoval; }
        }

        // тут надо принять параметры если таковые имеютя
        public virtual void SetParams(BaseParams baseParams)
        {
            
        }

        public virtual IDataResult CreateDocument(DocumentGji document)
        {
            var InspectionStageDomain = this.Container.Resolve<IDomainService<InspectionGjiStage>>();
            var DocumentInspectorDomain = this.Container.Resolve<IDomainService<DocumentGjiInspector>>();
            var ChildrenDomain = this.Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var ViolStageDomain = this.Container.Resolve<IDomainService<InspectionGjiViolStage>>();
            var ActRemovalDomain = this.Container.Resolve<IDomainService<ActRemoval>>();
            var ActRemovalViolationDomain = this.Container.Resolve<IDomainService<ActRemovalViolation>>();
            var DisposalProvDocDomain = this.Container.Resolve<IDomainService<DecisionProvidedDoc>>();
            var ActRemovalProvDocDomain = this.Container.Resolve<IDomainService<ActRemovalProvidedDoc>>();

            try
            {
				// Создаем и сохраняем документ Акта устранения
				var actRemoval = new ActRemoval
				{
					Inspection = document.Inspection,
					TypeDocumentGji = TypeDocumentGji.ActRemoval,
					TypeRemoval = YesNoNotSet.No
				};

                #region Формируем этап проверки
                var parentStage = document.Stage;
                if (parentStage != null && parentStage.Parent != null)
                {
                    parentStage = parentStage.Parent;
                }

                InspectionGjiStage newStage = null;
                var currentStage = InspectionStageDomain.GetAll()
                                       .FirstOrDefault(x => x.Parent.Id == parentStage.Id
                                                            && x.TypeStage == TypeStage.ActRemoval);

                if (currentStage == null)
                {
                    // Если этап ненайден, то создаем новый этап
                    currentStage = new InspectionGjiStage
                    {
                        Inspection = document.Inspection,
                        TypeStage = TypeStage.ActRemoval,
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

				actRemoval.Stage = currentStage;

                #endregion

				// Поскольку создается Акт проверки документа. То получаем список Id документов, по которым было создано распоряжение этого акта
				var parentId = ChildrenDomain.GetAll()
						.Where(x => x.Children.Id == document.Id)
						.Select(x => x.Parent.Id)
						.FirstOrDefault();

                #region Формируем связь с родителем

                var parentChildren = new DocumentGjiChildren
                {
                    Parent = document,
                    Children = actRemoval
                };

				var parentChildren2 = new DocumentGjiChildren
				{
					Parent = new DocumentGji { Id = parentId },
					Children = actRemoval
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
                        DocumentGji = actRemoval,
                        Inspector = new Inspector { Id = id }
                    });
                }
                #endregion

                #region Формируем акты устранения
                // Поскольку акт общий , то дома просто переносятся из Основания проверки
                var roIds = new List<long>();

                // Затем по этим документам получаем Id домов У которых нарушения неустранены
                var violData = ViolStageDomain.GetAll()
                                   .Where(
                                       x => parentId == x.Document.Id && x.InspectionViolation.DateFactRemoval == null)
                                   .Select(
                                       x =>
                                       new
                                       {
                                           RealityObjectId = x.InspectionViolation.RealityObject != null ? x.InspectionViolation.RealityObject.Id : 0,
                                           DocumentId = x.Document.Id,
                                           InspectionViolationId = x.InspectionViolation.Id,
                                           AreaMkd = x.InspectionViolation.RealityObject != null ? x.InspectionViolation.RealityObject.AreaMkd : null,
                                           x.DatePlanRemoval
                                       })
                                   .AsEnumerable();

                // получаем идентификаторы домов
                roIds.AddRange(violData
                        .Where(x => x.RealityObjectId > 0)
                        .Select(x => x.RealityObjectId)
                        .Distinct()
                        .ToList());

                var docRealObj = violData
                        .Where(x => x.RealityObjectId > 0)
                        .GroupBy(x => x.DocumentId)
                        .ToDictionary(x => x.Key, y => y
                            .Select(x => new
                            {
                                x.RealityObjectId,
                                x.AreaMkd
                            })
                            .Distinct()
                            .Sum(x => x.AreaMkd.ToDecimal()));

                // В поле "Площадь" акта проверки копируется сумма всех Общих площадей домов актов  проверки предписаний, по которым сформирован акт
                if (docRealObj.Count > 0)
                {
                    actRemoval.Area = docRealObj.Values.Sum();    
                }
                
                // Затем по этим документам сами нарушения
                var docViolations = violData
                        .GroupBy(x => x.DocumentId)
                        .ToDictionary(x => x.Key, y => y.Select(x => new { x.InspectionViolationId, x.DatePlanRemoval }).ToList());

                var listActRemovalViolations = new List<ActRemovalViolation>();
                // Теперь для акта устранения формируем таблицу нарушений через нарушения Документа ГЖИ
                // И ставим тип TypeViolationStage.Removal (Устранение)
                if (docViolations.ContainsKey(parentId))
                {
                    foreach (var viol in docViolations[parentId])
                    {
                        listActRemovalViolations.Add(new ActRemovalViolation
                        {
                            Document = actRemoval,
                            InspectionViolation = new InspectionGjiViol { Id = viol.InspectionViolationId },
                            TypeViolationStage = TypeViolationStage.Removal,
                            DatePlanRemoval = viol.DatePlanRemoval
                        });
                    }
                }
                #endregion

                #region Формируем Предоставленные дкоументы
                var listDocsToSave = new List<ActRemovalProvidedDoc>();
                var provDocIds = DisposalProvDocDomain.GetAll()
                    .Where(x => x.Decision.Id == document.Id)
                    .Select(x => x.ProvidedDoc.Id)
                    .Distinct()
                    .ToList();

                foreach (var id in provDocIds)
                {
	                listDocsToSave.Add(new ActRemovalProvidedDoc
                    {
                        ActRemoval = actRemoval,
                        ProvidedDoc = new ProvidedDocGji { Id = id }
                    });
                }
                #endregion


                #region Сохранение
                using (var tr = this.Container.Resolve<IDataTransaction>())
                {
                    try
                    {
                        if (newStage != null)
                        {
                            InspectionStageDomain.Save(newStage);
                        }

                        ActRemovalDomain.Save(actRemoval);

                        listInspectors.ForEach(DocumentInspectorDomain.Save);

                        ChildrenDomain.Save(parentChildren);
                        ChildrenDomain.Save(parentChildren2);

                        listActRemovalViolations.ForEach(ActRemovalViolationDomain.Save);

						listDocsToSave.ForEach(ActRemovalProvDocDomain.Save);

                        tr.Commit();
                    }
                    catch
                    {
                        tr.Rollback();
                        throw;
                    }
                }
                #endregion

                return new BaseDataResult(new { documentId = actRemoval.Id, inspectionId = document.Inspection.Id });
            }
            finally 
            {
                
                this.Container.Release(InspectionStageDomain);
                this.Container.Release(DocumentInspectorDomain);
                this.Container.Release(ChildrenDomain);
                this.Container.Release(ViolStageDomain);
                this.Container.Release(ActRemovalDomain);
                this.Container.Release(ActRemovalViolationDomain);
                this.Container.Release(DisposalProvDocDomain);
            }
            
        }

        public virtual IDataResult ValidationRule(DocumentGji document)
        {
            /*
             тут проверяем, Если Распоряжение не РАспоряжение проверки предписания то недаем выполнить
            */
            
            var DisposalDomain = this.Container.Resolve<IDomainService<Decision>>();

            try
            {

                if (document != null)
                {
                    var disposal = DisposalDomain.FirstOrDefault(x => x.Id == document.Id);

                    if (disposal == null)
                    {
                        return new BaseDataResult(
                            false, string.Format("Не удалось получить решение {0}", document.Id));
                    }

                    if (disposal.TypeDisposal != TypeDisposalGji.DocumentGji)
                    {
                        return new BaseDataResult(
                            false,
                            "Акт проверки предписания можно сформирвоать только для решения на проверку предписания");
                    }
                }

                return new BaseDataResult();
            }
            finally 
            {
                this.Container.Release(DisposalDomain);
            }
        }
    }
}
