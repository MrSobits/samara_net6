﻿namespace Bars.GkhGji.Regions.Nso.InspectionRules
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.InspectionRules;
    using Bars.GkhGji.Regions.Nso.Entities;

    using Castle.Windsor;

    /// <summary>
    /// Правило создание документа 'Акт проверки' из документа 'Распоряжение' (в случае если в проверке нет домов)
    /// Существуют проверки в которых нет домов соответсвенно и нарушения идут не на дома а на организации
    /// и данное правило создает акт бездомов
    /// </summary>
    public class DisposalToActCheckWithoutRoRule: IDocumentGjiRule
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
            get { return "DisposalToActCheckWithoutRoRule"; }
        }

        public virtual string Description
        {
            get { return "Правило создания документа 'Акт проверки' из документа 'Распоряжение' (если в основнии проверки нет домов)"; }
        }

        public virtual string ActionUrl
        {
            get { return "B4.controller.ActCheck"; }
        }

        public virtual string ResultName
        {
            get { return "Акт проверки"; }
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
            // не ожидаем ни каких параметров
        }

        public virtual IDataResult CreateDocument(DocumentGji document)
        {
            var ActCheckDomain = Container.Resolve<IDomainService<NsoActCheck>>();
            var InspectionStageDomain = Container.Resolve<IDomainService<InspectionGjiStage>>();
            var DocumentInspectorDomain = Container.Resolve<IDomainService<DocumentGjiInspector>>();
            var ActCheckRoDomain = Container.Resolve<IDomainService<ActCheckRealityObject>>();
            var ChildrenDomain = Container.Resolve<IDomainService<DocumentGjiChildren>>();
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
                // Поскольку в данном случае речь о доме не идет, то создаем фиктивную запись поскольку всеравно необходимо будет проставлять 
                // Ввыявлены или нет нарушения

                var actRo = new ActCheckRealityObject
                {
                    ActCheck = actCheck,
                    HaveViolation = YesNoNotSet.NotSet
                };

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

                        ActCheckRoDomain.Save(actRo);

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

                    if (InspectionRoDomain.GetAll().Any(x => x.Inspection.Id == document.Inspection.Id))
                    {
                        return new BaseDataResult(false, "Данный акт можно сформирвоать только для проверок без домов");
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
