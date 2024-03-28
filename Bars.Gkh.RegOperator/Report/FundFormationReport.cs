namespace Bars.Gkh.RegOperator.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;

    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Config;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Enums.Decisions;
    using Bars.Gkh.Overhaul.DomainService;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Properties;
    using Bars.Gkh.Utils;
    using Bars.Gkh.Decisions.Nso.Enums;
    using Bars.Gkh.Modules.RegOperator.Entities.RegOperator;
    using Bars.Gkh.Decisions.Nso.Entities.Decisions;
    using Castle.Windsor;

    public class FundFormationReport : BasePrintForm
    {
        private long[] _contragentIds;

        private MethodFormFund[] _formationTypeValues;

        private bool? haveReference;

        private long moId;

        private long parentMoId;

        /// <summary>
        /// Включить в отчет дома, не включенные в опубликованную программу
        /// </summary>
        private bool includeRosNotInPublishedProgram;

        public FundFormationReport()
            : base(new ReportTemplateBinary(Resources.FundFormationReport))
        {
        }

        public IWindsorContainer Container { get; set; }

        public override string Desciption
        {
            get
            {
                return "Отчет по разделу уведомлений о способе формирования фонда";
            }
        }

        public override string GroupName
        {
            get
            {
                return "Протоколы решений";
            }
        }

        public IDomainService<ManagingOrgRealityObject> ManagingOrgRealityObjectServ { get; set; }

        public override string Name
        {
            get
            {
                return "Отчет по разделу уведомлений о способе формирования фонда";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.FundFormationReport";
            }
        }

        public IDomainService<RegOperator> RegOperatorServ { get; set; }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.DecisionsNso.FundFormation";
            }
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var repoNotification = this.Container.Resolve<IRepository<DecisionNotification>>();
            var repoCrFund = this.Container.Resolve<IRepository<CrFundFormationDecision>>();
            var repoAccOwn = this.Container.Resolve<IRepository<AccountOwnerDecision>>();
            var repoCrorgDecision = this.Container.ResolveRepository<CreditOrgDecision>();
            var repoRealObj = this.Container.ResolveRepository<RealityObject>();
            var repoRoDecProt = this.Container.ResolveRepository<RealityObjectDecisionProtocol>();
            var roServiceOrgDomain = this.Container.ResolveDomain<RealityObjectServiceOrg>();
            var calcAccRealObjDomain = this.Container.ResolveDomain<CalcAccountRealityObject>();
            var roInProgramService = Container.Resolve<IRealityObjectsInPrograms>();
            var gkhParams = this.Container.Resolve<IGkhParams>();
            var municipalityDomain = this.Container.Resolve<IRepository<Municipality>>();

            // получаем список МО как в реестре опубликованных программ
            var gkhParameters = gkhParams.GetParams();
            var moLevel = gkhParameters.ContainsKey("MoLevel") && !string.IsNullOrEmpty(gkhParameters["MoLevel"].To<string>())
                ? gkhParameters["MoLevel"].To<MoLevel>()
                : MoLevel.MunicipalUnion;
            var moIds =
                municipalityDomain.GetAll()
                    .Select(x => new { x.Id, x.Level })
                    .ToList()
                    .Where(x => x.Level.ToMoLevel(Container) == moLevel)
                    .Select(x => x.Id)
                    .WhereIf(parentMoId != 0, x => x == parentMoId)
                    .ToArray();

            // Фильтруем по опубликованной программе (если не сказано обратного)
            var publProgRecRoIds = !includeRosNotInPublishedProgram ? roInProgramService.GetInPublishedProgramByMunicipality(moIds) : null;


            //выбраны все способы формирования
            var allFormationType = !_formationTypeValues.Any();

            // Собираем все протоколы решения собственников, удовлетворяющие условиям
            long[] notificationProtocolIds = null;
            if (this.haveReference.HasValue)
            {
                notificationProtocolIds =
                    repoNotification.GetAll()
                        .Where(x => x.CopyIncome == this.haveReference.Value && x.Protocol != null)
                        .Select(x => x.Protocol.Id)
                        .ToArray();
            }


            var crFundProtocolIds = new List<long>();

            var crFunds =
                repoCrFund.GetAll()
                    .Where(
                        x =>
                            x.Protocol.State.FinalState &&
                            (x.Decision == CrFundFormationDecisionType.RegOpAccount ||
                             x.Decision == CrFundFormationDecisionType.SpecialAccount)
                            && x.Protocol.ProtocolDate <= DateTime.Now)
                    .Select(
                        x =>
                            new
                            {
                                roId = x.Protocol.RealityObject.Id,
                                x.Protocol.ProtocolDate,
                                x.Decision,
                                ProtocolId = x.Protocol.Id
                            })
                    .AsEnumerable()
                    .GroupBy(x => x.roId)
                    .Select(x => x.OrderByDescending(y => y.ProtocolDate).FirstOrDefault())
                    .Where(x => x != null)
                    .ToArray();

            // способ формирования == На счете регоператора || выбраны все способы формирования
            if (_formationTypeValues.Contains(MethodFormFund.RegOperAccount) || allFormationType)
            {
                crFundProtocolIds.AddRange(crFunds
                    .Where(x => x.Decision == CrFundFormationDecisionType.RegOpAccount)
                    .Select(x => x.ProtocolId));
            }


            var accOwnProtocolsIds =
                repoAccOwn.GetAll()
                    .Where(x => x.DecisionType == AccountOwnerDecisionType.RegOp)
                    .Select(x => x.Protocol.Id)
                    .ToArray();

            // способ формирования == На специальном счете || выбраны все способы формирования
            if (_formationTypeValues.Contains(MethodFormFund.SpecialAccount) || allFormationType)
            {
                crFundProtocolIds.AddRange(
                    crFunds.Where(
                        x =>
                            x.Decision == CrFundFormationDecisionType.SpecialAccount &&
                            !accOwnProtocolsIds.Contains(x.ProtocolId)).Select(x => x.ProtocolId));
            }
            // способ формирования == На спец счете, владелец регоператор || выбраны все способы формирования
            if (_formationTypeValues.Contains(MethodFormFund.SpecialAccountWithRegOperOwner) || allFormationType)
            {
                crFundProtocolIds.AddRange(
                    crFunds.Where(
                        x =>
                            x.Decision == CrFundFormationDecisionType.SpecialAccount &&
                            accOwnProtocolsIds.Contains(x.ProtocolId)).Select(x => x.ProtocolId));
            }

            // Фильтруем по контрагенту
            long[] roIds = null;
            if (this._contragentIds.Any())
            {
                roIds =
                    roServiceOrgDomain.GetAll()
                        .Where(x => this._contragentIds.Contains(x.Organization.Id))
                        .Select(x => x.RealityObject.Id)
                        .ToArray();
            }

            // В зависимости от условий составляем результирующий массив
            var protocolIds = notificationProtocolIds != null
                ? crFundProtocolIds.Any()
                    ? notificationProtocolIds.Intersect(crFundProtocolIds).ToArray()
                    : notificationProtocolIds
                : crFundProtocolIds.ToArray();

            var realObjProtRepo = this.Container.Resolve<IRepository<RealityObjectDecisionProtocol>>();
            var data =
                realObjProtRepo.GetAll()
                    .Where(x => x.State.FinalState)
                    .Where(x => protocolIds.Contains(x.Id))
                    .WhereIf(this.parentMoId != 0, x => x.RealityObject.Municipality.Id == this.parentMoId)
                    .WhereIf(this.moId != 0, x => x.RealityObject.MoSettlement.Id == this.moId)
                    .WhereIf(roIds != null, x => roIds.Contains(x.RealityObject.Id))
                    .WhereIf(!includeRosNotInPublishedProgram,
                        x => publProgRecRoIds.Any(y => y.Id == x.RealityObject.Id))
                    .Select(
                        x =>
                            new
                            {
                                x.Id,
                                x.DocumentNum,
                                x.ProtocolDate,
                                isGovDecision=false,
                                roId = x.RealityObject.Id,
                                roAddress = x.RealityObject.Address,
                                moId = x.RealityObject.Municipality.Id,
                                moName = x.RealityObject.Municipality.Name,
                                moSatlementName =
                                    x.RealityObject.MoSettlement != null ? x.RealityObject.MoSettlement.Name : null
                            })
                    .OrderBy(x => x.moName)
                    .ThenBy(x => x.moSatlementName)
                    .ThenBy(x => x.roAddress)
                    .AsEnumerable()
                    .GroupBy(x => x.moId)
                    .ToDictionary(x => x.Key, x => x.ToList());

            var dictCrFund = this.GetFilteredDecisions(repoCrFund, protocolIds, roIds)
                .Select(x => new { x.Protocol.Id, x.Decision })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, y => y.Select(x => x.Decision).First());

            var dictAccountOwner =
                this.GetFilteredDecisions(repoAccOwn, protocolIds, roIds)
                    .Select(x => new { x.Protocol.Id, Decision = x.DecisionType })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.Select(x => x.Decision).First());

            var dictNotification =
                repoNotification.GetAll()
                    .Select(
                        x =>
                            new
                            {
                                x.Id,
                                ProtocolId = x.Protocol.Id,
                                x.OpenDate,
                                x.CloseDate,
                                x.CopyIncome,
                                x.IncomeNum,
                                x.Date
                            })
                    .AsEnumerable()
                    .GroupBy(x => x.ProtocolId)
                    .ToDictionary(x => x.Key, y => y.FirstOrDefault());

            var manOrgDict =
                this.ManagingOrgRealityObjectServ.GetAll()
                    .WhereIf(this.parentMoId != 0, x => x.RealityObject.Municipality.Id == this.parentMoId)
                    .WhereIf(this.moId != 0, x => x.RealityObject.MoSettlement.Id == this.moId)
                    .WhereIf(roIds != null, x => roIds.Contains(x.RealityObject.Id))
                    .OrderByDescending(x => x.ObjectCreateDate)
                    .Select(x => new { x.RealityObject.Id, x.ManagingOrganization.Contragent.Name })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.FirstOrDefault().Return(z => z.Name));


            reportParams.SimpleReportParams["Дата"] = DateTime.Now.ToString("dd.MM.yyyy");
            var index = 1;
            var sectionMu = reportParams.ComplexReportParams.ДобавитьСекцию("sectionMu");

            //способ формирования включает в себя "Не выбран"
            if (_formationTypeValues.Contains(MethodFormFund.NotSelected) || allFormationType)
            {
                var haveCrFundDecRoIds =
                    repoCrFund.GetAll()
                        .Where(x => x.Protocol.State.FinalState)
                        .Select(x => x.Protocol.RealityObject.Id)
                        .ToArray();

                //все объекты, удовлетворяющие условию
                var realityObjects =
                    repoRealObj.GetAll()
                        .WhereIf(this.parentMoId != 0, x => x.Municipality.Id == this.parentMoId)
                        .WhereIf(this.moId != 0, x => x.MoSettlement.Id == this.moId)
                        .WhereIf(!includeRosNotInPublishedProgram, x => publProgRecRoIds.Any(y => y.Id == x.Id))
                        .Select(
                            x =>
                                new
                                {
                                    x.Id,
                                    roAddress = x.Address,
                                    moId = x.Municipality.Id,
                                    moName = x.Municipality.Name,
                                    moSatlementName = x.MoSettlement != null ? x.MoSettlement.Name : null
                                })
                        .OrderBy(x => x.moName)
                        .ThenBy(x => x.moSatlementName)
                        .ThenBy(x => x.roAddress)
                        .AsEnumerable()
                        .GroupBy(x => x.moId)
                        .ToDictionary(x => x.Key);


                var realityObjectsDict =
                    repoRealObj.GetAll()
                        .Where(x => !haveCrFundDecRoIds.Contains(x.Id))
                        .WhereIf(this.parentMoId != 0, x => x.Municipality.Id == this.parentMoId)
                        .WhereIf(this.moId != 0, x => x.MoSettlement.Id == this.moId)
                        .WhereIf(!includeRosNotInPublishedProgram, x => publProgRecRoIds.Any(y => y.Id == x.Id))
                        .Select(
                            x =>
                                new
                                {
                                    x.Id,
                                    roAddress = x.Address,
                                    moId = x.Municipality.Id,
                                    moName = x.Municipality.Name,
                                    moSatlementName = x.MoSettlement != null ? x.MoSettlement.Name : null
                                })
                        .OrderBy(x => x.moName)
                        .ThenBy(x => x.moSatlementName)
                        .ThenBy(x => x.roAddress)
                        .AsEnumerable()
                        .GroupBy(x => x.moId)
                        .ToDictionary(x => x.Key);

                int count=0;
                foreach (var mo in realityObjects.Keys)
                {
                    count = 0;

                    sectionMu.ДобавитьСтроку();

                    var section = sectionMu.ДобавитьСекцию("section");

                    //отчет имеет способ формирования
                    if (data.ContainsKey(mo))
                    {
                        foreach (var protocol in data[mo])
                        {
                            section.ДобавитьСтроку();
                            section["Номер"] = index++;
                            section["МуниципальныйРайон"] = protocol.moName;
                            section["МуниципальноеОбразование"] = protocol.moSatlementName;
                            section["Адрес"] = protocol.roAddress;
                            section["УправляющаяОрганизация"] = manOrgDict.Get(protocol.roId).Or(" ");

                            CrFundFormationDecisionType crFundFormationDecisionType;
                            if (dictCrFund.TryGetValue(protocol.Id, out crFundFormationDecisionType))
                            {
                                string fundForm = null;

                                switch (crFundFormationDecisionType)
                                {
                                    case CrFundFormationDecisionType.SpecialAccount:
                                        if (dictAccountOwner.Get(protocol.Id) == AccountOwnerDecisionType.RegOp)
                                        {
                                            section["Владелец"] = "Региональный оператор";
                                            fundForm = "На специальном счете, владелец региональный оператор";
                                        }
                                        else
                                        {
                                            section["Владелец"] = manOrgDict.ContainsKey(protocol.Id)
                                                ? manOrgDict[protocol.roId]
                                                : string.Empty;
                                            fundForm = "На специальном счете";
                                        }

                                        break;
                                    case CrFundFormationDecisionType.RegOpAccount:
                                        section["Владелец"] = "Региональный оператор";
                                        fundForm = "На счете регионального оператора";
                                        break;
                                }
                                section["СпособФормирования"] = fundForm;
                            }

                            var notification = dictNotification.Get(protocol.Id);
                            if (notification != null)
                            {
                                section["ДатаОткрытия"] = notification.OpenDate != DateTime.MinValue
                                    ? notification.OpenDate.ToString("dd.MM.yyyy")
                                    : string.Empty;
                                section["ДатаЗакрытия"] = notification.CloseDate != DateTime.MinValue
                                    ? notification.CloseDate.ToString("dd.MM.yyyy")
                                    : string.Empty;
                                section["НаличиеСправки"] = notification.CopyIncome ? "Да" : "Нет";
                                section["НомерУведомления"] = notification.IncomeNum;
                                section["ДатаУведомления"] = notification.Date != DateTime.MinValue
                                    ? notification.Date.ToString("dd.MM.yyyy")
                                    : string.Empty;
                            }

                            section["Реквизиты"] = "{0} от {1}".FormatUsing(protocol.DocumentNum,
                                protocol.ProtocolDate.ToString("dd.MM.yyyy"));
                        }
                        count = data[mo].Count();
                    }
                    //способ формирования отчета не выбран
                    if (realityObjectsDict.ContainsKey(mo))
                    {
                        foreach (var ro in realityObjectsDict[mo])
                        {
                            section.ДобавитьСтроку();
                            section["Номер"] = index++;
                            section["МуниципальныйРайон"] = ro.moName;
                            section["МуниципальноеОбразование"] = ro.moSatlementName;
                            section["Адрес"] = ro.roAddress;
                            section["СпособФормирования"] = "Не выбрано";
                        }
                        count += realityObjectsDict[mo].Count();
                    }
                    sectionMu["ВсегоПоМР"] = count;
                }
            }
            //способ формирования не включает в себя "не выбран"
            else
            {
                if (_formationTypeValues.Contains(MethodFormFund.RegOperAccount))
                {
                    var govData = this.Container.Resolve<IRepository<GovDecision>>().GetAll()
                        .Where(x => x.State.FinalState)
                        .WhereIf(this.parentMoId != 0, x => x.RealityObject.Municipality.Id == this.parentMoId)
                        .WhereIf(this.moId != 0, x => x.RealityObject.MoSettlement.Id == this.moId)
                        .WhereIf(roIds != null, x => roIds.Contains(x.RealityObject.Id))
                        .WhereIf(!includeRosNotInPublishedProgram, x => publProgRecRoIds.Any(y => y.Id == x.RealityObject.Id))
                        .Select(x => new
                        {
                            x.Id,
                            DocumentNum = x.ProtocolNumber,
                            x.ProtocolDate,
                            isGovDecision = true,
                            roId = x.RealityObject.Id,
                            roAddress = x.RealityObject.Address,
                            moId = x.RealityObject.Municipality.Id,
                            moName = x.RealityObject.Municipality.Name,
                            moSatlementName =
                                x.RealityObject.MoSettlement != null ? x.RealityObject.MoSettlement.Name : null
                        })
                        .OrderBy(x => x.moName)
                        .ThenBy(x => x.moSatlementName)
                        .ThenBy(x => x.roAddress)
                        .AsEnumerable()
                        .GroupBy(x => x.moId)
                        .ToDictionary(x => x.Key, x => x.ToList());
                    foreach (var mo in govData.Keys)
                    {
                        if (!data.ContainsKey(mo))
                        {
                            data.Add(mo, govData[mo]);
                        }
                        else
                        {
                            data[mo].AddRange(govData[mo]);
                            data[mo] = data[mo].OrderBy(x => x.moName).ThenBy(x => x.moSatlementName).ThenBy(x => x.roAddress).ToList();
                        }
                    }
                }


                foreach (var mo in data.Keys)
                {
                    sectionMu.ДобавитьСтроку();

                    var section = sectionMu.ДобавитьСекцию("section");

                    if (data.ContainsKey(mo))
                        foreach (var protocol in data[mo])
                        {
                            section.ДобавитьСтроку();
                            section["Номер"] = index++;
                            section["МуниципальныйРайон"] = protocol.moName;
                            section["МуниципальноеОбразование"] = protocol.moSatlementName;
                            section["Адрес"] = protocol.roAddress;
                            section["УправляющаяОрганизация"] = manOrgDict.Get(protocol.roId).Or(" ");

                            CrFundFormationDecisionType crFundFormationDecisionType;
                            if (protocol.isGovDecision)
                            {
                                section["Владелец"] = "Региональный оператор";
                                section["СпособФормирования"] = "На счете регионального оператора";
                            }
                            else if (dictCrFund.TryGetValue(protocol.Id, out crFundFormationDecisionType))
                            {
                                string fundForm = null;

                                switch (crFundFormationDecisionType)
                                {
                                    case CrFundFormationDecisionType.SpecialAccount:
                                        if (dictAccountOwner.Get(protocol.Id) == AccountOwnerDecisionType.RegOp)
                                        {
                                            section["Владелец"] = "Региональный оператор";
                                            fundForm = "На специальном счете, владелец региональный оператор";
                                        }
                                        else
                                        {
                                            section["Владелец"] = manOrgDict.ContainsKey(protocol.Id)
                                                ? manOrgDict[protocol.roId]
                                                : string.Empty;
                                            fundForm = "На специальном счете";
                                        }

                                        break;
                                    case CrFundFormationDecisionType.RegOpAccount:
                                        section["Владелец"] = "Региональный оператор";
                                        fundForm = "На счете регионального оператора";
                                        break;
                                }
                                section["СпособФормирования"] = fundForm;
                            }

                            if (!protocol.isGovDecision)
                            {
                                var notification = dictNotification.Get(protocol.Id);
                                if (notification != null)
                                {
                                    section["ДатаОткрытия"] = notification.OpenDate != DateTime.MinValue
                                        ? notification.OpenDate.ToString("dd.MM.yyyy")
                                        : string.Empty;
                                    section["ДатаЗакрытия"] = notification.CloseDate != DateTime.MinValue
                                        ? notification.CloseDate.ToString("dd.MM.yyyy")
                                        : string.Empty;
                                    section["НаличиеСправки"] = notification.CopyIncome ? "Да" : "Нет";
                                    section["НомерУведомления"] = notification.IncomeNum;
                                    section["ДатаУведомления"] = notification.Date != DateTime.MinValue
                                        ? notification.Date.ToString("dd.MM.yyyy")
                                        : string.Empty;
                                }
                            }

                            section["Реквизиты"] = "{0} от {1}".FormatUsing(protocol.DocumentNum,
                                protocol.ProtocolDate.ToString("dd.MM.yyyy"));
                        }

                    sectionMu["ВсегоПоМР"] = data[mo].Count();
                }
            }


            var userIdentity = this.Container.Resolve<IUserIdentity>();
            var oper =
                this.Container.Resolve<IRepository<Operator>>()
                    .GetAll()
                    .FirstOrDefault(x => x.User.Id == userIdentity.UserId);

            reportParams.SimpleReportParams["ИмяПользователя"] = oper != null ? oper.Name : string.Empty;

            this.Container.Release(repoAccOwn);
            this.Container.Release(repoCrFund);
            this.Container.Release(repoCrorgDecision);
            this.Container.Release(repoNotification);
            this.Container.Release(repoRealObj);
            this.Container.Release(roServiceOrgDomain);
            this.Container.Release(repoRoDecProt);
            this.Container.Release(calcAccRealObjDomain);
            this.Container.Release(roInProgramService);
            this.Container.Release(gkhParams);
            this.Container.Release(municipalityDomain);
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            this.parentMoId = baseParams.Params["municipalityParent"].ToLong();
            this.moId = baseParams.Params["municipality"].ToLong();
            _contragentIds = ParseIds(baseParams.Params["contragentIds"].ToString());
            _formationTypeValues = ParseMethodFormFunds(baseParams.Params["formationTypeValuesList"].ToString());
            var haveRefInt = baseParams.Params["haveReference"].ToInt();

            switch (haveRefInt)
            {
                case 0:
                    this.haveReference = null;
                    break;
                case 1:
                    this.haveReference = true;
                    break;
                case 2:
                    this.haveReference = false;
                    break;
            }

            includeRosNotInPublishedProgram = baseParams.Params.GetAs("includeRosNotInPublishedProgram", false);
        }

        private IQueryable<T> GetFilteredDecisions<T>(IRepository<T> repo, long[] protocolIds, long[] roIds) where T : UltimateDecision
        {
            return
                repo.GetAll()
                    .WhereIf(protocolIds != null, x => protocolIds.Contains(x.Protocol.Id))
                    .WhereIf(this.parentMoId != 0, x => x.Protocol.RealityObject.Municipality.Id == this.parentMoId)
                    .WhereIf(this.moId != 0, x => x.Protocol.RealityObject.MoSettlement.Id == this.moId)
                    .WhereIf(roIds != null, x => roIds.Contains(x.Protocol.RealityObject.Id));
        }

        private static long[] ParseIds(string idString)
        {
            if (string.IsNullOrEmpty(idString))
            {
                return new long[0];
            }

            var ids = idString.Split(',')
                .Select(x => x.ToLong())
                .Where(x => x > 0)
                .Distinct()
                .ToArray();

            return ids;
        }

        private static MethodFormFund[] ParseMethodFormFunds(string idString)
        {
            if (string.IsNullOrEmpty(idString))
            {
                return new MethodFormFund[0];
            }

            var ids = idString.Split(',')
                .Select(x =>
                {
                    MethodFormFund val;
                    Enum.TryParse(x, out val);
                    return val;
                })
                .Where(x => x > 0)
                .Distinct()
                .ToArray();

            return ids;
        }
    }
}