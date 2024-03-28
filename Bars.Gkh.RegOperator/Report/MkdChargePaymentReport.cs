namespace Bars.Gkh.RegOperator.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Modules.Reports;
    using B4.Utils;
    using Bars.Gkh.Decisions.Nso.Domain;
    using Bars.Gkh.Enums.Decisions;
    using Bars.Gkh.RegOperator.Entities;
    using Gkh.Domain;
    using Gkh.Entities;
    using Gkh.Enums;
    using Overhaul.DomainService;
    using GkhRf.Entities;
    using Castle.Windsor;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Utils;

    public class MkdChargePaymentReport : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }
        public IDomainService<ManOrgContractRealityObject> ManorgContractRoDomain { get; set; }
        public IDomainService<ContractRfObject> ContractObjDomain { get; set; }
        public IDomainService<TransferCtr> TransferCtrDomain { get; set; }
        public IDomainService<RealityObjectChargeAccountOperation> RoChaccoperDomain { get; set; }
        public IRepository<RealityObject> RobjectRepository { get; set; }
        public IRegOpAccountDecisionRo TatDecisionRoService { get; set; }
        public IRealityObjectDecisionsService RoDecisionService { get; set; }
        public IRealityObjectsProgramVersion RealObjProgramVersion { get; set; }
        private long[] _muIds;
        private DateTime _startDate;
        private DateTime _endDate;
        private CrFundFormationDecisionType _fund;
        private YesNoNotSet _hasInDpkr;

        public MkdChargePaymentReport()
            : base(new ReportTemplateBinary(Properties.Resources.MkdChargePaymentReport))
        {
        }

        public override string Name
        {
            get { return "Сведения о поступлении взносов на капитальный ремонт от собственников МКД"; }
        }

        public override string Desciption
        {
            get { return "Сведения о поступлении взносов на капитальный ремонт от собственников МКД"; }
        }

        public override string GroupName
        {
            get { return "Отчеты ГЖИ"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.MkdChargePaymentReport"; }
        }

        public override string RequiredPermission
        {
            get { return "Reports.GkhRegOp.MkdChargePaymentReport"; }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            _muIds = baseParams.Params.GetAs<string>("parentMu").ToLongArray();

            _startDate = baseParams.Params.GetAs<DateTime>("startDate");
            _endDate = baseParams.Params.ContainsKey("endDate")
                ? baseParams.Params["endDate"].ToDateTime()
                : DateTime.MaxValue;

            var methodFormFund = baseParams.Params.GetAs<MethodFormFundCr>("fund");

            _fund = methodFormFund == MethodFormFundCr.RegOperAccount ? CrFundFormationDecisionType.RegOpAccount :
                methodFormFund == MethodFormFundCr.SpecialAccount ? CrFundFormationDecisionType.SpecialAccount : 
                CrFundFormationDecisionType.Unknown;

            _hasInDpkr = baseParams.Params.GetAs("hasInDpkr", YesNoNotSet.NotSet);
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var roInDpkrQuery = RealObjProgramVersion.GetMainVersionRealityObjects();

            var roQuery = RobjectRepository.GetAll()
                .WhereIf(_muIds.Any(), x => _muIds.Contains(x.Municipality.Id))
                .WhereIf(_hasInDpkr == YesNoNotSet.Yes, x => roInDpkrQuery.Any(y => y.Id == x.Id))
                .WhereIf(_hasInDpkr == YesNoNotSet.No, x => !roInDpkrQuery.Any(y => y.Id == x.Id));

            var dict = new Dictionary<long, CrFundFormationDecisionType>();
            if (TatDecisionRoService != null)
            {
                dict = TatDecisionRoService.GetRobjectFormFundCr(_muIds, _endDate);
            }
            else
            {
                dict = RoDecisionService.GetRobjectsFundFormation(roQuery.Select(x => x.Id))
                    .Where(x => x.Value.Any(y => y.Item1 < _endDate))
                    .ToDictionary(x => x.Key, 
                                 y => y.Value
                                       .Where(x => x.Item1 < _endDate)
                                       .OrderByDescending(x => x.Item1)
                                       .Select(x => x.Item2)
                                       .First());
            }

            //управляющие организации домов без решений о формировании КР
            var dictRoManorg = ManorgContractRoDomain.GetAll()
                .WhereIf(_muIds.Any(), x => _muIds.Contains(x.RealityObject.Municipality.Id))
                .Where(x => x.ManOrgContract.StartDate <= _endDate)
                .Where(x => !x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate >= _startDate)
                .Where(x => x.ManOrgContract.TypeContractManOrgRealObj != TypeContractManOrg.DirectManag)
                .Select(x => new
                {
                    x.RealityObject.Id,
                    ManOrg = x.ManOrgContract.ManagingOrganization.Contragent.Name,
                    OrgForm = x.ManOrgContract.ManagingOrganization.Contragent.OrganizationForm.Name,
                    TypeManagement = (TypeManagementManOrg?) x.ManOrgContract.ManagingOrganization.TypeManagement,
                    x.ManOrgContract.StartDate
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key,
                    y => y
                        .OrderByDescending(z => z.StartDate)
                        .ThenBy(x => x.TypeManagement)
                        .Select(z => new
                        {
                            z.Id,
                            z.ManOrg,
                            z.OrgForm
                        })
                        .FirstOrDefault());

            var dictRoContract = ContractObjDomain.GetAll()
                .WhereIf(_muIds.Any(), x => _muIds.Contains(x.RealityObject.Municipality.Id))
                .Where(x => x.ContractRf.DateBegin <= _endDate)
                .Where(x => !x.ContractRf.DateEnd.HasValue || x.ContractRf.DateEnd >= _startDate)
                .Select(x => new
                {
                    RoId = x.RealityObject.Id,
                    x.ContractRf.DateBegin,
                    ManOrg = x.ContractRf.ManagingOrganization.Contragent.Name,
                    OrgForm = x.ContractRf.ManagingOrganization.Contragent.OrganizationForm.Name,
                    x.ContractRf.ManagingOrganization.TypeManagement
                })
                .AsEnumerable()
                .GroupBy(x => x.RoId)
                .ToDictionary(x => x.Key,
                    y => y.OrderByDescending(x => x.DateBegin).ThenBy(x => x.TypeManagement).First());

            var dictValues = RoChaccoperDomain.GetAll()
                .WhereIf(_muIds.Any(), x => _muIds.Contains(x.Account.RealityObject.Municipality.Id))
                .Where(x => x.Period.StartDate <= _endDate)
                .Where(x => !x.Period.EndDate.HasValue || x.Period.EndDate >= _startDate)
                .Select(x => new
                {
                    x.Account.RealityObject.Id,
                    Charged = x.ChargedTotal + x.ChargedPenalty,
                    Paid = x.PaidTotal + x.PaidPenalty
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, y => new {Charged = y.Sum(x => x.Charged), Paid = y.Sum(x => x.Paid)});

            var transferFundsRf = TransferCtrDomain.GetAll()
                .Where(x => x.DateFrom <= _endDate && x.DateFrom >= _startDate)
                .Where(x => x.State.Code == "11")
                .Select(x => new
                {
                    RoId = x.ObjectCr.RealityObject.Id,
                    ContragentId = x.Builder.Contragent.Id,
                    x.PaidSum
                })
                .AsEnumerable()
                .GroupBy(x => x.RoId)
                .ToDictionary(x => x.Key, y => y.SafeSum(x => x.PaidSum));             

            var roByMuInfo = roQuery
                .Select(x => new
                {
                    x.Id,
                    x.Address,
                    x.AreaLivingNotLivingMkd,
                    Municipality = x.Municipality.Name,
                    MunicipalityId = x.Municipality.Id
                })
                .AsEnumerable()
                .OrderBy(x => x.Municipality)
                .ThenBy(x => x.Address)
                .GroupBy(x => x.MunicipalityId)
                .ToDictionary(x => x.Key, y => y.ToList());

            var sectionMo = reportParams.ComplexReportParams.ДобавитьСекцию("СекцияМо");

            int i = 0;
            decimal chargedSum = 0, paidSum = 0, transferSum = 0;

            reportParams.SimpleReportParams["EndPeriod"] = _endDate.ToShortDateString();
            foreach (var roInfo in roByMuInfo)
            {
                sectionMo.ДобавитьСтроку();
                sectionMo["AreaMu"] = roInfo.Value.SafeSum(x => x.AreaLivingNotLivingMkd.ToDecimal());
                sectionMo["MU"] = roInfo.Value.First().Return(x => x.Municipality);

                var section = sectionMo.ДобавитьСекцию("секция");

                var chargedMu = 0M;
                var paidMu = 0M;
                var transferMu = 0M;
                foreach (var item in roInfo.Value)
                {
                    if (!dict.ContainsKey(item.Id) || (_fund != CrFundFormationDecisionType.Unknown && dict[item.Id] != _fund))
                    {
                        continue;
                    }

                    section.ДобавитьСтроку();

                    section["Number"] = ++i;
                    section["MU"] = item.Municipality;
                    section["Address"] = item.Address;
                    section["Area"] = item.AreaLivingNotLivingMkd.HasValue ? item.AreaLivingNotLivingMkd.Value : 0;

                    var method = dict[item.Id];

                    section["Fund"] = method.GetEnumMeta().Display;

                    switch (method)
                    {
                        case CrFundFormationDecisionType.Unknown:
                        case CrFundFormationDecisionType.SpecialAccount:
                            if (dictRoManorg.ContainsKey(item.Id))
                            {
                                var moInfo = dictRoManorg[item.Id];
                                section["MoForm"] = moInfo.OrgForm;
                                section["ManOrg"] = moInfo.ManOrg;
                            }

                            break;
                        case CrFundFormationDecisionType.RegOpAccount:
                            if (dictRoContract.ContainsKey(item.Id))
                            {
                                var moInfo = dictRoContract[item.Id];
                                section["MoForm"] = moInfo.OrgForm;
                                section["ManOrg"] = moInfo.ManOrg;
                            }

                            break;
                    }

                    decimal charged = 0, paid = 0, transfer = 0;
                    if (dictValues.ContainsKey(item.Id))
                    {
                        var valInfo = dictValues[item.Id];
                        charged = valInfo.Charged;
                        paid = valInfo.Paid;
                    }

                    transfer = transferFundsRf.Get(item.Id);

                    section["Charged"] = charged.RegopRoundDecimal(2);
                    section["Paid"] = paid.RegopRoundDecimal(2);
                    section["Remain"] = (paid - transfer).RegopRoundDecimal(2);

                    chargedSum += charged;
                    paidSum += paid;
                    transferSum += transfer;

                    chargedMu += charged;
                    paidMu += paid;
                    transferMu += transfer;
                }

                sectionMo["ChargedSumMu"] = chargedMu.RegopRoundDecimal(2);
                sectionMo["PaidSumMu"] = paidMu.RegopRoundDecimal(2);
                sectionMo["RemainSumMu"] = (paidMu - transferMu).RegopRoundDecimal(2);
            }

            reportParams.SimpleReportParams["ChargedSum"] = chargedSum.RegopRoundDecimal(2);
            reportParams.SimpleReportParams["PaidSum"] = paidSum.RegopRoundDecimal(2);
            reportParams.SimpleReportParams["RemainSum"] = (paidSum - transferSum).RegopRoundDecimal(2);

            Container.Resolve<ISessionProvider>().CreateNewSession();
        }
    }
}