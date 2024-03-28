using Bars.Gkh.RegOperator.Domain.PersonalAccountOperations;
using Bars.Gkh.RegOperator.Domain.PersonalAccountOperations.Impl;

namespace Bars.Gkh.Qa.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Controller.Provider;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.IoC.Lifestyles.SessionLifestyle;
    using Bars.B4.Modules.Tasks.Common.Contracts.Result;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Qa.Steps.CommonSteps;
    using Bars.Gkh.Qa.Utils;
    using Bars.Gkh.RegOperator.DomainModelServices;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities;

    using FluentAssertions;

    using NUnit.Framework;

    using TechTalk.SpecFlow;

    [Binding]
    internal class BasePersonalAccountSteps : BindingBase
    {
        private BaseParams saldoBaseParams;

        [Given(@"Пользователь в реестре ЛС выбирает действие Установка и изменение сальдо")]
        public void ДопустимПользовательВыбираетДействиеУстановкаИИзменениеСальдо()
        {
            var fileDictionary = new Dictionary<string, FileData>();

            this.saldoBaseParams = new BaseParams
                                       {
                                           Files = fileDictionary,

                                           Params =
                                               {
                                                   {
                                                       "AccountId",
                                                       BasePersonalAccountHelper.Current.Id
                                                   }
                                               }
                                       };
        }

        [Given(@"Пользователь в реестре ЛС выбирает действие Смена абонента")]
        public void ДопустимПользовательВРеестреЛСВыбираетДействиеСменаАбонента()
        {
            ScenarioContext.Current["SetNewOwnerBaseParams"] = new BaseParams
                                                                   {
                                                                       Params =
                                                                           new DynamicDictionary
                                                                               {
                                                                                   {
                                                                                       "AccountId",
                                                                                       BasePersonalAccountHelper
                                                                                       .Current
                                                                                       .Id
                                                                                   }
                                                                               },
                                                                       Files =
                                                                           new Dictionary<string, FileData>()
                                                                   };
        }

        [Given(@"пользователь в форме смены абонента у ЛС заполняет поле Новый владелец ""(.*)""")]
        public void ДопустимПользовательВФормеСменыАбонентаУЛСЗаполняетПолеНовыйВладелец(string newOwnerName)
        {
            var setNewOwnerBaseParams = ScenarioContext.Current.Get<BaseParams>("SetNewOwnerBaseParams");

            var personalAccountOwner =
                Container.Resolve<IDomainService<PersonalAccountOwner>>().FirstOrDefault(x => x.Name == newOwnerName);

            if (personalAccountOwner == null)
            {
                throw new SpecFlowException(string.Format("Отсутствует абонент с ФИО {0}", newOwnerName));
            }

            setNewOwnerBaseParams.Params["NewOwner"] = personalAccountOwner.Id;
        }

        [Given(@"пользователь в форме смены абонента у ЛС заполняет поле Дата начала действия ""(.*)""")]
        public void ДопустимПользовательЗаполняетПолеДатаНачалаДействия(string actualFrom)
        {
            var setNewOwnerBaseParams = ScenarioContext.Current.Get<BaseParams>("SetNewOwnerBaseParams");

            setNewOwnerBaseParams.Params["ActualFrom"] = actualFrom.DateParse();
        }

        [Given(@"пользователь в форме смены абонента у ЛС заполняет поле Причина ""(.*)""")]
        public void ДопустимПользовательВФормеСменыАбонентаУЛСЗаполняетПолеПричина(string reason)
        {
            var setNewOwnerBaseParams = ScenarioContext.Current.Get<BaseParams>("SetNewOwnerBaseParams");

            setNewOwnerBaseParams.Params["Reason"] = reason;
        }

        [Given(@"пользователь в форме смены абонента у ЛС заполняет поле Документ-основание ""(.*)""")]
        public void ДопустимПользовательВФормеСменыАбонентаУЛСЗаполняетПолеДокумент_Основание(string fullFileName)
        {
            var setNewOwnerBaseParams = ScenarioContext.Current.Get<BaseParams>("SetNewOwnerBaseParams");
            var currentDirectory = Directory.GetCurrentDirectory();
            var path = string.Format("{0}\\.fileStorage\\{1}", currentDirectory, fullFileName);

            if (!File.Exists(path))
            {
                throw new FileNotFoundException(string.Format("Файл {0} не найден", path));
            }

            var data = File.ReadAllBytes(path);

            var fileInfo = new FileInfo(path);
            var fileName = fileInfo.Name.Remove(fileInfo.Name.LastIndexOf(fileInfo.Extension, System.StringComparison.Ordinal));


            var fileData = new FileData(fileName, fileInfo.Extension.Remove(0, 1), data);

            setNewOwnerBaseParams.Files["Document"] = fileData;
        }

        [Given(@"пользователь в изменении сальдо ЛС заполняет поле Новое значение ""(.*)""")]
        public void ДопустимПользовательВИзмененииСальдоЛСЗаполняетПолеНовоеЗначение(decimal newSaldo)
        {
            this.saldoBaseParams.Params.Add("NewValue", newSaldo);
        }

        [Given(@"пользователь в изменении сальдо ЛС заполняет поле Причина установки/изменения сальдо ""(.*)""")]
        public void ДопустимПользовательВИзмененииСальдоЛСЗаполняетПолеПричинаУстановкиИзмененияСальдо(string reason)
        {
            this.saldoBaseParams.Params.Add("Reason", reason);
        }

        [Given(@"пользователь в реестре ЛС выбирает лицевой счет ""(.*)""")]
        public void ДопустимПользовательВРеестреЛСВыбираетЛицевойСчет(string personalAccNum)
        {
            var basePersAcc =
                Container.Resolve<IDomainService<BasePersonalAccount>>()
                    .GetAll()
                    .FirstOrDefault(x => x.PersonalAccountNum == personalAccNum);

            if (basePersAcc != null)
            {
                BasePersonalAccountHelper.Current = basePersAcc;

                if (!BasePersonalAccountListHelper.Current.Contains(basePersAcc))
                {
                    BasePersonalAccountListHelper.Current.Add(basePersAcc);
                }
            }
            else
            {
                Assert.Fail("Не найден лицевой счет" + personalAccNum);
            }
        }

        [Given(@"пользователь в реестре ЛС выбирает текущий ЛС")]
        public void ДопустимПользовательВРеестреЛСВыбираетТекущийЛС()
        {
            BasePersonalAccountHelper.FilterBaseParams.Params.Add("accountIds", BasePersonalAccountHelper.Current.Id);
        }

        /// <summary>
        /// Не использовать, создан для теста в которои нет возможности откатить транзакции
        /// </summary>
        [Given(@"Пользователь выбирает случайный ЛС")]
        public void ДопустимПользовательВыбираетСлучайныйЛС()
        {
            var basePersonalAccountQuery = BasePersonalAccountHelper.DomainService.GetAll();

            var basePersonalAccount = basePersonalAccountQuery.First();

            BasePersonalAccountHelper.Current = basePersonalAccount;
        }
        
        [Given(@"пользователь выбирает действие Смена абонента")]
        public void ДопустимПользовательВыбираетДействиеСменаАбонента()
        {
            ScenarioContext.Current["accId"] = BasePersonalAccountListHelper.Current[0].Id;
        }

        [Given(@"пользователь у этого ЛС вызывает реадактирование Доли собственности")]
        public void ДопустимПользовательУЭтогоЛСВызываетРеадактированиеДолиСобственности()
        {
            var areaShareChangeProxy = new AreaShareChangeProxy
                                           {
                                               BasePersonalAccount = BasePersonalAccountHelper.Current
                                           };

            ScenarioContext.Current["BasePersAccAreaShareChangeProxy"] = areaShareChangeProxy;
        }

        [Given(@"в смене доли собственности заполняет поле Дата вступления значения в силу ""(.*)""")]
        public void ДопустимВСменеДолиСобственностиЗаполняетПолеДатаВступленияЗначенияВСилу(string dateActual)
        {
            var areaShareChangeProxy = ScenarioContext.Current.Get<AreaShareChangeProxy>("BasePersAccAreaShareChangeProxy");

            areaShareChangeProxy.DateActual = dateActual.DateParse().Value;
        }

        [Given(@"в смене доли собственности заполняет поле Новое значение ""(.*)""")]
        public void ДопустимВСменеДолиСобственностиЗаполняетПолеНовоеЗначение(decimal newAreaShare)
        {
            var areaShareChangeProxy = ScenarioContext.Current.Get<AreaShareChangeProxy>("BasePersAccAreaShareChangeProxy");

            areaShareChangeProxy.NewAreaShare = newAreaShare;
        }

        [Given(@"пользователь в реестре ЛС выбирает действие Повторное открытие")]
        public void ДопустимПользовательВРеестреЛСВыбираетДействиеПовторноеОткрытие()
        {
            ScenarioContext.Current["ReopenAccBaseParams"] = new BaseParams
            {
                Params =
                    new DynamicDictionary
                    {
                        {
                            "accId",
                            BasePersonalAccountHelper
                                .Current
                                .Id
                        }
                    },
                Files =
                    new Dictionary<string, FileData>()
            };
        }

        [Given(@"пользователь в повторном открытии заполняет поле Причина ""(.*)""")]
        public void ДопустимПользовательВПовторномОткрытииЗаполняетПолеПричина(string reason)
        {
            var reopenAccBaseParams = ScenarioContext.Current.Get<BaseParams>("ReopenAccBaseParams");

            if (reopenAccBaseParams == null)
            {
                throw new SpecFlowException("Перед заполнением Причины повторного открытия, необходимо вызвать действие Повторное открытие");
            }

            reopenAccBaseParams.Params.Add("Reason", reason);
        }

        [Given(@"пользователь в повторном открытии заполняет поле Дата повторного открытия ""(.*)""")]
        public void ДопустимПользовательВПовторномОткрытииЗаполняетПолеДатаПовторногоОткрытия(string reopenDate)
        {
            var parsedDate = reopenDate.DateParse();

            var reopenAccBaseParams = ScenarioContext.Current.Get<BaseParams>("ReopenAccBaseParams");

            if (reopenAccBaseParams == null)
            {
                throw new SpecFlowException("Перед заполнением Даты повторного открытия, необходимо вызвать действие Повторное открытие");
            }

            reopenAccBaseParams.Params.Add("openDate", parsedDate);
        }

        [Given(@"пользователь в повторном открытии заполняет поле Документ-основание ""(.*)""")]
        public void ДопустимПользовательВПовторномОткрытииЗаполняетПолеДокумент_Основание(string fullFileName)
        {
            var reopenAccBaseParams = ScenarioContext.Current.Get<BaseParams>("ReopenAccBaseParams");

            if (reopenAccBaseParams == null)
            {
                throw new SpecFlowException("Перед заполнением Даты повторного открытия, необходимо вызвать действие Повторное открытие");
            }

            var currentDirectory = Directory.GetCurrentDirectory();
            var path = string.Format("{0}\\.fileStorage\\{1}", currentDirectory, fullFileName);

            if (!File.Exists(path))
            {
                throw new FileNotFoundException(string.Format("Файл {0} не найден", path));
            }

            var data = File.ReadAllBytes(path);

            var fileInfo = new FileInfo(path);
            var fileName = fileInfo.Name.Remove(fileInfo.Name.LastIndexOf(fileInfo.Extension, System.StringComparison.Ordinal));


            var fileData = new FileData(fileName, fileInfo.Extension.Remove(0, 1), data);

            reopenAccBaseParams.Files["Document"] = fileData;
        }

        [When(@"пользователь сохраняет изменения сальдо ЛС")]
        public void ЕслиПользовательСохраняетИзмененияСальдоЛС()
        {
            var personalAccountService = Container.Resolve<IPersonalAccountChangeService>();

            personalAccountService.ChangePeriodBalance(this.saldoBaseParams);
        }

        [When(@"пользователь в реестре ЛС выбирает действие Выгрузка - Предпросмотр документов на оплату")]
        public void ЕслиПользовательВРеестреЛСВыбираетДействиеВыгрузка_ПредпросмотрДокументовНаОплату()
        {
            var controllerProvider = new ControllerProvider(Container);

            dynamic basePersonalAccountController = controllerProvider.GetController(
                Container,
                BasePersonalAccountHelper.Type.Name);

            try
            {
                JsonNetResult result = basePersonalAccountController.GetType()
                    .GetMethod("GetPaymentDocumentsHierarchyPreview")
                    .Invoke(basePersonalAccountController, new object[] { BasePersonalAccountHelper.FilterBaseParams });

                if (result.StatusCode != 200)
                {
                    ExceptionHelper
                        .AddException(
                        "BasePersonalAccountController.GetPaymentDocumentsHierarchyPreview",
                        result.StatusCode.ToString());
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.AddException(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [When(@"пользователь в реестре ЛС выбирает предпросмотр текущего ЛС")]
        public void ЕслиПользовательВРеестреЛСВыбираетПредпросмотрТекущегоЛС()
        {
            var paymentDocumentServiceInterface =
                Type.GetType(
                    "Bars.Gkh.RegOperator.DomainService.PersonalAccount.IPaymentDocumentService, Bars.Gkh.RegOperator");

            dynamic paymentDocumentService = Container.Resolve(paymentDocumentServiceInterface);

            try
            {
                paymentDocumentService.GetType()
                    .GetMethod("GetPaymentDocument")
                    .Invoke(paymentDocumentService, new object[] { BasePersonalAccountHelper.Current, ChargePeriodHelper.Current, false });
            }
            catch (Exception ex)
            {
                ExceptionHelper.AddException(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [When(@"Пользователь в реестре ЛС выбирает действие Выгрузка - Документы на оплату")]
        public void ЕслиПользовательВРеестреЛСВыбираетДействиеВыгрузка_ДокументыНаОплату()
        {
            var paymentDocumentServiceInterface = 
                Type.GetType(
                    "Bars.Gkh.RegOperator.DomainService.PersonalAccount.IPaymentDocumentService, Bars.Gkh.RegOperator");

            dynamic paymentDocumentService = Container.Resolve(paymentDocumentServiceInterface);

            var baseParams = new BaseParams();

            baseParams.Params.Add("accountIds", BasePersonalAccountHelper.Current.Id);
            baseParams.Params.Add("isZeroPaymentDoc", "false");
            baseParams.Params.Add("showAll", "false");
            baseParams.Params.Add("reportPerAccount", "true");
            baseParams.Params.Add("reportId", "PaymentDocument");
            baseParams.Params.Add("periodId", BasePersonalAccountHelper.FilterBaseParams.Params["periodId"]);

            baseParams.Params.Add("period", BasePersonalAccountHelper.FilterBaseParams.Params["periodId"]);
            baseParams.Params.Add("sourceUIForm", "PersonalAccountGrid");

            CreateTasksDataResult result =
                paymentDocumentService.GetType()
                    .GetMethod("GetPaymentDocuments")
                    .Invoke(paymentDocumentService, new object[] { baseParams });

            if (!result.Success)
            {
                ExceptionHelper.AddException("IPaymentDocumentService.GetPaymentDocuments", result.Message);
            }

            ScenarioContext.Current["TaskEntryParentTaskId"] = result.Data.ParentTaskId;
        }

        [When(@"пользователь удаляет лицевой счет с номером  ""(.*)""")]
        public void ЕслиПользовательУдаляетЛицевойСчетСНомером(string basePersonalAccountNumber)
        {
            var basePersonalAccountList = BasePersonalAccountHelper.DomainService.GetAll();

            if (basePersonalAccountList == null || !basePersonalAccountList.Any())
            {
                throw new SpecFlowException(string.Format("Отсутствуют лицевые счета. {0}", ExceptionHelper.GetExceptions()));
            }

            var requiredBasePersonalAccount = basePersonalAccountList
                .FirstOrDefault(x => x.PersonalAccountNum == basePersonalAccountNumber);

            if (requiredBasePersonalAccount == null)
            {
                throw new SpecFlowException(
                    string.Format(
                        "Отсутствует лицевой счёт с номером {0}. {1}",
                        basePersonalAccountNumber,
                        ExceptionHelper.GetExceptions()));
            }

            BasePersonalAccountHelper.Current = requiredBasePersonalAccount;

            var result = Container.Resolve<IPersonalAccountService>()
                .RemoveAccounts(new long[] { requiredBasePersonalAccount.Id });

            if (result.Success != true)
            {
                ExceptionHelper.AddException("IPersonalAccountService.RemoveAccounts", result.Message);
            }
        }

        [When(@"пользователь в смене абонента заполняет поле Новый владелец абонентом ""(.*)""")]
        public void ЕслиПользовательВСменеАбонентаЗаполняетПолеНовыйВладелецАбонентом(string name)
        {
            ScenarioContext.Current["newOwnerId"] =
                IndividualAccountListHelper.Current.FirstOrDefault(x => x.Name == name).Id;
        }

        [When(@"пользователь сохраняет эту смену абонента")]
        public void ЕслиПользовательСохраняетЭтуСменуАбонента()
        {
            var baseParams = new BaseParams
            {
                Params = new DynamicDictionary
                {
                    {"AccountId", ScenarioContext.Current["accId"]},
                    {"NewOwner", ScenarioContext.Current["newOwnerId"]},
                    {"ActualFrom", ScenarioContext.Current["actualFrom"]},
                    {"Reason", ""}
                },
                Files = new Dictionary<string, FileData>()
            };

            var result = Container.Resolve<IPersonalAccountChangeService>().ChangeOwner(baseParams);

            if (!result.Success)
            {
                throw new Exception(result.Message);
            }
        }

        [When(@"пользователь в смене абонента заполняет поле Дата начала действия ""(.*)""")]
        public void ЕслиПользовательВСменеАбонентаЗаполняетПолеДатаНачалаДействия(string date)
        {
            DateTime dateValue;

            if (DateTime.TryParse(date, out dateValue))
            {
                ScenarioContext.Current["actualFrom"] = dateValue;
            }
            else
            {
                ScenarioContext.Current["actualFrom"] = DateTime.Now;
            }
        }

        //[When(@"пользователь в смене доли собственности сохраняет изменения")]
        //public void ЕслиПользовательВСменеДолиСобственностиСохраняетИзменения()
        //{
        //    ExplicitSessionScope.CallInNewScope(() =>
        //    {
        //        var areaShareChangeProxy =
        //            ScenarioContext.Current.Get<AreaShareChangeProxy>("BasePersAccAreaShareChangeProxy");

        //        var service = Container.Resolve<IPersonalAccountChangeService>();
                
        //        try
        //        {
        //            service.ChangeAreaShare(
        //                areaShareChangeProxy.BasePersonalAccount,
        //                areaShareChangeProxy.NewAreaShare,
        //                areaShareChangeProxy.DateActual,
        //                areaShareChangeProxy.FileInfo);
        //        }
        //        catch (Exception ex)
        //        {
        //            ExceptionHelper.AddException("IPersonalAccountChangeService.ChangeAreaShare", ex.Message);

        //            Container.Resolve<ISessionProvider>().GetCurrentSession().Clear();
        //        }
        //    });
        //}

        [When(@"пользователь сохраняет смену абонента")]
        public void ЕслиПользовательСохраняетСменуАбонента()
        {
            var setNewOwnerBaseParams = ScenarioContext.Current.Get<BaseParams>("SetNewOwnerBaseParams");

            var result = Container.Resolve<IPersonalAccountChangeService>().ChangeOwner(setNewOwnerBaseParams);

            if (!result.Success)
            {
                ExceptionHelper.AddException("IPersonalAccountChangeService.ChangeOwner", result.Message);

                Container.Resolve<ISessionProvider>().GetCurrentSession().Clear();
            }
        }

        [When(@"пользователь сохраняет Повторное открытие")]
        public void ЕслиПользовательСохраняетПовторноеОткрытие()
        {
            var reopenAccBaseParams = ScenarioContext.Current.Get<BaseParams>("ReopenAccBaseParams");
            var reOpenAccountOperation = Container.Resolve<IPersonalAccountOperation>(reopenAccBaseParams);

            try
            {
                var result = reOpenAccountOperation.Execute(reopenAccBaseParams);

                if (!result.Success)
                {
                    ExceptionHelper.AddException("IPersonalAccountOperation.ReOpenAccountOperation", result.Message);

                    Container.Resolve<ISessionProvider>().GetCurrentSession().Clear();
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.AddException("IPersonalAccountOperation.ReOpenAccountOperation", ex.Message);

                Container.Resolve<ISessionProvider>().GetCurrentSession().Clear();
            }
            finally
            {
                Container.Release(reOpenAccountOperation);
            }
        }

        [Then(@"у этого ЛС в карточке заполнено поле ФИО/Наименование абонента ""(.*)""")]
        public void ТоУЭтогоЛСВКарточкеЗаполненоПолеФИОНаименованиеАбонента(string ownerName)
        {
            var personalAccount =
                Container.Resolve<IDomainService<BasePersonalAccount>>().Get(BasePersonalAccountHelper.Current.Id);

            personalAccount.AccountOwner.Name.Trim().Should()
                .Be(
                    ownerName,
                    string.Format("у этого ЛС в карточке поле ФИО/Наименование абонента должно быть {0}", ownerName));
        }

        [Then(@"у этого ЛС в карточке заполнено поле Начислено пени всего ""(.*)""")]
        public void ТоВКарточкеЛсЗаполненоПолеНачисленоПениВсего(decimal сhargedPenalty)
        {
            var baseParams = new BaseParams
            {
                Params = { { "id", BasePersonalAccountHelper.Current.Id } }
            };

            var basePersonalAccountVm = Container.Resolve<IViewModel<BasePersonalAccount>>();

            var basePersonalAccountProxy =
                basePersonalAccountVm.Get(BasePersonalAccountHelper.DomainService, baseParams).Data;

            var actualChargedPenalty = ReflectionHelper.GetPropertyValue<decimal>(basePersonalAccountProxy, "ChargedPenalty");

            actualChargedPenalty.Should()
                .Be(
                    сhargedPenalty,
                    string.Format(
                        "у этого лицевого счета поле Начислено пени всего должно быть {0}",
                        сhargedPenalty));
        }

        [Then(@"у этого ЛС в карточке заполнено поле Задолженность пени всего ""(.*)""")]
        public void ТоВКарточкеЛсЗаполненоПолеЗадолженностьПениВсего(decimal debtPenalty)
        {
            var baseParams = new BaseParams
            {
                Params = { { "id", BasePersonalAccountHelper.Current.Id } }
            };

            var basePersonalAccountVm = Container.Resolve<IViewModel<BasePersonalAccount>>();

            var basePersonalAccountProxy =
                basePersonalAccountVm.Get(BasePersonalAccountHelper.DomainService, baseParams).Data;

            var actualDebtPenalty = ReflectionHelper.GetPropertyValue<decimal>(basePersonalAccountProxy, "DebtPenalty");

            actualDebtPenalty.Should()
                .Be(
                    debtPenalty,
                    string.Format(
                        "у этого лицевого счета поле Задолженность пени всего должно быть {0}",
                        debtPenalty));
        }

        [Then(@"у этого ЛС в карточке заполнено поле Доля собственности ""(.*)""")]
        public void ТоВКарточкеЛсЗаполненоПолеДоляСобственности(decimal areaShare)
        {
            var baseParams = new BaseParams
            {
                Params = { { "id", BasePersonalAccountHelper.Current.Id } }
            };

            var basePersonalAccountVm = Container.Resolve<IViewModel<BasePersonalAccount>>();

            var basePersonalAccountProxy =
                basePersonalAccountVm.Get(BasePersonalAccountHelper.DomainService, baseParams).Data;

            var actualAreaShare = ReflectionHelper.GetPropertyValue<decimal>(basePersonalAccountProxy, "AreaShare");

            actualAreaShare.Should()
                .Be(
                    areaShare,
                    string.Format(
                        "у этого лицевого счета поле Доля собственности должно быть {0}",
                        areaShare));
        }

        [Then(@"у этого ЛС в карточке заполнено поле Задолженность по взносам всего ""(.*)""")]
        public void ТоУЭтогоЛицевогоСчетаЗадолженностьПоВзносамВсего(decimal debtBaseTariff)
        {
            var baseParams = new BaseParams
                                 {
                                     Params = { { "id", BasePersonalAccountHelper.Current.Id } }
                                 };

             dynamic basePersonalAccountVm = Container.Resolve<IViewModel<BasePersonalAccount>>();

            dynamic basePersonalAccountProxy = 
                ((IDataResult)basePersonalAccountVm.Get(BasePersonalAccountHelper.DomainService, baseParams)).Data;

            decimal actualDebtBaseTariff = ReflectionHelper.GetPropertyValue(basePersonalAccountProxy, "DebtBaseTariff");
               
            actualDebtBaseTariff.Should()
                .Be(
                    debtBaseTariff,
                    string.Format(
                        "у этого лицевого счета Задолженность по взносам всего должна быть {0}",
                        debtBaseTariff));
        }

        [Then(@"у этого ЛС в карточке заполнено поле Итого задолженность ""(.*)""")]
        public void ТоУЭтогоЛицевогоСчетаИтогоЗадолженность(decimal totalDebt)
        {
            var baseParams = new BaseParams
            {
                Params = { { "id", BasePersonalAccountHelper.Current.Id } }
            };

            var viewModelType = typeof(IViewModel<>).MakeGenericType(BasePersonalAccountHelper.Type);

            dynamic basePersonalAccountVm = Container.Resolve(viewModelType);

            dynamic basePersonalAccountProxy = 
                ((IDataResult)basePersonalAccountVm.Get(BasePersonalAccountHelper.DomainService, baseParams))
                .Data;

            decimal actualTotalDebt = ReflectionHelper.GetPropertyValue(basePersonalAccountProxy, "TotalDebt");

            actualTotalDebt.Should()
                .Be(totalDebt, string.Format("у этого лицевого счета Итого задолженность должно быть {0}", totalDebt));
        }

        [Then(@"у этого ЛС в карточке заполнено поле Уплачено пени всего ""(.*)""")]
        public void ТоУЭтогоЛСВКарточкеЗаполненоПолеУплаченоПениВсего(decimal paymentPenalty)
        {
            var baseParams = new BaseParams
            {
                Params = { { "id", BasePersonalAccountHelper.Current.Id } }
            };

            var viewModelType = typeof(IViewModel<>).MakeGenericType(BasePersonalAccountHelper.Type);

            dynamic basePersonalAccountVm = Container.Resolve(viewModelType);

            dynamic basePersonalAccountProxy =
                ((IDataResult)basePersonalAccountVm.Get(BasePersonalAccountHelper.DomainService, baseParams))
                .Data;

            decimal actualPaymentPenalty = ReflectionHelper.GetPropertyValue(basePersonalAccountProxy, "PaymentPenalty");

            actualPaymentPenalty.Should()
                .Be(paymentPenalty, string.Format("у этого лицевого счета Уплачено пени всего должно быть {0}", paymentPenalty));
        }

        [Then(@"у этого ЛС в карточке заполнено поле Начислено взносов по минимальному тарифу всего ""(.*)""")]
        public void ТоУЭтогоЛСВКарточкеЗаполненоПолеНачисленоВзносовПоМинимальномуТарифуВсего(decimal chargedBaseTariff)
        {
            var baseParams = new BaseParams
            {
                Params = { { "id", BasePersonalAccountHelper.Current.Id } }
            };

            var viewModelType = typeof(IViewModel<>).MakeGenericType(BasePersonalAccountHelper.Type);

            dynamic basePersonalAccountVm = Container.Resolve(viewModelType);

            dynamic basePersonalAccountProxy =
                ((IDataResult)basePersonalAccountVm.Get(BasePersonalAccountHelper.DomainService, baseParams))
                .Data;

            decimal actualChargedBaseTariff = ReflectionHelper.GetPropertyValue(basePersonalAccountProxy, "ChargedBaseTariff");

            actualChargedBaseTariff.Should()
                .Be(chargedBaseTariff, string.Format("у этого лицевого счета Начислено взносов по минимальному тарифу всего должно быть {0}", chargedBaseTariff));
        }

        [Then(@"у этого ЛС в карточке заполнено поле Уплачено взносов по минимальному тарифу всего ""(.*)""")]
        public void ТоУЭтогоЛСВКарточкеЗаполненоПолеУплаченоВзносовПоМинимальномуТарифуВсего(int paymentBaseTariff)
        {
            var baseParams = new BaseParams
            {
                Params = { { "id", BasePersonalAccountHelper.Current.Id } }
            };

            var viewModelType = typeof(IViewModel<>).MakeGenericType(BasePersonalAccountHelper.Type);

            dynamic basePersonalAccountVm = Container.Resolve(viewModelType);

            dynamic basePersonalAccountProxy =
                ((IDataResult)basePersonalAccountVm.Get(BasePersonalAccountHelper.DomainService, baseParams))
                .Data;

            decimal actualPaymentBaseTariff = ReflectionHelper.GetPropertyValue(basePersonalAccountProxy, "PaymentBaseTariff");

            actualPaymentBaseTariff.Should()
                .Be(paymentBaseTariff, string.Format("у этого лицевого счета Уплачено взносов по минимальному тарифу всего должно быть {0}", paymentBaseTariff));
        }

        [Then(@"у этого лицевого счета в истории изменений присутствует запись")]
        public void ТоУЭтогоЛицевогоСчетаВИсторииИзмененийПрисутствуетЗапись()
        {
            var baseParams = new BaseParams { Params = { { "id", BasePersonalAccountHelper.Current.Id } } };

            dynamic service =
                Container.Resolve(
                    Type.GetType(
                        "Bars.Gkh.RegOperator.DomainService.PersonalAccount.IPersonalAccountOperationLogService, Bars.Gkh.RegOperator"));

            using (Container.Using((object)service))
            {
                var result = service.GetOperationLog(baseParams) as ListDataResult;

                var data = ((IQueryable<dynamic>)result.Data).ToList();

                data.Should().NotBeEmpty(
                    "у этого лицевого счета в истории изменений должна присутствовать запись");

                var lastChange = data.OrderByDescending(x => x.DateApplied).FirstOrDefault();

                PersonalAccountOperationLogEntryHelper.Current = lastChange;
            }
        }

        [Then(@"у этого лицевого счета в истории изменений присутствует запись с наименованием параметра ""(.*)""")]
        public void ТоУЭтогоЛицевогоСчетаВИсторииИзмененийПрисутствуетЗаписьСНаименованиемПараметра(string paramName)
        {
            var baseParams = new BaseParams { Params = { { "id", BasePersonalAccountHelper.Current.Id } } };

            dynamic service =
                Container.Resolve(
                    Type.GetType(
                        "Bars.Gkh.RegOperator.DomainService.PersonalAccount.IPersonalAccountOperationLogService, Bars.Gkh.RegOperator"));

            using (Container.Using((object)service))
            {
                var result = service.GetOperationLog(baseParams) as ListDataResult;

                var data = ((IQueryable<dynamic>)result.Data).ToList();

                var lastChange = data.OrderByDescending(x => x.DateApplied)
                    .FirstOrDefault(x => ReflectionHelper.GetPropertyValue<string>(x, "ParameterName") == paramName);

                AssertionExtensions.Should((object)lastChange).NotBeNull(string.Format(
                            "у этого лицевого счета в истории изменений должна присутствовать запись с наименованием параметра {0}",
                            paramName));

                PersonalAccountOperationLogEntryHelper.Current = lastChange;
            }
        }

        [Then(@"у этого лицевого счета в истории изменений отсутствует запись с наименованием параметра ""(.*)""")]
        public void ТоУЭтогоЛицевогоСчетаВИсторииИзмененийОтсутствуетЗаписьСНаименованиемПараметра(string paramName)
        {
            var baseParams = new BaseParams { Params = { { "id", BasePersonalAccountHelper.Current.Id } } };

            dynamic service =
                Container.Resolve(
                    Type.GetType(
                        "Bars.Gkh.RegOperator.DomainService.PersonalAccount.IPersonalAccountOperationLogService, Bars.Gkh.RegOperator"));

            using (Container.Using((object)service))
            {
                var result = service.GetOperationLog(baseParams) as ListDataResult;

                var data = ((IQueryable<dynamic>)result.Data).ToList();

                data.Any(x => ReflectionHelper.GetPropertyValue<string>(x, "ParameterName") == paramName)
                    .Should()
                    .BeFalse(
                        string.Format(
                            "у этого лицевого счета в истории изменений должна отсутствовать запись с наименованием параметра {0}",
                            paramName));
            }
        }

        [Then(@"у этой записи, в истории изменений ЛС, Наименование параметра ""(.*)""")]
        public void ТоУЭтойЗаписиВИсторииИзмененийЛСНаименованиеПараметра(string paramNameExpected)
        {
            string parametrName = PersonalAccountOperationLogEntryHelper.Current.ParameterName;

            parametrName
                .Should()
                .Be(
                    paramNameExpected,
                    string.Format("Наименование параметра записи в истории изменений должно быть {0}", paramNameExpected));
        }

        [Then(@"у этой записи, в истории изменений ЛС, Описание измененного атрибута ""(.*)""")]
        public void ТоУЭтойЗаписиВИсторииИзмененийЛСОписаниеИзмененногоАтрибута(string propertyDescriptionExpected)
        {
            string propertyDescription = PersonalAccountOperationLogEntryHelper.Current.PropertyDescription;

            //Добавил потому, чтобы не было зависимости от выбранной CultureInfo
            propertyDescription = propertyDescription.Replace(",", ".");
            propertyDescriptionExpected = propertyDescriptionExpected.Replace(",", ".");

            propertyDescription
                .Should()
                .Be(
                    propertyDescriptionExpected.FormatUsing(CultureInfo.InvariantCulture),
                    string.Format("Описание измененного атрибута записи в истории изменений должно быть {0}", propertyDescriptionExpected));
        }

        [Then(@"у этой записи, в истории изменений ЛС, Значение ""(.*)""")]
        public void ТоУЭтойЗаписиВИсторииИзмененийЛСЗначение(string propertyValueExpected)
        {
            string propertyValue = PersonalAccountOperationLogEntryHelper.Current.PropertyValue;

            propertyValue = propertyValue.Replace(",", ".");
            propertyValueExpected = propertyValueExpected.Replace(",", ".");

            propertyValue
                 .Should()
                 .Be(
                     propertyValueExpected,
                     string.Format("Значение записи в истории изменений должно быть {0}", propertyValueExpected));
        }

        [Then(@"у этой записи, в истории изменений ЛС, Дата начала действия значения ""(.*)""")]
        public void ТоУЭтойЗаписиВИсторииИзмененийЛСДатаНачалаДействияЗначения(string dateActualChangeExpected)
        {
            var date = dateActualChangeExpected.DateParse().Value.Date;
            
            var dateActualChange = PersonalAccountOperationLogEntryHelper.Current.DateActualChange;

            if (dateActualChange == null)
            {
                throw new Exception("Отсутствует начала действия значения");
            }

            dateActualChange.Value.ToLocalTime().Date
                 .Should()
                 .Be(
                     date,
                     string.Format("Дата начала действия значения записи в истории изменений должно быть {0}", date));
        }

        [Then(@"у этой записи, в истории изменений ЛС, Дата установки значения ""(.*)""")]
        public void ТоУЭтойЗаписиВИсторииИзмененийЛСДатаУстановкиЗначения(string dateAppliedExpected)
        {
            var date = dateAppliedExpected.DateParse().Value.Date;

            var dateAppliedChange = PersonalAccountOperationLogEntryHelper.Current.DateApplied.ToLocalTime().Date;

            dateAppliedChange
                  .Should()
                  .Be(
                      date,
                      string.Format("Дата установки значения записи в истории изменений должно быть {0}", date));
        }

        [Then(@"у этой записи, в истории изменений ЛС, Причина ""(.*)""")]
        public void ТоУЭтойЗаписиВИсторииИзмененийЛСПричина(string reasonExpected)
        {
            string reason = PersonalAccountOperationLogEntryHelper.Current.Reason;

            reason
                .Should()
                .Be(
                    reasonExpected,
                    string.Format("Причина записи в истории изменений должна быть {0}", reasonExpected));
        }


        [Then(@"При добавлении нового сведения о помещениях есть возможность выбора этой комнаты")]
        public void ТоПриДобавленииНовогоСведенияОПомещенияхЕстьВозможностьВыбораЭтойКомнаты()
        {
            var dsRoom = Container.Resolve<IDomainService<Room>>();

            var baseParams  = new BaseParams();

            baseParams.Params.Add("realtyId", RealityObjectHelper.CurrentRealityObject.Id);
            baseParams.Params.Add("ownerId", IndividualAccountOwnerHelper.GetPropertyValue("Id"));

            var roomList = (IEnumerable<dynamic>)((ListDataResult)Container.Resolve<IViewModel<Room>>().List(dsRoom, baseParams)).Data;

            var roomId = RoomHelper.Current.Id;

            roomList.Any(x => ReflectionHelper.GetPropertyValue(x, "Id") == roomId)
                .Should().BeTrue("При добавлении нового сведения о помещениях должна быть возможность выбора этой комнаты");
        }

        [Then(@"этот лицевой счет отсутствует в списке лицевых счетов")]
        public void ТоЭтотЛицевойСчетОтсутствуетВСпискеЛицевыхСчетов()
        {
            //var bpAccountList = (List<dynamic>)Container.Resolve<IViewModel<BasePersonalAccount>>()
            //    .List(BasePersonalAccountHelper.DomainService, new BaseParams()).Data;

            BasePersonalAccountHelper.DomainService.GetAll().Any(x => x.Id == BasePersonalAccountHelper.Current.Id)
                .Should()
                .BeFalse(
                    string.Format(
                        "Лицевой счёт должен отсутствовать в реестре лицевых счетов. {0}",
                        ExceptionHelper.GetExceptions()));
        }

        [Then(@"у этого ЛС в карточке заполнено поле Статус ""(.*)""")]
        public void ТоУЭтогоЛицевогоСчетаЗаполненоПолеСтатус(string stateName)
        {
            var personalAcc =
                Container.Resolve<IDomainService<BasePersonalAccount>>().Get(BasePersonalAccountHelper.Current.Id);

            personalAcc.State.Name
                .Should().Be(stateName, string.Format("Наименование статуса ЛС должно быть {0}", stateName));
        }

        [Then(@"ЛС с текущими абонентом и помещением отсутствует в реестре ЛС")]
        public void ТоЛССТекущимиАбонентомИПомещениемОтсутствуетВРеестреЛС()
        {
            Container.Resolve<IDomainService<BasePersonalAccount>>()
                .GetAll()
                .FirstOrDefault(
                    x =>
                    x.AccountOwner.Id == PersonalAccountOwnerHelper.Current.Id && x.Room.Id == RoomHelper.Current.Id);
        }

        private class AreaShareChangeProxy
        {
            public BasePersonalAccount BasePersonalAccount { get; set; }

            public decimal NewAreaShare { get; set; }

            public DateTime DateActual { get; set; }

            public Bars.B4.Modules.FileStorage.FileInfo FileInfo { get; set; }
        }
    }
}
