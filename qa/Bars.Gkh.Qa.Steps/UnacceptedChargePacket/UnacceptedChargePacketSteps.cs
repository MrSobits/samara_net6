namespace Bars.Gkh.Qa.Steps
{
    using System.Collections;

    using Bars.B4.IoC.Lifestyles.SessionLifestyle;
    using Bars.Gkh.RegOperator.DomainService;
    using NHibernate.Util;
    using Bars.Gkh.Qa.Utils;
    using TechTalk.SpecFlow;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using B4.DataAccess;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.RegOperator.DomainService.Interface;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Enums;
    using Controllers;
    using DataResult;
    using Domain.ParameterVersioning;
    using Entities;
    using Extensions;
    using FluentAssertions;
    using Newtonsoft.Json.Linq;
    using NUnit.Framework;
    using Overhaul.DomainService;
    using Overhaul.Entities;
    using RegOperator.DomainModelServices;

    [Binding]
    internal class UnacceptedChargePacketSteps : BindingBase
    {
        private IDomainService<UnacceptedChargePacket> domainService =
            Container.Resolve<IDomainService<UnacceptedChargePacket>>();

        [When(@"в Реестре неподтвержденных начислений появляется запись начислений")]
        public void ТоВРеестреНеподтвержденныхНачисленийПоявляетсяЗаписьНачислений()
        {
            UnacceptedChargePacketHelper.Current =
                domainService.GetAll().OrderByDescending(x => x.ObjectCreateDate).ThenByDescending(x => x.Id).First();
        }

        [When(@"у этой записи Состояние Ожидает")]
        public void ЕслиУЭтойЗаписиСостояниеОжидает()
        {
            UnacceptedChargePacketHelper.Current.PacketState.Should()
                .Be(PaymentOrChargePacketState.Pending, "состояние записи должно быть = Ожидает");
        }

        [When(@"у этой записи начислений есть детальная информация")]
        public void ТоУЭтойЗаписиНачисленийЕстьДетальнаяИнформация()
        {
            UnacceptedChargePacketHelper.Current.Charges.Should().NotBeNullOrEmpty("В детальной информации нет записей");
        }

        [When(@"у этой детальной информации количество записей по лс = количеству лс, которые попадают в условия расчета лс")]
        public void ТоУЭтойДетальнойИнформацииКоличествоЗаписейПоЛсКоличествуЛсКоторыеПопадаютВУсловияРасчетаЛс()
        {
            //UnacceptedChargePacketHelper.Current.Charges.Count().Should().Be(10, "Фактическое количество ЛС отличается от ожидаемого");
        }

        [Then(@"у этой детальной информации по начислениям присутствует запись по ЛС ""(.*)""")]
        public void ТоУЭтойДетальнойИнформацииПоНачислениямПрисутствуетЗаписьПоЛС(string AccountNum)
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
                                {"property", "packetId"},
                                {"value", 189}//UnacceptedChargePacketHelper.Current.Id}
                            }
                        }
                    }
                }
            };

            var viewModel = Container.Resolve<IViewModel<UnacceptedCharge>>();
            var result =  viewModel.List(Container.Resolve<IDomainService<UnacceptedCharge>>(), baseParams) as ListSummaryResult;

            var chargeDetail = ((IEnumerable<object>) result.Data).Where( x  => x.GetType().GetProperty("AccountNum").GetValue(x) as String == AccountNum).ToList().First();
            chargeDetail.Should().NotBeNull(String.Format("у этой детальной информации по начислениям должна присутствовать запись по ЛС {0}", AccountNum));

            ScenarioContext.Current["chargeDetail"] = chargeDetail;
        }

        [Then(@"у этой записи по начислению для этого ЛС заполнено поле Статус ""(.*)""")]
        public void ТоУЭтойЗаписиПоНачислениюДляЭтогоЛСЗаполненоПолеСтатус(string accountState)
        {
            var chargeDetail = ScenarioContext.Current["chargeDetail"];
            chargeDetail.GetType().GetProperty("AccountState").GetValue(chargeDetail).Should().Be(accountState);
        }

        [Then(@"у этой записи по начислению для этого ЛС заполнено поле Начислено по базовому тарифу ""(.*)""")]
        public void ТоУЭтойЗаписиПоНачислениюДляЭтогоЛСЗаполненоПолеНачисленоПоБазовомуТарифу(Decimal chargeTariff)
        {
            var chargeDetail = ScenarioContext.Current["chargeDetail"];
            chargeDetail.GetType().GetProperty("ChargeTariff").GetValue(chargeDetail).Should().Be(chargeTariff, String.Format("Поле начислено по базовому тарифу должно быть = {0}", chargeTariff));
        }

        [Then(@"в реестре жилых домов есть запись по дому с Адресом ""(.*)""")]
        public void ТоВРеестреЖилыхДомовЕстьЗаписьПоДомуСАдресом(string address)
        {
            ScenarioContext.Current["house"] = Container.Resolve<IDomainService<RealityObject>>().FirstOrDefault(x => x.Address == address);
        }

        [Then(@"у этого дома есть запись в Сведениях о помещениях")]
        public void ТоУЭтогоДомаЕстьЗаписьВСведенияхОПомещениях()
        {
            var house = ScenarioContext.Current["house"] as RealityObject;
            ScenarioContext.Current["rooms"] = Container.Resolve<IDomainService<Room>>().GetAll().Where( x => x.RealityObject.Id == house.Id).ToList();
        }

        [Then(@"у этой записи в Сведениях о помещениях заполнено поле № квартиры/помещения ""(.*)""")]
        public void ТоУЭтойЗаписиВСведенияхОПомещенияхЗаполненоПолеКвартирыПомещения(string roomNum)
        {
            var rooms = ScenarioContext.Current["rooms"] as IList<Room>;
            var room = rooms.FirstOrDefault(x => x.RoomNum == roomNum);
            room.Should().NotBeNull(String.Format("не найдено помещение номер {0}", roomNum));

            RoomHelper.Current = room;
        }

        [Then(@"у этой записи неподтверждённых начислений Состояние ""(.*)""")]
        public void ТоУЭтойЗаписиНеподтверждённыхНачисленийСостояние(string paymentOrChargePacketState)
        {
            var expectedPaymentOrChargePacketState =
                EnumHelper.GetFromDisplayValue<PaymentOrChargePacketState>(paymentOrChargePacketState);

            UnacceptedChargePacketHelper.Current.PacketState.Should()
                .Be(
                    expectedPaymentOrChargePacketState,
                    string.Format(
                        "у этой записи неподтверждённых начислений Состояние должно быть {0}",
                        paymentOrChargePacketState));
        }

        [When(@"пользователь у этого помещения заполняет поле Общая площадь ""(.*)""")]
        public void ЕслиПользовательУЭтогоПомещенияЗаполняетПолеОбщаяПлощадь(int area)
        {
            var service = Container.Resolve<IVersionedEntityService>();

            var baseParams = new BaseParams
            {
                Params = new DynamicDictionary
                {
                    {"factDate", DateTime.Today.Date},
                    {"value", area},
                    {"className", "Room"},
                    {"propertyName", "Area"},
                    {"entityId", RoomHelper.Current.Id}     
                }
            };

            baseParams.Files = new Dictionary<string, FileData>();
            
            try
            {
                service.SaveParameterVersion(baseParams);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [When(@"пользователь подтверждает эту запись начислений")]
        public void ЕслиПользовательПодтверждаетЭтуЗаписьНачислений()
        {
            var baseParams = new BaseParams
                                 {
                                     Params =
                                         new DynamicDictionary
                                             {
                                                 {
                                                     "id",
                                                     UnacceptedChargePacketHelper.Current.Id
                                                 }
                                             }
                                 };

            var result = Container.Resolve<IPersonalAccountCharger>().AcceptUnaccepted(baseParams);

            if (!result.Success)
            {
                ExceptionHelper.TestExceptions.Add("IPersonalAccountCharger.AcceptUnaccepted", result.Message);

                Container.Resolve<ISessionProvider>().GetCurrentSession().Clear();
            }
        }

        [Then(@"у этой записи Состояние Подтверждено")]
        public void ТоУЭтойЗаписиСостояниеПодтверждено()
        {
            UnacceptedChargePacketHelper.Current.PacketState.Should()
                .Be(PaymentOrChargePacketState.Accepted, "состояние записи должно быть = Подтверждено");
        }

        [Then(@"в реестре абонентов есть запись по абоненту ""(.*)""")]
        public void ТоВРеестреАбонентовЕстьЗаписьПоАбоненту(string name)
        {
            ScenarioContext.Current["personalAccountOwner"] =
                Container.Resolve<IDomainService<Gkh.RegOperator.Entities.PersonalAccountOwner>>()
                    .FirstOrDefault(x => x.Name == name);
        }

        [Then(@"у этой записи по абоненту есть запись о помещении с лс ""(.*)""")]
        public void ТоУЭтойЗаписиПоАбонентуЕстьЗаписьОПомещенииСЛс(string personalAccountNum)
        {
            BasePersonalAccountHelper.Current =
                Container.Resolve<IDomainService<BasePersonalAccount>>()
                    .GetAll()
                    .FirstOrDefault(x => x.PersonalAccountNum == personalAccountNum);
        }

        [Then(@"пользователь у этой записи о помещении у доли собственности заполняет поле Новое значение ""(.*)""")]
        public void ТоПользовательУЭтойЗаписиОПомещенииУДолиСобственностиЗаполняетПолеНовоеЗначение(string areaShare)
        {
            var service = Container.Resolve<IPersonalAccountChangeService>();

            var baseParams = new BaseParams
            {
                Params = new DynamicDictionary
                {
                    {"factDate", DateTime.Today.Date},
                    {"value", areaShare},
                    {"className", "BasePersonalAccount"},
                    {"propertyName", "AreaShare"},
                    {"entityId", BasePersonalAccountHelper.Current.Id}     
                }
            };

            baseParams.Files = new Dictionary<string, FileData>();

            try
            {
                var a = service.ChangeAreaShare(baseParams);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [Given(@"в размере взносов на кр есть запись показателя и у этой записи показателя заполнено поле Окончание периода ""(.*)""")]
        public void ДопустимВРазмереВзносовНаКрЕстьЗаписьПоказателяИУЭтойЗаписиПоказателяЗаполненоПолеОкончаниеПериода(string date)
        {
            PaySizeHelper.Current =
                Container.Resolve<IDomainService<Paysize>>().FirstOrDefault(x => x.DateEnd == DateTime.Parse(date));

            PaySizeHelper.Current.Should().NotBeNull(String.Format("Не найден размер взносов на кр с датой окончания периода {0}", date));
        }

        [Given(@"у этой записи в детальной информации есть запись по муниципальному образованию ""(.*)""")]
        public void ДопустимУЭтойЗаписиВДетальнойИнформацииЕстьЗаписьПоМуниципальномуОбразованию(string municipalityName)
        {
            var baseParams = new BaseParams
            {
                Params = new DynamicDictionary
                {
                    {"paysizeId", PaySizeHelper.Current.Id}
                }
            };

            var result = Container.Resolve<IPaysizeRecordService>().ListTree(baseParams).Data as JArray;
            ScenarioContext.Current["paysizeByMunicipality"] = result.Where( x => x.GetValue("Name").ToString() == municipalityName).First();
        }

        [When(@"пользователь у этогй записи по муниципальному образованию заполняет поле Размер взноса ""(.*)""")]
        public void ЕслиПользовательУЭтогйЗаписиПоМуниципальномуОбразованиюЗаполняетПолеРазмерВзноса(Decimal value)
        {
            var paysizeByMunicipality = ScenarioContext.Current["paysizeByMunicipality"] as JToken;
            var Id = paysizeByMunicipality.GetValue("Id");

            if (Id == null)
            {
                Assert.Fail("Не найдено запись по муниципальному образованию в Размер взноса на кап.ремонт");
            }

            var baseParams = new BaseParams
            {
                Params = new DynamicDictionary
                {
                    {"Value", value},
                    {"Id", Id}
                }
            };

            var paySizeRecord = Container.Resolve<IDomainService<PaysizeRecord>>().FirstOrDefault(x => x.Id == Id.ToLong());
            paySizeRecord.Value = value;

            try
            {
                Container.Resolve<IDomainService<PaysizeRecord>>().Update(paySizeRecord);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [Then(@"у каждого лс из детальной информации есть Протокол расчета за текущий период")]
        public void ТоУКаждогоЛсИзДетальнойИнформацииЕстьПротоколРасчетаЗаТекущийПериод()
        {
            var baseParams = new BaseParams
            {
                Params = new DynamicDictionary
                {
                    {
                        "filter",
                        new[]
                        {
                            new DynamicDictionary {{"property", "accountId"}},
                            new DynamicDictionary {{"property", "periodId"}, {"value", ChargePeriodHelper.Current.Id}}
                        }
                    }
                }
            };

            foreach (var charge in UnacceptedChargePacketHelper.Current.Charges)
            {
                ((DynamicDictionary) ((IList) (baseParams.Params["filter"])).First())["value"] = charge.PersonalAccount.Id;

                var result = Container.Resolve<IPersonalAccountSummaryService>().ListChargeParameterTrace(baseParams);

                ((IList)(result.Data)).Should().NotBeNullOrEmpty("Протокол расчета для лс " + charge.PersonalAccount.PersonalAccountNum + " не содержит записей");
            }
        }
    }
}
        