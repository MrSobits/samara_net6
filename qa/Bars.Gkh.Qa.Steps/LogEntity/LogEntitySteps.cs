namespace Bars.Gkh.Qa.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.NHibernateChangeLog;
    using Bars.B4.Modules.Security;
    using Bars.B4.Utils;
    using Bars.Gkh.Qa.Utils;
    using Bars.Gkh.UserActionRetention.DomainService;

    using FluentAssertions;

    using TechTalk.SpecFlow;

    [Binding]
    internal class LogEntitySteps : BindingBase
    {
        [When(@"пользователь в фильтре журнала действий пользователя заполняет поле Объект ""(.*)""")]
        public void ЕслиПользовательВФильтреЖурналаДействийПользователяЗаполняетПолеОбъект(string objectName)
        {
            BaseParams baseParams;

            var objectToSelect = LogEntityHelper.GetLoggedEntitiesbyEntityName(objectName);

            if (ScenarioContext.Current.ContainsKey("AuditLogMapServiceListBaseParams"))
            {
                 baseParams = ScenarioContext.Current.Get<BaseParams>("AuditLogMapServiceListBaseParams");
            }
            else
            {
                baseParams = new BaseParams();
                ScenarioContext.Current.Add("AuditLogMapServiceListBaseParams", baseParams);
            }

            baseParams.Params.Add("entityTypes", objectToSelect.EntityType);
        }

        [When(@"пользователь в фильтре журнала действий пользователя заполняет поле Логин ""(.*)""")]
        public void ЕслиПользовательВФильтреЖурналаДействийПользователяЗаполняетПолеЛогин(string login)
        {
            var userLoginService = Container.Resolve<IUserLoginService>();

            var result = userLoginService.ListWithoutPaging(new BaseParams());

            var data = (IEnumerable<object>)result.Data;

            var userList = data.Select(x => new
                                                {
                                                    Id = (long)ReflectionHelper.GetPropertyValue(x, "UserId"),
                                                    Login = (string)ReflectionHelper.GetPropertyValue(x, "UserLogin")
                                                });

            var userToSelect = userList.FirstOrDefault(x => x.Login == login);

            if (userToSelect == null)
            {
                throw new SpecFlowException(string.Format("отсутствует пользователь с Логином {0}", login));
            }

            BaseParams baseParams;

            if (ScenarioContext.Current.ContainsKey("AuditLogMapServiceListBaseParams"))
            {
                baseParams = ScenarioContext.Current.Get<BaseParams>("AuditLogMapServiceListBaseParams");
            }
            else
            {
                baseParams = new BaseParams();
                ScenarioContext.Current.Add("AuditLogMapServiceListBaseParams", baseParams);
            }

            baseParams.Params.Add("userIds", userToSelect.Id);
        }

        [When(@"пользователь в фильтре журнала действий пользователя заполняет поле Дата с ""(.*)""")]
        public void ЕслиПользовательВФильтреЖурналаДействийПользователяЗаполняетПолеДатаС(string dateFrom)
        {
            BaseParams baseParams;

            DateTime date;

            if (!DateTime.TryParse(dateFrom, out date))
            {
                throw new SpecFlowException(
                    string.Format("поле \"Дата с\" в фильтре журнала действий пользователя не может быть {0}", dateFrom));
            }

            if (ScenarioContext.Current.ContainsKey("AuditLogMapServiceListBaseParams"))
            {
                baseParams = ScenarioContext.Current.Get<BaseParams>("AuditLogMapServiceListBaseParams");
            }
            else
            {
                baseParams = new BaseParams();
                ScenarioContext.Current.Add("AuditLogMapServiceListBaseParams", baseParams);
            }

            baseParams.Params.Add("dateFrom", date);
        }

        [When(@"пользователь в фильтре журнала действий пользователя заполняет поле по ""(.*)""")]
        public void ЕслиПользовательВФильтреЖурналаДействийПользователяЗаполняетПолеПо(string dateTo)
        {
            BaseParams baseParams;

            DateTime date;

            if (!DateTime.TryParse(dateTo, out date))
            {
                throw new SpecFlowException(
                    string.Format("поле \"Дата по\" в фильтре журнала действий пользователя не может быть {0}", dateTo));
            }

            if (ScenarioContext.Current.ContainsKey("AuditLogMapServiceListBaseParams"))
            {
                baseParams = ScenarioContext.Current.Get<BaseParams>("AuditLogMapServiceListBaseParams");
            }
            else
            {
                baseParams = new BaseParams();
                ScenarioContext.Current.Add("AuditLogMapServiceListBaseParams", baseParams);
            }

            baseParams.Params.Add("dateTo", date);
        }


        [When(@"пользователь в журнале действий пользователя вызывает обновление списка записей")]
        public void ЕслиПользовательВЖурналеДействийПользователяВызываетОбновлениеСпискаЗаписей()
        {
            var dsLogEntity = Container.Resolve<IDomainService<LogEntity>>();

            var baseParams = ScenarioContext.Current.ContainsKey("AuditLogMapServiceListBaseParams")
                ? ScenarioContext.Current.Get<BaseParams>("AuditLogMapServiceListBaseParams")
                : new BaseParams();

            if (ScenarioContext.Current.ContainsKey("transaction"))
            {
                var transaction = ScenarioContext.Current.Get<IDataTransaction>("transaction");

                transaction.Commit();

                ScenarioContext.Current.Remove("transaction");
            }

            var result = (ListDataResult)Container.Resolve<IViewModel<LogEntity>>().List(dsLogEntity, baseParams);

            ScenarioContext.Current.Add("UserActionRetentionList", result.Data);
        }
        
        [Then(@"в журнале действий пользователя присутствуют записи, у которых Объект ""(.*)""")]
        public void ТоВЖурналеДействийПользователяПрисутствуютЗаписиУКоторыхОбъект(string objectName)
        {
            var userActionRetentionList = ScenarioContext.Current.Get<IEnumerable<dynamic>>("UserActionRetentionList");

            userActionRetentionList.All(x => ReflectionHelper.GetPropertyValue(x, "EntityName") == objectName)
                .Should()
                .BeTrue(string.Format("В списке должны присутствовать только записи с объектом {0}", objectName));
        }

        [Then(@"в журнале действий пользователя присутствуют записи, у которых Логин ""(.*)""")]
        public void ТоВЖурналеДействийПользователяПрисутствуютЗаписиУКоторыхЛогин(string login)
        {
            var userActionRetentionList = ScenarioContext.Current.Get<IEnumerable<dynamic>>("UserActionRetentionList");

            userActionRetentionList.All(x => ReflectionHelper.GetPropertyValue(x, "UserLogin") == login)
                .Should()
                .BeTrue(string.Format("В списке должны присутствовать только записи с логином {0}", login));
        }

        [Then(@"в журнале действий пользователя присутствуют записи, у которых Дата/время больше или равно ""(.*)""")]
        public void ТоВЖурналеДействийПользователяПрисутствуютЗаписиУКоторыхДатаВремяБольшеИлиРавно(string dateFrom)
        {
            DateTime date;

            if (!DateTime.TryParse(dateFrom, out date))
            {
                throw new SpecFlowException(
                    string.Format("поле \"Дата с\" в фильтре журнала действий пользователя не может быть {0}", dateFrom));
            }

            var userActionRetentionList = ScenarioContext.Current.Get<IEnumerable<dynamic>>("UserActionRetentionList");

            userActionRetentionList.All(x => ReflectionHelper.GetPropertyValue(x, "EntityDateChange") >= date)
                .Should()
                .BeTrue(string.Format("В списке должны присутствовать только записи с Дата/время больше или равно {0}", dateFrom));
        }

        [Then(@"в журнале действий пользователя присутствуют записи, у которых Дата/время меньше или равно ""(.*)""")]
        public void ТоВЖурналеДействийПользователяПрисутствуютЗаписиУКоторыхДатаВремяМеньшеИлиРавно(string dateTo)
        {
            DateTime date;

            if (!DateTime.TryParse(dateTo, out date))
            {
                throw new SpecFlowException(
                    string.Format("поле \"Дата по\" в фильтре журнала действий пользователя не может быть {0}", dateTo));
            }

            var userActionRetentionList = ScenarioContext.Current.Get<IEnumerable<dynamic>>("UserActionRetentionList");

            userActionRetentionList.All(x => ReflectionHelper.GetPropertyValue(x, "EntityDateChange") <= date)
                .Should()
                .BeTrue(string.Format("В списке должны присутствовать только записи с Дата/время меньше или равно {0}", dateTo));
        }

        [Then(@"в журнале действий пользователя появилась новая запись")]
        public void ТоВЖурналеДействийПользователяПоявиласьНоваяЗапись()
        {
            var userActionRetentionList = ScenarioContext.Current.Get<IEnumerable<dynamic>>("UserActionRetentionList");

            var lastRetention = userActionRetentionList
                .OrderByDescending(x => ReflectionHelper.GetPropertyValue(x, "EntityDateChange"))
                .First();

            var entityDateChange = (DateTime)ReflectionHelper.GetPropertyValue(lastRetention, "EntityDateChange");

            CommonHelper.IsNow(entityDateChange.ToLocalTime())
                .Should()
                .BeTrue(string.Format("В списке журнала действий пользователя должна появиться запись. {0}", ExceptionHelper.GetExceptions()));

            LogEntityHelper.Current = Container
                .Resolve<IDomainService<LogEntity>>()
                .Get(ReflectionHelper.GetPropertyValue(lastRetention, "Id"));
        }

        [Then(@"у этой записи в журнале действий пользователя заполнено поле Объект ""(.*)""")]
        public void ТоУЭтойЗаписиВЖурналеДействийПользователяЗаполненоПолеОбъект(string entityName)
        {
            var dictEntityNames = Container.Resolve<IChangeLogInfoProvider>()
                                          .GetLoggedEntities()
                                          .With(x => x.ToDictionary(y => y.EntityType, z => z.EntityName)) ??
                                 new Dictionary<string, string>();

            dictEntityNames[LogEntityHelper.Current.EntityType]
                .Should()
                .Be(
                    entityName,
                    string.Format(
                        "У текущей запси в журнале действий пользователя поле Объект должно быть {0}",
                        entityName));
        }

        [Then(@"у этой записи в журнале действий пользователя заполнено поле Наименование объекта ""(.*)""")]
        public void ТоУЭтойЗаписиВЖурналеДействийПользователяЗаполненоПолеНаименованиеОбъекта(string entityDescription)
        {
            LogEntityHelper.Current.EntityDescription.Should()
                .Be(
                    entityDescription,
                    string.Format(
                        "У текущей запси в журнале действий пользователя поле Наименование объекта должно быть {0}",
                        entityDescription));
        }

        [Then(@"у этой записи в журнале действий пользователя заполнено поле Id объекта")]
        public void ТоУЭтойЗаписиВЖурналеДействийПользователяЗаполненоПолеIdОбъекта()
        {
            LogEntityHelper.Current.EntityId.Should()
                .BeGreaterThan(
                    0,
                    string.Format(
                        "У текущей запси в журнале действий пользователя поле Id объекта должно быть больше 0"));
        }

        [Then(@"у этой записи в журнале действий пользователя заполнено поле Логин")]
        public void ТоУЭтойЗаписиВЖурналеДействийПользователяЗаполненоПолеЛогин()
        {
            LogEntityHelper.Current.UserLogin.Should().NotBeNullOrEmpty(string.Format(
                        "У текущей запси в журнале действий пользователя поле Логин должно быть заполнено"));
        }

        [Then(@"у этой записи в журнале действий пользователя заполнено поле IP")]
        public void ТоУЭтойЗаписиВЖурналеДействийПользователяЗаполненоПолеIP()
        {
            LogEntityHelper.Current.UserIpAddress.Should().NotBeNullOrEmpty(string.Format(
                        "У текущей запси в журнале действий пользователя поле IP должно быть заполнено"));
        }

        [Then(@"у этой записи в журнале действий пользователя заполнено поле Событие ""(.*)""")]
        public void ТоУЭтойЗаписиВЖурналеДействийПользователяЗаполненоПолеСобытие(string actionKind)
        {
            var internalActionKind = EnumHelper.GetFromDisplayValue<ActionKind>(actionKind);

            LogEntityHelper.Current.ActionKind.Should()
                .Be(
                    internalActionKind,
                    string.Format(
                        "У текущей запси в журнале действий пользователя поле Событие объекта должно быть {0}",
                        actionKind));
        }
    }
}
