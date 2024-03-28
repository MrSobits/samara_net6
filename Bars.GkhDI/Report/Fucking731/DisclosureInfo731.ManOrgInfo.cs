namespace Bars.GkhDi.Report.Fucking731
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using B4;
    using B4.DataAccess;
    using B4.Utils;
    using Entities;
    using Gkh.DomainService;
    using Gkh.Entities;
    using Gkh.Enums;
    using Gkh.Utils;

    public partial class DisclosureInfo731
    {
        public IDomainService<DisclosureInfo> DiDomain { get; set; }

        protected void FillManOrgInfo(ManOrgRecord record, DisclosureInfo dinfo)
        {
            if (dinfo == null)
            {
                return;
            }

            var contragent = dinfo.ManagingOrganization.Contragent;

            record.ManOrg = contragent.Name;
            record.ManOrgOgrn = contragent.Ogrn;
            record.ManOrgRegDate = contragent.DateRegistration.ToDateString();
            record.ManOrgRegOrg = contragent.OgrnRegistration;
            record.ManOrgFactAddress = contragent.FactAddress;
            record.ManOrgMailAddress = contragent.MailingAddress;
            record.ManOrgPhone = contragent.Phone;
            record.ManOrgEmail = contragent.Email;

            FillManOrgWorkMode(record, dinfo);

            FillObjects(record, dinfo);

            FillMembershipUnions(record, dinfo);

            FillFundsInfo(record, dinfo);

            FillContractInfo(record, dinfo);

            FillAdminResp(record, dinfo);

            FillTerminateContracts(record, dinfo);

            FillFinActivityDoc(record, dinfo);

            FillFinActivityManag(record, dinfo);
        }
        
        protected void FillManOrgWorkMode(ManOrgRecord record, DisclosureInfo dinfo)
        {
            var workModeDomain = Container.ResolveDomain<ManagingOrgWorkMode>();

            var query = workModeDomain.GetAll()
                .Where(x => x.ManagingOrganization.Id == dinfo.ManagingOrganization.Id);

            var workMode = query.Where(x => x.TypeMode == TypeMode.WorkMode).ToArray();
            record.ManOrgWorkMode = AggregateWorkMode(workMode);

            var receptCits = query.Where(x => x.TypeMode == TypeMode.ReceptionCitizens).ToArray();
            record.ManOrgReceptCits = AggregateWorkMode(receptCits);
        }

        protected void FillObjects(ManOrgRecord record, DisclosureInfo dinfo)
        {
            var manorgRoService = Container.Resolve<IManagingOrgRealityObjectService>();
            var roRepos = Container.ResolveRepository<RealityObject>();

            var filterRo =
                manorgRoService
                    .GetAllActive(dinfo.PeriodDi.DateStart.GetValueOrDefault(), dinfo.PeriodDi.DateEnd)
                    .Where(x => x.ManOrgContract.ManagingOrganization.Id == dinfo.ManagingOrganization.Id)
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

            var sumArea = 0m;

            var i = 0;

            foreach (var obj in objectsInManag)
            {
                record.ManOrgObject.Add(new ManOrgObject
                {
                    Number = ++i,
                    Address = obj.Address,
                    AreaLiving = obj.AreaLiving ?? 0
                });

                sumArea += obj.AreaLiving.GetValueOrDefault();
            }

            record.ManOrgObjectsSumArea = sumArea;
        }

        protected void FillMembershipUnions(ManOrgRecord record, DisclosureInfo dinfo)
        {
            var membershipUnionDomain = Container.ResolveDomain<ManagingOrgMembership>();

            var data = membershipUnionDomain.GetAll()
                .Where(x => x.ManagingOrganization.Id == dinfo.ManagingOrganization.Id)
                .Where(x => x.DateStart <= dinfo.PeriodDi.DateEnd)
                .Where(x => !x.DateEnd.HasValue || x.DateEnd >= dinfo.PeriodDi.DateStart)
                .Select(x => new
                {
                    x.Address,
                    x.Name,
                    x.OfficialSite
                })
                .OrderBy(x => x.Name);

            var i = 0;

            record.ManOrgMembershipUnion = new List<ManOrgMembershipUnion>();

            foreach (var item in data)
            {
                record.ManOrgMembershipUnion.Add(new ManOrgMembershipUnion
                {
                    Number = ++i,
                    Address = item.Address,
                    Name = item.Name,
                    OfficialSite = item.OfficialSite
                });
            }
        }

        protected void FillFundsInfo(ManOrgRecord record, DisclosureInfo disclosure)
        {
            var fundsInfoDomain = Container.ResolveDomain<FundsInfo>();

            var data = fundsInfoDomain.GetAll()
                .Where(x => x.DisclosureInfo.Id == disclosure.Id)
                .Select(x => new
                {
                    x.DocumentName,
                    x.DocumentDate,
                    x.Size
                });

            var i = 0;

            foreach (var item in data)
            {
                record.ManOrgFundsInfo.Add(new ManOrgFundsInfo
                {
                    Number = ++i,
                    Name = item.DocumentName,
                    PaymentSize = item.Size ?? 0,
                    Date = item.DocumentDate.ToDateString()
                });
            }
        }

        protected void FillContractInfo(ManOrgRecord record, DisclosureInfo disclosure)
        {
            var domain = Container.ResolveDomain<InformationOnContracts>();

            var data = domain.GetAll()
                .Where(x => x.DisclosureInfo.Id == disclosure.Id)
                .Select(x => new
                {
                    x.Name,
                    x.RealityObject.Address,
                    x.Cost,
                    x.DateStart
                });

            var i = 0;

            foreach (var item in data)
            {
                record.ManOrgContractInfo.Add(new ManOrgContractInfo
                {
                    Number = ++i,
                    Address = item.Address,
                    Cost = item.Cost ?? 0,
                    Date = item.DateStart.ToDateString(),
                    Name = item.Name
                });
            }
        }

        protected void FillAdminResp(ManOrgRecord record, DisclosureInfo disclosure)
        {
            var domain = Container.ResolveDomain<AdminResp>();

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

            foreach (var item in data)
            {
                record.ManOrgAdminResp.Add(new ManOrgAdminResp
                {
                    Number = ++i,
                    Name = item.Name,
                    CountViolation = item.AmountViolation ?? 0,
                    Date = item.DateImpositionPenalty.ToDateString(),
                    DatePayment = item.DatePaymentPenalty.ToDateString(),
                    Sum = item.SumPenalty ?? 0
                });
            }
        }

        protected void FillTerminateContracts(ManOrgRecord record, DisclosureInfo disclosure)
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
                    AddressName = x.terminateContract.RealityObject.FiasAddress.Return(y => y.AddressName)
                })
                .ToList();

            var i = 0;

            foreach (var item in dataList)
            {
                record.ManOrgTerminateContract.Add(new ManOrgTerminateContract
                {
                    Number = ++i,
                    Address = item.AddressName,
                    Reason = item.TerminateReason
                });
            }

            record.ManOrgTerminateContractCount = i;
        }

        protected void FillFinActivityDoc(ManOrgRecord record, DisclosureInfo disclosure)
        {
            var docsDomain = Container.ResolveDomain<FinActivityDocs>();

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
                record.ExistBookkepingBalance = data.ExistBalance ? text : null;
                record.ExistBookkepingBalanceAnnex = data.ExistAnnex ? text : null;
            }
        }

        protected void FillFinActivityManag(ManOrgRecord record, DisclosureInfo disclosure)
        {
            var docsDomain = Container.ResolveDomain<FinActivityManagRealityObj>();

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

            int i = 0;

            foreach (var item in data)
            {
                record.ManOrgFinActivityManag.Add(new ManOrgFinActivityManag
                {
                    Number = ++i,
                    Address = item.Address,
                    AreaMkd = item.AreaMkd ?? 0,
                    PresentedToPay = item.PresentedToRepay ?? 0,
                    RecievedProvidedService = item.ReceivedProvidedService ?? 0,
                    SumDebt = item.SumDebt ?? 0
                });
            }
        }

        protected DisclosureInfo GetDisclosureInfo()
        {
            var dinfo = DiDomain.GetAll()
                .Where(x => x.PeriodDi.Id == _periodId)
                .FirstOrDefault(x => x.ManagingOrganization.Id == _manorgId);

            if (dinfo == null)
            {
                return null;
            }

            return DiDomain.Get(dinfo.Id);
        }

        protected string AggregateWorkMode(ManagingOrgWorkMode[] workModes)
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
    }
}