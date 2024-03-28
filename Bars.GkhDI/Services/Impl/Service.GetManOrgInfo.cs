namespace Bars.GkhDi.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhDi.Entities;
    using Bars.GkhDi.Enums;

    public partial class Service
    {
        public GetManOrgInfoResponse GetManOrgInfo(string manOrgId)
        {
            var ci = CultureInfo.InvariantCulture.Clone() as CultureInfo;
            NumberFormatInfo numberformat = null;
            if (ci != null)
            {
                ci.NumberFormat.NumberDecimalSeparator = ".";
                numberformat = ci.NumberFormat;
            }

            var idManOrg = manOrgId.ToLong();

            var fileManager = this.Container.Resolve<IFileManager>();

            if (idManOrg != 0)
            {
                var disInfos = this.Container.Resolve<IDomainService<DisclosureInfo>>()
                    .GetAll()
                    .Where(x => x.ManagingOrganization.Id == idManOrg)
                    .OrderBy(x => x.PeriodDi)
                    .ToArray();

                var contragentIds = disInfos.Select(x => x.ManagingOrganization.Contragent.Id).ToArray();
                var diIds = disInfos.Select(x => x.Id).ToArray();

                var contacts = this.Container.Resolve<IDomainService<ContragentContact>>()
                    .GetAll().Where(x => contragentIds.Contains(x.Contragent.Id))
                    .ToArray();

                var workModes = this.Container.Resolve<IDomainService<ManagingOrgWorkMode>>()
                    .GetAll()
                    .Where(x => x.ManagingOrganization.Id == idManOrg)
                    .GroupBy(x => x.TypeMode)
                    .ToDictionary(
                        x => x.Key,
                        y => y.GroupBy(x => x.TypeDayOfWeek)
                            .ToDictionary(
                                x => x.Key,
                                z => z.Select(
                                    x => new Day
                                    {
                                        StartTime = x.StartDate.HasValue ? x.StartDate.Value.ToShortTimeString() : string.Empty,
                                        FinishTime = x.EndDate.HasValue ? x.EndDate.Value.ToShortTimeString() : string.Empty,
                                        Pause = x.Pause.ToStr(),
                                        AroundClock = x.AroundClock ? "Круглосуточно" : null
                                    }).FirstOrDefault()));

                var documents = this.Container.Resolve<IDomainService<Documents>>()
                    .GetAll()
                    .Where(x => diIds.Contains(x.DisclosureInfo.Id))
                    .GroupBy(x => x.DisclosureInfo.Id)
                    .ToDictionary(
                        x => x.Key,
                        y => y.Select(
                            x => new
                            {
                                ProjectContract = x.FileProjectContract != null
                                    ? new BaseDoc
                                    {
                                        Id = x.FileProjectContract.Id,
                                        IdFile = x.FileProjectContract.Id,
                                        NameFile =
                                            string.Format("{0}.{1}", x.FileProjectContract.Name, x.FileProjectContract.Extention),
                                        NotAvailable =
                                            x.DisclosureInfo.ManagingOrganization.TypeManagement == TypeManagementManOrg.TSJ
                                                ? x.NotAvailable ? "Да" : "Нет"
                                                : null
                                    }
                                    : null,
                                ListCommunalService = x.FileCommunalService != null
                                    ? new BaseDoc
                                    {
                                        Id = x.FileCommunalService.Id,
                                        IdFile = x.FileCommunalService.Id,
                                        NameFile =
                                            string.Format("{0}.{1}", x.FileCommunalService.Name, x.FileCommunalService.Extention)
                                    }
                                    : null,
                                ListServiceApartment = x.FileServiceApartment != null
                                    ? new BaseDoc
                                    {
                                        Id = x.FileServiceApartment.Id,
                                        IdFile = x.FileServiceApartment.Id,
                                        NameFile =
                                            string.Format(
                                                "{0}.{1}",
                                                x.FileServiceApartment.Name,
                                                x.FileServiceApartment.Extention)
                                    }
                                    : null
                            }).FirstOrDefault());

                var adminResponsibilities = this.Container.Resolve<IDomainService<AdminResp>>()
                    .GetAll()
                    .Where(x => diIds.Contains(x.DisclosureInfo.Id))
                    .GroupBy(x => x.DisclosureInfo.Id)
                    .ToDictionary(
                        x => x.Key,
                        y => y.Select(
                            x => new AdminResponsibility
                            {
                                Id = x.Id,
                                ControlOrg = x.SupervisoryOrg.Name,
                                ViolCount = x.AmountViolation.HasValue ? x.AmountViolation.ToStr() : null,
                                DatePayFine =
                                    x.DatePaymentPenalty.HasValue ? x.DatePaymentPenalty.Value.ToShortDateString() : null,
                                FineDate =
                                    x.DateImpositionPenalty.HasValue ? x.DateImpositionPenalty.Value.ToShortDateString() : null,
                                SumFine =
                                    x.SumPenalty.HasValue
                                        ? x.SumPenalty.ToDecimal().RoundDecimal(2).ToString(numberformat)
                                        : null,
                                Arrangements = x.Actions,
                                File = x.File != null
                                    ? new BaseDoc
                                    {
                                        Id = x.File.Id,
                                        IdFile = x.File.Id,
                                        NameFile = string.Format("{0}.{1}", x.File.Name, x.File.Extention)
                                    }
                                    : null
                            }).ToArray());

                var finActivityAuditYearsDi = this.Container.Resolve<IDomainService<FinActivityAudit>>()
                    .GetAll()
                    .Where(x => x.ManagingOrganization.Id == idManOrg)
                    .GroupBy(y => y.Year)
                    .ToDictionary(
                        y => y.Key,
                        x => x.Select(
                            z => z.File != null
                                ? new BaseDoc
                                {
                                    Id = z.File.Id,
                                    IdFile = z.File.Id,
                                    NameFile = string.Format("{0}.{1}", z.File.Name, z.File.Extention)
                                }
                                : null).FirstOrDefault());

                var finActivityDocsDisInfo = this.Container.Resolve<IDomainService<FinActivityDocs>>()
                    .GetAll()
                    .Where(x => diIds.Contains(x.DisclosureInfo.Id))
                    .ToDictionary(
                        x => x.DisclosureInfo.Id,
                        y => new
                        {
                            BookkepingBalance = y.BookkepingBalance != null
                                ? new BaseDoc
                                {
                                    Id = y.BookkepingBalance.Id,
                                    IdFile = y.BookkepingBalance.Id,
                                    NameFile =
                                        string.Format("{0}.{1}", y.BookkepingBalance.Name, y.BookkepingBalance.Extention)
                                }
                                : null,
                            BookkepingBalanceAnnex = y.BookkepingBalanceAnnex != null
                                ? new BaseDoc
                                {
                                    Id = y.BookkepingBalanceAnnex.Id,
                                    IdFile = y.BookkepingBalanceAnnex.Id,
                                    NameFile =
                                        string.Format(
                                            "{0}.{1}",
                                            y.BookkepingBalanceAnnex.Name,
                                            y.BookkepingBalanceAnnex.Extention)
                                }
                                : null
                        });

                var finActivityDocsByYearDi = this.Container.Resolve<IDomainService<FinActivityDocByYear>>()
                    .GetAll()
                    .Where(x => x.ManagingOrganization.Id == idManOrg)
                    .Select(
                        x => new
                        {
                            x.Year,
                            x.TypeDocByYearDi,
                            Doc = x.File != null
                                ? new BaseDoc
                                {
                                    Id = x.File.Id,
                                    IdFile = x.File.Id,
                                    NameFile = string.Format("{0}.{1}", x.File.Name, x.File.Extention)
                                }
                                : null
                        })
                    .ToArray();

                var fundsInfo = this.Container.Resolve<IDomainService<FundsInfo>>()
                    .GetAll()
                    .Where(x => diIds.Contains(x.DisclosureInfo.Id))
                    .GroupBy(x => x.DisclosureInfo.Id)
                    .ToDictionary(
                        y => y.Key,
                        z => z.Select(
                            x => new InfoFund
                            {
                                Name = x.DocumentName,
                                Date = x.DocumentDate.HasValue ? x.DocumentDate.Value.ToShortDateString() : string.Empty,
                                Size = x.Size.ToDecimal().RoundDecimal(2).ToString(numberformat)
                            }).ToArray());

                var infoLicense = this.Container.ResolveDomain<DisclosureInfoLicense>()
                    .GetAll()
                    .Where(x => diIds.Contains(x.DisclosureInfo.Id))
                    .GroupBy(x => x.DisclosureInfo.Id)
                    .ToDictionary(y => y.Key, z => z.FirstOrDefault());

                var workMode = workModes.ContainsKey(TypeMode.WorkMode)
                    ? new WorkMode
                    {
                        Monday =
                            workModes[TypeMode.WorkMode].ContainsKey(TypeDayOfWeek.Monday)
                                ? workModes[TypeMode.WorkMode][TypeDayOfWeek.Monday]
                                : null,
                        Tuesday =
                            workModes[TypeMode.WorkMode].ContainsKey(TypeDayOfWeek.Tuesday)
                                ? workModes[TypeMode.WorkMode][TypeDayOfWeek.Tuesday]
                                : null,
                        Wednesday =
                            workModes[TypeMode.WorkMode].ContainsKey(TypeDayOfWeek.Wednesday)
                                ? workModes[TypeMode.WorkMode][TypeDayOfWeek.Wednesday]
                                : null,
                        Thursday =
                            workModes[TypeMode.WorkMode].ContainsKey(TypeDayOfWeek.Thursday)
                                ? workModes[TypeMode.WorkMode][TypeDayOfWeek.Thursday]
                                : null,
                        Friday =
                            workModes[TypeMode.WorkMode].ContainsKey(TypeDayOfWeek.Friday)
                                ? workModes[TypeMode.WorkMode][TypeDayOfWeek.Friday]
                                : null,
                        Saturday =
                            workModes[TypeMode.WorkMode].ContainsKey(TypeDayOfWeek.Saturday)
                                ? workModes[TypeMode.WorkMode][TypeDayOfWeek.Saturday]
                                : null,
                        Sunday =
                            workModes[TypeMode.WorkMode].ContainsKey(TypeDayOfWeek.Sunday)
                                ? workModes[TypeMode.WorkMode][TypeDayOfWeek.Sunday]
                                : null
                    }
                    : null;

                var receptionCitizens = workModes.ContainsKey(TypeMode.ReceptionCitizens)
                    ? new WorkMode
                    {
                        Monday =
                            workModes[TypeMode.ReceptionCitizens].ContainsKey(TypeDayOfWeek.Monday)
                                ? workModes[TypeMode.ReceptionCitizens][TypeDayOfWeek.Monday]
                                : null,
                        Tuesday =
                            workModes[TypeMode.ReceptionCitizens].ContainsKey(TypeDayOfWeek.Tuesday)
                                ? workModes[TypeMode.ReceptionCitizens][TypeDayOfWeek.Tuesday]
                                : null,
                        Wednesday =
                            workModes[TypeMode.ReceptionCitizens].ContainsKey(TypeDayOfWeek.Wednesday)
                                ? workModes[TypeMode.ReceptionCitizens][TypeDayOfWeek.Wednesday]
                                : null,
                        Thursday =
                            workModes[TypeMode.ReceptionCitizens].ContainsKey(TypeDayOfWeek.Thursday)
                                ? workModes[TypeMode.ReceptionCitizens][TypeDayOfWeek.Thursday]
                                : null,
                        Friday =
                            workModes[TypeMode.ReceptionCitizens].ContainsKey(TypeDayOfWeek.Friday)
                                ? workModes[TypeMode.ReceptionCitizens][TypeDayOfWeek.Friday]
                                : null,
                        Saturday =
                            workModes[TypeMode.ReceptionCitizens].ContainsKey(TypeDayOfWeek.Saturday)
                                ? workModes[TypeMode.ReceptionCitizens][TypeDayOfWeek.Saturday]
                                : null,
                        Sunday =
                            workModes[TypeMode.ReceptionCitizens].ContainsKey(TypeDayOfWeek.Sunday)
                                ? workModes[TypeMode.ReceptionCitizens][TypeDayOfWeek.Sunday]
                                : null
                    }
                    : null;

                var dispatcherWork = workModes.ContainsKey(TypeMode.DispatcherWork)
                    ? new WorkMode
                    {
                        Monday =
                            workModes[TypeMode.DispatcherWork].ContainsKey(TypeDayOfWeek.Monday)
                                ? workModes[TypeMode.DispatcherWork][TypeDayOfWeek.Monday]
                                : null,
                        Tuesday =
                            workModes[TypeMode.DispatcherWork].ContainsKey(TypeDayOfWeek.Tuesday)
                                ? workModes[TypeMode.DispatcherWork][TypeDayOfWeek.Tuesday]
                                : null,
                        Wednesday =
                            workModes[TypeMode.DispatcherWork].ContainsKey(TypeDayOfWeek.Wednesday)
                                ? workModes[TypeMode.DispatcherWork][TypeDayOfWeek.Wednesday]
                                : null,
                        Thursday =
                            workModes[TypeMode.DispatcherWork].ContainsKey(TypeDayOfWeek.Thursday)
                                ? workModes[TypeMode.DispatcherWork][TypeDayOfWeek.Thursday]
                                : null,
                        Friday =
                            workModes[TypeMode.DispatcherWork].ContainsKey(TypeDayOfWeek.Friday)
                                ? workModes[TypeMode.DispatcherWork][TypeDayOfWeek.Friday]
                                : null,
                        Saturday =
                            workModes[TypeMode.DispatcherWork].ContainsKey(TypeDayOfWeek.Saturday)
                                ? workModes[TypeMode.DispatcherWork][TypeDayOfWeek.Saturday]
                                : null,
                        Sunday =
                            workModes[TypeMode.DispatcherWork].ContainsKey(TypeDayOfWeek.Sunday)
                                ? workModes[TypeMode.DispatcherWork][TypeDayOfWeek.Sunday]
                                : null
                    }
                    : null;

                var periods = disInfos.Select(
                    x =>
                        new DiPeriod
                        {
                            Id = x.PeriodDi.Id,
                            Name = x.PeriodDi.Name,
                            PeriodCode = x.PeriodDi.Id,
                            StartDate =
                                x.PeriodDi.DateStart.HasValue
                                    ? x.PeriodDi.DateStart.Value.ToShortDateString()
                                    : string.Empty,
                            FinishDate =
                                x.PeriodDi.DateEnd.HasValue ? x.PeriodDi.DateEnd.Value.ToShortDateString() : string.Empty,
                            ManOrg = new ManOrg
                            {
                                Id = x.ManagingOrganization.Id,
                                Name = x.ManagingOrganization.Contragent.Name,
                                JurAddress = x.ManagingOrganization.Contragent.FiasJuridicalAddress != null
                                    ? x.ManagingOrganization.Contragent.FiasJuridicalAddress.AddressName
                                    : string.Empty,
                                FioDirector = this.GetPositionByCode(
                                    contacts,
                                    x.ManagingOrganization.Contragent.Id,
                                    x.PeriodDi,
                                    new List<string> {"1", "4"}),
                                Ogrn = x.ManagingOrganization.Contragent.Ogrn,
                                RegYear =
                                    x.ManagingOrganization.Contragent.DateRegistration.HasValue
                                        ? x.ManagingOrganization.Contragent.DateRegistration.Value.Year.ToStr()
                                        : string.Empty,
                                OgrnRegistration = x.ManagingOrganization.Contragent.OgrnRegistration,
                                MailAddress = x.ManagingOrganization.Contragent.FiasMailingAddress != null
                                    ? x.ManagingOrganization.Contragent.FiasMailingAddress.AddressName
                                    : string.Empty,
                                Phone = x.ManagingOrganization.Contragent.Phone,
                                DispatcherPhone = x.ManagingOrganization.Contragent.PhoneDispatchService,
                                Email = x.ManagingOrganization.Contragent.Email,
                                Twitter = x.ManagingOrganization.Contragent.TweeterAccount,
                                FrguRegNumber = x.ManagingOrganization.Contragent.FrguRegNumber,
                                FrguOrgNumber = x.ManagingOrganization.Contragent.FrguOrgNumber,
                                FrguServiceNumber = x.ManagingOrganization.Contragent.FrguServiceNumber,
                                OfficialWebsite = x.ManagingOrganization.Contragent.OfficialWebsite,
                                FactAddress = x.ManagingOrganization.Contragent.FiasFactAddress != null
                                    ? x.ManagingOrganization.Contragent.FiasFactAddress.AddressName
                                    : string.Empty,
                                RevComMember = this.GetPositionByCode(
                                    contacts,
                                    x.ManagingOrganization.Contragent.Id,
                                    x.PeriodDi,
                                    new List<string> {"5"}),
                                DirectionMember = this.GetPositionByCode(
                                    contacts,
                                    x.ManagingOrganization.Contragent.Id,
                                    x.PeriodDi,
                                    new List<string> {"6"}),
                                TypeManagment = x.ManagingOrganization.TypeManagement.GetEnumMeta().Display,
                                CurrentDirector = this.GetContactByCode(
                                    contacts,
                                    x.ManagingOrganization.Contragent.Id,
                                    x.PeriodDi,
                                    new List<string> {"1", "4"}),
                                WorkMode = workMode,
                                ReceptionCitizens = receptionCitizens,
                                DispatcherWork = dispatcherWork,
                                TermContracts = new TermContracts
                                {
                                    NoTerminate = x.TerminateContract == YesNoNotSet.Yes ? "Нет" : "Да",
                                    Contracts =
                                        x.TerminateContract == YesNoNotSet.Yes
                                            ? this.GetTerminateContracts(x.PeriodDi, idManOrg)
                                            : null
                                },
                                MembershipUnions = new MembershipUnions
                                {
                                    Unions =
                                        x.MembershipUnions == YesNoNotSet.Yes
                                            ? this.GetMembershipUnions(x.PeriodDi, idManOrg)
                                            : null
                                },
                                BaseIndicators = new BaseIndicators
                                {
                                    TermsOfService = new TermsOfService
                                    {
                                        ProjectContract =
                                            documents.ContainsKey(x.Id) ? documents[x.Id].ProjectContract : null,
                                        ListCommunalService =
                                            documents.ContainsKey(x.Id) ? documents[x.Id].ListCommunalService : null,
                                        ListServiceApartment =
                                            documents.ContainsKey(x.Id) ? documents[x.Id].ListServiceApartment : null,
                                        InfoAdminResponsibility = new InfoAdminResponsibility
                                        {
                                            IsAdminResponsibility = x.AdminResponsibility == YesNoNotSet.Yes ? "Нет" : "Да",
                                            AdminResponsibilities =
                                                x.AdminResponsibility == YesNoNotSet.Yes &&
                                                    adminResponsibilities.ContainsKey(x.Id)
                                                    ? adminResponsibilities[x.Id]
                                                    : null
                                        }
                                    }
                                },
                                FinanceActivity = new FinanceActivity
                                {
                                    BookBalance =
                                        finActivityDocsDisInfo.ContainsKey(x.Id)
                                            ? finActivityDocsDisInfo[x.Id].BookkepingBalance
                                            : null,
                                    BookBalanceAnnex =
                                        finActivityDocsDisInfo.ContainsKey(x.Id)
                                            ? finActivityDocsDisInfo[x.Id].BookkepingBalanceAnnex
                                            : null,
                                    EstimateCurrentYear =
                                        finActivityDocsByYearDi.FirstOrDefault(
                                            y =>
                                                y.TypeDocByYearDi == TypeDocByYearDi.EstimateIncome &&
                                                    y.Year == x.PeriodDi.DateStart.Value.Year) != null
                                            ? finActivityDocsByYearDi.FirstOrDefault(
                                                y =>
                                                    y.TypeDocByYearDi == TypeDocByYearDi.EstimateIncome &&
                                                        y.Year == x.PeriodDi.DateStart.Value.Year).Doc
                                            : null,
                                    EstimatePrevYear =
                                        finActivityDocsByYearDi.FirstOrDefault(
                                            y =>
                                                y.TypeDocByYearDi == TypeDocByYearDi.EstimateIncome &&
                                                    y.Year == x.PeriodDi.DateStart.Value.Year - 1) != null
                                            ? finActivityDocsByYearDi.FirstOrDefault(
                                                y =>
                                                    y.TypeDocByYearDi == TypeDocByYearDi.EstimateIncome &&
                                                        y.Year == x.PeriodDi.DateStart.Value.Year - 1).Doc
                                            : null,
                                    ReportEstimatePrevYear =
                                        finActivityDocsByYearDi.FirstOrDefault(
                                            y =>
                                                y.TypeDocByYearDi == TypeDocByYearDi.ReportEstimateIncome &&
                                                    y.Year == x.PeriodDi.DateStart.Value.Year - 1) != null
                                            ? finActivityDocsByYearDi.FirstOrDefault(
                                                y =>
                                                    y.TypeDocByYearDi == TypeDocByYearDi.ReportEstimateIncome &&
                                                        y.Year == x.PeriodDi.DateStart.Value.Year - 1).Doc
                                            : null,
                                    RevComCurrentYear =
                                        finActivityDocsByYearDi.FirstOrDefault(
                                            y =>
                                                y.TypeDocByYearDi == TypeDocByYearDi.ConclusionRevisory &&
                                                    y.Year == x.PeriodDi.DateStart.Value.Year) != null
                                            ? finActivityDocsByYearDi.FirstOrDefault(
                                                y =>
                                                    y.TypeDocByYearDi == TypeDocByYearDi.ConclusionRevisory &&
                                                        y.Year == x.PeriodDi.DateStart.Value.Year).Doc
                                            : null,
                                    RevComPrevYear =
                                        finActivityDocsByYearDi.FirstOrDefault(
                                            y =>
                                                y.TypeDocByYearDi == TypeDocByYearDi.ConclusionRevisory &&
                                                    y.Year == x.PeriodDi.DateStart.Value.Year - 1) != null
                                            ? finActivityDocsByYearDi.FirstOrDefault(
                                                y =>
                                                    y.TypeDocByYearDi == TypeDocByYearDi.ConclusionRevisory &&
                                                        y.Year == x.PeriodDi.DateStart.Value.Year - 1).Doc
                                            : null,
                                    RevComPrevPrevYear =
                                        finActivityDocsByYearDi.FirstOrDefault(
                                            y =>
                                                y.TypeDocByYearDi == TypeDocByYearDi.ConclusionRevisory &&
                                                    y.Year == x.PeriodDi.DateStart.Value.Year - 2) != null
                                            ? finActivityDocsByYearDi.FirstOrDefault(
                                                y =>
                                                    y.TypeDocByYearDi == TypeDocByYearDi.ConclusionRevisory &&
                                                        y.Year == x.PeriodDi.DateStart.Value.Year - 2).Doc
                                            : null,
                                    AuditCurrentYear =
                                        finActivityAuditYearsDi.ContainsKey(x.PeriodDi.DateStart.Value.Year)
                                            ? finActivityAuditYearsDi[x.PeriodDi.DateStart.Value.Year]
                                            : null,
                                    AuditPrevYear =
                                        finActivityAuditYearsDi.ContainsKey(x.PeriodDi.DateStart.Value.Year - 1)
                                            ? finActivityAuditYearsDi[x.PeriodDi.DateStart.Value.Year - 1]
                                            : null,
                                    AuditPrevPrevYear =
                                        finActivityAuditYearsDi.ContainsKey(x.PeriodDi.DateStart.Value.Year - 2)
                                            ? finActivityAuditYearsDi[x.PeriodDi.DateStart.Value.Year - 2]
                                            : null,
                                    IncomeAndExpenses = this.GetFinActivityRealObjs(x.PeriodDi, idManOrg, x.Id, numberformat),
                                    FundsInfo =
                                        x.ManagingOrganization.TypeManagement == TypeManagementManOrg.UK
                                            ? null
                                            : new InfoFunds
                                            {
                                                Id = x.Id,
                                                Payment = x.SizePayments.ToDecimal().RoundDecimal(2).ToString(numberformat),
                                                Funds =
                                                    x.FundsInfo == YesNoNotSet.Yes && fundsInfo.ContainsKey(x.Id)
                                                        ? fundsInfo[x.Id]
                                                        : null
                                            },
                                    Utilities = this.GetUtilites(x.Id, numberformat)
                                },
                                Houses = this.GetRealityObjects(x.PeriodDi, idManOrg, numberformat),
                                OrganizationForm = x.ManagingOrganization.Contragent.OrganizationForm.Name,
                                INN = x.ManagingOrganization.Contragent.Inn,
                                Fax = x.ManagingOrganization.Contragent.Fax,
                                DispatcherAdress = x.ManagingOrganization.IsDispatchCrrespondedFact
                                    ? x.ManagingOrganization?.Contragent?.FactAddress
                                    : x.ManagingOrganization?.DispatchAddress?.AddressName ?? string.Empty,
                                Charter = (x.ManagingOrganization.TypeManagement == TypeManagementManOrg.TSJ || x.ManagingOrganization.TypeManagement == TypeManagementManOrg.JSK) 
                                && x.ManagingOrganization.DispatchFile != null
                                    ? new File
                                    {
                                        Name = x.ManagingOrganization.DispatchFile.Name,
                                        IdFile = x.ManagingOrganization.DispatchFile.Id,
                                        Extension = x.ManagingOrganization.DispatchFile.Extention
                                    }
                                    : null,
                                Rating = new Rating
                                {
                                    WorkersCount =
                                        x.ManagingOrganization.NumberEmployees != null
                                            ? x.ManagingOrganization.NumberEmployees.ToStr()
                                            : string.Empty,
                                    StaffRegularAdministrative = x.AdminPersonnel?.ToString(numberformat) ?? string.Empty,
                                    StaffRegularEngineers = x.Engineer?.ToString(numberformat) ?? string.Empty,
                                    StaffRegularLabor = x.Work?.ToString(numberformat) ?? string.Empty,
                                    ProportionSf = x.ManagingOrganization.ShareSf?.ToString(numberformat) ?? string.Empty,
                                    ProportionMo = x.ManagingOrganization.ShareMo?.ToString(numberformat) ?? string.Empty,
                                },
                                License = new License
                                {
                                    NumLicense = x.HasLicense == YesNoNotSet.Yes ? infoLicense.Get(x.Id)?.LicenseNumber : string.Empty,
                                    DataLicense = x.HasLicense == YesNoNotSet.Yes ? infoLicense.Get(x.Id)?.DateReceived.ToShortDateString() : string.Empty,
                                    OrgLicense = x.HasLicense == YesNoNotSet.Yes ? infoLicense.Get(x.Id)?.LicenseOrg : string.Empty,
                                    DocLicense = (x.HasLicense == YesNoNotSet.Yes && infoLicense.Get(x.Id)?.LicenseDoc != null && fileManager.CheckFile(infoLicense.Get(x.Id).LicenseDoc.Id).Success)
                                        ? new File
                                        {
                                            Name = infoLicense.Get(x.Id).LicenseDoc.Name,
                                            IdFile = infoLicense.Get(x.Id).LicenseDoc.Id,
                                            Extension = infoLicense.Get(x.Id).LicenseDoc.Extention
                                        }
                                        : null
                                }
                            }
                        }).ToArray();

                return new GetManOrgInfoResponse {Period = periods};
            }

            return new GetManOrgInfoResponse();
        }

        private string GetPositionByCode(
            IEnumerable<ContragentContact> list,
            long contragentId,
            PeriodDi periodDi,
            List<string> listCodes)
        {
            return list
                .Where(
                    x => x.Position != null && x.Contragent.Id == contragentId && listCodes.Contains(x.Position.Code)
                        &&
                        ((x.DateStartWork.HasValue &&
                            (x.DateStartWork.Value >= periodDi.DateStart.Value &&
                                periodDi.DateEnd.Value >= x.DateStartWork.Value) || !x.DateStartWork.HasValue)
                            ||
                            (periodDi.DateStart.Value >= x.DateStartWork.Value &&
                                ((x.DateEndWork.HasValue && x.DateEndWork.Value >= periodDi.DateStart.Value) ||
                                    !x.DateEndWork.HasValue))))
                .OrderByDescending(x => x.DateStartWork ?? DateTime.MinValue)
                .Select(x => x.FullName).FirstOrDefault();
        }

        private CurrentDirector GetContactByCode(
            IEnumerable<ContragentContact> list,
            long contragentId,
            PeriodDi periodDi,
            List<string> listCodes)
        {
            return 
                list.Where(
                    x => x.Contragent.Id == contragentId && x.Position != null && listCodes.Contains(x.Position.Code)
                        &&
                        ((x.DateStartWork.HasValue &&
                                (x.DateStartWork.Value >= periodDi.DateStart.Value &&
                                    periodDi.DateEnd.Value >= x.DateStartWork.Value) || !x.DateStartWork.HasValue)
                            ||
                            (periodDi.DateStart.Value >= x.DateStartWork.Value &&
                                ((x.DateEndWork.HasValue && x.DateEndWork.Value >= periodDi.DateStart.Value) ||
                                    !x.DateEndWork.HasValue))))
                .OrderByDescending(x => x.DateStartWork ?? DateTime.MinValue)
                .Select(
                    x => new CurrentDirector
                    {
                        Fio = x.FullName,
                        Position = x.Position.Name,
                        StartDate = x.DateStartWork.HasValue ? x.DateStartWork.Value.ToShortDateString() : string.Empty,
                        FinishDate = x.DateEndWork.HasValue ? x.DateEndWork.Value.ToShortDateString() : string.Empty,
                        Phone = x.Phone
                    }).FirstOrDefault();
        }

        private RealityObjs GetRealityObjects(PeriodDi period, long manOrgId, NumberFormatInfo numberformat)
        {
            var realObjs = this.Container.Resolve<IDomainService<ManOrgContractRealityObject>>().GetAll()
                .Where(x => x.ManOrgContract.ManagingOrganization.Id == manOrgId)
                .Where(
                    x => ((x.ManOrgContract.StartDate.HasValue && period.DateStart.HasValue
                        && (x.ManOrgContract.StartDate.Value >= period.DateStart.Value)
                        || !period.DateStart.HasValue)
                        && (x.ManOrgContract.StartDate.HasValue && period.DateEnd.HasValue
                            && (period.DateEnd.Value >= x.ManOrgContract.StartDate.Value)
                            || !period.DateEnd.HasValue))
                        ||
                        ((x.ManOrgContract.StartDate.HasValue && period.DateStart.HasValue &&
                            (period.DateStart.Value >= x.ManOrgContract.StartDate.Value) ||
                            !x.ManOrgContract.StartDate.HasValue)
                            && (x.ManOrgContract.StartDate.HasValue && period.DateEnd.HasValue
                                && (x.ManOrgContract.EndDate.Value >= period.DateStart.Value)
                                || !x.ManOrgContract.EndDate.HasValue)))
                .Select(
                    x => new RealityObj
                    {
                        Id = x.RealityObject.Id,
                        Municipality = x.RealityObject.Municipality.Name,
                        Address = x.RealityObject.Address,
                        AreaMkd = x.RealityObject.AreaMkd.ToDecimal().RoundDecimal(2).ToString(numberformat),
                        AreaMkdDec = x.RealityObject.AreaMkd.ToDecimal(),
                        StartDate =
                            x.ManOrgContract.StartDate.HasValue
                                ? x.ManOrgContract.StartDate.Value.ToShortDateString()
                                : string.Empty,
                        FinishDate =
                            x.ManOrgContract.EndDate.HasValue
                                ? x.ManOrgContract.EndDate.Value.ToShortDateString()
                                : string.Empty,
                        GkhCode = x.RealityObject.GkhCode.ToStr(),
                    })
                .OrderBy(x => x.Address)
                .AsEnumerable()
                .Distinct(x => x.Id)
                .ToArray();

            return new RealityObjs
            {
                AreaMkd = realObjs.Sum(x => x.AreaMkdDec).RoundDecimal(2).ToString(numberformat),
                RealityObjList = realObjs
            };
        }

        private TermContract[] GetTerminateContracts(PeriodDi period, long manOrgId)
        {
            var moRoContractService = this.Container.Resolve<IDomainService<ManOrgContractRealityObject>>();

            var listTerminateContracts = moRoContractService.GetAll()
                .Where(
                    x => x.ManOrgContract.EndDate >= period.DateStart.Value.AddYears(-1)
                        && x.ManOrgContract.EndDate < period.DateEnd.Value.AddYears(-1)
                        && x.ManOrgContract.ManagingOrganization.Id == manOrgId)
                .ToList();

            var listContracts = moRoContractService.GetAll()
                .Where(x => x.ManOrgContract.ManagingOrganization.Id == manOrgId)
                .ToList();

            return listTerminateContracts
                .Select(
                    terminateContract =>
                        new
                        {
                            terminateContract,
                            count = listContracts.Count(
                                x =>
                                    x.RealityObject.Id == terminateContract.RealityObject.Id &&
                                        x.ManOrgContract.StartDate.HasValue
                                        && terminateContract.ManOrgContract.EndDate.HasValue
                                        &&
                                        x.ManOrgContract.StartDate.Value.Date ==
                                            terminateContract.ManOrgContract.EndDate.Value.Date.AddDays(1))
                        })
                .Where(x => x.count == 0)
                .Select(
                    x => new TermContract
                    {
                        Id = x.terminateContract.Id,
                        Reason = x.terminateContract.ManOrgContract.TerminateReason,
                        Address =
                            x.terminateContract.RealityObject.FiasAddress != null
                                ? x.terminateContract.RealityObject.FiasAddress.AddressName
                                : string.Empty,
                        StartDate =
                            x.terminateContract.ManOrgContract.StartDate.HasValue
                                ? x.terminateContract.ManOrgContract.StartDate.Value.ToShortDateString()
                                : null,
                        FinishDate =
                            x.terminateContract.ManOrgContract.EndDate.HasValue
                                ? x.terminateContract.ManOrgContract.EndDate.Value.ToShortDateString()
                                : null
                    })
                .ToArray();
        }

        private Union[] GetMembershipUnions(PeriodDi period, long manOrgId)
        {
            return this.Container.Resolve<IDomainService<ManagingOrgMembership>>().GetAll()
                .Where(x => x.ManagingOrganization.Id == manOrgId)
                .Where(
                    x =>
                        (x.DateStart.Value >= period.DateStart.Value
                            && period.DateEnd.Value >= x.DateStart.Value)
                            || (period.DateStart.Value >= x.DateStart.Value
                                && ((x.DateEnd.HasValue
                                    && x.DateEnd.Value >= period.DateStart.Value)
                                    || !x.DateEnd.HasValue)))
                .Select(
                    x => new Union
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Address = x.Address,
                        OfficialWebsite = x.OfficialSite
                    })
                .ToArray();
        }

        private Utilities GetUtilites(long diId, NumberFormatInfo numberformat)
        {
            var finActCommService =
                this.Container.Resolve<IDomainService<FinActivityCommunalService>>()
                    .GetAll()
                    .Where(x => x.DisclosureInfo.Id == diId)
                    .AsEnumerable()
                    .Select(
                        x => new ServiceCommunPlat
                        {
                            NameService = x.TypeServiceDi.GetDisplayName(),
                            IncomeProvision = x.IncomeFromProviding ?? 0,
                            Paid = x.Exact ?? 0,
                            DebtsPopulationAtBeginningPeriod = x.DebtPopulationStart ?? 0,
                            DebtsPopulationAtEndPeriod = x.DebtPopulationEnd ?? 0,
                            DebtsForUtilities = x.DebtManOrgCommunalService ?? 0,
                            PaidIndicationsPU = x.PaidByMeteringDevice ?? 0,
                            PaidBillsNeed = x.PaidByGeneralNeeds ?? 0,
                            PaymentOfClaims = x.PaymentByClaim ?? 0
                        }).ToArray();

            return new Utilities
            {
                IncomeProvision = finActCommService.Sum(x => x.IncomeProvision).RoundDecimal(2).ToString(numberformat),
                Paid = finActCommService.Sum(x => x.IncomeProvision).RoundDecimal(2).ToString(numberformat),
                DebtsPopulationAtBeginningPeriod = finActCommService.Sum(x => x.DebtsPopulationAtBeginningPeriod).RoundDecimal(2).ToString(numberformat),
                DebtsPopulationAtEndPeriod = finActCommService.Sum(x => x.DebtsPopulationAtEndPeriod).RoundDecimal(2).ToString(numberformat),
                DebtsForUtilities = finActCommService.Sum(x => x.DebtsForUtilities).RoundDecimal(2).ToString(numberformat),
                PaidIndicationsPU = finActCommService.Sum(x => x.PaidIndicationsPU).RoundDecimal(2).ToString(numberformat),
                PaidBillsNeed = finActCommService.Sum(x => x.PaidBillsNeed).RoundDecimal(2).ToString(numberformat),
                PaymentOfClaims = finActCommService.Sum(x => x.PaymentOfClaims).RoundDecimal(2).ToString(numberformat),
                ServiceCommunPlat = finActCommService
            };
        }

        private FinActivityRealObjs GetFinActivityRealObjs(
            PeriodDi period,
            long manOrgId,
            long diId,
            NumberFormatInfo numberformat)
        {
            // Получаем id домов
            var realityObjs = this.Container.Resolve<IDomainService<ManOrgContractRealityObject>>()
                .GetAll()
                .Where(x => x.ManOrgContract.ManagingOrganization.Id == manOrgId)
                .Where(
                    x => ((x.ManOrgContract.StartDate.HasValue && period.DateStart.HasValue
                        && (x.ManOrgContract.StartDate.Value >= period.DateStart.Value)
                        || !period.DateStart.HasValue)
                        &&
                        (x.ManOrgContract.StartDate.HasValue && period.DateEnd.HasValue &&
                            (period.DateEnd.Value >= x.ManOrgContract.StartDate.Value) || !period.DateEnd.HasValue))
                        ||
                        ((x.ManOrgContract.StartDate.HasValue && period.DateStart.HasValue &&
                            (period.DateStart.Value >= x.ManOrgContract.StartDate.Value) ||
                            !x.ManOrgContract.StartDate.HasValue)
                            && (x.ManOrgContract.StartDate.HasValue && period.DateEnd.HasValue
                                && (x.ManOrgContract.EndDate.Value >= period.DateStart.Value)
                                || !x.ManOrgContract.EndDate.HasValue))).Select(x => x.RealityObject).Distinct()
                .ToArray();

            var realityObjIds = realityObjs.Select(x => x.Id).ToArray();

            // Получаем уже имеющиеся в базе записи
            var finActManorgRealityObjs =
                this.Container.Resolve<IDomainService<FinActivityManagRealityObj>>()
                    .GetAll()
                    .Where(x => x.DisclosureInfo.Id == diId)
                    .AsEnumerable()
                    .GroupBy(x => x.RealityObject.Id)
                    .ToDictionary(
                        x => x.Key,
                        y => y.AsEnumerable()
                            .Select(
                                z => new FinActivityRealObj
                                {
                                    Id = z.RealityObject.Id,
                                    Address =
                                        z.RealityObject.FiasAddress != null
                                            ? z.RealityObject.FiasAddress.AddressName
                                            : string.Empty,
                                    AreaLiving =
                                        z.RealityObject.AreaLiving.HasValue
                                            ? z.RealityObject.AreaLiving.ToDecimal().RoundDecimal(2).ToString(numberformat)
                                            : null,
                                    AreaMkd =
                                        z.RealityObject.AreaMkd.HasValue
                                            ? z.RealityObject.AreaMkd.ToDecimal().RoundDecimal(2).ToString(numberformat)
                                            : null,
                                    PresentedToRepay = z.PresentedToRepay.ToDecimal().RoundDecimal(2).ToString(numberformat),
                                    ReceivedProvidedService =
                                        z.ReceivedProvidedService.ToDecimal().RoundDecimal(2).ToString(numberformat),
                                    SumDebt = z.SumDebt.ToDecimal().RoundDecimal(2).ToString(numberformat),
                                    SumFactExpense = z.SumFactExpense.ToDecimal().RoundDecimal(2).ToString(numberformat),
                                    PresentedToRepayDec = z.PresentedToRepay.ToDecimal(),
                                    ReceivedProvidedServiceDec = z.ReceivedProvidedService.ToDecimal(),
                                    SumDebtDec = z.SumDebt.ToDecimal(),
                                    SumFactExpenseDec = z.SumFactExpense.ToDecimal(),
                                    SumIncome = z.SumIncomeManage.ToDecimal()
                                })
                            .FirstOrDefault());

            var data = new List<FinActivityRealObj>();

            // Догоняем полученные дома уже имеющимися в базе данными
            foreach (var realityObjId in realityObjIds)
            {
                if (!finActManorgRealityObjs.ContainsKey(realityObjId))
                {
                    var ro = realityObjs.FirstOrDefault(x => x.Id == realityObjId);
                    if (ro != null)
                    {
                        data.Add(
                            new FinActivityRealObj
                            {
                                Id = realityObjId,
                                Address = ro.FiasAddress != null ? ro.FiasAddress.AddressName : string.Empty,
                                AreaLiving =
                                    ro.AreaLiving.HasValue
                                        ? ro.AreaLiving.ToDecimal().RoundDecimal(2).ToString(numberformat)
                                        : null,
                                AreaMkd =
                                    ro.AreaMkd.HasValue
                                        ? ro.AreaMkd.ToDecimal().RoundDecimal(2).ToString(numberformat)
                                        : null,
                                PresentedToRepay = "0",
                                ReceivedProvidedService = "0",
                                SumDebt = "0",
                                SumFactExpense = "0",
                            });
                    }
                }
                else
                {
                    data.Add(finActManorgRealityObjs[realityObjId]);
                }
            }

            return new FinActivityRealObjs
            {
                PresentedToRepay = data.Sum(x => x.PresentedToRepayDec).RoundDecimal(2).ToString(numberformat),
                ReceivedProvidedService =
                    data.Sum(x => x.ReceivedProvidedServiceDec).RoundDecimal(2).ToString(numberformat),
                SumDebt = data.Sum(x => x.SumDebtDec).RoundDecimal(2).ToString(numberformat),
                SumFactExpense = data.Sum(x => x.SumFactExpenseDec).RoundDecimal(2).ToString(numberformat),
                FinActRealObjs = data.ToArray(),
                SumIncome = data.Sum(x => x.SumIncome).RoundDecimal(2),
            };
        }
    }
}