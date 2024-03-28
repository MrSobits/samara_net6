namespace Bars.Gkh.Qa.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC.Lifestyles.SessionLifestyle;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain.ParameterVersioning;
    using Bars.Gkh.Qa.Steps.CommonSteps;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;
    using FluentAssertions;
    using NUnit.Framework;

    using Bars.B4.Modules.FIAS;
    using Bars.B4.Application;

    using Bars.Gkh.Qa.Utils;

    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    [Binding]
    public class RealityObjectSteps : BindingBase
    {
        private BindingBase.DomainServiceCashe<RealityObject> _cashe = new BindingBase.DomainServiceCashe<RealityObject>();

        [Given(@"пользователь добавляет новый дом в Реестр жилых домов")]
        public void ЕслиПользовательДобавляетНовыйДомВРеестрЖилыхДомов()
        {
            var realityObject = new RealityObject
            {
                TypeHouse = TypeHouse.ManyApartments
            };

            RealityObjectHelper.CurrentRealityObject = realityObject;
        }

        [Given(@"пользователь выбирает дом по адресу ""(.*)""")]
        public void ДопустимПользовательВыбираетДомПоАдресу(string address)
        {
            var realityObject = this._cashe.Current.GetAll().FirstOrDefault(x => x.Address == address);

            if (realityObject == null)
            {
                throw new SpecFlowException(string.Format("отсутвует дом с адресом {0}", address));
            }

            RealityObjectHelper.CurrentRealityObject = realityObject;
        }

        [Given(@"у этого дома устанавливает поле Населённый пункт")]
        public void ЕслиУЭтогоДомаУстанавливаетПолеНаселённыйПункт(Table placeTable)
        {
            var list = placeTable.CreateSet<PlaceTable>().ToList();

            var context = (GkhContext)ApplicationContext.Current;

            if (list.All(x => x.Region != context.Region))
            {
                Assert.Ignore();
            }

            var place = list.First(x => x.Region == context.Region).Place;

            var realityObject = RealityObjectHelper.CurrentRealityObject;

            realityObject.FiasAddress = realityObject.FiasAddress ?? new FiasAddress();

            realityObject.FiasAddress.PlaceName = place;
        }

        [Given(@"у этого дома устанавливает поле Улица")]
        public void ЕслиУЭтогоДомаУстанавливаетПолеУлица(Table streetTable)
        {
            var list = streetTable.CreateSet<StreetTable>().ToList();

            var context = (GkhContext)ApplicationContext.Current;

            if (list.All(x => x.Region != context.Region))
            {
                Assert.Ignore();
            }

            var street = list.First(x => x.Region == context.Region).Street;

            var realityObject = RealityObjectHelper.CurrentRealityObject;

            realityObject.FiasAddress = realityObject.FiasAddress ?? new FiasAddress();

            realityObject.FiasAddress.StreetName = street;
        }

        [Given(@"у этого дома устанавливает поле Номер Дома")]
        public void ЕслиУЭтогоДомаУстанавливаетПолеНомерДома(Table houseTable)
        {
            var list = houseTable.CreateSet<HouseTable>().ToList();

            var context = (GkhContext)ApplicationContext.Current;

            if (list.All(x => x.Region != context.Region))
            {
                Assert.Ignore();
            }

            var houseNumber = list.First(x => x.Region == context.Region).HouseNumber;

            var realityObject = RealityObjectHelper.CurrentRealityObject;

            realityObject.FiasAddress = realityObject.FiasAddress ?? new FiasAddress();

            realityObject.FiasAddress.House = houseNumber;
        }

        [Given(@"в реестр жилых домов добавлен новый дом")]
        public void RealityObjectFastCreate(Table realityObjectTable)
        {
            var list = realityObjectTable.CreateSet<RealityObjectTable>().ToList();

            var context = (GkhContext)ApplicationContext.Current;

            var realityObjectProxy = list.FirstOrDefault(x => x.Region == context.Region);

            if (realityObjectProxy == null)
            {
                Assert.Ignore();
            }

            var fiasRepository = Container.Resolve<IFiasRepository>();
            
            //Получаем населенные пункты по фильтру
            var cityList = fiasRepository.GetPlacesDinamicAddress(realityObjectProxy.City.Split(null).Last());

            if (!cityList.Any())
            {
                throw new SpecFlowException(string.Format("нет фиаса по населёному пункту {0}", realityObjectProxy.City));
            }

            //Получаем улицы
            var streetList = fiasRepository.GetStreetsDinamicAddress(realityObjectProxy.Street, cityList.FirstOrDefault().GuidId);

            if (!streetList.Any())
            {
                throw new SpecFlowException(string.Format("нет фиаса по улице {0}", realityObjectProxy.Street));
            }

            var fiasAddress = new FiasAddress
                                  {
                                      AddressName =
                                          cityList.First().Name + " " + streetList.First().Name + " "
                                          + realityObjectProxy.HouseNumber,
                                      AddressGuid = cityList.First().AddressGuid,
                                      PlaceName = cityList.First().Name,
                                      StreetName = streetList.First().Name,
                                      House = realityObjectProxy.HouseNumber
                                  };

            var ds = Container.Resolve<IDomainService<RealityObject>>();
            var ro = new RealityObject
                         {
                             FiasAddress = fiasAddress,
                             TypeHouse = RealityObjectHelper.GetTypeHouse(realityObjectProxy.HouseType),
                             HasPrivatizedFlats = true
                         };

            ds.Save(ro);

            RealityObjectHelper.CurrentRealityObject = ro;
        }

        [Given(@"пользователь у этого дома добавляет протокол решения")]
        public void ДопустимПользовательУЭтогоДомаДобавляетПротоколРешения()
        {
            var protocolDynamicDict = new DynamicDictionary
                                          {
                                              { "Id", 0 }, 
                                              { "RealityObject", new DynamicDictionary
                                                                     {
                                                                         {
                                                                             "Id",
                                                                             RealityObjectHelper
                                                                             .CurrentRealityObject.Id
                                                                         }
                                                                     }
                                              }
                                          };

            RealityObjectBothProtocolHelper.Current = new BaseParams
                                                          {
                                                              Files = new Dictionary<string, FileData>(),

                                                              Params =
                                                                  new DynamicDictionary
                                                                      {
                                                                          {
                                                                              "roId",
                                                                              RealityObjectHelper.CurrentRealityObject.Id
                                                                          },
                                                                          {
                                                                              "Protocol",
                                                                              protocolDynamicDict
                                                                          }
                                                                      }
                                                          };
        }

        [Given(@"пользователь у текущего дома редактирует поле Состояние дома")]
        public void ДопустимПользовательУТекущегоДомаРедактируетПолеСостояниеДома()
        {
            ScenarioContext.Current["ChangeConditionHouseProxy"] = new ChangeConditionHouseProxy
                                                                       {
                                                                           RealityObject =
                                                                               RealityObjectHelper
                                                                               .CurrentRealityObject,
                                                                           ConditionHouse =
                                                                               RealityObjectHelper
                                                                               .CurrentRealityObject
                                                                               .ConditionHouse
                                                                       };
        }

        [Given(@"пользователь в редактировании Состояния дома заполняет поле Дата вступления значения в силу ""(.*)""")]
        public void ДопустимПользовательВРедактированииСостоянияДомаЗаполняетПолеДатаВступленияЗначенияВСилу(string factDate)
        {
            var changeConditionHouseProxy =
                ScenarioContext.Current.Get<ChangeConditionHouseProxy>("ChangeConditionHouseProxy");

            changeConditionHouseProxy.FactDate = factDate.DateParse().Value;
        }

        [Given(@"пользователь в редактировании Состояния дома заполняет поле Новое значение ""(.*)""")]
        public void ДопустимПользовательВРедактированииСостоянияДомаЗаполняетПолеНовоеЗначение(string conditionHouse)
        {
            var changeConditionHouseProxy =
                ScenarioContext.Current.Get<ChangeConditionHouseProxy>("ChangeConditionHouseProxy");

            changeConditionHouseProxy.ConditionHouse = EnumHelper.GetFromDisplayValue<ConditionHouse>(conditionHouse);
        }

        [Given(@"пользователь у этого жилого дома заполняет поле Дом не участвует в программе КР ""(.*)""")]
        public void ДопустимПользовательУЭтогоДомаЗаполняетПолеДомНеУчаствуетВПрограммеКР(string isNotInvolvedCr)
        {
            RealityObjectHelper.CurrentRealityObject.IsNotInvolvedCr = bool.Parse(isNotInvolvedCr);
        }

        [When(@"пользователь сохраняет этот жилой дом")]
        public void ЕслиСохраняетЭтотДом()
        {
            try
            {
                var realityObject = RealityObjectHelper.CurrentRealityObject;

                var fias = realityObject.FiasAddress;

                fias.AddressName = string.Format("{0}, {1}, {2}", fias.PlaceName, fias.StreetName, fias.House);

                fias.AddressGuid = RealityObjectHelper.GetFiasGuid(fias.PlaceName);

                realityObject.HasPrivatizedFlats = true;

                this._cashe.Current.SaveOrUpdate(realityObject);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);

                Container.Resolve<ISessionProvider>().GetCurrentSession().Clear();
            }
        }

        [When(@"пользователь удаляет этот жилой дом")]
        public void ЕслиПользовательУдаляетЭтотЖилойДом()
        {
            try
            {
                this._cashe.Current.Delete(RealityObjectHelper.CurrentRealityObject.Id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [When(@"пользователь сохраняет это Состояние дома")]
        public void ЕслиПользовательСохраняетЭтоСостояниеДома()
        {
            var changeConditionHouseProxy =
                ScenarioContext.Current.Get<ChangeConditionHouseProxy>("ChangeConditionHouseProxy");

            var service = Container.Resolve<IVersionedEntityService>();

            var baseParams = new BaseParams
                                 {
                                     Params =
                                         new DynamicDictionary
                                             {
                                                 {
                                                     "factDate",
                                                     changeConditionHouseProxy.FactDate
                                                 },
                                                 {
                                                     "value",
                                                     changeConditionHouseProxy.ConditionHouse
                                                 },
                                                 { "className", "RealityObject" },
                                                 { "propertyName", "ConditionHouse" },
                                                 {
                                                     "entityId",
                                                     changeConditionHouseProxy.RealityObject
                                                     .Id
                                                 }
                                             }
                                 };

            try
            {
                service.SaveParameterVersion(baseParams);
            }
            catch (Exception ex)
            {
                ExceptionHelper.AddException(
                    "IVersionedEntityService.SaveParameterVersion(ChangeConditionHouse)",
                    ex.Message);

                Container.Resolve<ISessionProvider>().GetCurrentSession().Clear();
            }
        }

        [Then(@"запись по этому дому отсутствует в реестре жилых домов")]
        public void ТоЗаписьПоДомуВРеестреЖилыхДомовОтсутствует()
        {
            var realityObject = this._cashe.Current.Get(RealityObjectHelper.CurrentRealityObject.Id);

            realityObject.Should().BeNull(string.Format("Дом должен отсутствовать в реестре жилых домов.{0}", ExceptionHelper.GetExceptions()));
        }


        [Then(@"запись по этому дому присутствует в реестр жилых домов")]
        public void ТоЗаписьПоНовомуДомуДобавляетсяВРеестрЖилыхДомов()
        {
            var realityObject = this._cashe.Current.Get(RealityObjectHelper.CurrentRealityObject.Id);

            realityObject.Should().NotBeNull(string.Format("Дом должен присутствовать в реестре жилых домов.{0}", ExceptionHelper.GetExceptions()));
        }

        [Then(@"у этого дома заполнено поле Состояние дома ""(.*)""")]
        public void ТоУЭтогоДомаЗаполненоПолеСостояниеДома(string conditionHouse)
        {
            var expectedConditionHouse = EnumHelper.GetFromDisplayValue<ConditionHouse>(conditionHouse);

            RealityObjectHelper.CurrentRealityObject.ConditionHouse.Should()
                .Be(
                    expectedConditionHouse,
                    string.Format("у этого дома состояние дома должно быть {0}", conditionHouse));
        }

        [Then(@"у этого жилого дома заполнено поле Дом не участвует в программе КР ""(.*)""")]
        public void ТоУЭтогоЖилогоДомаЗаполненоПолеДомНеУчаствуетВПрограммеКР(string isNotInvolvedCr)
        {
            var expectedIsNotInvolvwdCr = bool.Parse(isNotInvolvedCr);

            var realityObject = _cashe.Current.Get(RealityObjectHelper.CurrentRealityObject.Id);

            realityObject.IsNotInvolvedCr.Should()
                .Be(
                    expectedIsNotInvolvwdCr,
                    string.Format(
                        "у этого жилого дома должно быть заполнено поле Дом не участвует в программе КР {0}",
                        isNotInvolvedCr));
        }

        public class PlaceTable
        {
            public string Region { get; set; }

            public string Place { get; set; }
        }

        public class StreetTable
        {
            public string Region { get; set; }

            public string Street { get; set; }
        }

        public class HouseTable
        {
            public string Region { get; set; }

            public string HouseNumber { get; set; }
        }

        public class RealityObjectTable
        {
            public string Region { get; set; }

            public string HouseType { get; set; }

            public string City { get; set; }

            public string Street { get; set; }

            public string HouseNumber { get; set; }
        }

        private class ChangeConditionHouseProxy
        {
            public RealityObject RealityObject { get; set; }

            public ConditionHouse ConditionHouse { get; set; }

            public DateTime FactDate { get; set; }
        }
    }
}
