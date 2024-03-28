namespace Bars.GkhGji.InspectionRules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Contracts;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Rules;

    using Castle.Windsor;

    /// <summary>
    /// Правило создания документа профвизит из основания проверки по обращению
    /// </summary>
    public class BaseStatementToPreventiveVisitRule : IInspectionGjiRule
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<PreventiveVisit> PreventiveVisitDomain { get; set; }

        public IDomainService<InspectionGjiStage> InspectionStageDomain { get; set; }

        public IDomainService<InspectionGjiInspector> InspectionInspectorDomain { get; set; }

        public IDomainService<DocumentGjiInspector> DocumentInspectorDomain { get; set; }
        public IDomainService<PreventiveVisitRealityObject> PreventiveVisitRealityObjectDomain { get; set; }
        public IDomainService<InspectionAppealCits> StatementDomain { get; set; }
        public IDomainService<InspectionGjiRealityObject> InspectionGjiRealityObjectDomain { get; set; }

        public string CodeRegion
        {
            get { return "Tat"; }
        }

        public string Id
        {
            get { return "BaseStatementToPreventiveVisitRule"; }
        }

        public string Description
        {
            get { return "Правило создание документа профвизит из основания проверки по обращению"; }
        }

        public string ActionUrl
        {
            get { return "B4.controller.PreventiveVisit"; }
        }

        public string ResultName
        {
            get { return "Профвизит"; }
        }

        public TypeBase TypeInspectionInitiator
        {
            get { return TypeBase.CitizenStatement; }
        }

        public TypeDocumentGji TypeDocumentResult
        {
            get { return TypeDocumentGji.PreventiveVisit; }
        }

        // тут надо принять параметры если таковые имеютя
        public void SetParams(BaseParams baseParams)
        {
            // обработка пользовательских параметров не требуется
        }

        public virtual IDataResult CreateDocument(InspectionGji inspection)
        {
            var smevRule = Container.ResolveAll<ISMEVRule>();

            try
            {
                var rule = smevRule.FirstOrDefault(x => x.Id == "BaseSMEVInspectionRule");
                if (rule != null)
                {
                    rule.SendRequests(inspection);
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
            finally
            {
                Container.Release(smevRule);
            }
            #region Формируем Профвизит
            var actpv = new PreventiveVisit()
            {
                Inspection = inspection,
                TypeDocumentGji = TypeDocumentGji.PreventiveVisit,
                Contragent = inspection.Contragent,
                PersonInspection = inspection.PersonInspection,
                PhysicalPerson = inspection.PhysicalPerson,
                PhysicalPersonInfo = inspection.PhysicalPersonInfo,
            };
            #endregion

            List<PreventiveVisitRealityObject> rosList = new List<PreventiveVisitRealityObject>();

            InspectionGjiRealityObjectDomain.GetAll()
               .Where(x => x.Inspection == inspection).Select(x => x.RealityObject.Id).ToList().ForEach(x =>
               {
                   rosList.Add(new PreventiveVisitRealityObject
                   {
                       PreventiveVisit = actpv,
                       RealityObject = new RealityObject { Id = x }
                   });
               });

            #region Формируем этап проверки
            var stageMaxPosition = InspectionStageDomain.GetAll()
                    .Where(x => x.Inspection.Id == inspection.Id)
                    .OrderByDescending(x => x.Position)
                    .FirstOrDefault();

            // Создаем Этап Проверки (Который показывается слева в дереве)
            var stage = new InspectionGjiStage
            {
                Inspection = inspection,
                Position = stageMaxPosition != null ? stageMaxPosition.Position + 1 : 1,
                TypeStage = TypeStage.PreventiveVisit
            };

            actpv.Stage = stage;
            #endregion           

            #region Забираем инспекторов из основания и переносим в Распоряжение

            var listInspectors = new List<DocumentGjiInspector>();
            var inspectorIds = InspectionInspectorDomain.GetAll().Where(x => x.Inspection.Id == inspection.Id)
                .Select(x => x.Inspector.Id).Distinct().ToList();

            foreach (var inspector in inspectorIds)
            {
                var newInspector = new DocumentGjiInspector
                {
                    DocumentGji = actpv,
                    Inspector = new Inspector { Id = inspector }
                };

                listInspectors.Add(newInspector);

            }

            inspectorIds.AddRange(StatementDomain.GetAll()
                             .Where(x => x.Inspection.Id == inspection.Id && x.AppealCits.Tester != null)
                             .Select(x => x.AppealCits.Tester.Id)
                             .AsEnumerable().Distinct().ToArray());

            #endregion

            #region Сохранение
            using (var tr = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    this.InspectionStageDomain.Save(stage);

                    this.PreventiveVisitDomain.Save(actpv);

                    listInspectors.ForEach(x => this.DocumentInspectorDomain.Save(x));
                    rosList.ForEach(x => this.PreventiveVisitRealityObjectDomain.Save(x));

                    tr.Commit();
                }
                catch(Exception e)
                {
                    tr.Rollback();
                    throw;
                }
            }
            #endregion

            return new BaseDataResult(new { documentId = actpv.Id, inspectionId = inspection.Id });
        }
          

        public IDataResult ValidationRule(InspectionGji inspection)
        {
            /*
             тут проверяем, если у проверки уже есть дкоумент Распоряжение, то нельзя больше создавать
            */
            if (inspection != null)
            {
                if (this.PreventiveVisitDomain.GetAll().Any(x => x.Inspection.Id == inspection.Id))
                {
                    return new BaseDataResult(false, "По данной проверке уже создан профвизит");
                }
            }

            return new BaseDataResult();
        }
    }
}
