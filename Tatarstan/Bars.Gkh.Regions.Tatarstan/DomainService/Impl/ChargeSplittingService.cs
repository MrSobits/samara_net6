namespace Bars.Gkh.Regions.Tatarstan.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Extensions;
    using Bars.Gkh.Modules.Gkh1468.Entities;
    using Bars.Gkh.Modules.Gkh1468.Entities.ContractPart;
    using Bars.Gkh.Regions.Tatarstan.DataProviders.Meta;
    using Bars.Gkh.Regions.Tatarstan.Entities;
    using Bars.Gkh.Regions.Tatarstan.Entities.Dicts;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    /// <summary>
    /// Сервис работы с расщеплением платежей
    /// </summary>
    internal class ChargeSplittingService : IChargeSplittingService, IBudgetOrgContractExportService, IPublicServiceOrgExportService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Менеджер пользователей
        /// </summary>
        public IGkhUserManager UserManager { get; set; }

        /// <summary>
        /// Домен <see cref="OperatorContragent" />
        /// </summary>
        public IDomainService<OperatorContragent> OperatorContragentDomain { get; set; }

        /// <summary>
        /// Домен <see cref="PublicServiceOrgContract" />
        /// </summary>
        public IDomainService<PublicServiceOrgContract> PublicServiceOrgContractDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="PublicServiceOrg"/>
        /// </summary>
        public IDomainService<PublicServiceOrg> PublicServiceOrgDomain { get; set; }

        /// <summary>
        /// Домен <see cref="PublicServiceOrgContractRealObj" />
        /// </summary>
        public IDomainService<PublicServiceOrgContractRealObj> RoInContractDomain { get; set; }

        /// <summary>
        /// Домен <see cref="RsoAndServicePerformerContract" />
        /// </summary>
        public IDomainService<RsoAndServicePerformerContract> RsoAndUoContractDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="BudgetOrgContract"/>
        /// </summary>
        public IDomainService<BudgetOrgContract> BudgetOrgContractDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="FuelEnergyResourceContract"/>
        /// </summary>
        public IDomainService<FuelEnergyResourceContract> FuelEnergyResourceContractDomain { get; set; }

        /// <summary>
        /// Домен <see cref="PublicServiceOrgContractService" />
        /// </summary>
        public IDomainService<PublicServiceOrgContractService> ServiceInContractDomain { get; set; }

        /// <summary>
        /// Домен <see cref="ContractPeriodSummUo" />
        /// </summary>
        public IDomainService<ContractPeriodSummUo> ContractPeriodSummUoDomain { get; set; }

        /// <summary>
        /// Домен <see cref="ContractPeriodSummRso" />
        /// </summary>
        public IDomainService<ContractPeriodSummRso> ContractPeriodSummRsoDomain { get; set; }

        /// <summary>
        /// Домен <see cref="PubServContractPeriodSumm" />
        /// </summary>
        public IDomainService<PubServContractPeriodSumm> PubServContractPeriodSummDomain { get; set; }

        /// <summary>
        /// Домен <see cref="ContractPeriodSummDetail" />
        /// </summary>
        public IDomainService<ContractPeriodSummDetail> ContractPeriodSummDetailDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="BudgetOrgContractPeriodSumm"/>
        /// </summary>
        public IDomainService<BudgetOrgContractPeriodSumm> BudgetOrgContractPeriodSummDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="ServiceOrgFuelEnergyResourcePeriodSumm"/>
        /// </summary>
        public IDomainService<ServiceOrgFuelEnergyResourcePeriodSumm> FuelEnergyResourcePeriodSummDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="FuelEnergyOrgContractDetail"/>
        /// </summary>
        public IDomainService<FuelEnergyOrgContractDetail> FuelEnergyOrgContractDetailDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="FuelEnergyOrgContractInfo"/>
        /// </summary>
        public IDomainService<FuelEnergyOrgContractInfo> FuelEnergyOrgContractInfoDomain { get; set; }

        /// <summary>
        /// Домен <see cref="ContractPeriod" />
        /// </summary>
        public IDomainService<ContractPeriod> ContractPeriodDomain { get; set; }

        /// <summary>
        /// Домен <see cref="PlanPaymentsPercentage" />
        /// </summary>
        public IDomainService<PlanPaymentsPercentage> PlanPaymentsPercentageDomain { get; set; }

        /// <summary>
        /// Провайдер статусов
        /// </summary>
        public IStateProvider StateProvider { get; set; }

        /// <summary>
        /// Для создания реестра договоров ТЭР
        /// </summary>
        private readonly List<ContractLight> contractsLightCache = new List<ContractLight>();

        private const string GasName = "Газ";
        private const string ElectrName = "Электроэнергия";

        /// <summary>
        /// Сформировать записи за период
        /// </summary>
        /// <param name="period">Отчетный период</param>
        /// <returns>Успешность</returns>
        public IDataResult CreateSummaries(ContractPeriod period)
        {
            var contractServiceQuery = this.ServiceInContractDomain.GetAll()
                .WherePeriodActiveIn(
                    period.StartDate, 
                    period.EndDate, 
                    x => x.ResOrgContract.DateStart ?? DateTime.MinValue, 
                    x => x.ResOrgContract.DateEnd ?? DateTime.MaxValue);

            this.CreateUoSummariesInternal(period, contractServiceQuery);
            this.CreateBoSummariesInternal(period, contractServiceQuery);
            this.CreateTerSummariesInternal(period, contractServiceQuery);

            return new BaseDataResult();
        }

        /// <summary>
        /// Пересчитать суммы
        /// </summary>
        /// <param name="periodSumm">Информация за период</param>
        /// <returns>Успешность</returns>
        public IDataResult RecalcSummary(PubServContractPeriodSumm periodSumm)
        {
            try
            {
                var details = this.ContractPeriodSummDetailDomain.GetAll()
                    .Where(x => x.ContractPeriodSumm.Id == periodSumm.Id)
                    .ToArray();

                if (details.Length > 0)
                {
                    periodSumm.ContractPeriodSummRso.ChargedManOrg = details.Sum(x => x.ChargedManOrg);
                    periodSumm.ContractPeriodSummRso.PaidManOrg = details.Sum(x => x.PaidManOrg);
                    periodSumm.ContractPeriodSummRso.SaldoOut = details.Sum(x => x.SaldoOut);

                    periodSumm.ContractPeriodSummUo.StartDebt = details.Sum(x => x.StartDebt);
                    periodSumm.ContractPeriodSummUo.ChargedResidents = details.Sum(x => x.ChargedResidents);
                    periodSumm.ContractPeriodSummUo.RecalcPrevPeriod = details.Sum(x => x.RecalcPrevPeriod);
                    periodSumm.ContractPeriodSummUo.ChangeSum = details.Sum(x => x.ChangeSum);
                    periodSumm.ContractPeriodSummUo.NoDeliverySum = details.Sum(x => x.NoDeliverySum);
                    periodSumm.ContractPeriodSummUo.PaidResidents = details.Sum(x => x.PaidResidents);
                    periodSumm.ContractPeriodSummUo.EndDebt = details.Sum(x => x.EndDebt);
                    periodSumm.ContractPeriodSummUo.ChargedToPay = details.Sum(x => x.ChargedToPay);
                    periodSumm.ContractPeriodSummUo.TransferredPubServOrg = details.Sum(x => x.TransferredPubServOrg);

                    this.ContractPeriodSummRsoDomain.Update(periodSumm.ContractPeriodSummRso);
                    this.ContractPeriodSummUoDomain.Update(periodSumm.ContractPeriodSummUo);
                }

                return new BaseDataResult();
            }
            catch (Exception exception)
            {
                return BaseDataResult.Error(exception.Message);
            }
        }

        /// <summary>
        /// Актуализировать сведения за период
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат</returns>
        public IDataResult ActualizeSummaries(BaseParams baseParams)
        {
            var periodId = baseParams.Params.GetAsId();

            var period = this.ContractPeriodDomain.Get(periodId);

            if (period.IsNull())
            {
                return BaseDataResult.Error("Не найден период");
            }

            this.CreateNotExistsSummaryDetails(period);

            this.CreateNotExistsSummaries(period);

            this.RecalcPeriodInfo(period);

            this.ContractPeriodDomain.Update(period);

            this.ActualizeFuelEnergyValuesInternal(period);

            return new BaseDataResult();
        }

        /// <summary>
        /// Актуализировать сведения по договорам ТЭР
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат запроса</returns>
        public IDataResult ActualizeFuelEnergyValues(BaseParams baseParams)
        {
            var periodId = baseParams.Params.GetAsId("periodId");

            var period = this.ContractPeriodDomain.Get(periodId);

            if (period.IsNull())
            {
                return BaseDataResult.Error("Не найден период");
            }

            try
            {
                this.ActualizeFuelEnergyValuesInternal(period);
            }
            catch (Exception exception)
            {
                return BaseDataResult.Error(exception.Message);
            }

            return new BaseDataResult();
        }

        private void ActualizeFuelEnergyValuesInternal(ContractPeriod period)
        {
            var uoSummaries = this.PubServContractPeriodSummDomain.GetAll()
                .Where(x => x.ContractPeriod == period)
                .Select(
                    x => new
                    {
                        x.ContractPeriodSummRso.PublicServiceOrg,
                        x.PublicService.Service,
                        Charged = x.ContractPeriodSummRso.ChargedManOrg,
                        Debt = x.ContractPeriodSummRso.SaldoOut,
                        Paid = x.ContractPeriodSummRso.PaidManOrg
                    })
                .AsEnumerable()
                .GroupBy(x => Tuple.Create(x.PublicServiceOrg, x.Service))
                .ToDictionary(
                    x => x.Key,
                    x => new ContractSummaryLight(
                        x.SafeSum(y => y.Charged),
                        x.SafeSum(y => y.Paid),
                        x.SafeSum(y => y.Debt)));

            var boSummaries = this.BudgetOrgContractPeriodSummDomain.GetAll()
                .Where(x => x.ContractPeriod == period)
                .Select(
                    x => new
                    {
                        x.PublicServiceOrg,
                        x.ContractService.Service,
                        x.Charged,
                        Debt = x.EndDebt,
                        x.Paid
                    })
                .AsEnumerable()
                .GroupBy(x => Tuple.Create(x.PublicServiceOrg, x.Service))
                .ToDictionary(
                    x => x.Key,
                    x => new ContractSummaryLight(
                        x.SafeSum(y => y.Charged),
                        x.SafeSum(y => y.Paid),
                        x.SafeSum(y => y.Debt)));

            // собираем все суммы в uoSummaries
            foreach (var boSummary in boSummaries)
            {
                if (uoSummaries.ContainsKey(boSummary.Key))
                {
                    var uoSummary = uoSummaries.Get(boSummary.Key);

                    uoSummary.Charged += boSummary.Value.Charged;
                    uoSummary.Debt += boSummary.Value.Debt;
                    uoSummary.Paid += boSummary.Value.Paid;
                }
                else
                {
                    uoSummaries.Add(boSummary.Key, boSummary.Value);
                }
            }

            var gasContractInfos = this.FuelEnergyOrgContractInfoDomain.GetAll()
                .Where(x => x.PeriodSummary.ContractPeriod == period)
                .Where(x => x.Resource.Name == ChargeSplittingService.GasName)
                .ToDictionary(x => x.PeriodSummary);

            var electrContractInfos = this.FuelEnergyOrgContractInfoDomain.GetAll()
                .Where(x => x.PeriodSummary.ContractPeriod == period)
                .Where(x => x.Resource.Name == ChargeSplittingService.ElectrName)
                .ToDictionary(x => x.PeriodSummary);

            var terContractDetails = this.FuelEnergyOrgContractDetailDomain.GetAll()
                .Where(x => x.PeriodSumm.ContractPeriod == period)
                .ToArray();

            var contractDetailForUpdate = new List<FuelEnergyOrgContractDetail>();

            foreach (var contractDetail in terContractDetails)
            {
                var summary = uoSummaries.Get(contractDetail.PeriodSumm.PublicServiceOrg, contractDetail.Service);

                contractDetail.Charged = summary.Charged;
                contractDetail.Debt = summary.Debt;
                contractDetail.Paid = summary.Paid;

                contractDetailForUpdate.Add(contractDetail);
            }

            var planPaysDict = contractDetailForUpdate.Select(
                x => new
                {
                    x.PeriodSumm,
                    x.PlanPayGas,
                    x.PlanPayElectricity
                })
                .GroupBy(x => x.PeriodSumm)
                .ToDictionary(
                    x => x.Key,
                    x => new
                    {
                        GasSum = x.SafeSum(y => y.PlanPayGas),
                        ElectrSum = x.SafeSum(y => y.PlanPayElectricity)
                    });

            foreach (var gasContractInfo in gasContractInfos)
            {
                gasContractInfo.Value.PlanPaid = planPaysDict.Get(gasContractInfo.Key).GasSum;
            }

            foreach (var electrContractInfo in electrContractInfos)
            {
                electrContractInfo.Value.PlanPaid = planPaysDict.Get(electrContractInfo.Key).ElectrSum;
            }

            contractDetailForUpdate.ForEach(this.FuelEnergyOrgContractDetailDomain.Update);
            gasContractInfos.Values.ForEach(this.FuelEnergyOrgContractInfoDomain.Update);
            electrContractInfos.Values.ForEach(this.FuelEnergyOrgContractInfoDomain.Update);
        }

        /// <summary>
        /// Экспорт расщепления УО
        /// </summary>
        /// <param name="baseParams">базовые параметры</param>
        /// <returns>report</returns>
        ReportStreamResult IPublicServiceOrgExportService.ExportToCsv(BaseParams baseParams)
        {
            var serviceIds = baseParams.Params.GetAs("service", new List<long>());
            var municipalityIds = baseParams.Params.GetAs("municipality", new List<long>());
            var manOrgIds = baseParams.Params.GetAs("manOrg", new List<long>());
            var pubServOrgIds = baseParams.Params.GetAs("pubServOrg", new List<long>());
            var periodId = baseParams.Params.GetAs<long>("period");

            var text = new StringBuilder();
            var fileName = string.Empty;

            var activeOperator = this.UserManager.GetContragentIds();

            if (activeOperator != null)
            {
                var opeartor = this.PublicServiceOrgDomain.GetAll()
                    .Where(x => activeOperator.Contains(x.Contragent.Id))
                    .Select(x => new OperProxy
                    {
                        Id = x.Id,
                        Inn = x.Contragent.Inn,
                        Kpp = x.Contragent.Kpp,
                        Name = x.Contragent.Name,
                        Ogrn = x.Contragent.Ogrn,
                        Phone = x.Contragent.Phone
                    }).FirstOrDefault();

                if (opeartor != null)
                {
                    var period = this.ContractPeriodDomain.GetAll().First(y => y.Id == periodId);

                    var operators = new List<Оператор>
                    {
                        new Оператор
                        {
                            ВерсияФормата = "1.0",
                            Инн = opeartor.Inn,
                            Кпп = opeartor.Kpp,
                            ОгрнОгрнип = opeartor.Ogrn,
                            Год = period.StartDate.Year,
                            Месяц = period.StartDate.Month,
                            ДатаиВремяФормированияФайла = DateTime.Now.ToString("dd MMMM yyyy"),
                            ФиоОтправителя = opeartor.Name,
                            ТелефонОтправителя = opeartor.Phone,
                            ТипПоставщикаИнформации = "1",
                        }

                    }.AsEnumerable();

                    text.AppendLine(
                        "Версия формата;ИНН;КПП;ОГРН (ОГРНИП);Год;Месяц;Дата и время формирования файла;ФИО отправителя;Телефон отправителя;Тип поставщика информации");

                    foreach (var opers in operators)
                    {
                        text.AppendFormat(
                            "{0};{1};{2};{3};{4};{5};{6};{7};{8};{9}",
                            opers.ВерсияФормата,
                            opers.Инн,
                            opers.Кпп,
                            opers.ОгрнОгрнип,
                            opers.Год,
                            opers.Месяц,
                            opers.ДатаиВремяФормированияФайла,
                            opers.ТелефонОтправителя,
                            opers.ТипПоставщикаИнформации,
                            Environment.NewLine);
                    }

                    var contractIdList = this.PubServContractPeriodSummDomain.GetAll()
                        .Where(x => x.ContractPeriod.Id == periodId)
                        .Where(x => x.ContractPeriodSummRso.PublicServiceOrg.Id == opeartor.Id)
                        .WhereIfContains(serviceIds.IsNotEmpty(), x => x.PublicService.Service.Id, serviceIds)
                        .WhereIfContains(municipalityIds.IsNotEmpty(), x => x.Municipality.Id, municipalityIds)
                        .WhereIfContains(manOrgIds.IsNotEmpty(), x => x.ContractPeriodSummUo.ManagingOrganization.Id, manOrgIds)
                        .WhereIfContains(pubServOrgIds.IsNotEmpty(), x => x.ContractPeriodSummRso.PublicServiceOrg.Id, pubServOrgIds)
                        .Select(x => x.Id);

                    var result = this.ContractPeriodSummDetailDomain.GetAll()
                        .Where(x => contractIdList.Any(y => y == x.ContractPeriodSumm.Id))
                        .Select(
                            x =>
                                new ДоговорРесурсоснабжения
                                {
                                    УправляющаяОрганизацияИнн = x.ContractPeriodSumm.ContractPeriodSummUo.ManagingOrganization.Contragent.Inn,
                                    УправляющаяОрганизацияКпп = x.ContractPeriodSumm.ContractPeriodSummUo.ManagingOrganization.Contragent.Kpp,
                                    НаименованиеУправляющейОрганизации = x.ContractPeriodSumm.ContractPeriodSummUo.ManagingOrganization.Contragent.Name,
                                    РесурсоснабжающаяОрганизацияИнн = x.ContractPeriodSumm.ContractPeriodSummRso.PublicServiceOrg.Contragent.Inn,
                                    РесурсоснабжающаяОрганизацияКпп = x.ContractPeriodSumm.ContractPeriodSummRso.PublicServiceOrg.Contragent.Kpp,
                                    НаименованиеРесурсоснабжающаяОрганизация =
                                        x.ContractPeriodSumm.ContractPeriodSummRso.PublicServiceOrg.Contragent.Name,
                                    КодУслуги = x.ContractPeriodSumm.PublicService.Service.Code,
                                    НаименованиеУслуги = x.ContractPeriodSumm.PublicService.Service.Name,
                                    КодДома = x.PublicServiceOrgContractRealObjInContract.RealityObject.Id,
                                    АдресДома = x.PublicServiceOrgContractRealObjInContract.RealityObject.Address,
                                    НачисленоУОзаМесяц = x.ChargedManOrg,
                                    ПоступившиеОплатыОтУо = x.PaidManOrg,
                                    ИсходящееСальдо = x.SaldoOut
                                }).AsEnumerable();

                    text.AppendLine(
                        "ИНН;КПП;Наименование Управляющей организации;ИНН;КПП;Наименование РСО;Код Услуги;Наименование услуги;" +
                            "Код дома;Адрес дома;Начислено УО за месяц;Поступившие оплаты от УО;Исходящее сальдо;Сумма выставленных счетов РСО;Оплачено РСО");

                    foreach (var res in result)
                    {
                        text.AppendFormat(
                            "{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13}",
                            res.УправляющаяОрганизацияИнн,
                            res.УправляющаяОрганизацияКпп,
                            res.НаименованиеУправляющейОрганизации,
                            res.РесурсоснабжающаяОрганизацияИнн,
                            res.РесурсоснабжающаяОрганизацияКпп,
                            res.НаименованиеРесурсоснабжающаяОрганизация,
                            res.КодУслуги,
                            res.НаименованиеУслуги,
                            res.КодДома,
                            res.АдресДома,
                            res.НачисленоУОзаМесяц,
                            res.ПоступившиеОплатыОтУо,
                            res.ИсходящееСальдо,
                            Environment.NewLine);
                    }

                    var myString = text.ToString();
                    var myByteArray = Encoding.GetEncoding(1251).GetBytes(myString);
                    var ms = new MemoryStream(myByteArray);

                    var report = new ReportStreamResult(ms, $"ДоговорыРСО{opeartor.Inn}{DateTime.Now.Year}{DateTime.Now.Month}.csv");

                    return report;
                }
            }

            return new ReportStreamResult(Stream.Null, fileName);
        }

        /// <summary>
        /// Экспорт расщепления Бюджет
        /// </summary>
        /// <param name="baseParams">базовые параметры</param>
        /// <returns>report</returns>
        ReportStreamResult IBudgetOrgContractExportService.ExportToCsv(BaseParams baseParams)
        {
            var periodId = baseParams.Params.GetAsId("periodId");
            var serviceIds = baseParams.Params.GetAs<long[]>("serviceIds");
            var municipalityIds = baseParams.Params.GetAs<long[]>("municipalityIds");
            var orgIds = baseParams.Params.GetAs<long[]>("orgIds");
            var pubServOrgIds = baseParams.Params.GetAs<long[]>("pubServOrgIds");

            var activeOperator = this.UserManager.GetContragentIds();
            if (activeOperator != null)
            {
                var contragentOperator =
                    this.PublicServiceOrgDomain.GetAll()
                        .Where(x => this.PublicServiceOrgDomain.GetAll().Any(y => y.Contragent.Id == x.Contragent.Id))
                        .Where(x => activeOperator.Contains(x.Contragent.Id))
                        .Select(
                            x =>
                                new OperProxy
                                {
                                    Id = x.Id,
                                    Inn = x.Contragent.Inn,
                                    Kpp = x.Contragent.Kpp,
                                    Name = x.Contragent.Name,
                                    Ogrn = x.Contragent.Ogrn,
                                    Phone = x.Contragent.Phone
                                })
                        .FirstOrDefault();

                if (contragentOperator != null)
                {
                    var memoryStream = new MemoryStream();
                    var text = new StreamWriter(memoryStream);

                    var period = this.ContractPeriodDomain.GetAll().First(y => y.Id == periodId);

                    var currentOperator = new Оператор
                    {
                        ВерсияФормата = "1.0",
                        Инн = contragentOperator.Inn,
                        Кпп = contragentOperator.Kpp,
                        ОгрнОгрнип = contragentOperator.Ogrn,
                        Год = period.StartDate.Year,
                        Месяц = period.StartDate.Month,
                        ДатаиВремяФормированияФайла = DateTime.Now.ToString("dd MMMM yyyy"),
                        ФиоОтправителя = contragentOperator.Name,
                        ТелефонОтправителя = contragentOperator.Phone,
                        ТипПоставщикаИнформации = "1"
                    };

                    text.WriteLine(
                        "Версия формата;ИНН;КПП;ОГРН (ОГРНИП);Год;Месяц;Дата и время формирования файла;ФИО отправителя;Телефон отправителя;Тип поставщика информации");

                    text.WriteLine(
                            "{0};{1};{2};{3};{4};{5};{6};{7};{8};{9}",
                            currentOperator.ВерсияФормата,
                            currentOperator.Инн,
                            currentOperator.Кпп,
                            currentOperator.ОгрнОгрнип,
                            currentOperator.Год,
                            currentOperator.Месяц,
                            currentOperator.ДатаиВремяФормированияФайла,
                            currentOperator.ТелефонОтправителя,
                            currentOperator.ТипПоставщикаИнформации,
                            Environment.NewLine);

                    var result = this.BudgetOrgContractPeriodSummDomain.GetAll()
                        .Where(x => x.ContractPeriod.Id == periodId)
                        .WhereIf(serviceIds.IsNotEmpty(), x => serviceIds.Contains(x.ContractService.Service.Id))
                        .WhereIf(municipalityIds.IsNotEmpty(), x => municipalityIds.Contains(x.Municipality.Id))
                        .WhereIf(orgIds.IsNotEmpty(), x => orgIds.Contains(x.BudgetOrgContract.Organization.Id))
                        .WhereIf(pubServOrgIds.IsNotEmpty(), x => pubServOrgIds.Contains(x.PublicServiceOrg.Id))
                        .Select(x => new
                        {
                            x.BudgetOrgContract.TypeCustomer,
                            x.BudgetOrgContract.Organization.Inn,
                            x.BudgetOrgContract.Organization.Kpp,
                            x.BudgetOrgContract.Organization.Name,
                            InnRso = x.PublicServiceOrg.Contragent.Inn,
                            KppRso = x.PublicServiceOrg.Contragent.Kpp,
                            NameRso = x.PublicServiceOrg.Contragent.Name,
                            ServiceCode = x.ContractService.Service.Code,
                            ServiceName = x.ContractService.Service.Name,
                            x.Charged,
                            x.Paid,
                            x.EndDebt
                        });

                    text.WriteLine("Вид потребителя;ИНН;КПП;ИНН;КПП;Наименование РСО;Код Услуги;Наименование услуги;Начислено;Оплачено;Задолженность на конец месяца");

                    foreach (var res in result)
                    {
                        text.WriteLine(
                            "{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12}",
                            res.TypeCustomer,
                            res.Inn,
                            res.Kpp,
                            res.Name,
                            res.InnRso,
                            res.KppRso,
                            res.NameRso,
                            res.ServiceCode,
                            res.ServiceName,
                            res.Charged,
                            res.Paid,
                            res.EndDebt,
                            Environment.NewLine);
                    }

                    return new ReportStreamResult(memoryStream, $"ДоговорыБюджет{contragentOperator.Inn}{DateTime.Now.Year}{DateTime.Now.Month}.csv");
                }
            }

            return new ReportStreamResult(Stream.Null, string.Empty);
        }

        private void CreateUoSummariesInternal(ContractPeriod period, IQueryable<PublicServiceOrgContractService> contractserviceQuery)
        {
            var rsoAndUoContractQuery = this.RsoAndUoContractDomain.GetAll()
                .Where(x => contractserviceQuery.Any(y => y.ResOrgContract == x.PublicServiceOrgContract));

            var rsoAndUoContractDict = rsoAndUoContractQuery
                .Select(
                    x => new
                    {
                        ContractId = x.PublicServiceOrgContract.Id,
                        Uo = x.ManagingOrganization
                    })
                .ToDictionary(x => x.ContractId, y => y.Uo);

            var serviceInContractDict = contractserviceQuery
                .Select(
                    x => new
                    {
                        ContractId = x.ResOrgContract.Id,
                        Service = x
                    })
                .AsEnumerable()
                .GroupBy(x => x.ContractId)
                .ToDictionary(x => x.Key, y => y.Select(x => x.Service).ToList());

            var rsoRoContractsQuery = this.PublicServiceOrgContractDomain.GetAll()
                .Where(x => contractserviceQuery.Any(y => y.ResOrgContract == x))
                .WhereContains(x => x.Id, rsoAndUoContractDict.Keys)
                .WhereContains(x => x.Id, serviceInContractDict.Keys)
                .Select(
                    x => new
                    {
                        x.Id,
                        Rso = x.PublicServiceOrg
                    });

            var contrPerSummUos = rsoRoContractsQuery
                .AsEnumerable()
                .Select(
                    x => new
                    {
                        ContractId = x.Id,
                        ContractPeriodSummUo = new ContractPeriodSummUo { ManagingOrganization = rsoAndUoContractDict.Get(x.Id) }
                    })
                .ToDictionary(x => x.ContractId, y => y.ContractPeriodSummUo);

            var contrPerSummRsos = rsoRoContractsQuery
                .AsEnumerable()
                .Select(
                    x => new
                    {
                        ContractId = x.Id,
                        ContractPeriodSummRso = new ContractPeriodSummRso { PublicServiceOrg = x.Rso }
                    })
                .ToDictionary(x => x.ContractId, y => y.ContractPeriodSummRso);

            var contrPerSumms = new List<PubServContractPeriodSumm>();

            var rsoRoContracts = rsoRoContractsQuery.ToList();

            foreach (var rsoRoContract in rsoRoContracts)
            {
                foreach (var service in serviceInContractDict.Get(rsoRoContract.Id))
                {
                    contrPerSumms.Add(new PubServContractPeriodSumm
                    {
                        Municipality = rsoAndUoContractDict.Get(rsoRoContract.Id).Contragent.Municipality,
                        ContractPeriod = period,
                        ContractPeriodSummRso = contrPerSummRsos.Get(rsoRoContract.Id),
                        ContractPeriodSummUo = contrPerSummUos.Get(rsoRoContract.Id),
                        PublicService = service
                    });
                }
            }

            foreach (var contrPerSummRso in contrPerSumms.Select(x => x.ContractPeriodSummRso))
            {
                this.StateProvider.SetDefaultState(contrPerSummRso);
                this.ContractPeriodSummRsoDomain.Save(contrPerSummRso);
            }

            foreach (var contrPerSummUo in contrPerSumms.Select(x => x.ContractPeriodSummUo))
            {
                this.StateProvider.SetDefaultState(contrPerSummUo);
                this.ContractPeriodSummUoDomain.Save(contrPerSummUo);
            }

            foreach (var contrPerSumm in contrPerSumms)
            {
                this.PubServContractPeriodSummDomain.Save(contrPerSumm);
            }

            this.CreateSummaryDetails(contrPerSumms.ToArray());

            this.RecalcPeriodInfo(period);

            this.contractsLightCache.AddRange(contrPerSumms.Select(x => new ContractLight(x.ContractPeriodSummRso.PublicServiceOrg, x.PublicService.Service)));
        }

        private void CreateBoSummariesInternal(ContractPeriod period, IQueryable<PublicServiceOrgContractService> contractServiceQuery)
        {
            var budgetOrgContractsQuery = this.BudgetOrgContractDomain.GetAll()
                .Where(x => contractServiceQuery.Any(y => y.ResOrgContract == x.PublicServiceOrgContract));

            var budgetOrgContracts = budgetOrgContractsQuery
                .Select(
                    x => new
                    {
                        ContractId = x.PublicServiceOrgContract.Id,
                        ContractPart = x
                    })
                .ToDictionary(x => x.ContractId, y => y.ContractPart);

            var serviceInContractDict = contractServiceQuery
                .Select(
                    x => new
                    {
                        ContractId = x.ResOrgContract.Id,
                        Service = x
                    })
                .AsEnumerable()
                .GroupBy(x => x.ContractId)
                .ToDictionary(x => x.Key, y => y.Select(x => x.Service).Distinct().ToList());

            var budgetOrgContractQuery = contractServiceQuery
                .WhereContains(x => x.ResOrgContract.Id, budgetOrgContracts.Keys)
                .WhereContains(x => x.ResOrgContract.Id, serviceInContractDict.Keys)
                .Select(x => x.ResOrgContract)
                .Select(
                    x => new
                    {
                        x.Id,
                        Rso = x.PublicServiceOrg
                    });

            var budgetOrgContractsForSave = new List<BudgetOrgContractPeriodSumm>();

            var budgetOrgContractList = budgetOrgContractQuery.ToList();

            foreach (var budgetOrgContract in budgetOrgContractList.Distinct(x => x.Id))
            {
                foreach (var service in serviceInContractDict.Get(budgetOrgContract.Id))
                {
                    budgetOrgContractsForSave.Add(new BudgetOrgContractPeriodSumm
                    {
                        Municipality = budgetOrgContracts.Get(budgetOrgContract.Id).PublicServiceOrgContract.PublicServiceOrg.Contragent.Municipality,
                        ContractPeriod = period,
                        ContractService = service,
                        BudgetOrgContract = budgetOrgContracts.Get(budgetOrgContract.Id),
                        PublicServiceOrg = budgetOrgContracts.Get(budgetOrgContract.Id).PublicServiceOrgContract.PublicServiceOrg
                    });
                }
            }

            foreach (var contract in budgetOrgContractsForSave)
            {
                this.BudgetOrgContractPeriodSummDomain.Save(contract);
            }

            var excepted = budgetOrgContractsForSave
                .Select(x => new ContractLight(x.PublicServiceOrg, x.ContractService.Service))
                .Except(this.contractsLightCache, new ContractLightEqualityComparer());

            this.contractsLightCache.AddRange(excepted);
        }

        private void CreateTerSummariesInternal(ContractPeriod period, IQueryable<PublicServiceOrgContractService> contractServiceQuery)
        {
            var gasEnergyPercents = this.PlanPaymentsPercentageDomain.GetAll()
                .WherePeriodActiveIn(period.StartDate, period.EndDate, x => x.DateStart, x => x.DateEnd)
                .Where(x => x.Resource.Name == ChargeSplittingService.GasName)
                .ToDictionary(x => Tuple.Create(x.PublicServiceOrg, x.Service));

            var electrEnergyPercents = this.PlanPaymentsPercentageDomain.GetAll()
                .WherePeriodActiveIn(period.StartDate, period.EndDate, x => x.DateStart, x => x.DateEnd)
                .Where(x => x.Resource.Name == ChargeSplittingService.ElectrName) 
                .ToDictionary(x => Tuple.Create(x.PublicServiceOrg, x.Service));

            var rsoServiceDict = this.GetRsoServiceDict(contractServiceQuery);

            var rsoIds = rsoServiceDict.Keys.Select(x => x.Id).ToArray();

            var existServicesDict = this.FuelEnergyOrgContractDetailDomain.GetAll()
                .WhereContains(x => x.PeriodSumm.PublicServiceOrg.Id, rsoIds)
                .Where(x => x.PeriodSumm.ContractPeriod == period)
                .Select(x => new
                {
                    x.Service,
                    x.PeriodSumm.PublicServiceOrg
                })
                .AsEnumerable()
                .GroupBy(x => x.PublicServiceOrg)
                .ToDictionary(x => x.Key, y => y.Select(x => x.Service).ToHashSet());

            var existsPeriodSummaries = this.FuelEnergyResourcePeriodSummDomain.GetAll()
                .Where(x => x.ContractPeriod == period)
                .WhereContains(x => x.PublicServiceOrg.Id, rsoIds)
                .ToDictionary(x => x.PublicServiceOrg);

            var periodSumms = rsoServiceDict.Keys
                .Where(x => !existsPeriodSummaries.Keys.Contains(x))
                .Select(
                x => new ServiceOrgFuelEnergyResourcePeriodSumm
                {
                    Municipality = x.Contragent.Municipality,
                    ContractPeriod = period,
                    PublicServiceOrg = x
                })
                .Union(existsPeriodSummaries.Values)
                .ToDictionary(x => x.PublicServiceOrg);

            var contractDetails = new List<FuelEnergyOrgContractDetail>();

            foreach (var rsoService in rsoServiceDict)
            {
                foreach (var service in rsoService.Value)
                {
                    var serviceExists = existServicesDict.Get(rsoService.Key)?.Contains(service) ?? false;
                    serviceExists |= contractDetails.Any(x => x.Service == service && x.PeriodSumm.PublicServiceOrg == rsoService.Key);

                    if (gasEnergyPercents.ContainsKey(rsoService.Key, service)
                        && electrEnergyPercents.ContainsKey(rsoService.Key, service)
                        && !serviceExists)
                    {
                        // создаем, только если заполнен справочник процентов
                        contractDetails.Add(
                            new FuelEnergyOrgContractDetail
                            {
                                PeriodSumm = periodSumms.Get(rsoService.Key),
                                Service = service,
                                GasEnergyPercents = gasEnergyPercents.Get(rsoService.Key, service),
                                ElectricityEnergyPercents = electrEnergyPercents.Get(rsoService.Key, service)
                            });
                    }
                }
            }

            // TODO: Добавить больше условий на запросы. Иначе опасно работать с таким количеством словарей.
            var fuelEnergyResourceContractsQuery = this.FuelEnergyResourceContractDomain.GetAll()
                .Where(x => !this.FuelEnergyOrgContractInfoDomain.GetAll().Any(y => y.FuelEnergyResourceContract == x));

            var contracts = fuelEnergyResourceContractsQuery
                .ToDictionary(x => x.PublicServiceOrgContract);

            var resourceDict = this.ServiceInContractDomain.GetAll()
                .Where(x => x.Service.TypeService == TypeServiceGis.Communal)
                .Where(x => fuelEnergyResourceContractsQuery.Any(y => y.PublicServiceOrgContract == x.ResOrgContract))
                .Select(
                    x => new
                    {
                        Contract = x.ResOrgContract,
                        PublicServiceOrg = x.ResOrgContract.PublicServiceOrg,
                        Resource = x.CommunalResource
                    })
                .AsEnumerable()
                .GroupBy(x => x.PublicServiceOrg)
                .ToDictionary(
                    x => x.Key, 
                    y => y.GroupBy(x => x.Contract)
                        .ToDictionary(
                            x => x.Key, 
                            x => x.Select(z => z.Resource).ToArray()));

            var result = new List<FuelEnergyOrgContractInfo>();

            foreach (var periodSumm in periodSumms)
            {
                foreach (var communalResources in resourceDict.Get(periodSumm.Key).AllOrEmpty())
                {
                    foreach (var communalResource in communalResources.Value)
                    {
                        result.Add(new FuelEnergyOrgContractInfo
                        {
                            Resource = communalResource,
                            FuelEnergyResourceContract = contracts.Get(communalResources.Key),
                            PeriodSummary = periodSumm.Value
                        });
                    }
                }
            }

            periodSumms.Values.ForEach(this.FuelEnergyResourcePeriodSummDomain.SaveOrUpdate);
            contractDetails.ForEach(this.FuelEnergyOrgContractDetailDomain.Save);
            result.ForEach(this.FuelEnergyOrgContractInfoDomain.Save);
        }

        private Dictionary<PublicServiceOrg, ServiceDictionary[]> GetRsoServiceDict(IQueryable<PublicServiceOrgContractService> contractServiceQuery)
        {
            var dict = this.contractsLightCache.GroupBy(x => x.PublicServiceOrg)
                .ToDictionary(x => x.Key, x => x.Select(y => y.Service).Distinct().ToArray());

            if (dict.IsEmpty())
            {
                dict = contractServiceQuery
                    .AsEnumerable()
                    .GroupBy(x => x.ResOrgContract.PublicServiceOrg)
                    .ToDictionary(x => x.Key, x => x.Select(y => y.Service).Distinct().ToArray());
            }

            return dict;
        }

        private void RecalcPeriodInfo(ContractPeriod period)
        {
            var periodSummQuery = this.PubServContractPeriodSummDomain.GetAll().Where(x => x.ContractPeriod.Id == period.Id);

            period.RoNumber = this.ContractPeriodSummDetailDomain.GetAll()
                .Where(x => periodSummQuery.Any(y => y.Id == x.ContractPeriodSumm.Id))
                .Select(x => x.PublicServiceOrgContractRealObjInContract.RealityObject)
                .Distinct()
                .Count();

            period.RsoNumber = periodSummQuery.Select(x => x.ContractPeriodSummRso.PublicServiceOrg).Distinct().Count();

            period.UoNumber = periodSummQuery.Select(x => x.ContractPeriodSummUo.ManagingOrganization).Distinct().Count();
        }

        private void CreateSummaryDetails(PubServContractPeriodSumm[] contrPerSumms)
        {
            var existsRoInContractDict = this.ContractPeriodSummDetailDomain.GetAll()
                .WhereContains(x => x.ContractPeriodSumm, contrPerSumms)
                .GroupBy(x => x.ContractPeriodSumm.Id)
                .ToDictionary(x => x.Key, y => y.Select(x => x.PublicServiceOrgContractRealObjInContract).ToArray());

            var roInContractDict = this.RoInContractDomain.GetAll()
                .WhereContains(x => x.RsoContract.Id, contrPerSumms.Select(x => x.PublicService.ResOrgContract.Id))
                .Select(
                    x => new
                    {
                        ContractId = x.RsoContract.Id,
                        Ro = x
                    })
                .GroupBy(x => x.ContractId)
                .ToDictionary(x => x.Key, y => y.Select(z => z.Ro).ToArray());

            foreach (var contrPerSumm in contrPerSumms)
            {
                foreach (var ro in roInContractDict.Get(contrPerSumm.PublicService.ResOrgContract.Id))
                {
                    if (existsRoInContractDict.ContainsKey(contrPerSumm.Id) && existsRoInContractDict.Get(contrPerSumm.Id).Contains(ro))
                    {
                        continue;
                    }

                    var detail = new ContractPeriodSummDetail
                    {
                        PublicServiceOrgContractRealObjInContract = ro,
                        ContractPeriodSumm = contrPerSumm
                    };

                    this.ContractPeriodSummDetailDomain.Save(detail);
                }
            }
        }

        private void CreateNotExistsSummaries(ContractPeriod period)
        {
            // создание отсутствующих договоров с УК
            var existsPeriodSummQuery = this.PubServContractPeriodSummDomain.GetAll().Where(y => y.ContractPeriod == period);
            var contractTypeQuery = this.RsoAndUoContractDomain.GetAll().Select(x => x.PublicServiceOrgContract);

            var contractServiceQuery = this.ServiceInContractDomain.GetAll()
                .Where(x => x.Service.TypeService == TypeServiceGis.Communal)
                .WherePeriodActiveIn(
                    period.StartDate,
                    period.EndDate,
                    x => x.ResOrgContract.DateStart ?? DateTime.MinValue,
                    x => x.ResOrgContract.DateEnd ?? DateTime.MaxValue)
                .Where(x => contractTypeQuery.Any(y => y == x.ResOrgContract))
                .Where(x => !existsPeriodSummQuery.Any(y => y.PublicService == x));

            this.CreateUoSummariesInternal(period, contractServiceQuery);

            // создание отсутствующих договоров с Бюджетом
            var existsBudgetOrgSummQuery = this.BudgetOrgContractPeriodSummDomain.GetAll().Where(y => y.ContractPeriod == period);
            contractTypeQuery = this.BudgetOrgContractDomain.GetAll().Select(x => x.PublicServiceOrgContract);

            contractServiceQuery = this.ServiceInContractDomain.GetAll()
                .Where(x => x.Service.TypeService == TypeServiceGis.Communal)
                .WherePeriodActiveIn(
                    period.StartDate,
                    period.EndDate,
                    x => x.ResOrgContract.DateStart ?? DateTime.MinValue,
                    x => x.ResOrgContract.DateEnd ?? DateTime.MaxValue)
                .Where(x => contractTypeQuery.Any(y => y == x.ResOrgContract))
                .Where(x => !existsBudgetOrgSummQuery.Any(y => y.ContractService == x));

            this.CreateBoSummariesInternal(period, contractServiceQuery);

            // создание отсутствующих договоров ТЭР
            var existsFeulEnergySummQuery = this.FuelEnergyOrgContractDetailDomain.GetAll().Where(y => y.PeriodSumm.ContractPeriod == period);

            contractServiceQuery = this.ServiceInContractDomain.GetAll()
                .Where(x => x.Service.TypeService == TypeServiceGis.Communal)
                .WherePeriodActiveIn(
                    period.StartDate,
                    period.EndDate,
                    x => x.ResOrgContract.DateStart ?? DateTime.MinValue,
                    x => x.ResOrgContract.DateEnd ?? DateTime.MaxValue)
                .Where(x => !existsFeulEnergySummQuery.Any(y => y.PeriodSumm.PublicServiceOrg == x.ResOrgContract.PublicServiceOrg && y.Service == x.Service));

            this.CreateTerSummariesInternal(period, contractServiceQuery);
        }

        private void CreateNotExistsSummaryDetails(ContractPeriod period)
        {
            var existsPeriodSummQuery = this.PubServContractPeriodSummDomain.GetAll().Where(y => y.ContractPeriod == period);

            this.CreateSummaryDetails(existsPeriodSummQuery.ToArray());
        }
    }

    /// <summary>
    /// Прокси Контрагента.
    /// </summary>
    internal class OperProxy
    {
        /// <summary>
        /// Контрагент ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Контрагент ИНН
        /// </summary>
        public string Inn { get; set; }

        /// <summary>
        /// Контрагент КПП
        /// </summary>
        public string Kpp { get; set; }

        /// <summary>
        /// Контрагент ФИО
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Контрагент ОГРН
        /// </summary>
        public string Ogrn { get; set; }

        /// <summary>
        /// Контрагент телефон
        /// </summary>
        public string Phone { get; set; }

    }

    internal class ContractSummaryLight
    {
        public ContractSummaryLight(decimal charged, decimal paid, decimal debt)
        {
            this.Charged = charged;
            this.Debt = debt;
            this.Paid = paid;
        }

        /// <summary>
        /// Начислено за месяц
        /// </summary>
        public decimal Charged { get; set; }

        /// <summary>
        /// Оплачено за месяц
        /// </summary>
        public decimal Paid { get; set; }

        /// <summary>
        /// Задолженность на конец месяца
        /// </summary>
        public decimal Debt { get; set; }
    }

    internal class ContractLight
    {
        public ContractLight(PublicServiceOrg publicServiceOrg, ServiceDictionary service)
        {
            this.PublicServiceOrg = publicServiceOrg;
            this.Service = service;
        }

        /// <summary>
        /// РСО
        /// </summary>
        public PublicServiceOrg PublicServiceOrg { get; }

        /// <summary>
        /// Услуга
        /// </summary>
        public ServiceDictionary Service { get; }
    }

    internal class ContractLightEqualityComparer : IEqualityComparer<ContractLight>
    {
        public bool Equals(ContractLight с1, ContractLight с2)
        {
            if (с2 == null && с1 == null)
                return true;
            if (с1 == null || с2 == null)
                return false;
            if (с1.PublicServiceOrg == с2.PublicServiceOrg && с1.Service == с2.Service)
                return true;
            return false;
        }

        public int GetHashCode(ContractLight с)
        {
            var hCode = с.PublicServiceOrg.Id ^ с.Service.Id;
            return hCode.GetHashCode();
        }
    }
}