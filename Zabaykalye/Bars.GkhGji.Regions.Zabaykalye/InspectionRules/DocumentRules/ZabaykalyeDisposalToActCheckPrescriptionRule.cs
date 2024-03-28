namespace Bars.GkhGji.Regions.Zabaykalye.InspectionRules
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
    using Bars.GkhGji.Regions.Zabaykalye.Entities;
    using Bars.GkhGji.Rules;

    using Castle.Windsor;

    /// <summary>
    /// Правило создание документа 'Акт проверки предписания' из документа 'Распоряжение'
    /// </summary>
    public class ZabaykalyeDisposalToActCheckPrescriptionRule : IDocumentGjiRule
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

        public IDomainService<DocumentGjiChildren> ChildrenDomain { get; set; }

        public IDomainService<RealityObject> RoDomain { get; set; }

        public IDomainService<InspectionGjiViolStage> ViolStageDomain { get; set; }

        public IDomainService<ActRemovalViolation> ActRemovalViolationDomain { get; set; }

        public IDomainService<DocumentViolGroup> ViolGroupDomain { get; set; }

        public IDomainService<DocumentViolGroupLongText> ViolGroupLongTextDomain { get; set; }

        public IDomainService<DocumentViolGroupPoint> ViolGroupPointDomain { get; set; }

        #endregion

        public virtual string CodeRegion
        {
            get { return "Tat"; }
        }

        public virtual string Id
        {
            get { return "DisposalToActCheckPrescriptionRule"; }
        }

        public virtual string Description
        {
            get { return "Правило создание документа 'Акт проверки предписания' из документа 'Распоряжение'"; }
        }

        public virtual string ActionUrl
        {
            get { return "B4.controller.ActCheck"; }
        }

        public virtual string ResultName
        {
            get { return "Акт проверки предписания"; }
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
            
        }

        public virtual IDataResult CreateDocument(DocumentGji document)
        {
            #region Формируем Акт Проверки
            var actCheck = new ActCheck
            {
                Inspection = document.Inspection,
                TypeActCheck = TypeActCheckGji.ActCheckDocumentGji,
                TypeDocumentGji = TypeDocumentGji.ActCheck
            };
            #endregion

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
                // Если этап ненайден, то создаем новый этап
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

            actCheck.Stage = currentStage;

            #endregion

            #region Формируем связь с родителем
            var parentChildren = new DocumentGjiChildren
            {
                Parent = document,
                Children = actCheck
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
                    DocumentGji = actCheck,
                    Inspector = new Inspector { Id = id }
                });
            }
            #endregion

            #region Формируем акты устранения
            // Поскольку акт общий , то дома просто переносятся из Основания проверки
            var roIds = new List<long>();

            // Поскольку создается Акт проверки документа. То получаем список Id документов, по которым было создано распоряжение этого акта
            var docIds = ChildrenDomain.GetAll()
                    .Where(x => x.Children.Id == document.Id)
                    .Select(x => x.Parent.Id)
                    .ToList();

            // Затем по этим документам получаем Id домов У которых нарушения неустранены
            var violData = ViolStageDomain.GetAll()
                               .Where(
                                   x => docIds.Contains(x.Document.Id) && x.InspectionViolation.DateFactRemoval == null)
                               .Select(
                                   x =>
                                   new
                                       {
                                           RealityObjectId = x.InspectionViolation.RealityObject != null ? x.InspectionViolation.RealityObject.Id : 0,
                                           DocumentId = x.Document.Id,
                                           InspectionViolationId = x.InspectionViolation.Id,
                                           AreaMkd =  x.InspectionViolation.RealityObject != null ? x.InspectionViolation.RealityObject.AreaMkd : null,
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
                actCheck.Area = docRealObj.Values.Sum();
            }

            // Теперь формируем Этап Проверки именно для актов устранения
            // Если этап не найден, то создаем новый этап
            InspectionGjiStage removalStage = null;
            
            var listActRemovals = new List<ActRemoval>();
            var listActRemovalInspectors = new List<DocumentGjiInspector>();
            var listActRemovalChildren = new List<DocumentGjiChildren>();
            var violToSave = new List<ActRemovalViolation>(); // Список на сохранение нарушений
            var violGroupToSave = new List<DocumentViolGroup>(); // Список на сохранение групп нарушений
            var violGroupLongTextToSave = new List<DocumentViolGroupLongText>(); // Список насохранение больших текстовых полей для групп нарушений
            var violGroupPointToSave = new List<DocumentViolGroupPoint>(); // Список на сохранение пунктов нарушений для групп нарушений 

            // получаем выбранные описания 
            var violGroupQuery = ViolGroupDomain.GetAll()
                                .Where(x => docIds.Contains(x.Document.Id));

            // получаем большой текст по выбранным описаниям
            var violGroupsLongText = ViolGroupLongTextDomain.GetAll()
                                       .Where(x => violGroupQuery.Any(y => y.Id == x.ViolGroup.Id))
                                       .GroupBy(x => x.ViolGroup.Id)
                                       .ToDictionary(x => x.Key, y => y.ToList());

            // получаем пункты нарушений по выбранным описаниям
            var violGroupsPoint = ViolGroupPointDomain.GetAll()
                                    .Where(x => violGroupQuery.Any(y => y.Id == x.ViolGroup.Id))
                                    .GroupBy(x => x.ViolGroup.Id)
                                    .ToDictionary(x => x.Key, y => y.ToList());

            var violGroups = violGroupQuery.AsEnumerable()
                                           .GroupBy(x => x.Document.Id)
                                           .ToDictionary(x => x.Key, y => y.ToList());

            // Теперь для каждого документа формируем Акт устранения
            foreach (var id in docIds)
            {

                if (removalStage == null)
                {
                    removalStage = new InspectionGjiStage
                    {
                        Inspection = actCheck.Inspection,
                        TypeStage = TypeStage.ActRemoval,
                        Parent = parentStage,
                        Position = currentStage.Position + 1
                    };
                }

                // Создаем документ Акта устранения
                var actRemoval = new ActRemoval
                {
                    Inspection = new InspectionGji { Id = actCheck.Inspection.Id },
                    TypeDocumentGji = TypeDocumentGji.ActRemoval,
                    Stage = removalStage,
                    TypeRemoval = YesNoNotSet.No,
                    Area = docRealObj.ContainsKey(id) ? docRealObj[id] : 0
                };

                listActRemovals.Add(actRemoval);

                foreach (var inspectorId in inspectorIds)
                {
                    listActRemovalInspectors.Add(new DocumentGjiInspector
                    {
                        DocumentGji = actRemoval,
                        Inspector = new Inspector { Id = inspectorId }
                    });
                }

                // Ставим ссылку на родительский Документ ГЖИ
                listActRemovalChildren.Add(new DocumentGjiChildren
                {
                    Parent = new DocumentGji { Id = id },
                    Children = actRemoval
                });

                // Ставим ссылку на новы Акт проверки (в качестве родительскаого)
                listActRemovalChildren.Add(new DocumentGjiChildren
                {
                    Parent = actCheck,
                    Children = actRemoval
                });

                // Теперь для акта устранения формируем таблицу нарушений через нарушения Документа ГЖИ
                // И ставим тип TypeViolationStage.Removal (Устранение)
                if (violGroups.ContainsKey(id))
                {

                    foreach (var violGr in violGroups.Get(id))
                    {
                        var newGroup = new DocumentViolGroup
                        {
                            Document = actRemoval,
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
                                var newViolationStage = new ActRemovalViolation
                                {
                                    Document = actRemoval,
                                    InspectionViolation = point.ViolStage.InspectionViolation,
                                    DatePlanRemoval = point.ViolStage.InspectionViolation.DatePlanRemoval,
                                    DateFactRemoval = point.ViolStage.InspectionViolation.DateFactRemoval,
                                    TypeViolationStage = TypeViolationStage.Removal
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
                }
            }
            #endregion

            #region Формируем дома акта
            var listRo = new List<ActCheckRealityObject>();

            if (roIds.Count > 0)
            {
                foreach (var id in roIds)
                {
                    listRo.Add(new ActCheckRealityObject
                    {
                        ActCheck = actCheck,
                        RealityObject = new RealityObject { Id = id },
                        HaveViolation = YesNoNotSet.No
                    });
                }

                actCheck.Area = RoDomain.GetAll().Where(x => roIds.Contains(x.Id)).Sum(x => x.AreaMkd);
            }
            else
            {
                listRo.Add(new ActCheckRealityObject
                {
                    ActCheck = actCheck,
                    HaveViolation = YesNoNotSet.No
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

                    if (removalStage != null)
                    {
                        this.InspectionStageDomain.Save(removalStage);
                    }

                    this.ActCheckDomain.Save(actCheck);

                    listActRemovals.ForEach(x => ActRemovalDomain.Save(x));

                    listInspectors.ForEach(x => this.DocumentInspectorDomain.Save(x));

                    listActRemovalInspectors.ForEach(x => this.DocumentInspectorDomain.Save(x));

                    listRo.ForEach(x => this.ActCheckRoDomain.Save(x));

                    this.ChildrenDomain.Save(parentChildren);

                    listActRemovalChildren.ForEach(x => this.ChildrenDomain.Save(x));

                    violGroupToSave.ForEach(x => this.ViolGroupDomain.Save(x));

                    violGroupLongTextToSave.ForEach(x => this.ViolGroupLongTextDomain.Save(x));

                    violToSave.ForEach(x => this.ActRemovalViolationDomain.Save(x));

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

            return new BaseDataResult(new { documentId = actCheck.Id, inspectionId = document.Inspection.Id });
        }

        public virtual IDataResult ValidationRule(DocumentGji document)
        {
            /*
             тут проверяем, Если Распоряжение не РАспоряжение проверки предписания то недаем выполнить
            */
            
            if (document != null)
            {
                var disposal = DisposalDomain.FirstOrDefault(x => x.Id == document.Id);

                if ( disposal == null)
                {
                    return new BaseDataResult(false, string.Format("Не удалось получить распоряжение {0}", document.Id));
                }

                if ( disposal.TypeDisposal != TypeDisposalGji.DocumentGji)
                {
                    return new BaseDataResult(false, "Акт проверки предписания можно сформирвоать только для распоряжения на проверку предписания");
                }
            }
            
            return new BaseDataResult();
        }
    }
}
