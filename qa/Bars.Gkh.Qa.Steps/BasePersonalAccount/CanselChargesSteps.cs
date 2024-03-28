using Bars.Gkh.RegOperator.Enums;

namespace Bars.Gkh.Qa.Steps
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using B4;
    using B4.IoC;
    using B4.Utils;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Qa.Steps.CommonSteps;
    using Bars.Gkh.RegOperator.Entities.Period;

    using FluentAssertions;

    using RegOperator.Domain.PersonalAccountOperations;
    using RegOperator.DomainService.PersonalAccount;
    using TechTalk.SpecFlow;
    using Utils;
    
    [Binding]
    public class CanselChargesSteps : BindingBase
    {
        BaseParams CancelChargesBaseParams = new BaseParams();

        [When(@"пользователь для текущего ЛС вызывает операцию Отмена начислений")]
        public void ЕслиПользовательДляТекущегоЛСВызываетОперациюОтменаНачислений()
        {
            var baseParams = new BaseParams
            {
                Params =
                {
                    {"periodId", ChargePeriodHelper.Current.Id},
                    {"persAccIds", BasePersonalAccountHelper.Current.Id}
                }
            };

            try
            {
                var result = Container.Resolve<IPersonalAccountService>().GetAccountChargeInfoInPeriod(baseParams);

                var charges = (IEnumerable<Object>)result.Data;

                ScenarioContext.Current["GetAccountChargeInfoInPeriodResult"] = charges;

                ScenarioContext.Current["charge"] = charges.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Bars.Gkh.Qa.Steps.ExceptionHelper.AddException(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [When(@"в форме Отмены начислений присутствует запись по отменяемым начислениям")]
        public void ЕслиВФормеОтменыНачисленийПрисутствуетЗаписьПоОтменяемымНачислениям()
        {
            var charges = ScenarioContext.Current["GetAccountChargeInfoInPeriodResult"] as IEnumerable<Object>;
            charges.Should().NotBeNullOrEmpty(String.Format("Список начислений ЛС {0} пуст", BasePersonalAccountHelper.Current.PersonalAccountNum));

            ScenarioContext.Current["charge"] = charges.FirstOrDefault();
        }

        [When(@"у этой записи по отменяемым начислениям заполнено поле Муниципальный район ""(.*)""")]
        public void ЕслиУЭтойЗаписиПоОтменяемымНачислениямЗаполненоПолеМуниципальныйРайон(string municipality)
        {
            object charge = ScenarioContext.Current["charge"];
            charge.GetType().GetProperty("Municipality").GetValue(charge)
                .Should().Be(municipality, "Наименование муципального района не совпадает с " + municipality);
        }

        [When(@"у этой записи по отменяемым начислениям заполнено поле Адрес ""(.*)""")]
        public void ЕслиУЭтойЗаписиПоОтменяемымНачислениямЗаполненоПолеАдрес(string address)
        {
            object charge = ScenarioContext.Current["charge"];
            charge.GetType().GetProperty("Address").GetValue(charge)
                .Should().Be(address, "Адрес не совпадает с " + address);
        }

        [When(@"у этой записи по отменяемым начислениям заполнено поле Номер ЛС ""(.*)""")]
        public void ЕслиУЭтойЗаписиПоОтменяемымНачислениямЗаполненоПолеНомерЛС(string personalAccountNum)
        {
            object charge = ScenarioContext.Current["charge"];
            charge.GetType().GetProperty("PersonalAccountNum").GetValue(charge).ToString().Replace(",", ".").Replace("0", String.Empty)
                .Should().Be(personalAccountNum.Replace(",", ".").Replace("0", String.Empty), "Номер ЛС не совпадает с " + personalAccountNum);
        }

        [When(@"у этой записи по отменяемым начислениям заполнено поле Сумма начислений за период ""(.*)""")]
        public void ЕслиУЭтойЗаписиПоОтменяемымНачислениямЗаполненоПолеСуммаНачисленийЗаПериод(string expectedChargeSum)
        {
            object charge = ScenarioContext.Current["charge"];

            var chargeSum = expectedChargeSum.DecimalParse();

            charge.GetType().GetProperty("ChargeSum").GetValue(charge)
                .Should().Be(chargeSum, "Сумма начислений за период не совпадает с " + expectedChargeSum);
        }

        [When(@"у этой записи по отменяемым начислениям заполнено поле Отменить начисления в размере ""(.*)""")]
        public void ЕслиУЭтойЗаписиПоОтменяемымНачислениямЗаполненоПолеОтменитьНачисленияВРазмере(decimal cancellationSum)
        {
            object charge = ScenarioContext.Current["charge"];
            charge.GetType().GetProperty("CancellationSum")
                .GetValue(charge).Should().Be(cancellationSum, "Поле Отменить начисления в размере не совпадает с " + cancellationSum);
        }

        [When(@"пользователь в отмене начислений заполняет поле Период ""(.*)""")]
        public void ЕслиПользовательЗаполняетПолеПериод(string periodName)
        {
            var periodList = Container.Resolve<IDomainService<PeriodCloseCheckResult>>().GetAll().ToList();

            var currentPeriod = periodName == "текущий"
                ? periodList.FirstOrDefault(x => x.CheckState != PeriodCloseCheckStateType.Pending)
                : periodList.FirstOrDefault(x => x.Name == periodName);

            if (currentPeriod == null)
            {
                throw new SpecFlowException(string.Format("Отсутствует период с наименованием {0}", periodName));
            }

            CancelChargesBaseParams.Params["chargePeriod"] = currentPeriod.Id;
            CancelChargesBaseParams.Params["chargePeriodId"] = currentPeriod.Id;
        }

        [When(@"пользователь в отмене начислений заполняет поле Причина ""(.*)""")]
        public void ЕслиПользовательЗаполняетПолеПричина(string reason)
        {
            CancelChargesBaseParams.Params["Reason"] = reason;
        }

        [When(@"пользователь в отмене начислений заполняет поле Документ-основание ""(.*)""")]
        public void ЕслиПользовательЗаполняетПолеДокумент_Основание(string documentName)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var path = string.Format("{0}\\.fileStorage\\{1}", currentDirectory, documentName);

            if (!File.Exists(path))
            {
                throw new FileNotFoundException(string.Format("Файл {0} не найден", path));
            }

            var data = File.ReadAllBytes(path);

            var fileInfo = new FileInfo(path);
            var fileName = fileInfo.Name.Remove(fileInfo.Name.LastIndexOf(fileInfo.Extension, System.StringComparison.Ordinal));


            var fileData = new FileData(fileName, fileInfo.Extension.Remove(0, 1), data);

            ScenarioContext.Current["CancelChargesDocument"] = fileData;
        }
        
        [When(@"пользователь сохраняет изменения в форме отмена начислений")]
        public void ЕслиПользовательСохраняетИзмененияВФормеОтменаНачислений()
        {
            object charge;
            DynamicDictionary[] modifRecs;

            var document = ScenarioContext.Current.Get<FileData>("CancelChargesDocument");

            try
            {
                charge = ScenarioContext.Current["charge"];

                modifRecs = new[]
                                {
                                    new DynamicDictionary
                                        {
                                            {
                                                "Id",
                                                charge.GetType().GetProperty("Id").GetValue(charge)
                                            },
                                            {
                                                "CancellationSum",
                                                charge.GetType()
                                                .GetProperty("CancellationSum")
                                                .GetValue(charge)
                                            }
                                        }
                                };
            }
            catch (Exception)
            {
                modifRecs = new DynamicDictionary[0];
            }

            CancelChargesBaseParams.Params["b4_pseudo_xhr"] = true;
            CancelChargesBaseParams.Params["modifRecs"] = modifRecs;
            CancelChargesBaseParams.Files = new Dictionary<string, FileData> { { "Document", document } };

            var cancelChargeOperation = Container.Resolve<IPersonalAccountOperation>("CancelChargeOperation");

            Container.Resolve<ISessionProvider>().GetCurrentSession().Clear();

            var result = cancelChargeOperation.Execute(CancelChargesBaseParams);

            if (!result.Success)
            {
                Bars.Gkh.Qa.Steps.ExceptionHelper.AddException(
                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                    result.Message);

                Container.Resolve<ISessionProvider>().GetCurrentSession().Clear();
            }
        }

        [When(@"пользователь для ЛС ""(.*)"" и ЛС ""(.*)"" вызывает операцию Отмена начислений")]
        public void ЕслиПользовательДляЛСИЛСВызываетОперациюОтменаНачислений(string p0, string p1)
        {
            var paymentPenalties = new BasePersonalAccountSteps();

            paymentPenalties.ДопустимПользовательВРеестреЛСВыбираетЛицевойСчет(p0);
            string persAccIds = BasePersonalAccountHelper.Current.Id.ToString() + ',';

            paymentPenalties = new BasePersonalAccountSteps();

            paymentPenalties.ДопустимПользовательВРеестреЛСВыбираетЛицевойСчет(p1);
            persAccIds += BasePersonalAccountHelper.Current.Id;

            var baseParams = new BaseParams
            {
                Params =
                {
                    {"periodId", ChargePeriodHelper.Current.Id},
                    {"persAccIds", persAccIds}
                }
            };

            try
            {
                ScenarioContext.Current["GetAccountChargeInfoInPeriodResultList"] = Container.Resolve<IPersonalAccountService>().GetAccountChargeInfoInPeriod(baseParams).Data;
            }
            catch (Exception ex)
            {
                Bars.Gkh.Qa.Steps.ExceptionHelper.AddException(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [When(@"присутствует запись по отменяемым начислениям по ЛС ""(.*)"" в форме Отмены начислений")]
        public void ЕслиПрисутствуетЗаписьПоОтменяемымНачислениямПоЛСВФормеОтменыНачислений(string personalAccountNum)
        {
            var charges = ScenarioContext.Current["GetAccountChargeInfoInPeriodResultList"] as IEnumerable<object>;
            var charge = charges.Where(x => x.GetType().GetProperty("PersonalAccountNum").GetValue(x).ToString() == personalAccountNum).FirstOrDefault();
            charge.Should().NotBeNull("Не найдена запись для лицевого счета " + personalAccountNum);
            ScenarioContext.Current["charge"] = charge;
        }

        [When(@"пользователь сохраняет изменения для двух лс в форме отмена начислений")]
        public void ЕслиПользовательСохраняетИзмененияДляДвухЛсВФормеОтменаНачислений()
        {
            var charges = ScenarioContext.Current["GetAccountChargeInfoInPeriodResultList"] as IEnumerable<object>;
            List<object> chargeList = new List<object>();
            foreach (var charge in charges)
            {
                chargeList.Add(new DynamicDictionary
                {
                    {"Id", charge.GetType().GetProperty("Id").GetValue(charge)},
                    {"CancellationSum", charge.GetType().GetProperty("CancellationSum").GetValue(charge)}
                });
            }

            CancelChargesBaseParams.Params["b4_pseudo_xhr"] = true;
            CancelChargesBaseParams.Params["chargePeriodId"] = ChargePeriodHelper.Current.Id;
            CancelChargesBaseParams.Params["modifRecs"] = chargeList;

            CancelChargesBaseParams.Files = new Dictionary<string, FileData> { { "Document", new FileData("txt", "new", new byte[0]) } };
            try
            {
                var cancelChargeOperation = Container.Resolve<IPersonalAccountOperation>("CancelChargeOperation");
                var result = cancelChargeOperation.Execute(CancelChargesBaseParams);
            }
            catch (Exception ex)
            {
                Bars.Gkh.Qa.Steps.ExceptionHelper.AddException(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [Then(@"в карточке ЛС в истории изменений нет записи ""(.*)""")]
        public void ТоВКарточкеЛСВИсторииИзмененийНетЗаписи(string paramName)
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

            string parametrName = PersonalAccountOperationLogEntryHelper.Current.ParameterName;

            parametrName
                .Should()
                .NotBe(paramName,
                    string.Format("Наименование параметра записи в истории изменений должно быть {0}", paramName));
        }

        [Then(@"количество записей ""(.*)"" в истории = (.*)")]
        public void ТоКоличествоЗаписейВИстории(string p0, int p1)
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

                var lastChange = data.Where(x => x.ParameterName.ToString() == p0).ToList();
                lastChange.Count.Should().Be(p1, "Количество записей " + p0 + " в истории изменений не равно" + p1);
            }
        }
    }
}
