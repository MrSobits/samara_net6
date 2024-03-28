namespace Bars.Gkh.Qa.Steps
{
    using System;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Qa.Utils;
    using TechTalk.SpecFlow;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4.Controller.Provider;
    using Bars.Gkh.Extensions;
    using Bars.Gkh.Utils;
    using FluentAssertions;
    using NHibernate.Mapping;
    using NHibernate.Util;
    using Bars.GkhGji.Entities;

    [Binding]
    public class AddCheckReportToOrderSteps : BindingBase
    {
        private BaseParams bParams = new BaseParams {Params = new DynamicDictionary()};
        private BaseParams actCheckParams = new BaseParams {Params = new DynamicDictionary()};
        
        private dynamic actDomainService
        {
            get
            {
                Type type = Type.GetType("Bars.GkhGji.Entities.ActCheck, Bars.GkhGji");
                var genericDS = typeof(IDomainService<>).MakeGenericType(type);
                return Container.Resolve(genericDS);
            }
        }

        [Given(@"добавлено это нарушение к этой группе нарушений")]
        public void ДопустимДобавленоЭтоНарушениеКЭтойГруппеНарушений()
        {
            var baseParams = new BaseParams
            {
                Params =
                    new DynamicDictionary
                    {
                        {"featureId", FeatureViolGjiHelper.Current.Id},
                        {"violationIds", ViolationsGjiHelper.Current.Id}
                    }
            };

            Type type =
                Type.GetType("Bars.GkhGji.DomainService.ViolationFeatureGjiService, Bars.GkhGji")
                    .GetInterface("IViolationFeatureGjiService");
            
            dynamic act = ((dynamic)Container.Resolve(type)).AddFeatureViolations(baseParams);
        }
        
        [Given(@"пользователь у этого приказа формирует акт проверки")]
        public void ДопустимПользовательУЭтогоПриказаФормируетАктПроверки()
        {
            var baseParams = new BaseParams
            {
                Params =
                    new DynamicDictionary
                    {
                        {"ruleId", "DisposalToActCheckWithoutRoRule"},
                        {"parentId", ScenarioContext.Current["prikazId"]},
                        {"actionUrl", "B4.controller.ActCheck"}
                    }
            };

            Type type =
                Type.GetType("Bars.GkhGji.InspectionRules.InspectionGjiProvider, Bars.GkhGji")
                    .GetInterface("IInspectionGjiProvider");

            var act = ((dynamic)Container.Resolve(type)).CreateDocument(baseParams);

            ScenarioContext.Current["documentId"] =
                act.Data.GetType().GetProperty("documentId").GetValue(act.Data);

        }
        
        [Given(@"пользователь у этого акта проверки заполняет поле Дата ""(.*)""")]
        public void ДопустимПользовательУЭтогоАктаПроверкиЗаполняетПолеДата(string data)
        {
            bParams.Params.Add("DocumentDate", data);
        }
        
        [Given(@"пользователь у этого акта проверки заполняет поле С копией приказа ознакомлен ""(.*)""")]
        public void ДопустимПользовательУЭтогоАктаПроверкиЗаполняетПолеСКопиейПриказаОзнакомлен(string acquainted)
        {
            bParams.Params.Add("AcquaintedWithDisposalCopy", acquainted);
        }
        
        [Given(@"пользователь у этого акта проверки заполняет поле Площадь ""(.*)""")]
        public void ДопустимПользовательУЭтогоАктаПроверкиЗаполняетПолеПлощадь(string area)
        {
            bParams.Params.Add("Area", area);
        }
        
        [When(@"пользователь сохраняет этот акт проверки")]
        public void ЕслиПользовательСохраняетЭтотАктПроверки()
        {
            bParams.Params.Add("Id", ScenarioContext.Current["documentId"]);
            var act = actDomainService.Update(bParams);
        }
        
        [When(@"пользователь в редактирует результат проверки")]
        public void ЕслиПользовательВРедактируетРезультатПроверки()
        {
            BaseParams geetingActObjectId = new BaseParams
            {
                Params = new DynamicDictionary {{"documentId", ScenarioContext.Current["documentId"]}}
            };

            Type type = Type.GetType("Bars.GkhGji.Entities.ActCheckRealityObject, Bars.GkhGji");

            dynamic actCheckRealityObjectViewModel = Container.Resolve(typeof(IViewModel<>).MakeGenericType(type));
            dynamic actCheckRealityObjectDomainService =
                Container.Resolve(typeof (IDomainService<>).MakeGenericType(type));

            var actRO = actCheckRealityObjectViewModel.List(actCheckRealityObjectDomainService, geetingActObjectId).Data;
            ScenarioContext.Current["actROId"] = ((IList)actRO)[0].GetType().GetProperty("Id").GetValue(((IList)actRO)[0]);

            actCheckParams.Params.Add("actObjectId", ScenarioContext.Current["actROId"]);
        }
        
        [When(@"пользователь у этого результата проверки заполняет поле Нарушения выявлены ""(.*)""")]
        public void ЕслиПользовательУЭтогоРезультатаПроверкиЗаполняетПолеНарушенияВыявлены(string p0)
        {
            if (p0 == "да")
            {
                actCheckParams.Params.Add("haveViolation", "10");
            }
        }
        
        [When(@"пользователь сохраняет этот результат проверки")]
        public void ЕслиПользовательСохраняетЭтотРезультатПроверки()
        {
            var controllerProvider = new ControllerProvider(Container);

            dynamic controller = controllerProvider.GetController(Container, "ActCheckRealityObject");

            try
            {
                controller.SaveParams(actCheckParams);

            }
            catch (Exception ex)
            {
                
                throw new Exception();
            }
        }
        
        [When(@"пользователь у этого результата проверки добавляет это нарушение из этой группы нарушений")]
        public void ЕслиПользовательУЭтогоРезультатаПроверкиДобавляетЭтоНарушениеИзЭтойГруппыНарушений()
        {
            actCheckParams.Params.Add("actViolationJson",
                new[]
                {
                    new DynamicDictionary
                    {
                        {"ViolationGjiId", ViolationsGjiHelper.Current.Id}
                    }
                });
        }
        
        [Then(@"у этой плановой проверки юридических лиц присутствует акт проверки в списке разделов")]
        public void ТоУЭтойПлановойПроверкиЮридическихЛицПрисутствуетАктПроверкиВСпискеРазделов()
        {
            ((Object)actDomainService.Get(ScenarioContext.Current["documentId"])).Should()
                .NotBeNull(
                    string.Format(
                        "запись по этому акту должна присутствовать{0}",
                        ExceptionHelper.GetExceptions()));
        }
        
        [Then(@"у этого акта проверки запись по результату проверки отсутствует в списке результатов проверки")]
        public void ТоУЭтогоАктаПроверкиЗаписьПоРезультатуПроверкиОтсутствуетВСпискеРезультатовПроверки()
        {
            var actCheckRO =
                Container.Resolve<IDomainService<ActCheckRealityObject>>()
                    .GetAll()
                    .Where(x => x.ActCheck.Id == (long)ScenarioContext.Current["documentId"]).ToList();


            actCheckRO.Should()
                .BeNullOrEmpty(string.Format(
                    "запись по результату проверки должна отсутствовать в акте проверки{0}",
                    ExceptionHelper.GetExceptions()));
        }
        
        [Then(@"у этого акта проверки запись по результату проверки присутствует в списке результатов проверки")]
        public void ТоУЭтогоАктаПроверкиЗаписьПоРезультатуПроверкиПрисутствуетВСпискеРезультатовПроверки()
        {
            var actCheckRO =
                Container.Resolve<IDomainService<ActCheckRealityObject>>()
                    .GetAll()
                    .Where(x => x.ActCheck.Id == (long) ScenarioContext.Current["documentId"]).ToList();


            actCheckRO.Should()
                .NotBeNullOrEmpty(string.Format(
                    "запись по результату проверки должна присутствовать в акте проверки{0}",
                    ExceptionHelper.GetExceptions()));
        }
    }
}
