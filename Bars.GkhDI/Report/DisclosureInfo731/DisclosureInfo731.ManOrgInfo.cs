namespace Bars.GkhDi.Report
{
    using System;
    using System.Linq;
    using System.Text;
    using B4.DataAccess;
    using B4.Utils;

    using Bars.B4.Modules.Reports;

    using Entities;
    using Gkh.DomainService;
    using Gkh.Entities;
    using Gkh.Enums;

    public partial class DisclosureInfo731
    {
        /// <summary>
        /// заполнение информации по уо (пункт 1. Общая информация об управляющей организации)
        /// </summary>
        /// <param name="reportParams"></param>
        /// <param name="dinfo"></param>
        private void FillManOrgInfo(ReportParams reportParams, DisclosureInfo dinfo)
        {
            var contragent = dinfo.ManagingOrganization.Contragent;

            reportParams.SimpleReportParams["ManOrg"] = contragent.Name;
            reportParams.SimpleReportParams["ManOrgOgrn"] = contragent.Ogrn;
            reportParams.SimpleReportParams["ManOrgRegDate"] = FormatDate(contragent.DateRegistration);

            reportParams.SimpleReportParams["ManOrgRegOrg"] = contragent.OgrnRegistration;
            reportParams.SimpleReportParams["ManOrgFactAddress"] = contragent.FactAddress;
            reportParams.SimpleReportParams["ManOrgMailAddress"] = contragent.MailingAddress;
            reportParams.SimpleReportParams["ManOrgPhone"] = contragent.Phone;
            reportParams.SimpleReportParams["ManOrgEmail"] = contragent.Email;

            FillManOrgWorkMode(reportParams, dinfo.ManagingOrganization);

            FillObjects(reportParams, dinfo.ManagingOrganization, dinfo.PeriodDi);

            FillMembershipUnions(reportParams, dinfo.ManagingOrganization, dinfo.PeriodDi);

            if (dinfo.ManagingOrganization.TypeManagement == TypeManagementManOrg.TSJ
                || dinfo.ManagingOrganization.TypeManagement == TypeManagementManOrg.JSK)
            {
                var section = reportParams.ComplexReportParams.ДобавитьСекцию("sectionForTsjJsk");
                section.ДобавитьСтроку();

                FillFundsInfo(section, dinfo);

                FillContractInfo(section, dinfo);
            }

            FillAdminResp(reportParams, dinfo);

            FillTerminateContracts(reportParams, dinfo);

            FillFinActivityDoc(reportParams, dinfo);

            FillFinActivityManag(reportParams, dinfo);
        }

        private void FillManOrgWorkMode(ReportParams reportParams, ManagingOrganization manorg)
        {
            var workModeDomain = Container.ResolveDomain<ManagingOrgWorkMode>();

            try
            {
                var query = workModeDomain.GetAll()
                    .Where(x => x.ManagingOrganization.Id == manorg.Id);

                var workMode = query.Where(x => x.TypeMode == TypeMode.WorkMode).ToArray();
                reportParams.SimpleReportParams["ManOrgWorkMode"] = AggregateWorkMode(workMode);

                var receptCits = query.Where(x => x.TypeMode == TypeMode.ReceptionCitizens).ToArray();
                reportParams.SimpleReportParams["ManOrgReceptCits"] = AggregateWorkMode(receptCits);
            }
            finally
            {
                Container.Release(workModeDomain);
            }
        }

        private static string AggregateWorkMode(ManagingOrgWorkMode[] workModes)
        {
            var result = new StringBuilder();
            foreach (TypeDayOfWeek wm in Enum.GetValues(typeof(TypeDayOfWeek)))
            {
                var currentDay = workModes.FirstOrDefault(x => x.TypeDayOfWeek == wm);
                if (currentDay != null)
                {
                    result.AppendFormat("{0}: {1}-{2}, перерыв:{3};", 
                        wm.GetEnumMeta().Display, 
                        currentDay.StartDate.ToDateTime().ToShortTimeString(),
                        currentDay.EndDate.ToDateTime().ToShortTimeString(), 
                        currentDay.Pause);
                }
            }

            return result.ToString();
        }

        private void FillObjects(ReportParams reportParams, ManagingOrganization manorg, PeriodDi period)
        {
            var manorgRoService = Container.Resolve<IManagingOrgRealityObjectService>();
            var roRepos = Container.ResolveRepository<RealityObject>();

            try
            {
                var filterRo =
                    manorgRoService
                        .GetAllActive(period.DateStart.Value, period.DateEnd)
                        .Where(x => x.ManOrgContract.ManagingOrganization.Id == manorg.Id)
                        .Select(x => x.RealityObject);

                var objectsInManag =
                    roRepos.GetAll()
                        .Where(x => filterRo.Any(y => y.Id == x.Id))
                        .Select(x => new
                        {
                            x.Address,
                            x.AreaLiving
                        })
                        .OrderBy(x => x.Address);

                var section = reportParams.ComplexReportParams.ДобавитьСекцию("sectionManOrgObjects");

                var sumArea = 0m;

                var i = 0;

                foreach (var obj in objectsInManag)
                {
                    section.ДобавитьСтроку();

                    section["Number"] = ++i;
                    section["Address"] = obj.Address;
                    section["AreaLiving"] = obj.AreaLiving;

                    sumArea += obj.AreaLiving.GetValueOrDefault();
                }

                reportParams.SimpleReportParams["ManOrgObjectsSumArea"] = sumArea;
            }
            finally
            {
                Container.Release(manorgRoService);
                Container.Release(roRepos);
            }
        }

        private void FillMembershipUnions(ReportParams reportParams, ManagingOrganization manorg, PeriodDi period)
        {
            var membershipUnionDomain = Container.ResolveDomain<ManagingOrgMembership>();

            try
            {
                var unions = membershipUnionDomain.GetAll()
                    .Where(x => x.ManagingOrganization.Id == manorg.Id)
                    .Where(x => x.DateStart <= period.DateEnd)
                    .Where(x => !x.DateEnd.HasValue || x.DateEnd >= period.DateStart)
                    .Select(x => new
                    {
                        x.Address,
                        x.Name,
                        x.OfficialSite
                    })
                    .OrderBy(x => x.Name);

                var sectionUnions = reportParams.ComplexReportParams.ДобавитьСекцию("sectionManOrgMembershipUnion");

                var i = 0;

                foreach (var union in unions)
                {
                    sectionUnions.ДобавитьСтроку();

                    sectionUnions["Number"] = ++i;
                    sectionUnions["Address"] = union.Address;
                    sectionUnions["Name"] = union.Name;
                    sectionUnions["OfficialSite"] = union.OfficialSite;
                }
            }
            finally
            {
                Container.Release(membershipUnionDomain);
            }
        }

        private void FillFundsInfo(Section section, DisclosureInfo disclosure)
        {
            var fundsInfoDomain = Container.ResolveDomain<FundsInfo>();

            try
            {
                var data = fundsInfoDomain.GetAll()
                    .Where(x => x.DisclosureInfo.Id == disclosure.Id)
                    .Select(x => new
                    {
                        x.DocumentName,
                        x.DocumentDate,
                        x.Size
                    });

                var sectionFunds = section.ДобавитьСекцию("sectionManOrgFundsInfo");

                var i = 0;

                foreach (var item in data)
                {
                    sectionFunds.ДобавитьСтроку();

                    sectionFunds["Number"] = ++i;
                    sectionFunds["Name"] = item.DocumentName;
                    sectionFunds["Size"] = item.Size;
                    sectionFunds["Date"] = item.DocumentDate;
                }
            }
            finally
            {
                Container.Release(fundsInfoDomain);
            }
        }

        private void FillContractInfo(Section section, DisclosureInfo disclosure)
        {
            var domain = Container.ResolveDomain<InformationOnContracts>();

            try
            {
                var data = domain.GetAll()
                    .Where(x => x.DisclosureInfo.Id == disclosure.Id)
                    .Select(x => new
                    {
                        x.Name,
                        x.RealityObject.Address,
                        x.Cost,
                        x.DateStart
                    });

                var sectionContractInfo = section.ДобавитьСекцию("sectionContractInfo");

                var i = 0;

                foreach (var item in data)
                {
                    sectionContractInfo.ДобавитьСтроку();

                    sectionContractInfo["Number"] = ++i;
                    sectionContractInfo["Name"] = item.Name;
                    sectionContractInfo["Address"] = item.Address;
                    sectionContractInfo["Cost"] = FormatDecimal(item.Cost);
                    sectionContractInfo["Date"] = FormatDate(item.DateStart);
                }
            }
            finally
            {
                Container.Release(domain);
            }
        }

        private void FillAdminResp(ReportParams reportParams, DisclosureInfo disclosure)
        {
            var domain = Container.ResolveDomain<AdminResp>();

            try
            {
                var data = domain.GetAll()
                    .Where(x => x.DisclosureInfo.Id == disclosure.Id)
                    .Select(x => new
                    {
                        x.SupervisoryOrg.Name,
                        x.AmountViolation,
                        x.SumPenalty,
                        x.DatePaymentPenalty,
                        x.DateImpositionPenalty
                    });

                var i = 0;

                var section = reportParams.ComplexReportParams.ДобавитьСекцию("sectionManOrgAdminResp");

                foreach (var item in data)
                {
                    section.ДобавитьСтроку();

                    section["Number"] = ++i;

                    section["Name"] = item.Name;
                    section["CountViolation"] = item.AmountViolation;
                    section["Date"] = FormatDate(item.DateImpositionPenalty);
                    section["Sum"] = FormatDecimal(item.SumPenalty);
                    section["DatePayment"] = FormatDate(item.DatePaymentPenalty);
                }
            }
            finally
            {
                Container.Release(domain);
            }
        }

        private void FillTerminateContracts(ReportParams reportParams, DisclosureInfo disclosure)
        {
            var domainService = Container.ResolveDomain<ManOrgContractRealityObject>();

            // Получаем список договоров у которых была дата расторжения в периоде по упр орг из раскрытия инф-ии. Список1
            var listTerminateContracts = domainService.GetAll()
                .Where(x => x.ManOrgContract.EndDate >= disclosure.PeriodDi.DateStart.Value.AddYears(-1)
                    && x.ManOrgContract.EndDate < disclosure.PeriodDi.DateEnd.Value.AddYears(-1)
                    && x.ManOrgContract.ManagingOrganization.Id == disclosure.ManagingOrganization.Id)
                .ToList();

            // Получаем список договоров по упр орг из раскрытия инф-ии. Список2
            var listContracts = domainService.GetAll()
                .Where(x => x.ManOrgContract.ManagingOrganization.Id == disclosure.ManagingOrganization.Id)
                .ToList();

            // бежим по списку1 и смотрим:
            // 1.Есть ли договор по данному дому в списке2
            // 2.Если есть то дата начала договора из списка2 == дате конца договора из списка1 + 1 (продлили на след день после окончания)
            // Если усл 1 и 2 выполняются то договор не считается расторгнутым, иначе кладем его в список расторгнутых договоров
            var dataList = listTerminateContracts
                .Select(terminateContract => new
                {
                    terminateContract,
                    count = listContracts
                        .Where(x => x.RealityObject.Id == terminateContract.RealityObject.Id)
                        .Count(x =>
                                x.ManOrgContract.StartDate.HasValue
                                && terminateContract.ManOrgContract.EndDate.HasValue
                                && x.ManOrgContract.StartDate.Value.Date == terminateContract.ManOrgContract.EndDate.Value.Date.AddDays(1))
                })
                .Where(x => x.count == 0)
                .Select(x => new
                {
                    x.terminateContract.Id,
                    x.terminateContract.ManOrgContract.TerminateReason,
                    AddressName =
                        x.terminateContract.RealityObject.FiasAddress != null
                            ? x.terminateContract.RealityObject.FiasAddress.AddressName
                            : string.Empty
                })
                .ToList();

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("sectionManOrgTerminateContract");

            var i = 0;

            foreach (var item in dataList)
            {
                section.ДобавитьСтроку();

                section["Number"] = ++i;
                section["Address"] = item.AddressName;
                section["Reason"] = item.TerminateReason;
            }

            reportParams.SimpleReportParams["ManOrgTerminateContractCount"] = i;
        }

        private void FillFinActivityDoc(ReportParams reportParams, DisclosureInfo disclosure)
        {
            var docsDomain = Container.ResolveDomain<FinActivityDocs>();

            try
            {
                var data = docsDomain.GetAll()
                    .Where(x => x.DisclosureInfo.Id == disclosure.Id)
                    .Select(x => new
                    {
                        ExistBalance = x.BookkepingBalance != null,
                        ExistAnnex = x.BookkepingBalanceAnnex != null
                    })
                    .FirstOrDefault();

                var text = "Да";

                if (data != null)
                {
                    reportParams.SimpleReportParams["ExistBookkepingBalance"] = data.ExistBalance ? text : null;
                    reportParams.SimpleReportParams["ExistBookkepingBalanceAnnex"] = data.ExistAnnex ? text : null;
                }
            }
            finally
            {
                Container.Release(docsDomain);
            }
        }

        private void FillFinActivityManag(ReportParams reportParams, DisclosureInfo disclosure)
        {
            var docsDomain = Container.ResolveDomain<FinActivityManagRealityObj>();

            try
            {
                var data = docsDomain.GetAll()
                    .Where(x => x.DisclosureInfo.Id == disclosure.Id)
                    .Select(x => new
                    {
                        x.RealityObject.Address,
                        x.RealityObject.AreaMkd,
                        x.PresentedToRepay,
                        x.ReceivedProvidedService,
                        x.SumDebt
                    });

                var section = reportParams.ComplexReportParams.ДобавитьСекцию("sectionManOrgFinActivityManag");

                int i = 0;

                foreach (var item in data)
                {
                    section.ДобавитьСтроку();

                    section["Number"] = ++i;
                    section["Address"] = item.Address;
                    section["AreaMkd"] = FormatDecimal(item.AreaMkd);
                    section["PresentedToPay"] = FormatDecimal(item.PresentedToRepay);
                    section["RecievedProvidedService"] = FormatDecimal(item.ReceivedProvidedService);
                    section["SumDebt"] = FormatDecimal(item.SumDebt);
                }
            }
            finally
            {
                Container.Release(docsDomain);
            }
        }
    }
}