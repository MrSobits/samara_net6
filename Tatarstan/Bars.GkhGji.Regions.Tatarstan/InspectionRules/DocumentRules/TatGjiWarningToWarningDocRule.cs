namespace Bars.GkhGji.Regions.Tatarstan.InspectionRules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Contracts;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.InspectionRules;
    using Bars.GkhGji.Rules;

    using Castle.Windsor;

    using NHibernate.Util;

    /// <summary>
    /// Правило создания из основания предостережения документа Предостережения
    /// </summary>
    public class TatGjiWarningToWarningDocRule : IInspectionGjiRule
    {
        public IWindsorContainer Container { get; set; }

        public IDisposalText DisposalTextService { get; set; }

        public IDomainService<WarningDoc> WarningDocDomain { get; set; }

        public IDomainService<DocumentGjiChildren> ChildrenDomain { get; set; }

        public IDomainService<Disposal> DisposalDomain { get; set; }

        public IDomainService<ActCheck> ActCheckDomain { get; set; }

        public IDomainService<InspectionGjiStage> InspectionStageDomain { get; set; }

        public IDomainService<InspectionGjiInspector> InspectionInspectorDomain { get; set; }

        public IDomainService<DocumentGjiInspector> DocumentInspectorDomain { get; set; }

        public IDomainService<InspectionGjiRealityObject> InspectionGjiRealityObjectDomain { get; set; }

        public IDomainService<WarningDocRealObj> WarningDocRoDomain { get; set; }

        public IDomainService<WarningDocViolations> WarningDocViolDomain { get; set; }

        public string CodeRegion => "Tat";

        public string Id => "GjiWarningToWarningDocRule";

        public string Description => "Правило создания из основания предостережения документа Предостережения";

        public string ActionUrl => "B4.controller.WarningDoc";

        public string ResultName => "Предостережение";

        public TypeBase TypeInspectionInitiator => TypeBase.GjiWarning;

        public TypeDocumentGji TypeDocumentResult => TypeDocumentGji.WarningDoc;

        private long[] RealityObjectIds { get; set; }

        // тут надо принять параметры если таковые имеютя
        public void SetParams(BaseParams baseParams)
        {
            // В данном методе принимаем параметр Id выбранных пользователем домов
            var realityIds = baseParams.Params.GetAs("realityIds", string.Empty).ToLongArray();

            if (realityIds.Length == 0)
            {
                throw new Exception("Необходимо выбрать дома");
            }

            this.RealityObjectIds = realityIds;
        }

        public virtual IDataResult CreateDocument(InspectionGji inspection)
        {
            #region Формируем распоряжение распоряжение
            var documentGji = new WarningDoc
            {
                Inspection = inspection,
                TypeDocumentGji = TypeDocumentGji.WarningDoc,
                DocumentDate = DateTime.Today
            };
            #endregion

            #region Формируем этап проверки
            var currentStage = this.InspectionStageDomain.GetAll()
                .FirstOrDefault(x => x.Inspection.Id == inspection.Id && x.TypeStage == TypeStage.WarningDoc);

            InspectionGjiStage newStage = null;

            if (currentStage == null || inspection.TypeBase == TypeBase.GjiWarning)
            {
                // Если этап не найден, то создаем новый этап
                var stageMaxPosition = this.InspectionStageDomain.GetAll()
                    .Where(x => x.Inspection.Id == inspection.Id)
                    .Select(x => (int?)x.Position)
                    .Max() ?? 0;

                // Создаем Этап Проверки (Который показывается слева в дереве)
                currentStage = new InspectionGjiStage
                {
                    Inspection = inspection,
                    Position = stageMaxPosition + 1,
                    TypeStage = TypeStage.WarningDoc
                };

                newStage = currentStage;
            }

            documentGji.Stage = currentStage;

            #endregion

            #region Забираем инспекторов из основания и переносим в Распоряжение

            var listInspectors = new List<DocumentGjiInspector>();
            var inspectorIds = this.InspectionInspectorDomain.GetAll().Where(x => x.Inspection.Id == inspection.Id)
                .Select(x => x.Inspector.Id).Distinct().ToList();

            foreach (var inspector in inspectorIds)
            {
                var newInspector = new DocumentGjiInspector
                {
                    DocumentGji = documentGji,
                    Inspector = new Inspector { Id = inspector }
                };

                listInspectors.Add(newInspector);

            }

            #endregion

            #region Формируем дома акта и формируем нарушение требования

            // Поскольку пользователи сами выбирают дома то их и переносим
            var listRo = new List<WarningDocRealObj>();
            var warDocViolList = new List<WarningDocViolations>();

            foreach (var id in this.RealityObjectIds)
            {
                var realityObject = new RealityObject() { Id = id };
                
                var warningDocRo = new WarningDocRealObj
                {
                    WarningDoc = documentGji,
                    RealityObject = realityObject
                };
                
                var warDocViol = new WarningDocViolations()
                {
                    WarningDoc = documentGji,
                    RealityObject = realityObject
                };

                listRo.Add(warningDocRo);
                warDocViolList.Add(warDocViol);
            }
            #endregion

            #region Сохранение
            using (var tr = this.Container.Resolve<IDataTransaction>())
            {
                try
                {
                    if (newStage != null)
                    {
                        this.InspectionStageDomain.Save(newStage);
                    }

                    this.WarningDocDomain.Save(documentGji);

                    listInspectors.ForEach(x => this.DocumentInspectorDomain.Save(x));

                    listRo.ForEach(x => this.WarningDocRoDomain.Save(x));
                    
                    warDocViolList.ForEach(x => this.WarningDocViolDomain.Save(x));

                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                    throw;
                }
            }
            #endregion

            return new BaseDataResult(new { documentId = documentGji.Id, inspectionId = inspection.Id });
        }

        public IDataResult ValidationRule(InspectionGji inspection)
        {
            if (inspection != null)
            {
                if (this.ActCheckDomain.GetAll().Any(x => x.Inspection.Id == inspection.Id))
                {
                    return BaseDataResult.Error("");
                }

                // смотрим если созданные предострежения из акта без взаимодействия
                if (this.ChildrenDomain.GetAll().Any(x => x.Children.Inspection.Id == inspection.Id && x.Parent.TypeDocumentGji == TypeDocumentGji.ActIsolated))
                {
                    return BaseDataResult.Error("");
                }

                if (this.WarningDocDomain.GetAll().Any(x => x.Inspection.Id == inspection.Id))
                {
                    return BaseDataResult.Error("По данному основанию уже создано предостережение");
                }

                if (!this.InspectionGjiRealityObjectDomain.GetAll().Any(x => x.Inspection.Id == inspection.Id))
                {
                    return BaseDataResult.Error("Отсутствуют проверяемые дома");
                }
            }

            return new BaseDataResult();
        }
    }
}
