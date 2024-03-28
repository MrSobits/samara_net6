namespace Bars.Gkh.Qa.Steps
{
    using Bars.B4.IoC.Lifestyles.SessionLifestyle;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Qa.Steps.CommonSteps;

    using Utils;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Utils;
    using Entities;
    using FluentAssertions;
    using GkhCr.Entities;
    using NHibernate.Util;
    using NUnit.Framework;
    using Overhaul.Entities;
    using RegOperator.DomainService;
    using RegOperator.Entities;
    using TechTalk.SpecFlow;

    [Binding]
    public class PaySizeSteps : BindingBase
    {
        private IDomainService<Paysize> paysizeDomainService = Container.Resolve<IDomainService<Paysize>>();

        [Given(@"в размер взносов на кр есть запись с пустым полем Окончание периода")]
        public void ДопустимВРазмерВзносовНаКрЕстьЗаписьСПустымПолемОкончаниеПериода()
        {
            var paysize = paysizeDomainService.FirstOrDefault( x => x.DateEnd == null);

            if (paysize == null)
            {
                Assert.Fail("Не найден размер взносов с пустым полем Окончание периода");
            }
            else
            {
                PaySizeHelper.Current = paysize;
            }
        }

        [Given(@"пользователь добавляет новый Размер взносов на КР")]
        public void ДопустимПользовательДобавляетНовыйРазмерВзносовНаКР()
        {
            PaySizeHelper.Current = new Paysize();
        }

        [Given(@"пользователь у этого размера взносов на КР заполняет поле С ""(.*)""")]
        public void ДопустимПользовательУЭтогоРазмераВзносовНаКРЗаполняетПолеС(string dateStart)
        {
            PaySizeHelper.Current.DateStart = dateStart.DateParse().Value;
        }

        [Given(@"пользователь у этого размера взносов на КР заполняет поле По ""(.*)""")]
        public void ДопустимПользовательУЭтогоРазмераВзносовНаКРЗаполняетПолеПо(string dateEnd)
        {
            PaySizeHelper.Current.DateEnd = dateEnd.DateParse();
        }
        
        [Given(@"у этой детальной информации есть запис по муниципальному образованию ""(.*)""")]
        public void ДопустимУЭтойДетальнойИнформацииЕстьЗаписПоМуниципальномуОбразованию(string municipalityName)
        {
            var paysizeRecord =
                Container.Resolve<IDomainService<PaysizeRecord>>().FirstOrDefault(x => x.Municipality.Name == municipalityName && x.Paysize == PaySizeHelper.Current);

            if (paysizeRecord == null)
            {
                Assert.Fail("Не записи в размере взносов на кр для муниципального образования " + municipalityName);
            }

            ScenarioContext.Current["paysizeRecord"] = paysizeRecord;
        }
        
        [Given(@"у этой записи по муниципальному образованию заполнено поле Размер взноса ""(.*)""")]
        public void ДопустимУЭтойЗаписиПоМуниципальномуОбразованиюЗаполненоПолеРазмерВзноса(Decimal p0)
        {
            var paysizeRecord = ScenarioContext.Current.Get<PaysizeRecord>("paysizeRecord");
            paysizeRecord.Value = p0;

            try
            {
                Container.Resolve<IDomainService<PaysizeRecord>>().Update(paysizeRecord);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [Given(@"пользователь в Размерах взносов на КР выбирает запись у которой поле Окончание периода ""(.*)""")]
        public void ДопустимПользовательВРазмерахВзносовНаКРВыбираетЗаписьУКоторойПолеОкончаниеПериода(string dateEnd)
        {
            var expectedDate = dateEnd.DateParse();

            PaySizeHelper.Current = this.paysizeDomainService.GetAll().FirstOrDefault(x => x.DateEnd == expectedDate);
        }

        [When(@"пользователь сохраняет этот Размер взносов на КР")]
        public void ЕслиПользовательСохраняетЭтотРазмерВзносовНаКР()
        {
            try
            {
                paysizeDomainService.SaveOrUpdate(PaySizeHelper.Current);
            }
            catch (Exception ex)
            {
                ExceptionHelper.AddException("пользователь сохраняет этот Размер взносов на КР", ex.Message);

                Container.Resolve<ISessionProvider>().GetCurrentSession().Clear();
            }
        }

        [When(@"пользователь удаляет этот Размер взносов на КР")]
        public void ЕслиПользовательУдаляетЭтотРазмерВзносовНаКР()
        {
            try
            {
                paysizeDomainService.Delete(PaySizeHelper.Current.Id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.AddException("пользователь удаляет этот Размер взносов на КР", ex.Message);

                Container.Resolve<ISessionProvider>().GetCurrentSession().Clear();
            }
        }

        [Then(@"у этой детальной информации по начислениям отсутствует запись по ЛС ""(.*)""")]
        public void ТоУЭтойДетальнойИнформацииПоНачислениямОтсутствуетЗаписьПоЛС(string personalAccountNum)
        {
            Int64 id = UnacceptedChargePacketHelper.Current.Id;

            var packet =
                Container.Resolve<IDomainService<UnacceptedCharge>>().GetAll().Where(x => x.Packet.Id == id && x.PersonalAccount.PersonalAccountNum == personalAccountNum).ToList();
            
            packet.Should().BeNullOrEmpty("лс должен отсутствовать в детальной информации начисления");
        }

        [Then(@"в протоколе расчета по текущему периоду у ЛС ""(.*)"" отсутствует запись по начислению")]
        public void ТоВПротоколеРасчетаПоТекущемуПериодуУЛСОтсутствуетЗаписьПоНачислению(string accounNumber)
        {
            var baseParams = new BaseParams
            {
                Params = new DynamicDictionary
                {
                    {
                        "filter", new[]
                        {
                            new DynamicDictionary
                            {
                                {"property", "accountId"},
                                {"value", BasePersonalAccountHelper.Current.Id}
                            },
                            new DynamicDictionary
                            {
                                {"property", "periodId"},
                                {
                                    "value", ChargePeriodHelper.Current.Id

                                }
                            }
                        }
                    }
                }
            };

            var result =
                (ListDataResult)
                    Container.Resolve<IPersonalAccountSummaryService>().ListChargeParameterTrace(baseParams);

            ((IList) result.Data).Should().BeNullOrEmpty("в протоколе расчета по текущему периоду у ЛС {0} отсутствует запись по начислению", accounNumber);
        }

        [Then(@"эта запись отсутствует в реестре Размеры взносов на КР")]
        public void ТоЭтаЗаписьОтсутствуетВРеестреРазмерыВзносовНаКр()
        {
            var paysize = this.paysizeDomainService.Get(PaySizeHelper.Current.Id);

            paysize.Should().BeNull("Текущая запись должна отсутствовать в реестре Размеры взносов на КР");
        }

        [Then(@"эта запись присутствует в реестре Размеры взносов на КР")]
        public void ТоЭтаЗаписьПрисутствуетВРеестреРазмерыВзносовНаКР()
        {
            var paysizeId = PaySizeHelper.Current.Id;

            var paysize = this.paysizeDomainService.Get(paysizeId);

            paysize.Should().NotBeNull("Текущая запись должна присутствовать в реестре Размеры взносов на КР");
        }

        [Then(@"у этого размера взносов на КР поле Окончание периода ""(.*)""")]
        public void ТоУЭтогоРазмераВзносовНаКРПолеОкончаниеПериода(string dateEnd)
        {
            var expectedDate = dateEnd.DateParse();

            PaySizeHelper.Current.DateEnd.Should()
                .Be(
                    expectedDate,
                    string.Format("у этого размера взносов на КР поле Окончание периода должно быть {0}", dateEnd));
        }
    }
}
