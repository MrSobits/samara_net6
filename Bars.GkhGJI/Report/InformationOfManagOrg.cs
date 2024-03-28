namespace Bars.GkhGji.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    
    using Bars.B4.Modules.States;
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;

    using Castle.Windsor;

    public class InformationOfManagOrg : BasePrintForm
    {
        #region Свойства

        public IWindsorContainer Container { get; set; }

        private long[] municipalityIds;
        private List<TypeManagementManOrg> manOrgType = new List<TypeManagementManOrg>();
        private bool includeServiceOrganizations = false;
        private bool includeSupplyResourceOrgs = false;
        private DateTime reportDate = DateTime.MinValue;
        private ContragentState status = ContragentState.Active;


        public InformationOfManagOrg()
            : base(new ReportTemplateBinary(Bars.GkhGji.Properties.Resources.InformationOfManagOrg))
        {
        }

        public override string Name
        {
            get
            {
                return "Информация об УО (полная)";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Информация об УО (полная)";
            }
        }

        public override string GroupName
        {
            get
            {
                return "Отчеты ГЖИ";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.InformationOfManagOrg";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GJI.InformationOfManagOrg";
            }
        }

        #endregion

        public override void SetUserParams(BaseParams baseParams)
        {
            var municipalityIdsList = baseParams.Params.GetAs("municipalityIds", string.Empty);
            this.reportDate = baseParams.Params["reportDate"].ToDateTime();
            this.municipalityIds = !string.IsNullOrEmpty(municipalityIdsList)
                                  ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToArray()
                                  : new long[0];

            var orgTypeIdsList = baseParams.Params.GetAs("orgTypeIds", string.Empty);
            var orgTypeIds = !string.IsNullOrEmpty(orgTypeIdsList)
                                  ? orgTypeIdsList.Split(',').Select(id => id.ToLong()).ToArray()
                                  : new long[0];

            if (orgTypeIds.Contains(10) || orgTypeIds.Length == 0)
            {
                this.manOrgType.Add(TypeManagementManOrg.UK);
            }

            if (orgTypeIds.Contains(20) || orgTypeIds.Length == 0)
            {
                this.manOrgType.Add(TypeManagementManOrg.TSJ);
            }

            if (orgTypeIds.Contains(30) || orgTypeIds.Length == 0)
            {
                this.manOrgType.Add(TypeManagementManOrg.JSK);
            }
            
            if (orgTypeIds.Contains(40) || orgTypeIds.Length == 0)
            {
                this.includeSupplyResourceOrgs = true;
            }

            if (orgTypeIds.Contains(50) || orgTypeIds.Length == 0)
            {
                this.includeServiceOrganizations = true;
            }

            var statusId = baseParams.Params["statusId"].ToLong();
            switch (statusId)
            {
                case 0:
                    this.status = ContragentState.Active;
                    break;
                case 1:
                    this.status = ContragentState.NotManagementService;
                    break;
                case 2:
                    this.status = ContragentState.Bankrupt;
                    break;
                case 3:
                    this.status = ContragentState.Liquidated;
                    break;
                default:
                    this.status = ContragentState.Active;
                    break;
            }
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var serMunicipality = this.Container.Resolve<IDomainService<Municipality>>();
            var serManagingOrganization = this.Container.Resolve<IDomainService<ManagingOrganization>>();
            var serContragentContact = this.Container.Resolve<IDomainService<ContragentContact>>();
            var serContragentBank = this.Container.Resolve<IDomainService<ContragentBank>>();
            var serMoRealObj = this.Container.Resolve<IDomainService<ManagingOrgRealityObject>>();

            var manOrgList = new List<ContragentOrgInfoProxy>();
            var supplyResourceOrgList = new List<ContragentOrgInfoProxy>();
            var serviceOrganizationList = new List<ContragentOrgInfoProxy>();

            var municipalityDict = serMunicipality.GetAll()
                .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.Id))
                .Select(x => new { muId = x.Id, muName = x.Name })
                .ToDictionary(x => x.muId, x => x.muName);
            
            var manOrgInfoQuery = serManagingOrganization.GetAll()
                .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.Contragent.Municipality.Id))
                .Where(x => this.manOrgType.Contains(x.TypeManagement))
                .Where(x => x.Contragent != null && x.Contragent.Municipality != null)
                .Where(x => x.Contragent.ContragentState == this.status);

            var supplyResourceInfoQuery = this.Container.Resolve<IDomainService<SupplyResourceOrg>>().GetAll()
                .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.Contragent.Municipality.Id))
                .Where(x => x.Contragent != null && x.Contragent.Municipality != null)
                .Where(x => x.Contragent.ContragentState == this.status);

            var serviceOrganizationInfoQuery = this.Container.Resolve<IDomainService<ServiceOrganization>>().GetAll()
                .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.Contragent.Municipality.Id))
                .Where(x => x.Contragent != null && x.Contragent.Municipality != null)
                .Where(x => x.Contragent.ContragentState == this.status);

            var contragentMoIdsQuery = manOrgInfoQuery.Select(x => x.Contragent.Id);
            var contragentSupplyResIdsQuery = supplyResourceInfoQuery.Select(x => x.Contragent.Id);
            var contragentServOrgIdsQuery = serviceOrganizationInfoQuery.Select(x => x.Contragent.Id);

            var contragentReltyObjectsDict = serMoRealObj.GetAll()
                .Where(x => contragentMoIdsQuery.Contains(x.ManagingOrganization.Contragent.Id))
                .Select(x => new
                {
                    x.ManagingOrganization.Contragent.Id,
                    roId = x.RealityObject.Id,
                    x.RealityObject.AreaMkd,
                    x.RealityObject.TypeHouse
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(
                x => x.Key,
                x =>
                    {
                        var data = x.Distinct().ToList();
                        return new
                        {
                            roCount = data.Count(),
                            roBlockCount = data.Count(y => y.TypeHouse == TypeHouse.BlockedBuilding),
                            roIndividualCount = data.Count(y => y.TypeHouse == TypeHouse.Individual),
                            socialBehaviorCount = data.Count(y => y.TypeHouse == TypeHouse.SocialBehavior),
                            roTotalArea = data.Sum(y => y.AreaMkd)
                        };
                    });

            manOrgList = manOrgInfoQuery
                .Select(x => new ContragentOrgInfoProxy
                {
                    contrId = x.Contragent.Id,
                    muId = (long?)x.Contragent.Municipality.Id,
                    muName = x.Contragent.Municipality.Name,
                    contrName = x.Contragent.Name,
                    orgForm = x.Contragent.OrganizationForm.Name ?? string.Empty,
                    typeManag = x.TypeManagement.GetEnumMeta().Display,
                    numEmpl = x.NumberEmployees,
                    offSite = x.Contragent.OfficialWebsite,
                    countSrf = x.CountSrf,
                    countMo = x.CountMo,
                    countOffices = x.CountOffices,
                    shareSf = x.ShareSf,
                    shareMo = x.ShareMo,
                    contrInn = x.Contragent.Inn,
                    contrKpp = x.Contragent.Kpp,
                    jurAddress = x.Contragent.FiasJuridicalAddress.AddressName,
                    factAddress = x.Contragent.FactAddress,
                    mailAddress = x.Contragent.MailingAddress,
                    phone = x.Contragent.Phone,
                    email = x.Contragent.Email,
                    addressOutSub = x.Contragent.AddressOutsideSubject,
                    ogrn = x.Contragent.Ogrn,
                    dateRegist = x.Contragent.DateRegistration,
                    ogrnRegist = x.Contragent.OgrnRegistration,
                    okpo = (int?)x.Contragent.Okpo,
                    okved = x.Contragent.Okved,
                    phoneDisp = x.Contragent.PhoneDispatchService,
                    subBox = x.Contragent.SubscriberBox,
                    termination = (GroundsTermination?)x.ActivityGroundsTermination,
                    dateTerm = x.Contragent.DateTermination
                }).ToList();

            if (this.includeServiceOrganizations)
            {
                serviceOrganizationList = supplyResourceInfoQuery
                .Select(x => new ContragentOrgInfoProxy
                {
                    contrId = x.Contragent.Id,
                    muId = (long?)x.Contragent.Municipality.Id,
                    muName = x.Contragent.Municipality.Name,
                    contrName = x.Contragent.Name,
                    orgForm = x.Contragent.OrganizationForm.Name ?? string.Empty,
                    typeManag = "Поставщик жилищных услуг",
                    numEmpl = null,
                    offSite = x.Contragent.OfficialWebsite,
                    countSrf = null,
                    countMo = null,
                    countOffices = null,
                    shareSf = null,
                    shareMo = null,
                    contrInn = x.Contragent.Inn,
                    contrKpp = x.Contragent.Kpp,
                    jurAddress = x.Contragent.FiasJuridicalAddress.AddressName,
                    factAddress = x.Contragent.FactAddress,
                    mailAddress = x.Contragent.MailingAddress,
                    phone = x.Contragent.Phone,
                    email = x.Contragent.Email,
                    addressOutSub = x.Contragent.AddressOutsideSubject,
                    ogrn = x.Contragent.Ogrn,
                    dateRegist = x.Contragent.DateRegistration,
                    ogrnRegist = x.Contragent.OgrnRegistration,
                    okpo = (int?)x.Contragent.Okpo,
                    okved = x.Contragent.Okved,
                    phoneDisp = x.Contragent.PhoneDispatchService,
                    subBox = x.Contragent.SubscriberBox,
                    termination = (GroundsTermination?)x.ActivityGroundsTermination,
                    dateTerm = x.Contragent.DateTermination
                }).ToList();
            }

            if (this.includeSupplyResourceOrgs)
            {
                supplyResourceOrgList = serviceOrganizationInfoQuery
                .Select(x => new ContragentOrgInfoProxy
                {
                    contrId = x.Contragent.Id,
                    muId = (long?)x.Contragent.Municipality.Id,
                    muName = x.Contragent.Municipality.Name,
                    contrName = x.Contragent.Name,
                    orgForm = x.Contragent.OrganizationForm.Name ?? string.Empty,
                    typeManag = "Поставщик коммунальных услуг",
                    numEmpl = null,
                    offSite = x.Contragent.OfficialWebsite,
                    countSrf = null,
                    countMo = null,
                    countOffices = null,
                    shareSf = null,
                    shareMo = null,
                    contrInn = x.Contragent.Inn,
                    contrKpp = x.Contragent.Kpp,
                    jurAddress = x.Contragent.FiasJuridicalAddress.AddressName,
                    factAddress = x.Contragent.FactAddress,
                    mailAddress = x.Contragent.MailingAddress,
                    phone = x.Contragent.Phone,
                    email = x.Contragent.Email,
                    addressOutSub = x.Contragent.AddressOutsideSubject,
                    ogrn = x.Contragent.Ogrn,
                    dateRegist = x.Contragent.DateRegistration,
                    ogrnRegist = x.Contragent.OgrnRegistration,
                    okpo = (int?)x.Contragent.Okpo,
                    okved = x.Contragent.Okved,
                    phoneDisp = x.Contragent.PhoneDispatchService,
                    subBox = x.Contragent.SubscriberBox,
                    termination = (GroundsTermination?)x.ActivityGroundsTermination,
                    dateTerm = x.Contragent.DateTermination
                }).ToList();
            }

            var orgInfoList = manOrgList.Union(serviceOrganizationList.Union(supplyResourceOrgList)).ToList();

            var manOrgInfoDict = orgInfoList
                .OrderBy(x => x.muName)
                .ThenBy(x => x.contrName)
                .GroupBy(x => x.muId.ToLong())
                .ToDictionary(x => x.Key, x => x.ToList());

            var contragContacts = serContragentContact.GetAll()
                .Where(x => contragentMoIdsQuery.Contains(x.Contragent.Id)
                || contragentSupplyResIdsQuery.Contains(x.Contragent.Id)
                || contragentServOrgIdsQuery.Contains(x.Contragent.Id))
                .Where(x => x.Position.Code == "1" || x.Position.Code == "4")
                .Where(x => x.DateEndWork == null)
                .Select(x => new
                {
                    contrId = x.Contragent.Id,
                    fio = x.FullName,
                    phone = x.Phone,
                    email = x.Email
                })
                .AsEnumerable()
                .GroupBy(x => x.contrId)
                .ToDictionary(
                    x => x.Key,
                    x => new
                    {
                        fio = x.Select(y => y.fio).FirstOrDefault(),
                        phone = x.Select(y => y.phone).FirstOrDefault(),
                        email = x.Select(y => y.email).FirstOrDefault()
                    });

            var contragBanks = serContragentBank.GetAll()
                .Where(x => contragentMoIdsQuery.Contains(x.Contragent.Id)
                || contragentSupplyResIdsQuery.Contains(x.Contragent.Id)
                || contragentServOrgIdsQuery.Contains(x.Contragent.Id))
                .Select(x => new
                {
                    contrId = x.Contragent.Id,
                    bankId = x.Id,
                    bankName = x.Name,
                    bankBik = x.Bik,
                    corrAcc = x.CorrAccount,
                    settlAcc = x.SettlementAccount
                })
                .AsEnumerable()
                .GroupBy(x => x.contrId)
                .ToDictionary(
                    x => x.Key,
                    x => x.GroupBy(y => y.bankId)
                          .ToDictionary(
                                y => y.Key,
                                y => new
                                {
                                    bankName = y.Select(z => z.bankName).FirstOrDefault(),
                                    bankBik = y.Select(z => z.bankBik).FirstOrDefault(),
                                    corrAcc = y.Select(z => z.corrAcc).FirstOrDefault(),
                                    settlAcc = y.Select(z => z.settlAcc).FirstOrDefault()
                                }));

            var contragNotice = this.Container.Resolve<IDomainService<BusinessActivity>>().GetAll()
                .Join(
                    this.Container.Resolve<IDomainService<State>>().GetAll(),
                    x => x.State.Id,
                    y => y.Id,
                    (a, b) => new { BusinessActivity = a, State = b })
                .Where(x => contragentMoIdsQuery.Contains(x.BusinessActivity.Contragent.Id)
                || contragentSupplyResIdsQuery.Contains(x.BusinessActivity.Contragent.Id)
                || contragentServOrgIdsQuery.Contains(x.BusinessActivity.Contragent.Id))
                .Where(x => x.BusinessActivity.ObjectCreateDate <= this.reportDate.AddDays(1))
                .Select(x => new
                {
                    businessActivityId = x.BusinessActivity.Id,
                    contrId = x.BusinessActivity.Contragent.Id,
                    stateName = x.State.Name,
                    stateId = x.State.Id,
                    x.BusinessActivity.RegNum,
                    x.BusinessActivity.ObjectCreateDate
                })
                .AsEnumerable()
                .GroupBy(x => x.contrId)
                .ToDictionary(
                x => x.Key,
                x =>
                    {
                        var data = x.OrderByDescending(y => y.ObjectCreateDate).First();
                        return new { finalState = data.stateName, data.businessActivityId, data.RegNum };
                    });

            var finalStatesDict = this.GetBusinessActivityStatus(contragentMoIdsQuery, contragentSupplyResIdsQuery, contragentServOrgIdsQuery);

            var vertical = reportParams.ComplexReportParams.ДобавитьСекцию("vertical");
            var sectionMu = reportParams.ComplexReportParams.ДобавитьСекцию("sectionMu");
            var section = sectionMu.ДобавитьСекцию("section");

            if (this.status != ContragentState.Active)
            {
                vertical.ДобавитьСтроку();
                vertical["date"] = "Дата прекращения деятельности";
                vertical["col27"] = "$column27$";
            }

            var count = 0;
            foreach (var muData in manOrgInfoDict)
            {
                sectionMu.ДобавитьСтроку();
                sectionMu["MuName"] = municipalityDict.ContainsKey(muData.Key) ? municipalityDict[muData.Key] : string.Empty;

                var municipalityTotals = new long[8];
                var municipalityArea = 0M;

                var orgData = muData.Value;

                foreach (var orgInfo in orgData)
                {
                    section.ДобавитьСтроку();
                    section["column1"] = ++count;
                    section["column2"] = orgInfo.muName;
                    section["column3"] = orgInfo.contrName;
                    section["column4"] = orgInfo.orgForm;
                    section["column5"] = orgInfo.typeManag;
                    section["column6"] = orgInfo.numEmpl;
                    section["column7"] = orgInfo.offSite;
                    section["column8"] = orgInfo.countSrf;
                    municipalityTotals[0] += orgInfo.countSrf.ToLong();
                    section["column9"] = orgInfo.countMo;
                    municipalityTotals[1] += orgInfo.countMo.ToLong();
                    section["column10"] = orgInfo.countOffices;
                    municipalityTotals[2] += orgInfo.countOffices.ToLong();
                    section["column11"] = orgInfo.shareSf;
                    section["column12"] = orgInfo.shareMo;

                    if (contragNotice.ContainsKey(orgInfo.contrId))
                    {
                        municipalityTotals[3] += 1;
                        var contragentNotice = contragNotice[orgInfo.contrId];
                        section["regNumNotice"] = contragentNotice.RegNum;
                        section["haveNotice"] = finalStatesDict.ContainsKey(contragentNotice.businessActivityId) ? finalStatesDict[contragentNotice.businessActivityId] : contragentNotice.finalState;
                    }

                    section["column13"] = string.Format("'{0}", orgInfo.contrInn);
                    section["column14"] = string.Format("'{0}", orgInfo.contrKpp);
                    section["column15"] = orgInfo.jurAddress;
                    section["column16"] = orgInfo.factAddress;
                    section["column17"] = orgInfo.mailAddress;
                    section["phone"] = orgInfo.phone;
                    section["email"] = orgInfo.email;
                    section["column18"] = orgInfo.addressOutSub;
                    section["column19"] = string.Format("'{0}", orgInfo.ogrn);
                    section["column20"] = orgInfo.dateRegist.HasValue ? orgInfo.dateRegist.Value.ToShortDateString() : string.Empty;
                    section["column21"] = orgInfo.ogrnRegist;
                    section["column22"] = orgInfo.okpo.HasValue ? string.Format("'{0}", orgInfo.okpo) : string.Empty;
                    section["column23"] = orgInfo.okved;
                    section["column24"] = orgInfo.phoneDisp;
                    section["column25"] = orgInfo.subBox;
                    section["column26"] = orgInfo.termination;

                    if (contragentReltyObjectsDict.ContainsKey(orgInfo.contrId))
                    {
                        var contragentRoData = contragentReltyObjectsDict[orgInfo.contrId];

                        section["roCount"] = contragentRoData.roCount;
                        municipalityTotals[4] += contragentRoData.roCount;
                        section["roBlockCount"] = contragentRoData.roBlockCount;
                        municipalityTotals[5] += contragentRoData.roBlockCount;
                        section["roIndividualCount"] = contragentRoData.roIndividualCount;
                        municipalityTotals[6] += contragentRoData.roIndividualCount;
                        section["socialBehaviorCount"] = contragentRoData.socialBehaviorCount;
                        municipalityTotals[7] += contragentRoData.socialBehaviorCount;
                        section["roArea"] = contragentRoData.roTotalArea;
                        municipalityArea += contragentRoData.roTotalArea.ToDecimal();
                    }

                    if (this.status != ContragentState.Active)
                    {
                        section["column27"] = orgInfo.dateTerm.HasValue ?
                                                orgInfo.dateTerm.Value.ToShortDateString()
                                                : string.Empty;
                    }

                    if (contragContacts.ContainsKey(orgInfo.contrId))
                    {
                        var contactDict = contragContacts[orgInfo.contrId];
                        section["column28"] = contactDict.fio;
                        section["column29"] = contactDict.phone;
                        section["column30"] = contactDict.email;
                    }

                    if (contragBanks.ContainsKey(orgInfo.contrId))
                    {
                        var banksDict = contragBanks[orgInfo.contrId];
                        var bankCount = 0;
                        foreach (var bank in banksDict.Values)
                        {
                            ++bankCount;
                            section["column31"] = bank.bankName;
                            section["column32"] = bank.bankBik;
                            section["column33"] = string.Format("'{0}", bank.corrAcc);
                            section["column34"] = string.Format("'{0}", bank.settlAcc);

                            if (bankCount != banksDict.Keys.Count)
                            {
                                section.ДобавитьСтроку();
                            }
                        }
                    }
                }

                // заполнение итогов по мун. образованию
                sectionMu["MuCount"] = orgData.Count;
                sectionMu["mucolumn8"] = municipalityTotals[0];
                sectionMu["mucolumn9"] = municipalityTotals[1];
                sectionMu["mucolumn10"] = municipalityTotals[2];
                sectionMu["muCountNotice"] = municipalityTotals[3];
                sectionMu["roCountMu"] = municipalityTotals[4];
                sectionMu["roBlockCountMu"] = municipalityTotals[5];
                sectionMu["roIndividualCountMu"] = municipalityTotals[6];
                sectionMu["socialBehaviorCountMu"] = municipalityTotals[7];
                sectionMu["roAreaMu"] = municipalityArea;
            }
        }

        private Dictionary<long, string> GetBusinessActivityStatus(IQueryable<long> contragentMoIdsQuery, IQueryable<long> contragentSupplyResIdsQuery, IQueryable<long> contragentServOrgIdsQuery)
        {
            return
                this.Container.Resolve<IDomainService<BusinessActivity>>()
                    .GetAll()
                    .Join(
                        this.Container.Resolve<IDomainService<StateHistory>>().GetAll(),
                        x => x.Id,
                        y => y.EntityId,
                        (a, b) => new { BusinessActivity = a, StateHistory = b })
                    .Where(
                        x =>
                        contragentMoIdsQuery.Contains(x.BusinessActivity.Contragent.Id)
                        || contragentSupplyResIdsQuery.Contains(x.BusinessActivity.Contragent.Id)
                        || contragentServOrgIdsQuery.Contains(x.BusinessActivity.Contragent.Id))
                    .Where(x => x.BusinessActivity.ObjectCreateDate <= this.reportDate.AddDays(1))
                    .Where(x => x.StateHistory.TypeId == "gji_business_activity")
                    .Select(
                        x => new
                        {
                            businessActivityId = x.BusinessActivity.Id,
                            x.StateHistory.ChangeDate,
                            startName = x.StateHistory.StartState.Name,
                            finalName = x.StateHistory.FinalState.Name
                        })
                    .AsEnumerable()
                    .GroupBy(x => x.businessActivityId)
                    .ToDictionary(
                    x => x.Key,
                    x =>
                    {
                        var allChanges = x.OrderBy(y => y.ChangeDate).ToList();
                        var changesToReportDate = allChanges.Where(y => y.ChangeDate <= this.reportDate.AddDays(1)).ToList();

                        return changesToReportDate.Count > 0 ? changesToReportDate.Last().finalName : allChanges.First().startName;
                    });
        }
    }

    internal sealed class ContragentOrgInfoProxy
    {
        public long contrId;

        public long? muId;

        public string muName;

        public string contrName;

        public string orgForm;

        public string typeManag;

        public int? numEmpl;

        public string offSite;

        public int? countSrf;

        public int? countMo;

        public int? countOffices;

        public decimal? shareSf;

        public decimal? shareMo;

        public string contrInn;

        public string contrKpp;

        public string jurAddress;

        public string factAddress;

        public string mailAddress;

        public string phone;

        public string email;

        public string addressOutSub;

        public string ogrn;

        public DateTime? dateRegist;

        public string ogrnRegist;

        public int? okpo;

        public string okved;

        public string phoneDisp;

        public string subBox;

        public GroundsTermination? termination;

        public DateTime? dateTerm;
    }
}