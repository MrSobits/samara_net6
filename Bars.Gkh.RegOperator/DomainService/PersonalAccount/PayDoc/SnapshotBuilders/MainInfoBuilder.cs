namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc.SnapshotBuilders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Decisions.Nso.Entities.Decisions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums.Decisions;
    using Bars.Gkh.RegOperator.DataProviders;
    using Bars.Gkh.RegOperator.DataProviders.Meta;
    using Bars.Gkh.RegOperator.Domain;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc.SnapshotCreators;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Dict;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Tasks.PaymentDocuments;
    using Bars.Gkh.Utils;

    using NHibernate;
    using NHibernate.Linq;

    /// <summary>
    /// Основная информация
    /// </summary>
    public class MainInfoBuilder : AbstractSnapshotBuilder
    {
        /// <summary>
        /// Id
        /// </summary>
        public static string Id => nameof(MainInfoBuilder);

        /// <summary>
        /// Код источника
        /// </summary>
        public override string Code => MainInfoBuilder.Id;

        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name => "Источник основной информации";

        /// <summary>
        /// Описание заполняемых полей 
        /// </summary>
        public override string Description => "";

        private readonly IBuilderInfo[] builderInfos =
        {
            new BuilderInfo(
                "MainInfo",
                "Основная информация",
                "AccountId; ВнешнийЛС; АдресКвартиры; Индекс; ЗначениеQRКода; Municipality; Settlement; RoomType; ЛицевойСчет; " +
                    "НаименованиеПериода; ДатаНачалаПериода; ДатаОкончанияПериода; МесяцГодНачисления; ДатаДокумента; " +
                    "НаименованиеПолучателя; НаименованиеПолучателяКраткое; ИннПолучателя; КппПолучателя; ОргнПолучателя; " +
                    "ТелефоныПолучателя; АдресПолучателя; EmailПолучателя; WebSiteПолучателя; ШтрихКод; РсчетПолучателя; АдресБанка; " +
                    "НаименованиеБанка; НаименованиеБанкаДляПечати; БикБанка; КсБанка; RoCalcAccountId; ПоОткрытомуПериоду; Информация; АгентДоставки; НомерПомещения; " +
                    "НомерДокумента, НаселенныйПункт; Улица; Дом; Литер; Корпус; Секция; СтатусЛС; ТипПомещения; Примечание; МрЛицевойСчет; НомерКомнаты;"),
            new BuilderInfo("FundDecisions", "Информация о способе формирования фонда дома", "ОплатитьДо; СпособФормированияФонда; ФондСпецСчет"),
            new BuilderInfo("DebtorInfo", "Иформация о должниках", "СуммаЗадолженостиИзДолжников; КолличетсвоДнейПросрочки")
        };

        public MainInfoBuilder()
        {
            this.builderInfos.ForEach(this.AddChildSource);
        }

        private Dictionary<long, InvoiceBankProxy> bankDict;
        private Dictionary<long, ManagingOrganization> manOrgByRealObjDict;
        private Dictionary<long, string> delAgentByRealObj;
        private HashSet<long> roWithCustomDecisionType;
        private HashSet<long> roWithCrDunDecision;
        private HashSet<long> roWithRegopDecision;
        private List<PaymentDocInfo> paymentDocInfoList;

        private Dictionary<long, CrFundFormationDecision> crFundFormDecisionDict;
        private Dictionary<long, GovDecision> crFundFormGovDecisionDict;
        private Dictionary<long, PenaltyDelayDecision> penaltyDelayDecisionDict;
        private List<PaymentPenalties> penaltyParams;
        //для Смоленска Для печати угрозы неплательщикам в квитанции
        private Dictionary<long, decimal[]> debtorInfo;

        /// <summary>
        /// Инициализация кэша
        /// </summary>
        /// <param name="docCache">Кэш, в котором регистрируются сущности</param>
        /// <param name="mainInfo">Основной запрос</param>
        /// <param name="session">Сессия</param>
        public override void InitCache(DocCache docCache, PersonalAccountRecord[] mainInfo, IStatelessSession session)
        {
            if (!this.SectionEnabled("MainInfo"))
            {
                return;
            }

            var roIds = mainInfo.Select(x => x.RealityObjectId)
                    .Distinct()
                    .ToArray();
            
            foreach (var roIdPart in roIds.SplitArray())
            {
                this.InitRobjectInfo(docCache, session.Query<RealityObject>().Where(x => roIdPart.Contains(x.Id)), session);
            }

            if (this.SectionEnabled("FundDecisions"))
            {
                docCache.Cache.RegisterEntity<PaymentPenalties>()
                    .SetQueryBuilder(
                        r => r.GetAll()
                            .WhereIf(docCache.Period.EndDate.HasValue, x => x.DateStart <= docCache.Period.EndDate));
            }

            docCache.Cache.RegisterEntity<PaymentDocInfo>()
                .SetQueryBuilder(r => r.GetAll()
                    .Where(x => !docCache.Period.EndDate.HasValue || x.DateStart <= docCache.Period.EndDate)
                    .Where(x => !x.DateEnd.HasValue || x.DateEnd >= docCache.Period.StartDate));
            //для Смоленска Для печати угрозы неплательщикам в квитанции
            if (this.SectionEnabled("DebtorInfo"))
            {
                var accIds = mainInfo.Select(x => x.AccountId).ToArray();
                docCache.Cache.RegisterEntity<Debtor>().SetQueryBuilder(r => r.GetAll()
                    .WhereContains(x => x.PersonalAccount.Id, accIds)
                );
            }
        }

        private void InitRobjectInfo(DocCache docCache, IQueryable<RealityObject> robjectQuery, IStatelessSession session)
        {
            docCache.Cache.RegisterEntity<ManOrgContractRealityObject>()
                .SetQueryBuilder(r => r.GetAll()
                    .Fetch(y => y.RealityObject)
                    .Fetch(y => y.ManOrgContract)
                    .ThenFetch(y => y.ManagingOrganization)
                    .ThenFetch(y => y.Contragent)
                    .Where(y => robjectQuery.Any(x => x.Id == y.RealityObject.Id))
                    .Where(y => y.ManOrgContract.StartDate <= docCache.Period.StartDate)
                    .Where(y => !y.ManOrgContract.EndDate.HasValue || y.ManOrgContract.EndDate >= docCache.Period.StartDate));

            docCache.Cache.RegisterEntity<DeliveryAgentRealObj>()
                .SetQueryBuilder(r => r.GetAll()
                    .Where(y => robjectQuery.Any(x => x.Id == y.RealityObject.Id))
                    .Fetch(y => y.RealityObject)
                    .Fetch(y => y.DeliveryAgent)
                    .ThenFetch(y => y.Contragent));

            docCache.Cache.RegisterDto<CalcAccountRealityObject, InvoiceBankProxy>()
                .SetQueryBuilder(r => r.GetAll()
                    .Where(y => !y.DateEnd.HasValue || y.DateEnd > DateTime.Now)
                    .Where(y => robjectQuery.Any(x => x.Id == y.RealityObject.Id))
                    .Select(y => new
                    {
                        RoId = y.RealityObject.Id,
                        y.Account
                    })
                    .Select(y => new InvoiceBankProxy
                    {
                        RoCalcAccountId = y.Account.Id,
                        RoId = y.RoId,
                        SettlementAccount = (y.Account as RegopCalcAccount).ContragentCreditOrg.SettlementAccount ?? y.Account.AccountNumber,
                        Name = (y.Account as RegopCalcAccount).ContragentCreditOrg.CreditOrg.Name ?? y.Account.CreditOrg.Name,
                        PrintName = (y.Account as RegopCalcAccount).ContragentCreditOrg.CreditOrg.PrintName ?? y.Account.CreditOrg.PrintName ?? (y.Account as RegopCalcAccount).ContragentCreditOrg.CreditOrg.Name ?? y.Account.CreditOrg.Name,
                        Bik = (y.Account as RegopCalcAccount).ContragentCreditOrg.CreditOrg.Bik ?? y.Account.CreditOrg.Bik,
                        CorrAccount = (y.Account as RegopCalcAccount).ContragentCreditOrg.CreditOrg.CorrAccount ?? y.Account.CreditOrg.CorrAccount,
                        Address = (y.Account as RegopCalcAccount).ContragentCreditOrg.CreditOrg.Address ?? y.Account.CreditOrg.Address
                    }));

            if (this.SectionEnabled("FundDecisions"))
            {
                docCache.Cache.RegisterEntity<GovDecision>()
                 .SetQueryBuilder(r => r.GetAll()
                     .Fetch(y => y.RealityObject)
                     .OrderByDescending(y => y.DateStart)
                     .Where(y => y.DateStart <= docCache.Period.GetEndDate())
                     .Where(y => y.State.FinalState)
                     .Where(y => robjectQuery.Any(x => x.Id == y.RealityObject.Id)));
            }

            var protocolIds = session.Query<RealityObjectDecisionProtocol>()
                .Where(y => y.DateStart <= docCache.Period.GetEndDate())
                .Where(y => y.State.FinalState)
                .Where(y => robjectQuery.Any(x => x.Id == y.RealityObject.Id))
                .Select(y => new { RoId = y.RealityObject.Id, y.Id, y.ProtocolDate })
                .ToArray()
                .GroupBy(y => y.RoId)
                .Select(y => y.OrderByDescending(x => x.ProtocolDate).First().Id)
                .ToArray();

            this.InitDecisions(docCache, protocolIds);

        }

        private void InitDecisions(DocCache docCache, long[] protocolIds)
        {
            //протоколы
            docCache.Cache.RegisterEntity<CrFundFormationDecision>()
                .SetQueryBuilder(r => r.GetAll()
                    .Fetch(x => x.Protocol)
                    .ThenFetch(x => x.RealityObject)
                    .Where(x => protocolIds.Contains(x.Protocol.Id)));

            docCache.Cache.RegisterEntity<DecisionNotification>()
                .SetQueryBuilder(r => r.GetAll()
                    .Fetch(x => x.Protocol)
                    .ThenFetch(x => x.RealityObject)
                    .Where(x => protocolIds.Contains(x.Protocol.Id)));

            docCache.Cache.RegisterEntity<AccountOwnerDecision>()
                .SetQueryBuilder(r => r.GetAll()
                    .Fetch(x => x.Protocol)
                    .ThenFetch(x => x.RealityObject)
                    .Where(x => protocolIds.Contains(x.Protocol.Id)));

            docCache.Cache.RegisterEntity<CreditOrgDecision>()
                .SetQueryBuilder(r => r.GetAll()
                    .Fetch(x => x.Protocol)
                    .ThenFetch(x => x.RealityObject)
                    .Fetch(x => x.Decision)
                    .Where(x => protocolIds.Contains(x.Protocol.Id))
                    .Where(x => x.Decision != null));

            if (this.SectionEnabled("FundDecisions"))
            {
                docCache.Cache.RegisterEntity<PenaltyDelayDecision>()
                .SetQueryBuilder(r => r.GetAll()
                    .Fetch(x => x.Protocol)
                    .ThenFetch(x => x.RealityObject)
                    .Where(x => protocolIds.Contains(x.Protocol.Id)));
            }
        }

        /// <summary>
        /// Получение конкретных данных из кэша для последующей работы
        /// </summary>
        /// <param name="creator">Инициатор</param>
        public override void WarmCache(SnapshotCreator creator)
        {
            if (!this.SectionEnabled("MainInfo"))
            {
                return;
            }

            var activeNotifAccNum = creator.Cache.GetCache<DecisionNotification>().GetEntities()
                .Select(
                    x => new
                    {
                        RoId = x.Protocol.RealityObject.Id,
                        x.AccountNum
                    })
                .AsEnumerable()
                .GroupBy(x => x.RoId)
                .ToDictionary(x => x.Key, x => x.Select(y => y.AccountNum).FirstOrDefault());

            this.bankDict = creator.Cache.GetCache<InvoiceBankProxy>().GetEntities()
                .GroupBy(x => x.RoId)
                .ToDictionary(x => x.Key, x => x.OrderByDescending(y => y.RoCalcAccountId).FirstOrDefault());

            this.bankDict.Apply(
                creator.Cache.GetCache<CreditOrgDecision>().GetEntities()
                    .Select(
                        x => new
                        {
                            RoId = x.Protocol.RealityObject.Id,
                            x.Decision.Name,
                            x.Decision.PrintName,
                            x.Decision.Bik,
                            x.Decision.CorrAccount,
                            x.Decision.Address
                        })
                    .GroupBy(x => x.RoId)
                    .ToDictionary(
                        x => x.Key,
                        y => y.Select(
                            x => new InvoiceBankProxy
                            {
                                RoCalcAccountId = this.bankDict.Get(x.RoId)?.RoCalcAccountId ?? 0,
                                RoId = x.RoId,
                                SettlementAccount = activeNotifAccNum.Get(x.RoId),
                                Name = x.Name,
                                PrintName = x.PrintName,
                                Bik = x.Bik,
                                CorrAccount = x.CorrAccount,
                                Address = x.Address
                            })
                            .FirstOrDefault()));

            this.manOrgByRealObjDict = creator.Cache.GetCache<ManOrgContractRealityObject>().GetEntities()
                .Select(
                    x => new
                    {
                        x.ManOrgContract.StartDate,
                        x.ManOrgContract.ManagingOrganization,
                        x.RealityObject.Id
                    })
                .GroupBy(x => x.Id)
                .ToDictionary(
                    x => x.Key,
                    y => y.OrderByDescending(x => x.StartDate).Select(x => x.ManagingOrganization).First());

            // получаем только те дома у которых тип Решения = Специальный
            this.roWithCrDunDecision = creator.Cache.GetCache<CrFundFormationDecision>().GetEntities()
                .Where(x => x.Decision == CrFundFormationDecisionType.SpecialAccount)
                .Select(x => x.Protocol.RealityObject.Id)
                .ToHashSet();

            // получаем только те дома у которых тип Владелеца счета =  Рег. оператор
            this.roWithRegopDecision = creator.Cache.GetCache<AccountOwnerDecision>().GetEntities()
                .Where(x => x.DecisionType == AccountOwnerDecisionType.RegOp)
                .Select(x => x.Protocol.RealityObject.Id)
                .ToHashSet();

            this.roWithCustomDecisionType = creator.Cache.GetCache<AccountOwnerDecision>().GetEntities()
                .Where(x => x.DecisionType == AccountOwnerDecisionType.Custom)
                .Select(x => x.Protocol.RealityObject.Id)
                .ToHashSet();

            this.paymentDocInfoList = creator.Cache.GetCache<PaymentDocInfo>().GetEntities()
                .Where(x => !creator.Period.EndDate.HasValue || x.DateStart <= creator.Period.EndDate)
                .Where(x => (!x.DateEnd.HasValue || x.DateEnd >= creator.Period.StartDate))
                .ToList();

            this.delAgentByRealObj = creator.Cache.GetCache<DeliveryAgentRealObj>().GetEntities()
                .GroupBy(x => x.RealityObject.Id)
                .ToDictionary(x => x.Key, y => y.Select(x => x.DeliveryAgent.Contragent.Name).First());
            
            if (this.SectionEnabled("DebtorInfo"))
            {
                this.debtorInfo = new Dictionary<long, decimal[]>();
                creator.Cache.GetCache<Debtor>().GetEntities().OrderByDescending(x=> x.Id).ForEach(x =>
                {
                    if (!this.debtorInfo.ContainsKey(x.PersonalAccount.Id))
                    {
                        debtorInfo.Add(x.PersonalAccount.Id, new[] { x.DebtSum, x.ExpirationDaysCount });
                    }
                });
                  //  .ToDictionary(x => x.PersonalAccount.Id, y => new[] { y.DebtSum, y.ExpirationDaysCount });
            }

            if (this.SectionEnabled("FundDecisions"))
            {
                this.crFundFormDecisionDict = creator.Cache.GetCache<CrFundFormationDecision>().GetEntities()
                    .GroupBy(x => x.Protocol.RealityObject.Id)
                    .ToDictionary(x => x.Key, z => z.First());

                this.crFundFormGovDecisionDict = creator.Cache.GetCache<GovDecision>().GetEntities()
                    .GroupBy(x => x.RealityObject.Id)
                    .ToDictionary(x => x.Key, z => z.First());

                this.penaltyDelayDecisionDict = creator.Cache.GetCache<PenaltyDelayDecision>().GetEntities()
                    .GroupBy(x => x.Protocol.RealityObject.Id)
                    .ToDictionary(x => x.Key, z => z.First());

                this.penaltyParams = creator.Cache.GetCache<PaymentPenalties>().GetEntities().ToList();
            }
        }

        /// <summary>
        /// Заполнение одной записи модели с использованием данных, полученных в WarmCache 
        /// </summary>
        /// <param name="creator">Инициатор</param>
        /// <param name="record">Запись</param>
        /// <param name="account">Информация о лс</param>
        public override void FillRecord(
            SnapshotCreator creator,
            InvoiceInfo record,
            PersonalAccountPaymentDocProxy account)
        {
            if (!this.SectionEnabled("MainInfo"))
            {
                return;
            }

            var contragent = this.roWithCustomDecisionType.Contains(account.RoId)
                ? this.manOrgByRealObjDict.Get(account.RoId).Return(x => x.Contragent)
                : creator.RegOperator.Return(x => x.Contragent);

            var bank = this.bankDict.Get(account.RoId);

            var fundFormationType = this.roWithCrDunDecision.Contains(account.RoId)
                ? this.roWithRegopDecision.Contains(account.RoId)
                    ? FundFormationType.SpecRegop
                    : FundFormationType.Special
                : FundFormationType.Regop;

            var info = this.paymentDocInfoList
                .Where(x => ((x.RealityObject == null || x.RealityObject.Id == account.RoId)
                             &&
                             (x.RealityObject != null || x.FundFormationType == FundFormationType.NotSet ||
                              x.FundFormationType == fundFormationType)
                        && (x.LocalityAoGuid == null || x.LocalityAoGuid == account.PlaceGuidId || account.PlaceGuidId == null)
                        && (x.MoSettlement == null || x.MoSettlement.Id == account.SettlementId)
                        && (x.Municipality == null || x.Municipality.Id == account.MunicipalityId))
                    || x.IsForRegion)
                .Select(x => new
                {
                    x.Information,
                    // Частная информация приоритетнее общей
                    Priority = x.RealityObject != null
                        ? 1
                        : (x.LocalityAoGuid != null
                            ? 2
                            : (x.MoSettlement != null ? 3 : 4))
                })
                .GroupBy(x => x.Priority)
                .Select(x => new { Priopity = x.Key, info = x.Select(y => y.Information).ToList() })
                .OrderBy(x => x.Priopity)
                .Select(x => x.info.AggregateWithSeparator("\n"))
                .FirstOrDefault();
            //для Смоленска Для печати угрозы неплательщикам в квитанции
            if (this.SectionEnabled("DebtorInfo"))
            {
                if (this.debtorInfo.ContainsKey(account.Id))
                {

                    record.КолличетсвоДнейПросрочки = this.debtorInfo[account.Id][1];
                    record.СуммаЗадолженостиИзДолжников = this.debtorInfo[account.Id][0];
                }
                else
                {

                    record.КолличетсвоДнейПросрочки = 0;
                    record.СуммаЗадолженостиИзДолжников = 0;
                }
            }

            record.AccountId = account.Id;
            record.ВнешнийЛС = account.ExternalId;

            // Если у квартиры выставлена галочка, что у помещения отсутствует номер и при этом указано "Примечание"
            // То вместо номера квартиры ставим Примечание из карточки квартиры
            record.АдресКвартиры = $"{account.AddressName}{(account.IsRoomHasNoNumber ? account.Notation : ", кв. " + account.RoomNum)}";

            record.НаселенныйПункт = account.PlaceName;
            record.Улица = account.StreetName;
            record.Дом = account.House;
            record.Литер = account.Letter;
            record.Корпус = account.Housing;
            record.Секция = account.Building;
            record.Индекс = account.PostCode;
            record.ЗначениеQRКода = creator.isZeroPayment ? string.Empty : account.PersonalAccountNum;

            record.Municipality = account.Municipality;
            record.Settlement = account.Settlement;
            record.ЛицевойСчет = account.PersonalAccountNum;
            record.СтатусЛС = account.State;

            record.RoomType = (int)account.RoomType;
            record.ТипПомещения = account.RoomType.GetDisplayName();
            record.НомерПомещения = account.RoomNum;
            record.НомерКомнаты = account.ChamberNum;
            record.Примечание = account.Notation;
            record.Примечание = account.Notation;

            record.НаименованиеПериода = creator.Period.Name;
            record.ДатаНачалаПериода = creator.Period.StartDate.ToString("dd.MM.yyyy");
            record.ДатаОкончанияПериода = creator.Period.EndDate.ToDateString("dd.MM.yyyy");
            record.МесяцГодНачисления = creator.Period.StartDate.ToString("MMMM yyyy");

            record.ПоОткрытомуПериоду = !creator.Period.IsClosed;

            record.ДатаДокумента = DateTime.Today.ToShortDateString();

            if (contragent != null)
            {
                record.НаименованиеПолучателя = contragent.Name;
                record.НаименованиеПолучателяКраткое = contragent.ShortName;

                record.ИннПолучателя = contragent.Inn;
                record.КппПолучателя = contragent.Kpp;
                record.ОргнПолучателя = contragent.Ogrn;

                record.ТелефоныПолучателя = contragent.Phone;
                record.АдресПолучателя = contragent.MailingAddress;
                record.EmailПолучателя = contragent.Email;
                record.WebSiteПолучателя = contragent.OfficialWebsite;

                if (!creator.isZeroPayment)
                {
                    record.ШтрихКод = record.ИннПолучателя != null && record.ИннПолучателя.Length >= 10
                        ? record.ИннПолучателя.Substring(5, 5) + record.ЛицевойСчет
                        : string.Empty;
                }
            }

            record.РсчетПолучателя = bank.Return(x => x.SettlementAccount, string.Empty);
            record.АдресБанка = bank.Return(x => x.Address, string.Empty);
            record.НаименованиеБанка = bank.Return(x => x.Name, string.Empty);
            record.НаименованиеБанкаДляПечати = bank.Return(x => x.PrintName, string.Empty);
            record.БикБанка = bank.Return(x => x.Bik, string.Empty);
            record.КсБанка = bank.Return(x => x.CorrAccount, string.Empty);
            record.RoCalcAccountId = bank.Return(x => x.RoCalcAccountId);

            record.Информация = info;
            record.АгентДоставки = this.delAgentByRealObj.Get(account.RoId);

            if (this.SectionEnabled("FundDecisions"))
            {
                this.FillFundTypeInfo(creator, record, account);
            }
        }

        private void FillFundTypeInfo(
            SnapshotCreator creator,
            InvoiceInfo record,
            PersonalAccountPaymentDocProxy account)
        {
            var crFoundMessage = this.roWithCrDunDecision.Contains(account.RoId)
                     && this.roWithRegopDecision.Contains(account.RoId)
                ? "Дом формирует фонд капитального ремонта на специальном счёте"
                : string.Empty;

            var roFundDecision = this.crFundFormDecisionDict.Get(account.RoId);
            var roFundGovDecision = this.crFundFormGovDecisionDict.Get(account.RoId);

            CrFundFormationDecisionType? crFType = null;
            string payTo = null;

            if (roFundDecision != null || roFundGovDecision != null)
            {
                //если есть только протокол решения дома или этот протокол более актуальный, чем протокол решения гос власти
                if (roFundGovDecision == null ||
                         (roFundDecision != null && roFundDecision.Protocol.DateStart > roFundGovDecision.DateStart))
                {
                    crFType = roFundDecision.Decision;
                }
                else
                {
                    crFType = CrFundFormationDecisionType.RegOpAccount;
                }

                var penaltyDelayDecision = this.penaltyDelayDecisionDict.Get(account.RoId);

                if (penaltyDelayDecision != null)
                {
                    var penaltyDelay = penaltyDelayDecision.Decision
                        .Where(x => !x.To.HasValue || x.To >= creator.Period.StartDate)
                        .FirstOrDefault(x => x.From <= creator.Period.StartDate);

                    if (penaltyDelay != null)
                    {
                        payTo = penaltyDelay.MonthDelay
                            ? creator.Period.StartDate.AddMonths(2).ToShortDateString()
                            : creator.Period.StartDate.AddMonths(1)
                                .AddDays(penaltyDelay.DaysDelay)
                                .ToShortDateString();
                    }
                }

                if (payTo.IsEmpty())
                {
                    var penaltyParameter = this.penaltyParams
                        .Where(x => x.DateStart <= creator.Period.StartDate)
                        .OrderByDescending(x => x.DateStart)
                        .FirstOrDefault(x => x.DecisionType == crFType);

                    if (penaltyParameter != null)
                    {
                        payTo = creator.Period.StartDate.AddMonths(1)
                            .AddDays(penaltyParameter.Return(x => x.Days))
                            .ToShortDateString();
                    }
                }
            }
            record.СпособФормированияФонда = crFType.HasValue
                ? crFType.Value.EnumToStr()
                : string.Empty;

            record.ФондСпецСчет = crFoundMessage;
            record.ОплатитьДо = payTo;
        }
    }

    public class InvoiceBankProxy
    {
        public long RoCalcAccountId { get; set; }
        public long RoId { get; set; }
        public string SettlementAccount { get; set; }
        public string Name { get; set; }
        public string PrintName { get; set; }
        public string Bik { get; set; }
        public string CorrAccount { get; set; }
        public string Address { get; set; }
    }
}