namespace Bars.Gkh.Qa.Steps
{
    using FluentAssertions;
    using System.Collections;
    using NHibernate.Util;
    using System;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Qa.Utils;
    using TechTalk.SpecFlow;

    [Binding]
    public class AddOrderToBaseJurPersonSteps : BindingBase
    {
        private dynamic DomainService
        {
            get
            {
                Type type = Type.GetType("Bars.GkhGji.Entities.Disposal, Bars.GkhGji");
                var genericDS = typeof(IDomainService<>).MakeGenericType(type);
                return Container.Resolve(genericDS);
            }
        }

        private BaseParams paramsDictionary = new BaseParams { Params = new DynamicDictionary()};

        [Given(@"пользователь у этой плановой проверки юридических лиц формирует приказ")]
        public void ДопустимПользовательУЭтойПлановойПроверкиЮридическихЛицФормируетПриказ()
        {
            var baseParams = new BaseParams
            {
                Params = new DynamicDictionary()
            };

            baseParams.Params.Add("parentId", BaseJurPersonHelper.Current.Id);
            baseParams.Params.Add("ruleId", "BaseJurPersonToDisposalRule");
            baseParams.Params.Add("actionUrl", "B4.controller.Disposal");

            Type interFace = Type.GetType("Bars.GkhGji.InspectionRules.InspectionGjiProvider, Bars.GkhGji").GetInterface("IInspectionGjiProvider");
            dynamic ds = Container.Resolve(interFace);
            BaseDataResult ids = ds.CreateDocument(baseParams);

            if (!ids.Success)
            {
                ExceptionHelper.AddException("IInspectionGjiProvider.CreateDocument", ids.Message);

                throw new SpecFlowException(ExceptionHelper.GetExceptions());
            }

            ScenarioContext.Current["inspectionId"] =
                ids.Data.GetType().GetProperty("documentId").GetValue(ids.Data).ToString();
            ScenarioContext.Current["documentId"] =
                ids.Data.GetType().GetProperty("documentId").GetValue(ids.Data).ToString();
            
        }
        
        [Given(@"пользователь у этого приказа заполняет поле Дата ""(.*)""")]
        public void ДопустимПользовательУЭтогоПриказаЗаполняетПолеДата(string date)
        {
            ScenarioContext.Current["date"] = DateTime.Today;
        }
        
        [Given(@"пользователь у этого приказа заполняет поле Период проведения проверки с ""(.*)""")]
        public void ДопустимПользовательУЭтогоПриказаЗаполняетПолеПериодПроведенияПроверкиС(string date)
        {
            ScenarioContext.Current["dateStart"] = DateTime.Today;
        }
        
        [Given(@"пользователь у этого приказа заполняет поле по ""(.*)""")]
        public void ДопустимПользовательУЭтогоПриказаЗаполняетПолеПо(string date)
        {
            ScenarioContext.Current["dateEnd"] = DateTime.Today;
        }
        
        [Given(@"пользователь у этого приказа заполняет поле ДЛ, вынесшее Приказ этим инспектором")]
        public void ДопустимПользовательУЭтогоПриказаЗаполняетПолеДЛВынесшееПриказЭтимИнспектором()
        {
            //ScenarioContext.Current["inspectionId"]
        }
        
        [When(@"пользователь сохраняет этот приказ")]
        public void ЕслиПользовательСохраняетЭтотПриказ()
        {
            paramsDictionary.Params.Add("id", ScenarioContext.Current["documentId"].ToLong());
            paramsDictionary.Params.Add("records",
                new[]
                {
                    new DynamicDictionary
                    {
                        {"DocumentNumber", ""},
                        {"IssuedDisposal", "1"},
                        {"DateStart", (DateTime)ScenarioContext.Current["dateStart"]},
                        {"DateEnd", (DateTime)ScenarioContext.Current["dateEnd"]},
                        {"KindCheck", "1"},
                        {"NcNum", ""},
                        {"NcNumLatter", ""},
                        {"MotivatedRequestNumber", ""},
                        {"PeriodCorrect", ""},
                        {"NoticePlaceCreation", ""},
                        {"NoticeDescription", ""},
                        {"ProsecutorDecNumber", ""},
                        {"DocumentDate", (DateTime)ScenarioContext.Current["date"]},
                        {"Id", ScenarioContext.Current["documentId"].ToLong()}
                    }
                });

            try
            {
                dynamic prikaz = ((IList)((BaseDataResult)DomainService.Update(paramsDictionary)).Data).First();
                ScenarioContext.Current["prikazId"] = prikaz.Id;
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [Then(@"у этой плановой проверки юридических лиц присутствует приказ в списке разделов")]
        public void ТоУЭтойПлановойПроверкиЮридическихЛицПрисутствуетПриказВСпискеРазделов()
        {
            ((Object)DomainService.Get(ScenarioContext.Current["prikazId"])).Should()
                .NotBeNull(
                    string.Format(
                        "запись по этому отчетному периоду должна присутствовать в разделе отчетных периодов{0}",
                        ExceptionHelper.GetExceptions()));
        }
    }
}
