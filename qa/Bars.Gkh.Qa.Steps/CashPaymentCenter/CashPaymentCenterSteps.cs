namespace Bars.Gkh.Qa.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC.Lifestyles.SessionLifestyle;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Qa.Steps.CommonSteps;
    using Bars.Gkh.Qa.Utils;
    using Bars.Gkh.RegOperator.DomainService;
    using Bars.Gkh.RegOperator.Entities;
    using FluentAssertions;
    using NUnit.Framework;
    using TechTalk.SpecFlow;

    [Binding]
    public class CashPaymentCenterSteps : BindingBase
    {
        private IDomainService<CashPaymentCenter> DomainService = Container.Resolve<IDomainService<CashPaymentCenter>>();
        private BaseParams baseParams = new BaseParams();

        [Given(@"пользователь выбирает РКЦ у которого контрагент ""(.*)""")]
        public void ЕслиПользовательОткрываетРКЦ(string p0)
        {
            var cashPayment = DomainService.GetAll().ToList().FirstOrDefault(x => x.Contragent.Name == p0);
            
            cashPayment.Should().NotBeNull("Не найдена РКЦ, с именем контрагента = " + p0);

            CashPaymentCenterHelper.Current = cashPayment;
        }

        [Given(@"пользователь у этого РКЦ выбирает объект с номером ЛС ""(.*)""")]
        public void ДопустимПользовательУЭтогоРКЦВыбираетОбъектСНомеромЛС(string personalAccountNum)
        {
            var list = Container.Resolve<IDomainService<CashPaymentCenterPersAcc>>().GetAll();

            var cashPaymentCenterPersAcc = list.FirstOrDefault(
                x =>
                x.CashPaymentCenter.Id == CashPaymentCenterHelper.Current.Id
                && x.PersonalAccount.PersonalAccountNum == personalAccountNum);

            if (cashPaymentCenterPersAcc == null)
            {
                throw new SpecFlowException(string.Format("Отсутствует объект РКЦ с номером ЛС {0}", personalAccountNum));
            }

            CashPaymentCenterPersAccHelper.Current = cashPaymentCenterPersAcc;
        }

        
        [Given(@"пользователь в объектах РКЦ добавляет новый объект")]
        public void ЕслиПользовательВОбъектахРкцДобавляетНовыйОбъект()
        {
            baseParams.Params = new DynamicDictionary
            {
                {"isShowPersAcc", "true"},
                {"cashPaymentCenterId", CashPaymentCenterHelper.Current.Id}
            };
        }
        
        [Given(@"пользователь у этого объекта РКЦ заполняет поле Дата начала действия договора ""(.*)""")]
        public void ЕслиПользовательУЭтогоОбъектаЗаполняетПолеДатаНачалаДействияДоговора(string startDate)
        {
            baseParams.Params["dateStart"] = startDate.DateParse();
        }

        [Given(@"пользователь у этого объекта РКЦ заполняет поле Дата окончания действия договора ""(.*)""")]
        public void ЕслиПользовательУЭтогоОбъектаЗаполняетПолеДатаОкончанияДействияДоговора(string dateEnd)
        {
            baseParams.Params["dateEnd"] = dateEnd.DateParse();
        }

        [Given(@"пользователь у этого объекта РКЦ добавляет лицевой счет ""(.*)""")]
        public void ЕслиПользовательУЭтогоОбъектаДобавляетЛицевойСчет(string personalAccountNum)
        {
            var lsList =
                ((IEnumerable<object>)
                    (Container.Resolve<ICashPaymentCenterService>().ListObjForCashPaymentCenter(baseParams).Data));

            var ls = lsList.FirstOrDefault(
                    x => ReflectionHelper.GetPropertyValue<string>(x, "PersonalAccountNum") == personalAccountNum);

            if (ls == null )
            {
                throw new Exception("Не найден лицевой счет - " + personalAccountNum);
            }
            
            this.baseParams.Params["objectIds"] = new[] { ReflectionHelper.GetPropertyValue<long>(ls, "Id") };

            this.baseParams.Params["isPersAcc"] = true;

            ScenarioContext.Current["PersonalAccountNum"] = personalAccountNum;

            CashPaymentCenterPersAccHelper.Current = new CashPaymentCenterPersAcc();
        }

        [Given(@"пользователь добавляет новый РКЦ")]
        public void ДопустимПользовательДобавляетНовыйРкц()
        {
            CashPaymentCenterHelper.Current = new CashPaymentCenter();
        }

        [Given(@"пользователь у этого РКЦ заполняет поле Контрагент ""(.*)""")]
        public void ДопустимПользовательУЭтогоРкцЗаполняетПолеКонтрагент(string name)
        {
            var contragent = Container.Resolve<IDomainService<Contragent>>().FirstOrDefault(x => x.Name == name);

            if (contragent != null)
            {
                CashPaymentCenterHelper.Current.Contragent = contragent;
            }
            else
            {
                throw new Exception("Не найден контрагент " + name);
            }
        }

        [Given(@"пользователь у этого РКЦ заполняет поле Идентификатор РКЦ ""(.*)""")]
        public void ДопустимПользовательУЭтогоРкцЗаполняетПолеИдентификаторРКЦ(string identifier)
        {
            CashPaymentCenterHelper.Current.Identifier = identifier;
        }
        
        [When(@"пользователь сохраняет этот объект РКЦ")]
        public void ЕслиПользовательСохраняетЭтотОбъект()
        {
            var service = Container.Resolve<ICashPaymentCenterService>();

            if (CashPaymentCenterPersAccHelper.Current.Id != 0)
            {
                var paramsDic = baseParams.Params;

                if (paramsDic.ContainsKey("dateStart"))
                {
                    CashPaymentCenterPersAccHelper.Current.DateStart = paramsDic.GetAs<DateTime>("dateStart");
                }

                if (paramsDic.ContainsKey("dateEnd"))
                {
                    CashPaymentCenterPersAccHelper.Current.DateEnd = paramsDic.GetAs<DateTime>("dateEnd");
                }


                try
                {
                    Container.Resolve<IDomainService<CashPaymentCenterPersAcc>>()
                        .Update(CashPaymentCenterPersAccHelper.Current);
                }
                catch (Exception ex)
                {
                    ExceptionHelper.AddException("IDomainService<CashPaymentCenterPersAcc>.Update", ex.Message);
                }

                return;
            }

            try
            {
                var result = service.AddObjects(this.baseParams);

                if (result.Message
                    == "Некоторые счета имеют действующий договор. Для добавления нового договора необходимо закрыть прошлый.")
                {
                    Assert.Fail("Нужно удалить объект из РКЦ в ручную перед запуском теста");
                }

                if (!result.Success)
                {
                    ExceptionHelper.AddException("ICashPaymentCenterService.AddObjects", result.Message);
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.AddException("ICashPaymentCenterService.AddObjects", ex.Message);
            }
        }
        
        [When(@"пользователь у этого РКЦ удаляет объект с лс ""(.*)""")]
        public void ЕслиПользовательУЭтогоРКЦУдаляетОбъектСЛс(string personalAccountNum)
        {
            var cashPaymentCenterPerAcc =
                Container.Resolve<IDomainService<CashPaymentCenterPersAcc>>()
                    .GetAll()
                    .ToList()
                    .FirstOrDefault(
                        x =>
                        x.CashPaymentCenter == CashPaymentCenterHelper.Current
                        && x.PersonalAccount.PersonalAccountNum == personalAccountNum);

            if (cashPaymentCenterPerAcc == null)
            {
                throw new SpecFlowException(
                    string.Format(
                        "У расчётно кассового центра {0} отсутствует объект РКЦ с номером ЛС {1}",
                        CashPaymentCenterHelper.Current.Identifier,
                        personalAccountNum));
            }

            var cashPaymentCenterPerAccId = cashPaymentCenterPerAcc.Id;

            var service = Container.Resolve<ICashPaymentCenterService>();

            var session = Container.Resolve<ISessionProvider>().GetCurrentSession();

            ExplicitSessionScope.CallInNewScope(
                () =>
                    {
                        try
                        {
                            var res =
                                service.DeleteObjects(
                                    new BaseParams
                                        {
                                            Params =
                                                new DynamicDictionary
                                                    {
                                                        { "ids", cashPaymentCenterPerAccId }
                                                    }
                                        });

                            if (!res.Success)
                            {
                                ExceptionHelper.AddException("ICashPaymentCenterService.DeleteObjects", res.Message);

                                session.Clear();
                            }
                        }
                        catch (Exception ex)
                        {
                            ExceptionHelper.AddException("ICashPaymentCenterService.DeleteObjects", ex.Message);

                            session.Clear();
                        }
                    });
        }

        [When(@"пользователь удаляет этот объект")]
        public void ЕслиПользовательУдаляетЭтотОбъект()
        {
            var personalAccountNum = ScenarioContext.Current.Get<string>("PersonalAccountNum");

            var cashPaymentCenterPerAcc =
                Container.Resolve<IDomainService<CashPaymentCenterPersAcc>>()
                    .GetAll()
                    .ToList()
                    .FirstOrDefault(
                        x =>
                        x.CashPaymentCenter == CashPaymentCenterHelper.Current
                        && x.PersonalAccount.PersonalAccountNum == personalAccountNum);

            if (cashPaymentCenterPerAcc == null)
            {
                throw new SpecFlowException(
                    string.Format(
                        "У расчётно кассового центра {0} отсутствует объект РКЦ с номером ЛС {1}",
                        CashPaymentCenterHelper.Current.Identifier,
                        personalAccountNum));
            }

            var cashPaymentCenterPerAccId = cashPaymentCenterPerAcc.Id;

            var service = Container.Resolve<ICashPaymentCenterService>();
            var res = service.DeleteObjects(new BaseParams { Params = new DynamicDictionary { { "ids", cashPaymentCenterPerAccId } } });

            if (!res.Success)
            {
                ExceptionHelper.AddException("ICashPaymentCenterService.DeleteObjects", res.Message);
            }
        }

        [When(@"пользователь удаляет это муниципальное образование у этого РКЦ")]
        public void ЕслиПользовательУдаляетЭтоМуниципальноеОбразованиеСЭтогоРкц()
        {
            try
            {
                Container.Resolve<IDomainService<CashPaymentCenterMunicipality>>()
                    .Delete((long)ScenarioContext.Current["cashPaymentCenterMunicipalityId"]);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [When(@"пользователь сохраняет этот РКЦ")]
        public void ЕслиПользовательСохраняетЭтотРкц()
        {
            try
            {
                DomainService.SaveOrUpdate(CashPaymentCenterHelper.Current);
            }
            catch (Exception ex)
            {
                ExceptionHelper.AddException(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [When(@"пользователь удаляет этот РКЦ")]
        public void ЕслиПользовательУдаляетЭтотРкц()
        {
            try
            {
                DomainService.Delete(CashPaymentCenterHelper.Current.Id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [When(@"пользователь к этому РКЦ добавляет муниципальное образование ""(.*)""")]
        public void ЕслиПользовательДобавляетМуниципальноеОбразованиеКЭтомуРкц(string name)
        {
            var municipality = Container.Resolve<IDomainService<Municipality>>().FirstOrDefault(x => x.Name == name);

            var service = Container.Resolve<ICashPaymentCenterService>();

            var baseParams = new BaseParams
            {
                Params =
                    new DynamicDictionary
                    {
                        {"muIds", new[] {municipality.Id}},
                        {"cashPaymentCenterId", CashPaymentCenterHelper.Current.Id}
                    }
            };

            try
            {
                service.AddMunicipalities(baseParams);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            ScenarioContext.Current["municipalityForCashPayment"] = municipality;
        }
        
        [Then(@"запись по этому объекту присутствует в списке объектов у этого РКЦ")]
        public void ТоЗаписьПоЭтомуОбъектуПрисутствуетВСпискеОбъектовУЭтогоРкц()
        {
            var list =
                Container.Resolve<IDomainService<CashPaymentCenterPersAcc>>()
                    .GetAll()
                    .ToList()
                    .Where(x => x.CashPaymentCenter == CashPaymentCenterHelper.Current && x.PersonalAccount.PersonalAccountNum == ScenarioContext.Current["PersonalAccountNum"].ToString());

            list.Should().NotBeNullOrEmpty("Запись по этому объекту должна присутствовать в списке объектов у этого РКЦ");
        }

        [Then(@"запись по этому объекту отсутствует в списке объектов у этого РКЦ")]
        public void ТоЗаписьПоЭтомуОбъектуОтсутствуетВСпискеОбъектовУЭтогоРкц()
        {
            var list =
                Container.Resolve<IDomainService<CashPaymentCenterPersAcc>>()
                    .GetAll()
                    .ToList()
                    .Where(x => x.CashPaymentCenter == CashPaymentCenterHelper.Current && x.PersonalAccount.PersonalAccountNum == ScenarioContext.Current["PersonalAccountNum"].ToString());

            list.Should().BeNullOrEmpty("Запись по этому объекту должна отсутствовать в списке объектов у тэтого РКЦ");
        }

        [Then(@"запись по этому РКЦ присутствует в разделе расчетно-кассовых центров")]
        public void ТоЗаписьПоЭтомуРкцПрисутствуетВРазделеРасчетно_КассовыхЦентров()
        {
            DomainService.Get(CashPaymentCenterHelper.Current.Id)
                .Should()
                .NotBeNull("Запись по этому РКЦ должна присутствует в разделе расчетно-кассовых центров");
        }

        [Then(@"запись по этому РКЦ отсутствует в разделе расчетно-кассовых центров")]
        public void ТоЗаписьПоЭтомуРкцОтсутствуетВРазделеРасчетно_КассовыхЦентров()
        {
            DomainService.Get(CashPaymentCenterHelper.Current.Id)
                .Should()
                .BeNull("Запись по этому РКЦ должна отсутствовать в разделе расчетно-кассовых центров");
        }

        [Then(@"записи по этому муниципальному образованию присутствуют в списке муниципальных образований этого РКЦ")]
        public void ТоЗаписиПоЭтомуМуниципальномуОбразованиюПрисутствуютВСпискеМуниципальныхОбразованийЭтогоРкц()
        {
            var cashPaymentCenterMunicipalityList =
                Container.Resolve<IDomainService<CashPaymentCenterMunicipality>>()
                    .GetAll()
                    .ToList()
                    .Where(x => x.CashPaymentCenter == CashPaymentCenterHelper.Current);

            var cashPaymentCenterMunicipality = cashPaymentCenterMunicipalityList.FirstOrDefault(
                x => x.Municipality == (Municipality) ScenarioContext.Current["municipalityForCashPayment"]);

            cashPaymentCenterMunicipality.Should()
                .NotBeNull(
                    "Записи по этому мунициапальному образованию должны быть в списке муниципальных образований этог РКЦ");

            ScenarioContext.Current["cashPaymentCenterMunicipalityId"] = cashPaymentCenterMunicipality.Id;
        }

        [Then(@"записи по этому муниципальному образованию отсутствуют в списке муниципальных образований этого РКЦ")]
        public void ТоЗаписиПоЭтомуМуниципальномуОбразованиюОтсутствуютВСпискеМуниципальныхОбразованийЭтогоРкц()
        {
            Container.Resolve<IDomainService<CashPaymentCenterMunicipality>>()
                .Get((long)ScenarioContext.Current["cashPaymentCenterMunicipalityId"])
                .Should()
                .BeNull(
                    "Записи по этому муниципальному образованию должны отсутствовать в списке муниципальных образований этого РКЦ");
        }

        [Then(@"у этого РКЦ отсутствует запись по объекту с ЛС ""(.*)""")]
        public void ТоУЭтогоРкцОтсутствуетОбъектУКоторогоЛс(string personalAccountNum)
        {
            var list = Container.Resolve<IDomainService<CashPaymentCenterPersAcc>>().GetAll();

            list.Any(
                x =>
                x.CashPaymentCenter.Id == CashPaymentCenterHelper.Current.Id
                && x.PersonalAccount.PersonalAccountNum == personalAccountNum)
                .Should()
                .BeFalse(string.Format("у этого РКЦ должен отсутствовать объект у которого лс {0}", personalAccountNum));
        }

        [Then(@"у этого РКЦ присутствует запись по объекту с ЛС ""(.*)""")]
        public void ТоУЭтогоРКЦПрисутствуетЗаписьПоОбъектуСЛс(string personalAccountNum)
        {
            var list = Container.Resolve<IDomainService<CashPaymentCenterPersAcc>>().GetAll();

            list.Any(
                x =>
                x.CashPaymentCenter.Id == CashPaymentCenterHelper.Current.Id
                && x.PersonalAccount.PersonalAccountNum == personalAccountNum)
                .Should()
                .BeTrue(string.Format("у этого РКЦ должен присутствовать объект у которого лс {0}", personalAccountNum));
        }

        [Then(@"у этого объекта РКЦ заполнено поле Дата окончания действия договора ""(.*)""")]
        public void ТоУОбъектаРКЦЗаполненоПолеДатаОкончанияДействияДоговора(string endDate)
        {
            var date = endDate.DateParse(true);

            CashPaymentCenterPersAccHelper.Current.DateEnd.Should()
                .Be(
                    date,
                    string.Format("у этого объекта РКЦ поле Дата окончания действия договора должно быть {0}", endDate));
        }
    }
}
