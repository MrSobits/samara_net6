namespace Bars.GkhGji.Regions.Tula.InspectionRules
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
    using Castle.Windsor;

    /// <summary>
    /// в сахе нельзя создать приказ пока нет домов в доменте 
    /// </summary>
    public class TulaBasePlanActionToActCheckRule : IInspectionGjiRule
    {
        public IWindsorContainer Container { get; set; }

        public string CodeRegion
        {
            get { return "Tat"; }
        }

        public string Id
        {
            get { return "TulaBasePlanActionToActCheckRule"; }
        }

        public string Description
        {
            get { return "Правило создание документа Акт проверки из основания проверки по плану мероприятий"; }
        }

        public string ActionUrl
        {
            get { return "B4.controller.ActCheck"; }
        }

        public string ResultName
        {
            get { return "Акт проверки"; }
        }

        public TypeBase TypeInspectionInitiator
        {
            get { return TypeBase.PlanAction; }
        }

        public TypeDocumentGji TypeDocumentResult
        {
            get { return TypeDocumentGji.ActCheck; }
        }

        // тут надо принять параметры если таковые имеютя
        public void SetParams(BaseParams baseParams)
        {
        }

        public IDataResult ValidationRule(InspectionGji inspection)
        {
            var actCheckDomain = Container.ResolveDomain<ActCheck>();

            if (inspection != null)
            {
                if (actCheckDomain.GetAll().Any(x => x.Inspection.Id == inspection.Id))
                {
                    return new BaseDataResult(false, "По данной проверке уже создан акт проверки");
                }
            }

            return new BaseDataResult();
        }

        public  IDataResult CreateDocument(InspectionGji inspection)
        {
            var actCheckDomain = Container.ResolveDomain<ActCheck>();
            var inspectionStageDomain = Container.ResolveDomain<InspectionGjiStage>();
            var inspInspectorDomain = Container.ResolveDomain<InspectionGjiInspector>();
            var docInspectorDomain = Container.ResolveDomain<DocumentGjiInspector>();
            var actCheckRoDomain = Container.ResolveDomain<ActCheckRealityObject>();

            try
            {
                #region Формируем Акт Проверки

                var actCheck = new ActCheck
                {
                    Inspection = inspection,
                    TypeActCheck = TypeActCheckGji.ActCheckIndividual,
                    TypeDocumentGji = TypeDocumentGji.ActCheck
                };

                #endregion

                #region Формируем этап проверки

                var stageMaxPosition = inspectionStageDomain.GetAll()
                    .Where(x => x.Inspection.Id == inspection.Id)
                    .OrderByDescending(x => x.Position)
                    .FirstOrDefault();

                // Создаем Этап Проверки (Который показывается слева в дереве)
                var stage = new InspectionGjiStage
                {
                    Inspection = inspection,
                    Position = stageMaxPosition != null ? stageMaxPosition.Position + 1 : 1,
                    TypeStage = TypeStage.ActCheck
                };

                actCheck.Stage = stage;

                #endregion

                #region Формируем Инспекторов

                var listInspectors = new List<DocumentGjiInspector>();
                var inspectorIds = inspInspectorDomain.GetAll().Where(x => x.Inspection.Id == inspection.Id)
                    .Select(x => x.Inspector.Id).Distinct().ToList();

                foreach (var inspector in inspectorIds)
                {
                    var newInspector = new DocumentGjiInspector
                    {
                        DocumentGji = actCheck,
                        Inspector = new Inspector {Id = inspector}
                    };

                    listInspectors.Add(newInspector);

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

                #region Сохранение

                using (var tr = Container.Resolve<IDataTransaction>())
                {
                    try
                    {

                        inspectionStageDomain.Save(stage);

                        actCheckDomain.Save(actCheck);

                        listInspectors.ForEach(docInspectorDomain.Save);

                        actCheckRoDomain.Save(actRo);

                        tr.Commit();
                    }
                    catch
                    {
                        tr.Rollback();
                        throw;
                    }
                }

                #endregion

                return new BaseDataResult(new {documentId = actCheck.Id, inspectionId = inspection.Id});
            }
            finally
            {
                Container.Release(inspectionStageDomain);
                Container.Release(inspInspectorDomain);
                Container.Release(docInspectorDomain);
                Container.Release(actCheckRoDomain);
            }
        }
    }
}
