namespace Bars.GkhGji.Regions.Tatarstan.InspectionRules.DocumentRules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Analytics.Utils;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.InspectionRules;

    using Castle.Windsor;

    public class ActCheckToWarningDocRule : IDocumentGjiRule
    {
        public IDomainService<WarningDoc> WarningDocDomain { get; set; }

        public IDomainService<InspectionGjiStage> InspectionStageDomain { get; set; }

        public IDomainService<WarningDocRealObj> WarnDocRoDomain { get; set; }

        public IDomainService<WarningDocViolations> WarnDocViolDomain { get; set; }

        public IDomainService<WarningDocViolationsDetail> WarnDocViolDetailDomain { get; set; }

        public IDomainService<ActCheckViolation> ActCheckViolationDomain { get; set; }

        public IDomainService<ViolationNormativeDocItemGji> ViolNormativeDomain { get; set; }

        public IDomainService<DocumentGjiChildren> ChildrenDomain { get; set; }

        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public string CodeRegion => "Tat";

        /// <inheritdoc />
        public string Id => nameof(ActCheckToWarningDocRule);

        /// <inheritdoc />
        public string Description => "Правило создания документа 'Предостережение' из документа 'Акт проверки'";

        /// <inheritdoc />
        public string ResultName => "Предостережение";

        /// <inheritdoc />
        public string ActionUrl => "B4.controller.WarningDoc";

        /// <inheritdoc />
        public TypeDocumentGji TypeDocumentInitiator => TypeDocumentGji.ActCheck;

        /// <inheritdoc />
        public TypeDocumentGji TypeDocumentResult => TypeDocumentGji.WarningDoc;

        /// <inheritdoc />
        public void SetParams(BaseParams baseParams)
        {
            // В данном методе принимаем параметр Id выбранных пользователем домов
            var realityIds = baseParams.Params.GetAs("realityIds", "");

            this.RealityObjectIds = realityIds.ToLongArray();

            if (this.RealityObjectIds.Length == 0)
            {
                throw new Exception("Необходимо выбрать дома");
            }
        }

        /// <inheritdoc />
        public IDataResult CreateDocument(DocumentGji document)
        {
            #region Формируем этап проверки
            var parentStage = document.Stage;

            if (parentStage != null && parentStage.Parent != null)
            {
                parentStage = parentStage.Parent;
            }

            var newStage = new InspectionGjiStage
            {
                Inspection = document.Inspection,
                TypeStage = TypeStage.WarningDoc,
                Parent = parentStage,
                Position = 1
            };

            var stageMaxPosition = this.InspectionStageDomain.GetAll()
                .Where(x => x.Inspection.Id == document.Inspection.Id)
                .OrderByDescending(x => x.Position)
                .FirstOrDefault();

            if (stageMaxPosition != null)
                newStage.Position = stageMaxPosition.Position + 1;

            #endregion

            #region Формируем документ

            var warningDoc = new WarningDoc
            {
                Inspection = document.Inspection,
                TypeDocumentGji = TypeDocumentGji.WarningDoc,
                Stage = newStage
            };
            #endregion

            #region Формируем связь с родителем
            var parentChildren = new DocumentGjiChildren
            {
                Parent = document,
                Children = warningDoc
            };
            #endregion

            #region Формируем нарушения
            var listRo = new List<WarningDocRealObj>();
            var listViolations = new List<WarningDocViolations>();
            var listViolationDetail = new List<WarningDocViolationsDetail>();

            var listActViol = this.ActCheckViolationDomain.GetAll()
                .Where(x => x.ActObject.ActCheck.Id == document.Id && this.RealityObjectIds.Contains(x.ActObject.RealityObject.Id) &&
                    x.ActObject.HaveViolation == YesNoNotSet.Yes)
                .Select(x => new
                {
                    x.ActObject.RealityObject,
                    x.InspectionViolation.Violation
                });

            var listNormative = this.ViolNormativeDomain.GetAll()
                .Where(x => listActViol.Any(y => x.ViolationGji.Id == y.Violation.Id))
                .Select(x => new
                {
                    x.ViolationGji,
                    x.NormativeDocItem.NormativeDoc
                })
                .AsEnumerable()
                .GroupBy(x => x.ViolationGji)
                .ToDictionary(x => x.Key, y => y.Select(x => x.NormativeDoc));

            var groupList = listActViol
                .AsEnumerable()
                .SelectMany(x =>
                {
                    var listNormativeDocs = listNormative.TryGetValue(x.Violation, out var normativeList)
                        ? normativeList
                        : new List<NormativeDoc>();

                    var resultList = new List<ViolationInfoDto>();

                    if (listNormativeDocs.Any())
                    {
                        foreach (var normative in listNormativeDocs)
                        {
                            resultList.Add(new ViolationInfoDto
                            {
                                RealityObject = x.RealityObject,
                                Violation = x.Violation,
                                NormativeDoc = normative
                            });
                        }

                        return resultList;
                    }

                    return new List<ViolationInfoDto>
                    {
                        new ViolationInfoDto
                        {
                            RealityObject = x.RealityObject,
                            Violation = x.Violation
                        }
                    };
                })
                .GroupBy(x => new
                {
                    x.RealityObject,
                    x.NormativeDoc
                })
                .ToDictionary(x => x.Key,
                    y => y.Select(x => x.Violation));

            foreach (var group in groupList)
            {
                if (group.Key.NormativeDoc.IsNull())
                {
                    return new BaseDataResult(false,
                        $"Нарушения: {string.Join(",", group.Value.Select(x => x.Name))} не связаны с нормативно-правовым документом. Необходимо указать соответствие в справочнике Нарушения.");
                }

                if (!listRo.Any(x => x.RealityObject == group.Key.RealityObject))
                {
                    listRo.Add(new WarningDocRealObj
                    {
                        WarningDoc = warningDoc,
                        RealityObject = group.Key.RealityObject
                    });
                }

                var violationGroup = new WarningDocViolations
                {
                    WarningDoc = warningDoc,
                    RealityObject = group.Key.RealityObject,
                    NormativeDoc = group.Key.NormativeDoc
                };

                listViolations.Add(violationGroup);

                if (group.Value.Any())
                {
                    listViolationDetail.AddRange(group.Value.Select(violation => new WarningDocViolationsDetail
                    {
                        WarningDocViolations = violationGroup,
                        ViolationGji = violation
                    }));
                }
            }
            #endregion

            #region Сохранение
            using (var tr = Container.Resolve<IDataTransaction>())
            {
                if (newStage != null)
                {
                    this.InspectionStageDomain.Save(newStage);
                }

                this.WarningDocDomain.Save(warningDoc);

                this.ChildrenDomain.Save(parentChildren);

                listRo.ForEach(x => this.WarnDocRoDomain.Save(x));
                listViolations.ForEach(x => this.WarnDocViolDomain.Save(x));
                listViolationDetail.ForEach(x => this.WarnDocViolDetailDomain.Save(x));

                tr.Commit();
            }
            #endregion

            return new BaseDataResult(new { documentId = warningDoc.Id, inspectionId = warningDoc.Inspection.Id });
        }

        /// <inheritdoc />
        public IDataResult ValidationRule(DocumentGji document)
        {
            if (this.ChildrenDomain.GetAll()
                .Any(
                    x =>
                        x.Parent.Id == document.Id
                        && x.Children.TypeDocumentGji == TypeDocumentGji.WarningDoc))
            {
                return new BaseDataResult(false, "У акта может быть только 1 предостережение");
            }

            var hasViolations = this.ActCheckViolationDomain.GetAll()
                .Any(x => x.ActObject.ActCheck == document && x.ActObject.HaveViolation == YesNoNotSet.Yes);

            if (!hasViolations)
            {
                return new BaseDataResult(false, "Отсутствуют нарушения");
            }

            return new BaseDataResult();
        }

        private class ViolationInfoDto
        {
            internal RealityObject RealityObject { get; set; }

            internal ViolationGji Violation { get; set; }

            internal NormativeDoc NormativeDoc { get; set; }
        }

        private long[] RealityObjectIds { get; set; }
    }
}
