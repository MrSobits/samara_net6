namespace Bars.GkhGji.Regions.Nso.InspectionRules
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
    using Bars.GkhGji.Regions.Nso.Entities;

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
            var ActCheckDomain = Container.Resolve<IDomainService<NsoActCheck>>();
            var InspectionStageDomain = Container.Resolve<IDomainService<InspectionGjiStage>>();
            var DocumentInspectorDomain = Container.Resolve<IDomainService<DocumentGjiInspector>>();
            var ActCheckRoDomain = Container.Resolve<IDomainService<ActCheckRealityObject>>();
            var ChildrenDomain = Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var RoDomain = Container.Resolve<IDomainService<RealityObject>>();
            var DisposalProvDocDomain = Container.Resolve<IDomainService<DisposalProvidedDoc>>();
            var ActCheckProvDocDomain = Container.Resolve<IDomainService<ActCheckProvidedDoc>>();
           
            try
            {
                #region Формируем Акт Проверки
                var actCheck = new NsoActCheck
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

                foreach (var id in RealityObjectIds)
                {
                    var actRo = new ActCheckRealityObject
                    {
                        ActCheck = actCheck,
                        RealityObject = new RealityObject { Id = id },
                        HaveViolation = YesNoNotSet.NotSet
                    };

                    listRo.Add(actRo);
                }

                actCheck.Area = RoDomain.GetAll().Where(x => RealityObjectIds.Contains(x.Id)).Sum(x => x.AreaMkd);
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
                using (var tr = Container.Resolve<IDataTransaction>())
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
                Container.Release(ActCheckDomain);
                Container.Release(InspectionStageDomain);
                Container.Release(DocumentInspectorDomain);
                Container.Release(ActCheckRoDomain);
                Container.Release(ChildrenDomain);
                Container.Release(RoDomain);
                Container.Release(DisposalProvDocDomain);
                Container.Release(ActCheckProvDocDomain);
            }
        }

        public virtual IDataResult ValidationRule(DocumentGji document)
        {
            /*
             тут проверяем, Если Распоряжение не Основное то недаем выполнить формирование
            */

            var DisposalDomain = Container.Resolve<IDomainService<Disposal>>();
            var InspectionRoDomain = Container.Resolve<IDomainService<InspectionGjiRealityObject>>();

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
                Container.Release(DisposalDomain);
                Container.Release(InspectionRoDomain);
            }

        }

        private string GetPlaceDocument(InspectionGji inspection)
        {
            if (inspection == null)
            {
                return string.Empty;
            }

            var InspectionRoDomain = Container.Resolve<IDomainService<InspectionGjiRealityObject>>();
            var BaseJurPersonDomain = Container.Resolve<IDomainService<BaseJurPerson>>();
            var BaseStatementDomain = Container.Resolve<IDomainService<BaseStatement>>();
            var BaseProsClaimDomain = Container.Resolve<IDomainService<BaseProsClaim>>();
            var BaseDispHeadDomain = Container.Resolve<IDomainService<BaseDispHead>>();

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
                Container.Release(InspectionRoDomain);
                Container.Release(BaseJurPersonDomain);
                Container.Release(BaseStatementDomain);
                Container.Release(BaseProsClaimDomain);
                Container.Release(BaseDispHeadDomain);
            }

            return string.Empty;
        }
    }
}
