namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc.SnapshotCreators
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Config;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Decisions.Nso.Entities.Decisions;
    using Bars.Gkh.Domain.Cache;
    using Bars.Gkh.Domain.PaymentDocumentNumber;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums.Decisions;
    using Bars.Gkh.RegOperator.DataProviders;
    using Bars.Gkh.RegOperator.DataProviders.Meta;
    using Bars.Gkh.RegOperator.Distribution.Impl;
    using Bars.Gkh.RegOperator.Domain.ParametersVersioning;
    using Bars.Gkh.RegOperator.DomainEvent;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Dict;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    using Newtonsoft.Json;

    using NHibernate.Mapping;

    /// <summary>
    /// Создатель слепков данных
    /// </summary>
    [System.Obsolete("use class SnapshotCreator")]
    internal class InvoiceRegistryAndActSnapshotCreator //: InvoiceInfoSnapshotCreatorBase<InvoiceRegistryAndActInfo>
    {
        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="container">IoC контейнер</param>
        /// <param name="cache">Кэш данных</param>
        /// <param name="configProv">Провайдер конфигурации</param>
        /// <param name="period">Перод сборки</param>
        /*public InvoiceRegistryAndActSnapshotCreator(
            IWindsorContainer container,
            GkhCache cache,
            IGkhConfigProvider configProv,
            ChargePeriod period) 
            : base(container, cache, configProv, period)
        {
        }
        
        /// <summary>
        /// Создание слепка данных
        /// </summary>
        /// <param name="isZeroPayment">Допускаются нулевые платежи</param>
        /// <param name="personalAccountOwnerType">Тип абонента</param>
        public override void CreateSnapshots(bool isZeroPayment, PersonalAccountOwnerType personalAccountOwnerType)
        {
            // Т.к. изначально кэш уже отфильтрован по ЛС,
            // то здесь уже не фильтруем
           
            this.WarmCache();

            var formatter = new ReportFormatter();
            
            var byOwnerIds = this.Cache.GetCache<BasePersonalAccount>()
                    .GetEntities()
                    .OrderBy(x => x.Room.RealityObject.Municipality.Name)
                    .ThenBy(x => x.Room.RealityObject.MoSettlement.Return(z => z.Name))
                    .ThenBy(x => x.Room.RealityObject.FiasAddress.PlaceName)
                    .ThenBy(x => x.Room.RealityObject.FiasAddress.StreetName)
                .ThenBy(x => x.Room.RealityObject.FiasAddress.House + x.Room.RealityObject.FiasAddress.Letter, new NumericComparer())
                .ToList();

            var persaccData = this.Cache.GetCache<BasePersonalAccount>()
                .GetEntities()
                .Select(x => new { x.Id, OwnerId = x.AccountOwner.Id, RoId = x.Room.RealityObject.Id })
                .ToDictionary(x => x.Id);

            var periodSummaryData = this.Cache.GetCache<PersonalAccountPeriodSummary>().GetEntities()
                    .Where(x => x.Period.Id == this.Period.Id)
                    .Select(x => new
                    {
                        AccountId = x.PersonalAccount.Id,
                        persaccData[x.PersonalAccount.Id].OwnerId,
                        Recalc = x.RecalcByBaseTariff,
                        x.ChargedByBaseTariff,
                        x.BaseTariffChange,
                        x.DecisionTariffChange,
                        x.PenaltyChange,
                        x.ChargeTariff,
                        x.Penalty,
                        x.SaldoIn,
                        x.SaldoOut,
                        BaseTariffDebt = x.BaseTariffDebt.RegopRoundDecimal(2),
                        x.DecisionTariffDebt,
                        x.PenaltyDebt,
                        x.RecalcByBaseTariff,
                        x.RecalcByDecisionTariff,
                        x.TariffPayment,
                        x.TariffDecisionPayment,
                        x.RecalcByPenalty,
                        x.PenaltyPayment,
                        persaccData[x.PersonalAccount.Id].RoId
                    })
                    .ToArray();
            var chargesDict = this.Cache.GetCache<PersonalAccountCharge>().GetEntities()
                .GroupBy(x => persaccData[x.BasePersonalAccount.Id].OwnerId)
                .ToDictionary(
                    x => x.Key, 
                    y => new
                    {
                        ChargeTariff = y.Sum(x => x.ChargeTariff),
                        OverPlus = y.Sum(x => x.OverPlus),
                        Penalty = y.Sum(x => x.Penalty)                                  
                    });

            var periodSummaryCache = this.Cache.GetCache<PersonalAccountPeriodSummary>();

            var allSummaries = periodSummaryCache.GetEntities()
                .GroupBy(x => x.PersonalAccount.Id)
                .ToDictionary(x => x.Key, x => x.Select(y => y));

            var periodSummaryDictByAccount = periodSummaryData
                .GroupBy(x => x.AccountId)
                .ToDictionary(x => x.Key, x => x.FirstOrDefault());

            var periodSummaryDictByOwner = periodSummaryData
                .GroupBy(x => x.OwnerId)
                .ToDictionary(x => x.Key, x => new
                {
                    Recalc = x.Sum(y => y.Recalc),
                    ChargeTariff = x.Sum(y => y.ChargeTariff),
                    Penalty = x.Sum(y => y.Penalty),
                    SaldoOut = x.Sum(y => y.SaldoOut),
                    ChargedByBaseTariff = x.Sum(y => y.ChargedByBaseTariff),
                    BaseTariffChange = x.Sum(y => y.BaseTariffChange),
                    PenaltyChange = x.Sum(y => y.PenaltyChange),
                    DecisionTariffChange = x.Sum(y => y.DecisionTariffChange),
                    BaseTariffDebt = x.Sum(y => y.BaseTariffDebt),
                    DecisionTariffDebt = x.Sum(y => y.DecisionTariffDebt),
                    PenaltyDebt = x.Sum(y => y.PenaltyDebt),
                    RecalcByBaseTariff = x.Sum(y => y.RecalcByBaseTariff),
                    RecalcByDecisionTariff = x.Sum(periodSummaryDatay => y.RecalcByDecisionTariff),
                    TariffPayment = x.Sum(y => y.TariffPayment),
                    TariffDecisionPayment = x.Sum(y => y.TariffDecisionPayment),
                    ReclacByPenalty = x.Sum(y => y.RecalcByPenalty),
                    PenaltyPayment = x.Sum(y => y.PenaltyPayment),
                    x.First().RoId
                });

            // словарь для получения переменных ОбщийДебет, ОбщийКредит, ОбщийДебетПени, ОбщийКредитПени
            var periodSummarySaldoDictByOwner = periodSummaryData
                .GroupBy(x => x.OwnerId)
                .ToDictionary(x => x.Key, x => new
                {
                    PenaltyDebet = x.Where(y => y.Penalty > 0).Sum(y => y.Penalty),
                    SaldoOutDebet = x.Where(y => y.SaldoOut > 0).Sum(y => y.SaldoOut),
                    PenaltyKredit = x.Where(y => y.Penalty < 0).Sum(y => y.Penalty),
                    SaldoOutKredit = x.Where(y => y.SaldoOut < 0).Sum(y => y.SaldoOut)
                });

            var commonData = new List<RecordPersonalAccountPaymentDoc>();//????
            var owners = this.Cache.GetCache<LegalAccountOwner>()
                .GetEntities()
                .ToDictionary(x => x.Id);

            foreach (var account in byOwnerIds.Where(x => periodSummaryData.Any(z => z.AccountId == x.Id)))
            {
                var personalAccountPaymentDocProxy = new PersonalAccountPaymentDocProxy(account);//, owners.Get(account.AccountOwner.Id));

                //доля собственности
                decimal areaShare;
                //площадь
                decimal area;
                decimal tariff;
                //площадь*долю собственности
                decimal areaRoom;

                var calcParamTraceData = this.TraceCache.GetParameters(account);

                if (calcParamTraceData != null)
                {
                    areaShare = calcParamTraceData.Get(VersionedParameters.AreaShare).ToDecimal();
                    tariff = calcParamTraceData.Get(VersionedParameters.BaseTariff).ToDecimal();
                    area = calcParamTraceData.Get(VersionedParameters.RoomArea).ToDecimal();
                    areaRoom = areaShare * area;
                }
                else
                {
                    var tarifParam = this.ParamTracker.GetParameter(
                        VersionedParameters.BaseTariff,
                        personalAccountPaymentDocProxy.Account, this.Period);

                    tariff = tarifParam.GetActualByDate<decimal>(
                        personalAccountPaymentDocProxy.Account, this.Period.EndDate ?? this.Period.StartDate.AddMonths(1).AddDays(-1),
                        false);

                    var areaParam = this.ParamTracker.GetParameter(VersionedParameters.RoomArea, personalAccountPaymentDocProxy.Account, this.Period);
                    area = areaParam.GetActualByDate<decimal?>(
                        personalAccountPaymentDocProxy.Account, this.Period.EndDate ?? this.Period.StartDate,
                        true,
                        true) ?? personalAccountPaymentDocProxy.Area;

                    var areaShareParam = this.ParamTracker.GetParameter(
                                        VersionedParameters.AreaShare,
                                        personalAccountPaymentDocProxy.Account, this.Period);

                    areaShare = areaShareParam.GetActualByDate<decimal?>(
                        personalAccountPaymentDocProxy.Account, this.Period.EndDate ?? this.Period.StartDate,
                        true) ?? personalAccountPaymentDocProxy.AreaShare;

                    areaRoom = area * areaShare;
                }

                var roomAddress = string.Format(
                    "{0} {1}",
                    personalAccountPaymentDocProxy.AddressName,
                    personalAccountPaymentDocProxy.IsRoomHasNoNumber
                        ? personalAccountPaymentDocProxy.Notation
                        : ", кв. " + personalAccountPaymentDocProxy.RoomNum);

                var summary = periodSummaryDictByAccount.Get(account.Id);

                var summa = summary.Return(z => z.ChargeTariff) + summary.Return(z => z.Penalty) + summary.Return(z => z.Recalc);

                commonData.Add(new RecordPersonalAccountPaymentDoc
                {
                    AccountId = personalAccountPaymentDocProxy.Id,
                    OwnerId = personalAccountPaymentDocProxy.OwnerId,
                    НомерЛС = personalAccountPaymentDocProxy.PersonalAccountNum,
                    АдресПомещения = roomAddress,
                    Тариф = tariff,
                    ПлощадьПомещения = areaRoom,
                    ТипПомещения = personalAccountPaymentDocProxy.RoomType.GetEnumMeta().Display,
                    НаименованиеРабот = string.Empty,
                    Сумма = summa,
                    ОплаченоФКР = allSummaries.Get(personalAccountPaymentDocProxy.Id).Return(z => z.Sum(y => y.TariffPayment + y.TariffDecisionPayment)),
                    ПерерасчетВсего = allSummaries.Get(personalAccountPaymentDocProxy.Id).Return(z => z.Sum(y => y.RecalcByBaseTariff + y.RecalcByDecisionTariff + y.RecalcByPenalty)),
                    НачисленоВсего = allSummaries.Get(personalAccountPaymentDocProxy.Id).Return(z => z.Sum(y => y.ChargeTariff)),
                    НачисленоПениВсего = allSummaries.Get(personalAccountPaymentDocProxy.Id).Return(z => z.Sum(y => y.Penalty)),
                    RoId = personalAccountPaymentDocProxy.RoId,
                    AreaLivingNotLivingMkd = personalAccountPaymentDocProxy.AreaLivingNotLivingMkd,
                    Municipality = personalAccountPaymentDocProxy.Municipality,
                    Settlement = personalAccountPaymentDocProxy.Settlement,
                    AccountProxy = personalAccountPaymentDocProxy
                });
            }

            var accountIdByOwnerDict = commonData.GroupBy(x => x.OwnerId)
                .ToDictionary(
                    x => x.Key,
                    x => new
                    {
                        x.First().AccountId,
                        x.First().RoId,
                        x.First().Municipality,
                        x.First().Settlement,
                        x.First().AreaLivingNotLivingMkd,
                        x.First().Тариф,
                        x.First().НомерЛС,
                        x.First().AccountProxy.Account.Room.RealityObject.FiasAddress.PostCode,
                        AreaShare = x.Sum(y => y.ПлощадьПомещения),//!!
                        AccountInfo = x.First().AccountProxy,
                        PersAccsInfo = x.ToArray(),
                        ОплаченоФКР = x.Sum(y => y.ОплаченоФКР),//!!
                        ПерерасчетВсего = x.Sum(y => y.ПерерасчетВсего),//!
                        НачисленоВсего = x.Sum(y => y.НачисленоВсего),//!
                        НачисленоПениВсего = x.Sum(y => y.НачисленоПениВсего)//!
                    });

            var ownersDict = this.Cache.GetCache<LegalAccountOwner>().GetEntities()
                .Select(
                    x => new
                    {
                        OwnerId = x.Id,
                        ContragentName = x.Contragent.Name,
                        x.Contragent.Inn,
                        x.Contragent.Kpp,
                        AddressName = x.Contragent.With(c => c.FiasJuridicalAddress).With(c => c.AddressName)
                        ?? x.Contragent.With(c => c.AddressOutsideSubject),
                        FiasMailingAddress = x.Contragent.With(c => c.FiasMailingAddress),
                        ContragentId = (long?)x.Contragent.Id,
                        x.PrintAct,
                        x.OwnerType
                    })
                .GroupBy(x => x.OwnerId)
                .ToDictionary(x => x.Key, x => x.First());

            var contragentIdList = ownersDict.Select(x => x.Value.ContragentId).Where(x => x.HasValue).ToList();

            var contragentContractDomain = this.Cache.GetCache<ContragentContact>();

            var contragentContactsDict = contragentContractDomain.GetEntities()
                .Where(x => contragentIdList.Contains(x.Contragent.Id))
                .Where(x => x.DateStartWork == null || x.DateStartWork <= this.Period.EndDate)
                .Where(x => x.DateEndWork == null || x.DateEndWork >= this.Period.StartDate)
                .Select(x => new { x.Contragent.Id, x.FullName, PositionName = x.Position.Name })
                .GroupBy(x => x.Id)
                .ToDictionary(
                    x => x.Key,
                    x => x.GroupBy(y => y.PositionName.ToLower().Trim()).ToDictionary(y => y.Key, y => y.First().FullName));

            var contacts = contragentContractDomain.GetEntities()
                .Where(x => this.RegOperator.Contragent.Id == x.Contragent.Id)
                .GroupBy(x => x.Position.Name.Trim().ToUpper())
                .ToDictionary(
                    x => x.Key,
                    y => y.Return(
                        x =>
                        {
                            var rec = y.First();
                            return string.Format(
                                "{0} {1}. {2}.",
                                rec.Surname,
                                rec.Name.FirstOrDefault(),
                                rec.Patronymic.FirstOrDefault());
                        }));
                       

            // словарь в котором идет по дому к какому типу фонда он относится
            var crFundFormDecisions = this.Cache.GetCache<CrFundFormationDecision>()
                    .GetEntities()
                .Where( x => x.Decision == CrFundFormationDecisionType.SpecialAccount
                            || x.Decision == CrFundFormationDecisionType.RegOpAccount)
                    .GroupBy(x => x.Protocol.RealityObject.Id)
                    .ToDictionary(x => x.Key, y => y.First());

            //все дома с решением о формировании фонда от гос власти
            var crFundFormGovDecisions = this.Cache.GetCache<GovDecision>()
                    .GetEntities()
                    .Where(x => x.DateStart <= this.Period.StartDate)
                    .OrderByDescending(x => x.DateStart)
                    .GroupBy(x => x.RealityObject.Id)
                    .ToDictionary(x => x.Key, z => z.First());

            var paymentPenalties = this.Cache.GetCache<PaymentPenalties>().GetEntities().ToArray();

	        var bankStatementMaxDate = Cache.GetCache<BankAccountStatement>()
		        .GetEntities()
		        .Max(x => x.DocumentDate as DateTime?);
			
            var ownersData = new List<InvoiceRegistryAndActInfo>();

            foreach (var ownerModel in ownersDict.Values.Where(x => accountIdByOwnerDict.ContainsKey(x.OwnerId)))
            {
                var record = new InvoiceRegistryAndActInfo();

                //наименование получателя неверно заполнялось
                var hasActiveRegOpDecision = this.RegOperator != null && !this.RoWithCustomDecisionType.Contains(accountIdByOwnerDict[ownerModel.OwnerId].RoId);
                
                

                //словарь
                //норм!! можно адаптировать
                var bank = this.Banks.Get(accountIdByOwnerDict[ownerModel.OwnerId].RoId);

                record.OwnerId = ownerModel.OwnerId;//+
                record.Плательщик = ownerModel.ContragentName;//+
                record.ИННСобственника = ownerModel.Inn;//+
                record.КППСобственника = ownerModel.Kpp;//+
                record.АдресКонтрагента = ownerModel.AddressName;//+
                record.ПечататьАкт = ownerModel.PrintAct;//+

                //record.ТипСобственника = (int)ownerModel.OwnerType;//это вообще не нужное поле, тк все равно только юр лица мб

                this.ApplyAddressInfo(record, ownerModel.FiasMailingAddress);//+

                record.НаименованиеПолучателяКраткое = hasActiveRegOpDecision//+
                    ? this.RegOperator.Contragent.ShortName
                    : string.Empty;

                if (ownerModel.ContragentId.HasValue && contragentContactsDict.ContainsKey(ownerModel.ContragentId.Value))
                {
                    var contragentContacts = contragentContactsDict[ownerModel.ContragentId.Value];

                    record.Руководитель = contragentContacts.Get("руководитель") ?? string.Empty;//+
                    record.ГлавБух = contragentContacts.Get("главный бухгалтер") ?? string.Empty;//+
                }
                
                record.ОбщаяПлощадь = accountIdByOwnerDict[ownerModel.OwnerId].AreaShare;//-

                record.Номер = this.GetDocNumber(accountIdByOwnerDict[ownerModel.OwnerId].AccountId);//+
                record.ПорядковыйНомерВГоду = this.GetNextSerialNumber();//+

                record.НаименованиеПериода = this.Period.Name;//+
                record.ДатаНачалаПериода = this.Period.StartDate.ToString("dd.MM.yyyy");//+
                record.ДатаОкончанияПериода = this.Period.EndDate.HasValue//+
                    ? this.Period.EndDate.Value.ToString("dd.MM.yyyy")
                    : string.Empty;
                record.ПериодНачислений = isZeroPayment//+
                    ? string.Empty
                    : "за период с {0} года по {1} года".FormatUsing(
                        record.ДатаНачалаПериода,
                        record.ДатаОкончанияПериода);
                record.МесяцГодНачисления = this.Period.StartDate.ToString("MMMM yyyy");//+
                record.ДатаДокумента = DateTime.Today.ToString("dd.MM.yyyy");//+

                record.АдресКвартиры = string.Format(//+
                    "{0} {1}",
                    accountIdByOwnerDict[ownerModel.OwnerId].AccountInfo.AddressName,
                    accountIdByOwnerDict[ownerModel.OwnerId].AccountInfo.IsRoomHasNoNumber ? accountIdByOwnerDict[ownerModel.OwnerId].AccountInfo.Notation : ", кв. " + accountIdByOwnerDict[ownerModel.OwnerId].AccountInfo.RoomNum);

                record.ЛицевойСчет = accountIdByOwnerDict[ownerModel.OwnerId].НомерЛС;//+
				if (record.ЛицевойСчет.IsNotEmpty() && bankStatementMaxDate != null)//+
	            {
		            record.ДатаПоследнейОплаты = bankStatementMaxDate.Value.ToString("dd.MM.yyyy");
	            }

                record.Индекс = accountIdByOwnerDict[ownerModel.OwnerId].PostCode;//+
                record.Municipality = accountIdByOwnerDict[ownerModel.OwnerId].Municipality;//+
                record.Settlement = accountIdByOwnerDict[ownerModel.OwnerId].Settlement;//+
                record.ФИОСобственника = accountIdByOwnerDict[ownerModel.OwnerId].AccountInfo.OwnerName;//+
                record.Тариф = accountIdByOwnerDict[ownerModel.OwnerId].Тариф;//+

                record.НаименованиеПолучателя = hasActiveRegOpDecision
                    ? this.RegOperator.Contragent.Name
                    : string.Empty;
                record.ИннПолучателя = hasActiveRegOpDecision ? this.RegOperator.Contragent.Inn : string.Empty;
                record.КппПолучателя = hasActiveRegOpDecision ? this.RegOperator.Contragent.Kpp : string.Empty;
                record.ОргнПолучателя = hasActiveRegOpDecision ? this.RegOperator.Contragent.Ogrn : string.Empty;
                record.РсчетПолучателя = bank != null ? bank.SettlementAccount : string.Empty;
                record.АдресБанка = bank != null ? bank.Address : string.Empty;
                record.НаименованиеБанка = bank != null ? bank.Name : string.Empty;
                record.БикБанка = bank != null ? bank.Bik : string.Empty;
                record.КсБанка = bank != null ? bank.CorrAccount : string.Empty;
                record.ТелефоныПолучателя = hasActiveRegOpDecision
                    ? this.RegOperator.Contragent.Phone
                    : string.Empty;
                record.АдресПолучателя = hasActiveRegOpDecision
                    ? this.RegOperator.Contragent.MailingAddress
                    : string.Empty;
                record.РуководительПолучателя = contacts.Get("РУКОВОДИТЕЛЬ");
                record.ГлБухПолучателя = contacts.Get("ГЛАВНЫЙ БУХГАЛТЕР");
                record.НаименованиеПолучателяКраткое = hasActiveRegOpDecision
                    ? this.RegOperator.Contragent.ShortName
                    : string.Empty;

                if (periodSummaryDictByOwner.ContainsKey(record.OwnerId) && !isZeroPayment)
                {
                    var periodSummary = periodSummaryDictByOwner[record.OwnerId];
                    var charges = chargesDict.Get(record.OwnerId);

                    var payedFkr = 0m;
                    var fkr = 0m;

                    CrFundFormationDecisionType? crFType = null;
                    var roFundDecision = crFundFormDecisions.Get(periodSummary.RoId);
                    var roFundGovDecision = crFundFormGovDecisions.Get(periodSummary.RoId);

                    if (roFundDecision != null || roFundGovDecision != null)
                    {
                        //если есть только протокол решения дома или этот протокол более актуальный, чем протокол решения гос власти
                        if (roFundGovDecision == null ||
                                 (roFundDecision != null && roFundDecision.StartDate > roFundGovDecision.DateStart))
                        {
                            crFType = roFundDecision.Decision;
                        }
                        else
                        {
                            crFType = CrFundFormationDecisionType.RegOpAccount;
                        }
                    }
                    record.СпособФормированияФонда = crFType.HasValue//+
                        ? crFType.Value.EnumToStr()
                        : string.Empty;
                    var paymentPenaltie = paymentPenalties.FirstOrDefault(//+
                        y => crFType.HasValue
                            && crFType.Value == y.DecisionType
                            && y.DateStart <= this.Period.StartDate
                            && (!y.DateEnd.HasValue || y.DateEnd.Value >= this.Period.EndDate));
                    record.ОплатитьДо = paymentPenaltie != null
                        ? (this.Period.EndDate.HasValue
                            ? this.Period.EndDate.Value.AddDays(paymentPenaltie.Days + 1).ToString("dd.MM.yyyy")
                            : string.Empty)
                        : (this.Period.EndDate.HasValue
                            ? this.Period.EndDate.Value.ToString("dd.MM.yyyy")
                            : string.Empty);
                    if (accountIdByOwnerDict.ContainsKey(ownerModel.OwnerId)
                        && allSummaries.ContainsKey(accountIdByOwnerDict[ownerModel.OwnerId].AccountId))
                    {
                        payedFkr = Math.Round(
                                Math.Abs(accountIdByOwnerDict[ownerModel.OwnerId].ОплаченоФКР),//ну так
                                2,
                                MidpointRounding.AwayFromZero);
                    }

                            record.УплаченоФКР = payedFkr;
                            record.ПотраченоНаКР = fkr;

                    record.ПерерасчетВсего = accountIdByOwnerDict[ownerModel.OwnerId].ПерерасчетВсего;
                    record.НачисленоВсего = accountIdByOwnerDict[ownerModel.OwnerId].ПерерасчетВсего;
                    record.НачисленоПениВсего = accountIdByOwnerDict[ownerModel.OwnerId].НачисленоПениВсего;
                    
                    record.ДолгНаКонец = periodSummary.SaldoOut;
                    record.Итого = periodSummary.ChargeTariff;
                    record.Пени = periodSummary.Penalty;
                    record.Перерасчет = periodSummary.Recalc;
                    record.СуммаВсего = record.Итого + record.Пени + record.Перерасчет;
                    record.СальдоБазТарифНачало = periodSummary.BaseTariffDebt;
                    record.СальдоТарифРешНачало = periodSummary.DecisionTariffDebt;
                    record.ПерерасчетБазовый = periodSummary.RecalcByBaseTariff;
                    record.ПерерасчетТарифРешения = periodSummary.RecalcByDecisionTariff;
                    record.ОплаченоБазовый = periodSummary.TariffPayment;
                    record.ОплаченоТарифРешения = periodSummary.TariffDecisionPayment;
                    record.ПерерасчетПени = periodSummary.ReclacByPenalty;
                    record.ДолгПениНаНачало = periodSummary.PenaltyDebt;
                    record.ОплаченоПени = periodSummary.PenaltyPayment;

                    record.НачисленоБазовыйПериод = charges.ReturnSafe(z => z.ChargeTariff - z.OverPlus);
                    record.НачисленоПениПериод = charges?.Penalty ?? 0;
                    record.НачисленоТарифРешенияПериод = charges?.OverPlus ?? 0;

                    record.ПерерасчетБазовыйПериод = periodSummary.RecalcByBaseTariff
                                                        + periodSummary.BaseTariffChange
                                                        + periodSummary.ChargedByBaseTariff
                                                        - record.НачисленоБазовыйПериод;

                    record.ПерерасчетПениПериод = periodSummary.ReclacByPenalty
                                                        + periodSummary.PenaltyChange
                                                        + periodSummary.Penalty
                                                        - record.НачисленоПениПериод;

                    record.ПерерасчетТарифРешенияПериод = periodSummary.RecalcByDecisionTariff
                                                        + periodSummary.DecisionTariffChange
                                                        + periodSummary.ChargeTariff
                                                        - periodSummary.ChargedByBaseTariff
                                                        - record.НачисленоТарифРешенияПериод;

                    if (periodSummarySaldoDictByOwner.ContainsKey(record.OwnerId))
                    {
                        var periodSummarySaldo = periodSummarySaldoDictByOwner[record.OwnerId];

                        record.ОбщийДебет = periodSummarySaldo.SaldoOutDebet;
                        record.ОбщийДебетПени = periodSummarySaldo.PenaltyDebet;
                        record.ОбщийКредит = periodSummarySaldo.SaldoOutKredit;
                        record.ОбщийКредитПени = periodSummarySaldo.PenaltyKredit;
                    }
                }
                record.СуммаСтрокой = isZeroPayment
                    ? string.Empty
                    : formatter.Format(
                        "СуммаСтрокой",
                        record.СуммаВсего,
                        CultureInfo.InvariantCulture.NumberFormat);

                record.OwnerType = (int)ownerModel.OwnerType;

                record.ПоОткрытомуПериоду = !this.Period.IsClosed;

                ownersData.Add(record);
            }

            this.CreateSnapshots(ownersData);
        }

        private void CreateSnapshots(List<InvoiceRegistryAndActInfo> ownersData)
        {
            using (var paymentDocumentNumberBuilder = new PaymentDocumentNumberBuilder(this.Container))
            {
                foreach (var info in ownersData)
                {
                    var snapshot = new PaymentDocumentSnapshot
                    {
                        Data = JsonConvert.SerializeObject(info),
                        HolderId = info.OwnerId,
                        HolderType = PaymentDocumentData.OwnerholderType,
                        Address = info.АдресКонтрагента,
                        OwnerType = (PersonalAccountOwnerType) info.OwnerType,
                        Municipality = info.Municipality,
                        Settlement = info.Settlement,
                        //Payer = info.ТипСобственника == (int) PersonalAccountOwnerType.Legal ? info.Плательщик : info.ФИОСобственника,
                        PaymentReceiverAccount = info.РсчетПолучателя,
                        Period = this.Period,
                        TotalCharge = info.СуммаВсего
                    };

                    if (info.ТипСобственника == (int) PersonalAccountOwnerType.Legal)
                    {
                        snapshot.DocDate = info.ДатаОкончанияПериода.ToDateTime();
                        snapshot.DocNumber = paymentDocumentNumberBuilder.GetDocumentNumber(snapshot);
                        info.НомерДокумента = snapshot.DocNumber;
                        snapshot.Data = JsonConvert.SerializeObject(info);
                    }

                    DomainEvents.Raise(new SnapshotEvent(snapshot));
                }
            }
        }
        
        private class RecordPersonalAccountPaymentDoc
        {
            public long AccountId { get; set; }
            public long OwnerId { get; set; }
            public string АдресПомещения { get; set; }
            public decimal Тариф { get; set; }
            public decimal ПлощадьПомещения { get; set; }
            public string ТипПомещения { get; set; }
            public string НаименованиеРабот { get; set; }
            public decimal Сумма { get; set; }
            public decimal ОплаченоФКР { get; set; }
            public long RoId { get; set; }
            public string Municipality { get; set; }
            public decimal? AreaLivingNotLivingMkd { get; set; }
            public string Settlement { get; set; }
            public PersonalAccountPaymentDocProxy AccountProxy { get; set; }
            public string НомерЛС { get; set; }
            public decimal ПерерасчетВсего { get; set; }
            public decimal НачисленоВсего { get; set; }
            public decimal НачисленоПениВсего { get; set; }
       }*/
    }
}
