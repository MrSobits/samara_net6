namespace Bars.Gkh.RegOperator.Report.PaymentDocument
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Analytics.Reports.Generators.Models;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.RegOperator.Domain.ParametersVersioning;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Properties;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    [Obsolete("use class SnapshotCreator")]
    public class InvoiceRegistryAndAct : Receipt
    {
        public bool _printAct;
        private IDomainService<CalculationParameterTrace> _calcParamTraceDomain;

        private IDomainService<PersonalAccountCharge> _accountChargeDomain;

        public InvoiceRegistryAndAct(IWindsorContainer container)
            : base(container)
        {
        }

        protected IDomainService<CalculationParameterTrace> CalcParamTraceDomain
        {
            get
            {
                return this._calcParamTraceDomain
                    ?? (this._calcParamTraceDomain = this.Container.ResolveDomain<CalculationParameterTrace>());
            }
        }

        protected IDomainService<PersonalAccountCharge> AccountChargeDomain
        {
            get
            {
                return this._accountChargeDomain
                    ?? (this._accountChargeDomain = this.Container.ResolveDomain<PersonalAccountCharge>());
            }
        }

        public override Stream GetTemplate()
        {
            return this._printAct
                ? new ReportTemplateBinary(Resources.PaymentDocumentInvoiceRegistryAndAct).GetTemplate()
                : new ReportTemplateBinary(Resources.PaymentDocumentInvoiceRegistry).GetTemplate();
        }

        protected override object GetData(ChargePeriod period)
        {
            this.WarmCache(period);

            var formatter = new ReportFormatter();

            /*var byAccountIds = Cache.GetCache<BasePersonalAccount>().GetEntities()//Container.ResolveDomain<BasePersonalAccount>().GetAll()
                .WhereIf(AccountIds.Any(), x => AccountIds.Contains(x.Id));*/

            var byOwnerIds = this.Cache.GetCache<BasePersonalAccount>()
                .GetEntities()
                .WhereIf(this.AccountIds.Any(), x => this.AccountIds.Contains(x.Id))
                .Where(x => this.OwnerIds.Contains(x.AccountOwner.Id))
                .OrderBy(x => x.Room.RealityObject.Municipality.Name)
                .ThenBy(x => x.Room.RealityObject.MoSettlement.Name)
                .ThenBy(x => x.Room.RealityObject.FiasAddress.PlaceName)
                .ThenBy(x => x.Room.RealityObject.FiasAddress.StreetName)
                .ThenBy(x => x.Room.RealityObject.FiasAddress.House + x.Room.RealityObject.FiasAddress.Letter, new NumericComparer());

            var accountChargeData = this.AccountChargeDomain.GetAll()
                .Where(x => x.ChargePeriod.Id == period.Id)
                .Where(x => x.IsActive)
                .Select(x => new
                {
                    accountId = x.BasePersonalAccount.Id,
                    x.Guid
                })
                .ToList();

            var calcParamTraceData = this.CalcParamTraceDomain.GetAll()
                .Where(x => x.CalculationType == CalculationTraceType.Charge)
                .Select(x => new { x.ParameterValues, x.CalculationGuid });

            var accountsData = byOwnerIds
                .Select(x => new
                {
                    x.Id,
                    x.Room.IsRoomHasNoNumber,
                    x.Room.Notation,
                    x.Room.RoomNum,
                    x.Room.RealityObject.FiasAddress.AddressName,
                    x.Room.Area,
                    x.AreaShare,
                    RoomType = x.Room.Type,
                    MunicipalityId = x.Room.RealityObject.Municipality.Id,
                    RoId = x.Room.RealityObject.Id,
                    x.Room.RealityObject.AreaLivingNotLivingMkd,
                    OwnerId = x.AccountOwner.Id,
                    x.PersonalAccountNum,
                    x.AccountOwner.OwnerType,
                    Account = x
                })
                .AsEnumerable()
                .Select(x =>
                {
                    var roomAddress = string.Format("{0} {1}", x.AddressName, x.IsRoomHasNoNumber ? x.Notation : ", кв. " + x.RoomNum);

                    //вычисление площади
                    //доля собственности
                    var areaShare = 0m;

                    //площадь
                    decimal area = 0m;

                    //площадь*долю собственности
                    decimal areaRoom = 0m;

                    decimal tarif = 0m;

                    var accountChargeGuid = accountChargeData.Where(y => y.accountId == x.Id).Select(y => y.Guid).FirstOrDefault();

                    if (accountChargeGuid != null)
                    {
                        var acountCalcParamTraceData = calcParamTraceData.FirstOrDefault(y => accountChargeGuid == y.CalculationGuid);

                        //но такого не должно быть!
                        if (acountCalcParamTraceData != null)
                        {
                            areaShare = acountCalcParamTraceData.ParameterValues.Get(VersionedParameters.AreaShare).ToDecimal();
                            tarif = acountCalcParamTraceData.ParameterValues.Get(VersionedParameters.BaseTariff).ToDecimal();
                            area = acountCalcParamTraceData.ParameterValues.Get(VersionedParameters.RoomArea).ToDecimal();
                            areaRoom = areaShare * area;
                        }
                    }
                    else
                    {
                        var tarifParam = this.ParamTracker.GetParameter(VersionedParameters.BaseTariff, x.Account, period);

                        tarif = tarifParam.GetActualByDate<decimal>(
                            x.Account,
                            period.EndDate ?? period.StartDate.AddMonths(1).AddDays(-1),
                            false).Value;

                        var areaParam = this.ParamTracker.GetParameter(VersionedParameters.RoomArea, x.Account, period);
                        area = areaParam.GetActualByDate<decimal?>(
                            x.Account,
                            period.EndDate ?? period.StartDate,
                            true,
                            true).Value ?? x.Area;

                        var areaShareParam = this.ParamTracker.GetParameter(VersionedParameters.AreaShare, x.Account, period);

                        areaShare = areaShareParam.GetActualByDate<decimal?>(
                            x.Account,
                            period.EndDate ?? period.StartDate,
                            true).Value ?? x.AreaShare;

                        areaRoom = area * areaShare;
                    }

                    var account = new Account
                    {
                        AccountId = x.Id,
                        OwnerId = x.OwnerId,
                        НомерЛС = x.PersonalAccountNum,
                        АдресПомещения = roomAddress,
                        Тариф = tarif,
                        ПлощадьПомещения = areaRoom,
                        ТипПомещения = x.RoomType.GetEnumMeta().Display,
                        НаименованиеРабот = string.Empty,
                        Сумма = 0,
                        RoId = x.RoId,
                        AreaLivingNotLivingMkd = x.AreaLivingNotLivingMkd
                    };

                    return account;
                })
                .ToList();

            var accountIdByOwnerDict = accountsData.GroupBy(x => x.OwnerId)
                .ToDictionary(x => x.Key,
                    x => new
                    {
                        x.First().AccountId,
                        x.First().RoId,
                        x.First().AreaLivingNotLivingMkd,
                        AreaShare = x.Sum(y => y.ПлощадьПомещения)
                    });

            var periodSummaryData =
                this.Cache.GetCache<PersonalAccountPeriodSummary>().GetEntities() //Container.ResolveDomain<PersonalAccountPeriodSummary>().GetAll()
                    .Where(x => this.OwnerIds.Contains(x.PersonalAccount.AccountOwner.Id))
                    .Where(x => x.Period.Id == this.PeriodId)
                    .Select(x => new
                    {
                        AccountId = x.PersonalAccount.Id,
                        OwnerId = x.PersonalAccount.AccountOwner.Id,
                        Recalc = x.RecalcByBaseTariff, // TODO fix recalc
                        x.ChargeTariff,
                        x.Penalty,
                        x.SaldoIn
                    })
                    .ToArray();

            var periodSummaryCache = this.Cache.GetCache<PersonalAccountPeriodSummary>();

            var allSummaries = periodSummaryCache.GetEntities()
                .Where(x => this.AccountIds.Contains(x.PersonalAccount.Id))
                .Where(x => x.Period.StartDate <= period.StartDate)
                .AsEnumerable()
                .GroupBy(x => x.PersonalAccount.Id)
                .ToDictionary(x => x.Key, x => x.Select(y => y));

            var periodSummaryDictByAccount = periodSummaryData
                .GroupBy(x => x.AccountId)
                .ToDictionary(x => x.Key, x => x.FirstOrDefault());

            var periodSummaryDictByOwner = periodSummaryData
                .GroupBy(x => x.OwnerId)
                .ToDictionary(x => x.Key,
                    x => new
                    {
                        Recalc = x.Sum(y => y.Recalc),
                        ChargeTariff = x.Sum(y => y.ChargeTariff),
                        Penalty = x.Sum(y => y.Penalty)
                    });

            var ownersDict = this.Cache.GetCache<LegalAccountOwner>().GetEntities() //Container.ResolveDomain<LegalAccountOwner>().GetAll()
                .Where(x => this.OwnerIds.Contains(x.Id))
                .Select(x => new
                {
                    OwnerId = x.Id,
                    ContragentName = x.Contragent.Name,
                    x.Contragent.Inn,
                    x.Contragent.Kpp,
                    AddressName = x.Contragent.With(c => c.FiasJuridicalAddress).With(c => c.AddressName),
                    FiasMailingAddress = x.Contragent.With(c => c.FiasMailingAddress),
                    ContragentId = (long?)x.Contragent.Id,
                    x.PrintAct
                })
                .AsEnumerable()
                .GroupBy(x => x.OwnerId)
                .ToDictionary(x => x.Key, x => x.First());

            var contragentIdList = ownersDict.Select(x => x.Value.ContragentId).Where(x => x.HasValue).ToList();

            var contragentContractDomain = this.Cache.GetCache<ContragentContact>(); //Container.ResolveDomain<ContragentContact>();

            var contragentContactsDict = contragentContractDomain.GetEntities() //.GetAll()
                .Where(x => contragentIdList.Contains(x.Contragent.Id))
                .Where(x => x.DateStartWork == null || x.DateStartWork <= period.EndDate)
                .Where(x => x.DateEndWork == null || x.DateEndWork >= period.StartDate)
                .Select(x => new { x.Contragent.Id, x.FullName, PositionName = x.Position.Name })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, x => x.GroupBy(y => y.PositionName.ToLower().Trim()).ToDictionary(y => y.Key, y => y.First().FullName));

            var contacts = contragentContractDomain.GetEntities() //.GetAll()
                .Where(x => this.RegOperator.Contragent.Id == x.Contragent.Id)
                .AsEnumerable()
                .GroupBy(x => x.Position.Name.Trim().ToUpper())
                .ToDictionary(x => x.Key,
                    y => y.Return(x =>
                    {
                        var rec = y.First();
                        return string.Format("{0} {1}. {2}.", rec.Surname, rec.Name.FirstOrDefault(), rec.Patronymic.FirstOrDefault());
                    }));

            var contragentBankCreditOrgDomain = this.Cache.GetCache<ContragentBankCreditOrg>();
            var contragentBankCreditOrgsDict = contragentBankCreditOrgDomain.GetEntities()
                .Where(x => contragentIdList.Contains(x.Contragent.Id))
                .Select(
                    x => new
                    {
                        ContragentId = x.Contragent.Id,
                        x.Name,
                        x.Okpo,
                        x.Okonh,
                        x.SettlementAccount
                    })
                .AsEnumerable()
                .GroupBy(x => x.ContragentId)
                .ToDictionary(x => x.Key, x => x.First());

            var accounts = accountsData
                .SelectMany(account =>
                {
                    var accountList = new List<Account>();

                    if (periodSummaryDictByAccount.ContainsKey(account.AccountId))
                    {
                        var periodSummary = periodSummaryDictByAccount[account.AccountId];

                        var operationsDict = new Dictionary<string, decimal>();
                        operationsDict["Взносы на капитальный ремонт"] = periodSummary.ChargeTariff;
                        operationsDict["Пени за несвоевременную оплату взноса"] = periodSummary.Penalty;
                        operationsDict["Перерасчёт"] = periodSummary.Recalc;

                        accountList.AddRange(
                            operationsDict
                                .Where(y => y.Value != 0)
                                .Select(y => new Account
                                {
                                    Сумма = y.Value,
                                    НаименованиеРабот = y.Key,
                                    OwnerId = account.OwnerId,
                                    НомерЛС = account.НомерЛС,
                                    АдресПомещения = account.АдресПомещения,
                                    ПлощадьПомещения = account.ПлощадьПомещения,
                                    Тариф = account.Тариф,
                                    ТипПомещения = account.ТипПомещения,
                                    RoId = account.RoId
                                }));
                    }

                    if (!accountList.Any())
                    {
                        accountList.Add(account);
                    }

                    return accountList;
                })
                .ToList();

            this._printAct = ownersDict.Values
                .FirstOrDefault(x => accountIdByOwnerDict.ContainsKey(x.OwnerId))
                .Return(x => x.PrintAct);

            var realObjIds = accounts.Select(x => x.RoId).ToArray();

            var roPaymentAccOperCreditList = this.RoPaymentAccOperationService.GetAll()
                .Where(x => (realObjIds.Contains(x.Account.RealityObject.Id)))
                .Where(x => (x.Date >= period.StartDate && x.Date < period.EndDate))
                .Where(x => (x.OperationType == PaymentOperationType.OutcomeAccountPayment))
                .AsEnumerable()
                .GroupBy(x => x.Account.RealityObject.Id)
                .ToDictionary(x => x.Key, y => y.Sum(x => x.OperationSum));

            var data = ownersDict.Values
                .Where(x => accountIdByOwnerDict.ContainsKey(x.OwnerId))
                .Select(x =>
                {
                    var record = new ExtendedReportRecord();
                    var hasActiveRegOpDecision = this.RegOperator != null
                        && !this.RealObjsWithCustomOwnDecType.Contains(accountIdByOwnerDict[x.OwnerId].RoId);
                    var bank = this.Banks.Get(accountIdByOwnerDict[x.OwnerId].RoId);

                    record.OwnerId = x.OwnerId;
                    record.Плательщик = x.ContragentName;
                    record.ИННСобственника = x.Inn;
                    record.КППСобственника = x.Kpp;
                    record.АдресКонтрагента = x.AddressName;

                    if (x.ContragentId != null)
                    {
                        var contragentCreditOrg = contragentBankCreditOrgsDict.Get(x.ContragentId.Value);
                        if (contragentCreditOrg != null)
                        {
                            record.Наименование = contragentCreditOrg.Name;
                            record.РСчетБанка = contragentCreditOrg.SettlementAccount;
                            record.ОКПО = contragentCreditOrg.Okpo;
                            record.ОКОНХ = contragentCreditOrg.Okonh;
                        }
                    }

                    if (x.FiasMailingAddress != null)
                    {
                        record.ПочтовыйИндексКонтрагента = x.FiasMailingAddress.PostCode;

                        // По задаче 44880 адрес должен выводится не так: Камчатский край, Карагинский р-н, с. Карага, ул. Лукашевского, д. 14
                        // а вот так: ул. Лукашевского, д. 14, с. Карага, Карагинский р-н, Камчатский край
                        var mailingAdress = x.FiasMailingAddress.AddressName;

                        var streetAdderss = x.FiasMailingAddress.StreetName.IsNotEmpty()
                            ? mailingAdress.Substring(mailingAdress.IndexOf(x.FiasMailingAddress.StreetName, StringComparison.Ordinal))
                            : string.Empty;

                        var other = streetAdderss.IsNotEmpty() ? mailingAdress.Replace(streetAdderss, string.Empty) : mailingAdress;

                        record.ПочтовыйАдресКонтрагента = streetAdderss;

                        var splitOther = other.Split(',');

                        var idx = splitOther.Length - 1;
                        var item = string.Empty;

                        // теперь оставшуюся част ьадреса пишем в обратнубю сторону чтобы получилось так
                        // ул. Лукашевского, д. 14, с. Карага, Карагинский р-н, Камчатский край
                        while (idx >= 0)
                        {
                            item = splitOther[idx];

                            if (item != null && !string.IsNullOrEmpty(item.Trim()))
                            {
                                record.ПочтовыйАдресКонтрагента += string.Format(", {0}", item.Trim());
                            }

                            idx--;
                        }
                    }
                    else if (x.ContragentId != null)
                    {
                        var contragentAccount = this.Container.Resolve<IDomainService<Contragent>>().Get(x.ContragentId);
                        record.ПочтовыйИндексКонтрагента = contragentAccount.FiasJuridicalAddress.PostCode;
                    }

                    record.НаименованиеПолучателяКраткое = hasActiveRegOpDecision ? this.RegOperator.Contragent.ShortName : string.Empty;

                    if (x.ContragentId.HasValue && contragentContactsDict.ContainsKey(x.ContragentId.Value))
                    {
                        var contragentContacts = contragentContactsDict[x.ContragentId.Value];

                        record.Руководитель = contragentContacts.Get("руководитель") ?? string.Empty;
                        record.ГлавБух = contragentContacts.Get("главный бухгалтер") ?? string.Empty;
                    }

                    record.ОбщаяПлощадь = accountIdByOwnerDict[x.OwnerId].AreaShare;
                    record.Номер = this.GetDocNumber(accountIdByOwnerDict[x.OwnerId].AccountId, period.Id);

                    record.НаименованиеПериода = period.Name;
                    record.ДатаНачалаПериода = period.StartDate.ToString("dd.MM.yyyy");
                    record.ДатаОкончанияПериода = period.EndDate.HasValue ? period.EndDate.Value.ToString("dd.MM.yyyy") : string.Empty;
                    record.ПериодНачислений = this.IsZeroPaymentDoc
                        ? string.Empty
                        : "за период с {0} года по {1} года".FormatUsing(record.ДатаНачалаПериода, record.ДатаОкончанияПериода);
                    record.МесяцГодНачисления = period.StartDate.ToString("MMMM yyyy");
                    record.ДатаДокумента = DateTime.Today.ToString("dd.MM.yyyy");
                    record.АдресКвартиры = accounts.Count() > 1 ? "В соответствии с реестром" : "---";
                    record.ЛицевойСчет = "---";

                    record.НаименованиеПолучателя = hasActiveRegOpDecision ? this.RegOperator.Contragent.Name : string.Empty;
                    record.ИннПолучателя = hasActiveRegOpDecision ? this.RegOperator.Contragent.Inn : string.Empty;
                    record.КппПолучателя = hasActiveRegOpDecision ? this.RegOperator.Contragent.Kpp : string.Empty;
                    record.ОргнПолучателя = hasActiveRegOpDecision ? this.RegOperator.Contragent.Ogrn : string.Empty;
                    record.РсчетПолучателя = bank != null ? bank.SettlementAccount : string.Empty;
                    record.АдресБанка = bank != null ? bank.Address : string.Empty;
                    record.НаименованиеБанка = bank != null ? bank.Name : string.Empty;
                    record.БикБанка = bank != null ? bank.Bik : string.Empty;
                    record.КсБанка = bank != null ? bank.CorrAccount : string.Empty;
                    record.ТелефоныПолучателя = hasActiveRegOpDecision ? this.RegOperator.Contragent.Phone : string.Empty;
                    record.АдресПолучателя = hasActiveRegOpDecision ? this.RegOperator.Contragent.MailingAddress : string.Empty;
                    record.РуководительПолучателя = contacts.Get("РУКОВОДИТЕЛЬ");
                    record.ГлБухПолучателя = contacts.Get("ГЛАВНЫЙ БУХГАЛТЕР");
                    record.НаименованиеПолучателяКраткое = hasActiveRegOpDecision ? this.RegOperator.Contragent.ShortName : string.Empty;

                    if (periodSummaryDictByOwner.ContainsKey(record.OwnerId) && !this.IsZeroPaymentDoc)
                    {
                        var periodSummary = periodSummaryDictByOwner[record.OwnerId];

                        var payedFkr = 0m;
                        var fkr = 0m;

                        if (accountIdByOwnerDict.ContainsKey(x.OwnerId) && allSummaries.ContainsKey(accountIdByOwnerDict[x.OwnerId].AccountId))
                        {
                            payedFkr = Math.Round(Math.Abs(allSummaries.Get(accountIdByOwnerDict[x.OwnerId].AccountId)
                                    .ReturnSafe(y => y.Sum(z => z.TariffPayment + z.TariffDecisionPayment))),
                                2,
                                MidpointRounding.AwayFromZero);
                        }

                        if (accountIdByOwnerDict.ContainsKey(x.OwnerId)
                            && roPaymentAccOperCreditList.ContainsKey(accountIdByOwnerDict[x.OwnerId].RoId))
                        {
                            fkr = Math.Round(
                                (decimal)
                                ((roPaymentAccOperCreditList[accountIdByOwnerDict[x.OwnerId].RoId]
                                    / accountIdByOwnerDict[x.OwnerId].AreaLivingNotLivingMkd
                                    * accountIdByOwnerDict[x.OwnerId].AreaShare)),
                                2,
                                MidpointRounding.AwayFromZero);
                        }

                        record.УплаченоФКР = payedFkr;
                        record.ПотраченоНаКР = fkr;

                        record.Итого = periodSummary.ChargeTariff;
                        record.Пени = periodSummary.Penalty;
                        record.Перерасчет = periodSummary.Recalc;
                        record.СуммаВсего = record.Итого + record.Пени + record.Перерасчет;
                    }
                    record.СуммаСтрокой = this.IsZeroPaymentDoc
                        ? string.Empty
                        : formatter.Format("СуммаСтрокой", record.СуммаВсего, CultureInfo.InvariantCulture.NumberFormat);
                    return record;
                })
                .ToList();

            this.DataSources.Add(new MetaData
            {
                SourceName = "ЛицевойСчет",
                MetaType = nameof(Account),
                Data = accounts
            });

            return data;
        }

        private class ExtendedReportRecord : ReportRecord
        {
            public string ИННСобственника { get; set; }

            public string КППСобственника { get; set; }

            public string Плательщик { get; set; }

            public string АдресКонтрагента { get; set; }

            public string ПочтовыйИндексКонтрагента { get; set; }

            public string ПочтовыйАдресКонтрагента { get; set; }

            public string Руководитель { get; set; }

            public string СуммаСтрокой { get; set; }

            public string ГлавБух { get; set; }

            public int Номер { get; set; }

            public string РуководительПолучателя { get; set; }

            public string ГлБухПолучателя { get; set; }

            public string НаименованиеПолучателяКраткое { get; set; }

            public long OwnerId { get; set; }

            public string ПериодНачислений { get; set; }

            public string Наименование { get; set; }

            public string РСчетБанка { get; set; }

            public string ОКПО { get; set; }

            public string ОКОНХ { get; set; }
        }

        private class Account
        {
            public string НомерЛС { get; set; }

            public string АдресПомещения { get; set; }

            public string ТипПомещения { get; set; }

            public decimal ПлощадьПомещения { get; set; }

            public decimal Тариф { get; set; }

            public decimal Сумма { get; set; }

            public string НаименованиеРабот { get; set; }

            public long OwnerId { get; set; }

            public long AccountId { get; set; }

            public long RoId { get; set; }

            public decimal? AreaLivingNotLivingMkd { get; set; }
        }
    }
}