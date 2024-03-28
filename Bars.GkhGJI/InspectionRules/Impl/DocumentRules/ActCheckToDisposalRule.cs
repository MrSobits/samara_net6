namespace Bars.GkhGji.InspectionRules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Contracts;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Rules;

    using Castle.Windsor;

    /// <summary>
    /// Правило создание документа 'Распоряжения на проверку предписания' из документа 'Акт проверки'
    /// </summary>
    public class ActCheckToDisposalRule : ActCheckToDisposalBaseRule<Disposal>
    {
        /// <inheritdoc />
        protected override bool UseParentDocumentTypeValidating => false;
    }

    /// <summary>
    /// Базовый класс с функционалом для создания "Распоряжения"
    /// (или наследованного от него документа) на основе "Акта проверкаи"
    /// </summary>
    public class ActCheckToDisposalBaseRule<T> : IDocumentGjiRule
        where T : Disposal, new()
    {
        #region injection
        public IWindsorContainer Container { get; set; }

        public IDisposalText DisposalTextService { get; set; }

        public IDomainService<T> DisposalDomain { get; set; }

        public IDomainService<InspectionGjiStage> InspectionStageDomain { get; set; }

        public IDomainService<DocumentGjiInspector> DocumentInspectorDomain { get; set; }

        public IDomainService<DocumentGjiChildren> ChildrenDomain { get; set; }

        public IDomainService<KindCheckGji> KindCheckDomain { get; set; }

        public IDomainService<KindCheckRuleReplace> KindCheckRuleReplaceDomain { get; set; }
        #endregion

        #region Внутренние переменные
        private long[] ParentDocumentIds { get; set; }
        #endregion

        public string CodeRegion => "Tat";

        public virtual string Id => "ActCheckToDisposalRule";

        public virtual string Description => $"Правило создание документа '{this.ResultName}' " +
            "из документа 'Акт проверки' (по выбранным предписаниям)";

        public virtual string ActionUrl => "B4.controller.Disposal";

        public virtual string ResultName => $"{this.DisposalTextService.SubjectiveCase} на проверку предписания";

        public TypeDocumentGji TypeDocumentInitiator => TypeDocumentGji.ActCheck;

        public virtual TypeDocumentGji TypeDocumentResult => TypeDocumentGji.Disposal;

        protected virtual TypeStage InspectionTypeStageResult => TypeStage.DisposalPrescription;

        /// <summary>
        /// Использовать проверку соответствия типа родительского документа с результирующим типом
        /// </summary>
        protected virtual bool UseParentDocumentTypeValidating => true;

        // тут надо принять параметры если таковые имеютя
        public void SetParams(BaseParams baseParams)
        {
            // В данном методе принимаем параметр parentIds выбранных пользователем предписаний
            var parentIds = baseParams.Params.GetAs("parentIds", "");

            this.ParentDocumentIds = parentIds.ToLongArray();

            if (this.ParentDocumentIds.Length == 0)
            {
                throw new Exception("Необходимо выбрать предписания");
            }
        }

        public IDataResult CreateDocument(DocumentGji document)
        {
            #region Формируем распоряжение распоряжение на проверку предписания
            var disposal = new T
            {
                Inspection = document.Inspection,
                TypeDisposal = TypeDisposalGji.DocumentGji,
                TypeDocumentGji = this.TypeDocumentResult,
                TypeAgreementProsecutor = TypeAgreementProsecutor.NotSet,
                TypeAgreementResult = TypeAgreementResult.NotSet
            };
            #endregion

            #region Формируем этап проверки
            var stageMaxPosition = this.InspectionStageDomain.GetAll()
                .Where(x => x.Inspection.Id == document.Inspection.Id)
                .OrderByDescending(x => x.Position)
                .FirstOrDefault();

            // Создаем Этап Проверки (Который показывается слева в дереве)
            var stage = new InspectionGjiStage
            {
                Inspection = document.Inspection,
                Position = stageMaxPosition != null ? stageMaxPosition.Position + 1 : 1,
                TypeStage = this.InspectionTypeStageResult
            };

            disposal.Stage = stage;
            #endregion

            #region Проставляем вид проверки
            var rules = this.Container.ResolveAll<IKindCheckRule>().OrderBy(x => x.Priority);

            using (this.Container.Using(rules))
            {
                foreach (var rule in rules)
                {
                    if (rule.Validate(disposal))
                    {
                        var replace = this.KindCheckRuleReplaceDomain.GetAll()
                            .FirstOrDefault(x => x.RuleCode == rule.Code);

                        disposal.KindCheck = replace != null
                            ? this.KindCheckDomain.GetAll().FirstOrDefault(x => x.Code == replace.Code)
                            : this.KindCheckDomain.GetAll().FirstOrDefault(x => x.Code == rule.DefaultCode);
                    }
                }
            }
            #endregion

            #region Формируем инспекторов (берем из предписаний, которые выбрал пользователь)
            var listInspectors = new List<DocumentGjiInspector>();
            var inspectorIds = this.DocumentInspectorDomain.GetAll()
                .Where(x => this.ParentDocumentIds.Contains(x.DocumentGji.Id))
                .Select(x => x.Inspector.Id).Distinct().ToList();

            foreach (var inspector in inspectorIds)
            {
                listInspectors.Add(new DocumentGjiInspector
                {
                    DocumentGji = disposal,
                    Inspector = new Inspector { Id = inspector }
                });
            }
            #endregion

            #region Формируем связь с родителями (то есть выбранные предписания пользователей)
            var listParentChildren = new List<DocumentGjiChildren>();
            foreach (var id in this.ParentDocumentIds)
            {
                listParentChildren.Add(new DocumentGjiChildren
                {
                    Parent = new DocumentGji { Id = id },
                    Children = disposal // Данное распоряжение проставляем как дочернее
                });
            }
            #endregion

            #region Сохранение
            using (var tr = this.Container.Resolve<IDataTransaction>())
            {
                try
                {
                    this.InspectionStageDomain.Save(stage);

                    this.DisposalDomain.Save(disposal);

                    listInspectors.ForEach(x => this.DocumentInspectorDomain.Save(x));

                    listParentChildren.ForEach(x => this.ChildrenDomain.Save(x));

                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                    throw;
                }
            }
            #endregion

            return new BaseDataResult(new { documentId = disposal.Id, typeDocument = this.TypeDocumentResult, inspectionId = document.Inspection.Id });
        }

        public virtual IDataResult ValidationRule(DocumentGji document)
        {
            var success = document.Inspection.TypeBase != TypeBase.GjiWarning;

            if (this.UseParentDocumentTypeValidating)
            {
                var parent = this.ChildrenDomain.GetAll()
                    .FirstOrDefault(x => x.Children.Id == document.Id)?.Parent;

                success = success && parent?.TypeDocumentGji == this.TypeDocumentResult;
            }

            return new BaseDataResult(success, "");
        }
    }
}