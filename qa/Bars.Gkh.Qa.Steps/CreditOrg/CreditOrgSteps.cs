using System;
using System.Linq;
using Bars.B4;
using Bars.B4.Application;
using Bars.B4.Modules.FIAS;
using Bars.Gkh.Entities;
using Bars.Gkh.Qa.Utils;
using FluentAssertions;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Bars.Gkh.Qa.Steps
{
    [Binding]
    public class CreditOrgSteps : BindingBase
    {

        private dynamic DomainService
        {
            get
            {
                Type service = Type.GetType("Bars.Gkh.Overhaul.Entities.CreditOrg, Bars.Gkh.Overhaul");

                if (service != null)
                {
                    return Container.Resolve(typeof(IDomainService<>).MakeGenericType(service));
                }
                throw new Exception("не найден тип в Bats.Gkh.Overhaul");
            }
        }

        [Given(@"пользователь добавляет новую кредитную организацию")]
        public void ДопустимПользовательДобавляетНовуюКредитнуюОрганизацию()
        {
            CreditOrgHelper.Current = Activator.CreateInstance("Bars.Gkh.Overhaul", "Bars.Gkh.Overhaul.Entities.CreditOrg").Unwrap();
        }
        
        [Given(@"пользователь у этой кредитной организации заполняет поле Наименование ""(.*)""")]
        public void ДопустимПользовательУЭтойКредитнойОрганизацииЗаполняетПолеНаименование(string name)
        {
            CreditOrgHelper.Current.Name = name;
        }
        
        [Given(@"пользователь у этой кредитной организации заполняет поле ИНН ""(.*)""")]
        public void ДопустимПользовательУЭтойКредитнойОрганизацииЗаполняетПолеИНН(string inn)
        {
            CreditOrgHelper.Current.Inn = inn;
        }

        [Given(@"пользователь выбирает Адрес")]
        public void ДопустимПользовательВыбираетАдрес(Table table)
        {
            var context = (GkhContext)ApplicationContext.Current;

            var address = table.Rows.FirstOrDefault(x => x["region"] == context.Region);

            var fiasRepository = Container.Resolve<IFiasRepository>();

            if (address == null)
            {
                Assert.Ignore("Нет адреса для указанного региона");
            }

            //Получаем населенные пункты по фильтру
            var cityList = fiasRepository.GetPlacesDinamicAddress(address["city"].Split(null).Last());

            if (!cityList.Any())
            {
                throw new SpecFlowException(string.Format("нет фиаса по населёному пункту {0}", address["city"]));
            }

            //Получаем улицы
            var streetList = fiasRepository.GetStreetsDinamicAddress(address["street"], cityList.FirstOrDefault().GuidId);

            if (!streetList.Any())
            {
                throw new SpecFlowException(string.Format("нет фиаса по улице {0}", address["street"]));
            }

            var fiasAddress = new FiasAddress
            {
                AddressName =
                    cityList.First().Name + " " + streetList.First().Name + " "
                    + address["houseNumber"],
                AddressGuid = cityList.First().AddressGuid,
                PlaceName = cityList.First().Name,
                StreetName = streetList.First().Name,
                House = address["houseNumber"]
            };

            ScenarioContext.Current["fiasAddress"] = fiasAddress;
        }

        [Given(@"пользователь у этой кредитной организации заполняет поле Адрес в пределах субъекта")]
        public void ДопустимПользовательУЭтойКредитнойОрганизацииЗаполняетПолеАдресВПределахСубъекта()
        {
            CreditOrgHelper.Current.FiasAddress = (FiasAddress)ScenarioContext.Current["fiasAddress"];
        }


        [When(@"пользователь сохраняет эту кредитную организацию")]
        public void ЕслиПользовательСохраняетЭтуКредитнуюОрганизацию()
        {
            try
            {
                 if (CreditOrgHelper.Current.Id == 0)
                 {
                     DomainService.Save(CreditOrgHelper.Current);
                 }
                 else
                 {
                     DomainService.Update(CreditOrgHelper.Current);
                 }
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
           
        }
        
        [When(@"пользователь удаляет эту кредитную организацию")]
        public void ЕслиПользовательУдаляетЭтуКредитнуюОрганизацию()
        {
            try
            {
                DomainService.Delete(CreditOrgHelper.Current.Id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [Then(@"запись по этой кредитной организации присутствует в списке")]
        public void ТоЗаписьПоЭтойКредитнойОрганизацииПрисутствуетВСписке()
        {
            ( (object)DomainService.Get(CreditOrgHelper.Current.Id)).Should().NotBeNull(string.Format("кредитная организация должна присутствовать в списке.{0}", ExceptionHelper.GetExceptions()));
        }
        
        [Then(@"запись по этой кредитной организации отсутствует в списке")]
        public void ТоЗаписьПоЭтойКредитнойОрганизацииОтсутствуетВСписке()
        {
            ( (object)DomainService.Get(CreditOrgHelper.Current.Id)).Should().BeNull(string.Format("кредитная организация должна отсутствовать в списке.{0}", ExceptionHelper.GetExceptions()));

        }

        [Given(@"пользователь у этой кредитной организации заполняет поле Наименование (.*) знаков '(.*)'")]
        public void ДопустимПользовательУЭтойКредитнойОрганизацииЗаполняетПолеНаименованиеЗнаков(int count, char ch)
        {
            CreditOrgHelper.Current.Name = new string(ch, count);
        }

        [Given(@"пользователь у этой кредитной организации заполняет поле БИК (.*) знаков '(.*)'")]
        public void ДопустимПользовательУЭтойКредитнойОрганизацииЗаполняетПолеБИКЗнаков(int count, char ch)
        {
            CreditOrgHelper.Current.Bik = new string(ch, count);
        }

    }
}
