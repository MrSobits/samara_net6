namespace Bars.GkhGji.Regions.BaseChelyabinsk.InspectionRules.Impl.DocumentRules
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
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActCheck;

    using Castle.Windsor;

    /// <summary>
    /// Правило создание документа 'Акт проверки' из документа 'Распоряжение' (по выбранным домам)
    /// </summary>
    public class DisposalToActCheckByRoRule : IDocumentGjiRule
    {
        #region injection
        public IWindsorContainer Container { get; set; }
        #endregion

        #region Внутренние переменные
        private long[] RealityObjectIds { get; set; }
        #endregion

        public virtual string CodeRegion
        {
            get { return "Tat"; }
        }

        public virtual string Id
        {
            get { return "DisposalToActCheckByRoRule"; }
        }

        public virtual string Description
        {
            get { return "Правило создание документа 'Акт проверки' из документа 'Распоряжение' (по выбранным домам)"; }
        }

        public virtual string ActionUrl
        {
            get { return "B4.controller.ActCheck"; }
        }

        public virtual string ResultName
        {
            get { return "Акт проверки (на 1 дом)"; }
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
            // В данном методе принимаем параметр Id выбранных пользователем домов
            var realityIds = baseParams.Params.GetAs("realityIds", "");

            this.RealityObjectIds = !string.IsNullOrEmpty(realityIds)
                                  ? realityIds.Split(',').Select(id => id.ToLong()).ToArray()
                                  : new long[0];

            if (this.RealityObjectIds.Length == 0)
            {
                throw new Exception("Необходимо выбрать дома");
            }
        }

        public virtual IDataResult CreateDocument(DocumentGji document)
        {
            var ActCheckDomain = this.Container.Resolve<IDomainService<ChelyabinskActCheck>>();
            var InspectionStageDomain = this.Container.Resolve<IDomainService<InspectionGjiStage>>();
            var DocumentInspectorDomain = this.Container.Resolve<IDomainService<DocumentGjiInspector>>();
            var ActCheckRoDomain = this.Container.Resolve<IDomainService<ActCheckRealityObject>>();
            var ChildrenDomain = this.Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var RoDomain = this.Container.Resolve<IDomainService<RealityObject>>();
            var DisposalProvDocDomain = this.Container.Resolve<IDomainService<DisposalProvidedDoc>>();
            var ActCheckProvDocDomain = this.Container.Resolve<IDomainService<ActCheckProvidedDoc>>();
           
            try
            {
                #region Формируем Акт Проверки
                var actCheck = new ChelyabinskActCheck
                {
                    Inspection = document.Inspection,
                    TypeActCheck = TypeActCheckGji.ActCheckIndividual,
                    TypeDocumentGji = TypeDocumentGji.ActCheck,
                    DocumentPlace = this.GetPlaceDocument(document.Inspection)
                };
                #endregion

                #region Формируем этап проверки
                var parentStage = document.Stage;
                if (parentStage != null && parentStage.Parent != null)
                {
                    parentStage = parentStage.Parent;
                }

                InspectionGjiStage newStage = null;
                var currentStage = InspectionStageDomain.GetAll()
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

                #region Формируем дома акта

                // Поскольку пользователи сами выбирают дома то их и переносим
                var listRo = new List<ActCheckRealityObject>();

                foreach (var id in this.RealityObjectIds)
                {
                    var actRo = new ActCheckRealityObject
                    {
                        ActCheck = actCheck,
                        RealityObject = new RealityObject { Id = id },
                        HaveViolation = YesNoNotSet.NotSet
                    };

                    listRo.Add(actRo);
                }

                actCheck.Area = RoDomain.GetAll().Where(x => this.RealityObjectIds.Contains(x.Id)).Sum(x => x.AreaMkd);
                #endregion

                #region Формируем Предосталвенные дкоументы
                var listDocsToSave = new List<ActCheckProvidedDoc>();
                var docIds = DisposalProvDocDomain.GetAll()
                    .Where(x => x.Disposal.Id == document.Id)
                    .Select(x => x.ProvidedDoc.Id)
                    .Distinct()
                    .ToList();

                foreach (var id in docIds)
                {
                    listDocsToSave.Add(new ActCheckProvidedDoc
                    {
                        ActCheck = actCheck,
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

                        ActCheckDomain.Save(actCheck);

                        ChildrenDomain.Save(parentChildren);

                        listInspectors.ForEach(DocumentInspectorDomain.Save);

                        listRo.ForEach(ActCheckRoDomain.Save);

                        listDocsToSave.ForEach(ActCheckProvDocDomain.Save);

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
            finally 
            {
                this.Container.Release(ActCheckDomain);
                this.Container.Release(InspectionStageDomain);
                this.Container.Release(DocumentInspectorDomain);
                this.Container.Release(ActCheckRoDomain);
                this.Container.Release(ChildrenDomain);
                this.Container.Release(RoDomain);
                this.Container.Release(DisposalProvDocDomain);
                this.Container.Release(ActCheckProvDocDomain);
            }
        }

        public virtual IDataResult ValidationRule(DocumentGji document)
        {
            /*
             тут проверяем, Если Распоряжение не Основное то недаем выполнить формирование
            */

            var DisposalDomain = this.Container.Resolve<IDomainService<Disposal>>();
            var InspectionRoDomain = this.Container.Resolve<IDomainService<InspectionGjiRealityObject>>();

            try
            {
                if (document != null)
                {
                    var disposal = DisposalDomain.FirstOrDefault(x => x.Id == document.Id);

                    if (disposal == null)
                    {
                        return new BaseDataResult(false, string.Format("Не удалось получить распоряжение {0}", document.Id));
                    }

                    if (disposal.TypeDisposal != TypeDisposalGji.Base)
                    {
                        return new BaseDataResult(false, "Акт проверки можно сформирвоать только из основного распоряжения");
                    }

                    if (!InspectionRoDomain.GetAll().Any(x => x.Inspection.Id == document.Inspection.Id))
                    {
                        return new BaseDataResult(false, "Данный акт можно сформирвоать только для проверок с домами");
                    }
                }

                return new BaseDataResult();
            }
            finally 
            {
                this.Container.Release(DisposalDomain);
                this.Container.Release(InspectionRoDomain);
            }

        }

        private string GetPlaceDocument(InspectionGji inspection)
        {
            if (inspection == null)
            {
                return string.Empty;
            }

            var InspectionRoDomain = this.Container.Resolve<IDomainService<InspectionGjiRealityObject>>();
            var BaseJurPersonDomain = this.Container.Resolve<IDomainService<BaseJurPerson>>();
            var BaseStatementDomain = this.Container.Resolve<IDomainService<BaseStatement>>();
            var BaseProsClaimDomain = this.Container.Resolve<IDomainService<BaseProsClaim>>();
            var BaseDispHeadDomain = this.Container.Resolve<IDomainService<BaseDispHead>>();

            try
            {
                var inspectionIsExit = false;
                if (inspection.TypeBase == TypeBase.PlanJuridicalPerson)
                {
                    var jurPerson = BaseJurPersonDomain.Get(inspection.Id);
                    if (jurPerson != null && jurPerson.TypeForm == TypeFormInspection.Exit)
                    {
                        inspectionIsExit = true;
                    }
                }
                else if (inspection.TypeBase == TypeBase.CitizenStatement)
                {
                    var baseStatement = BaseStatementDomain.Get(inspection.Id);
                    if (baseStatement != null && baseStatement.TypeForm == TypeFormInspection.Exit)
                    {
                        inspectionIsExit = true;
                    }
                }
                else if (inspection.TypeBase == TypeBase.ProsecutorsClaim)
                {
                    var prosClaim = BaseProsClaimDomain.Get(inspection.Id);
                    if (prosClaim != null && prosClaim.TypeForm == TypeFormInspection.Exit)
                    {
                        inspectionIsExit = true;
                    }
                }
                else if (inspection.TypeBase == TypeBase.DisposalHead)
                {
                    var disposalHead = BaseDispHeadDomain.Get(inspection.Id);
                    if (disposalHead != null && disposalHead.TypeForm == TypeFormInspection.Exit)
                    {
                        inspectionIsExit = true;
                    }
                }

                if (inspectionIsExit)
                {
                    var ro = InspectionRoDomain.GetAll()
                        .Where(x => x.Inspection.Id == inspection.Id)
                        .Select(x => x.RealityObject)
                        .FirstOrDefault();

                    if (ro != null)
                    {
                        return ro.Municipality.Name;
                    }
                }
                else
                {
                    return string.Empty;
                }
            }
            finally
            {
                this.Container.Release(InspectionRoDomain);
                this.Container.Release(BaseJurPersonDomain);
                this.Container.Release(BaseStatementDomain);
                this.Container.Release(BaseProsClaimDomain);
                this.Container.Release(BaseDispHeadDomain);
            }

            return string.Empty;
        }
    }
}
