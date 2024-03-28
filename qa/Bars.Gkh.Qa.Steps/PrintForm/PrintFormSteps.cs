namespace Bars.Gkh.Qa.Steps
{
    using System;
    using System.Collections;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Modules.ReportPanel.Entities;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Qa.Utils;
    using FluentAssertions;
    using NUnit.Framework;
    using TechTalk.SpecFlow;

    [Binding]
    public class PrintFormSteps : BindingBase
    {
        private BaseParams BaseParams = new BaseParams {Params = new DynamicDictionary()};

        [When(@"пользователь в параметрах этого отчета заполняет поле Муниципальные образования ""(.*)""")]
        public void ЕслиПользовательВПараметрахЭтогоОтчетаЗаполняетПолеМуниципальныеОбразования(string mo)
        {
            if (mo == "Все МО")
            {
                BaseParams.Params.Add("muIds", null);
            }
            else
            {
                var result = Container.Resolve<IMunicipalityService>().ListByParamPaging(new BaseParams());

                if (result == null)
                {
                    ExceptionHelper.TestExceptions.Add("IMunicipalityService.ListByParamPaging", "возвратил null");
                }

                var currentMo = ((IList)result.Data).Cast<object>().FirstOrDefault(x => x.GetType().GetProperty("Name").GetValue(x).ToString() == mo);

                if (currentMo == null)
                {
                    throw new Exception("не найден МО "+ mo);
                }

                BaseParams.Params["muIds"] = currentMo.GetType().GetProperty("Id").GetValue(currentMo).ToString();
            }
        }
        
        [When(@"пользователь в параметрах этого отчета заполняет поле Адреса ""(.*)""")]
        public void ЕслиПользовательВПараметрахЭтогоОтчетаЗаполняетПолеАдреса(string address)
        {
            if (address == "Все адреса")
            {
                BaseParams.Params.Add("roIds", null);
            }
            else
            {
                var bp = new BaseParams
                {
                    Params = new DynamicDictionary
                    {
                        {"settlementId", BaseParams.Params["muIds"] ?? 0}
                    }
                };

                var result = Container.Resolve<IRealityObjectService>().ListByMoSettlement(bp); ;

                if (result == null)
                {
                    ExceptionHelper.TestExceptions.Add("IRealityObjectService.ListByMoSettlement", "возвратил null");
                }

                var currentAddress = ((IList)result.Data).Cast<object>().FirstOrDefault(x => x.GetType().GetProperty("Name").GetValue(x).ToString() == address);

                if (currentAddress == null)
                {
                    throw new Exception("не найден адрес " + address);
                }

                BaseParams.Params["roIds"] = currentAddress.GetType().GetProperty("Id").GetValue(currentAddress).ToString();
            }
        }
        
        [When(@"пользователь в параметрах этого отчета заполняет поле Тип протокола ""(.*)""")]
        public void ЕслиПользовательВПараметрахЭтогоОтчетаЗаполняетПолеТипПротокола(string decisionType)
        {
            switch (decisionType)
            {
                case "Протокол решений собственников":
                    BaseParams.Params.Add("decisionType", 1);
                    break;
                    case "Протокол органов гос. власти":
                    BaseParams.Params.Add("decisionType", 2);
                    break;
                    case "Нет протокола":
                    BaseParams.Params.Add("decisionType", 3);
                    break;
                    default:
                    throw new SpecFlowException("нет типа протокола - " + decisionType);
            }
        }

        [Then(@"происходит вызов печати отчета ""(.*)""")]
        public void ТоПроисходитВызовПечатиОтчета(string printFormName)
        {
            PrintForm printForm = Container.Resolve<IDomainService<PrintForm>>().GetAll().FirstOrDefault(x => x.Name == printFormName);

            if (printForm == null)
            {
                throw new SpecFlowException(string.Format("Отсутствует отчёт с наименованием {0}. {1}", printFormName, ExceptionHelper.GetExceptions()));
            }

            BaseParams.Params.Add("id", printForm.Id);

            var result =
                Container.Resolve<IPrintFormService>().GetPrintFormResult(BaseParams);

            result.Success.Should()
                .BeTrue(
                    string.Format(
                        "При выполнении печати отчёта выпала ошибка {0}. {1}",
                        result.Message,
                        ExceptionHelper.GetExceptions()));
        }
    }
}
