namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.RapidResponseSystem
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhCalendar.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tatarstan.ConfigSections.Appeal;
    using Bars.GkhGji.Regions.Tatarstan.DomainService;
    using Bars.GkhGji.Regions.Tatarstan.Entities.RapidResponseSystem;

    /// <summary>
    /// Модель представления для <see cref="RapidResponseSystemAppealDetails"/>
    /// </summary>
    public class RapidResponseSystemAppealDetailsViewModel : BaseViewModel<RapidResponseSystemAppealDetails>
    {
        private readonly IRapidResponseSystemAppealService _rapidResponseSystemAppealService;
        private readonly IDomainService<Day> _dayDomain;
        private readonly IDomainService<RapidResponseSystemAppeal> _appealDomain;
        private readonly IDomainService<AppealCitsStatSubject> _appealCitsStatSubjectDomain;

        public RapidResponseSystemAppealDetailsViewModel(
            IRapidResponseSystemAppealService rapidResponseSystemAppealService,
            IDomainService<Day> dayDomain,
            IDomainService<RapidResponseSystemAppeal> appealDomain,
            IDomainService<AppealCitsStatSubject> appealCitsStatSubjectDomain)
        {
            _rapidResponseSystemAppealService = rapidResponseSystemAppealService;
            _dayDomain = dayDomain;
            _appealDomain = appealDomain;
            _appealCitsStatSubjectDomain = appealCitsStatSubjectDomain;
        }

        /// <inheritdoc />
        public override IDataResult Get(IDomainService<RapidResponseSystemAppealDetails> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();
            var appealDetails = domainService.Get(id);
            var appeal = this._appealDomain.GetAll()
                .Single(x => appealDetails.RapidResponseSystemAppeal.Id == x.Id);
            var subjects = this._appealCitsStatSubjectDomain
                .GetAll()
                .Where(x => x.AppealCits.Id == appeal.AppealCits.Id)
                .AggregateWithSeparator(x => x.Subject.Name, "\n");

            return new BaseDataResult(new
            {
                appealDetails.Id,
                Number = appeal.AppealCits.DocumentNumber,
                AppealDate = appeal.AppealCits.DateFrom,
                appeal.AppealCits.TypeCorrespondent,
                appeal.AppealCits.Correspondent,
                appeal.AppealCits.CorrespondentAddress,
                CorrespondentEmail = appeal.AppealCits.Email,
                CorrespondentFlatNum = appeal.AppealCits.FlatNum,
                CorrespondentPhone = appeal.AppealCits.Phone,
                AppealKind = appeal.AppealCits.KindStatement?.Name,
                ProblemDescription = appeal.AppealCits.Description,
                appeal.AppealCits.QuestionsCount,
                AppealFileName = appeal.AppealCits.File?.Name,
                AppealFileId = appeal.AppealCits.File?.Id,
                ContragentName = appeal.Contragent?.Name,
                ContragentId = appeal.Contragent?.Id,
                appealDetails.State,
                Municipality = appealDetails.AppealCitsRealityObject?.RealityObject.Municipality.Name,
                RealityObjectId = appealDetails.AppealCitsRealityObject?.RealityObject.Id,
                appealDetails.AppealCitsRealityObject?.RealityObject.Address,
                appealDetails.ReceiptDate,
                appealDetails.ControlPeriod,
                Subjects = subjects
            });
        }

        /// <inheritdoc />
        public override IDataResult List(IDomainService<RapidResponseSystemAppealDetails> domainService, BaseParams baseParams)
        {
            var appealCitsId = baseParams.Params.GetAsId("appealCitizensId");
            var appealDateFrom = baseParams.Params.GetAs<DateTime?>("appealDateFrom");
            var appealDateTo = baseParams.Params.GetAs<DateTime?>("appealDateTo");
            var controlPeriodFrom = baseParams.Params.GetAs<DateTime?>("controlPeriodFrom");
            var controlPeriodTo = baseParams.Params.GetAs<DateTime?>("controlPeriodTo");
            var roIds = baseParams.Params.GetAs<long[]>("roIds");
            var contragentIds = baseParams.Params.GetAs<long[]>("contragentIds");
            var isExport = baseParams.Params.GetAs<bool>("isExport");

            var config = this.Container.GetGkhConfig<AppealConfig>();

            var nowDate = DateTime.Now.Date;

            // Последний рабочий день для контрольного срока (+2 рабочих дня от текущей даты)
            var controlPeriodMaxWorkDay = this._rapidResponseSystemAppealService.GetControlPeriodMaxDay(2, nowDate);

            return this._rapidResponseSystemAppealService.GetSoprAppeals()
                .WhereIf(appealCitsId > 0, x => x.Appeal.AppealCits.Id == appealCitsId)
                .WhereIf(appealDateFrom.HasValue, x => x.Appeal.AppealCits.DateFrom >= appealDateFrom)
                .WhereIf(appealDateTo.HasValue, x => x.Appeal.AppealCits.DateFrom <= appealDateTo)
                .WhereIf(controlPeriodFrom.HasValue, x => x.AppealDetails.ControlPeriod >= controlPeriodFrom)
                .WhereIf(controlPeriodTo.HasValue, x => x.AppealDetails.ControlPeriod <= controlPeriodTo)
                .WhereIf(roIds != null, x => roIds.Contains(x.AppealDetails.AppealCitsRealityObject.RealityObject.Id))
                .WhereIf(contragentIds != null, x => x.Appeal.Contragent != null && contragentIds.Contains(x.Appeal.Contragent.Id))
                .SelectMany(x => this._appealCitsStatSubjectDomain.GetAll()
                        .Where(y => y.AppealCits.Id == x.Appeal.AppealCits.Id)
                        .DefaultIfEmpty(),
                    (x, y) => new
                    {
                        x.Appeal,
                        x.AppealDetails,
                        AppelCitsStatSubject = y
                    }
                )
                .AsEnumerable()
                .GroupBy(x => new
                {
                    x.AppealDetails
                }, 
                (x, y) => new
                {
                    Appeal = y.Select(z => z.Appeal).First(),
                    x.AppealDetails,
                    AppelCitsStatSubjectNames = y
                        .Where(z => z.AppelCitsStatSubject != null)
                        .AggregateWithSeparator(z => z.AppelCitsStatSubject.Subject.Name, ", ")
                })
                .Select(x => new
                {
                    x.AppealDetails.Id,
                    Number = x.Appeal.AppealCits?.DocumentNumber,
                    AppealDate = x.Appeal.AppealCits?.DateFrom,
                    ContragentName = x.Appeal.Contragent?.Name,
                    x.AppealDetails.State,
                    Municipality = x.AppealDetails.AppealCitsRealityObject?.RealityObject?.Municipality?.Name,
                    x.AppealDetails.AppealCitsRealityObject?.RealityObject?.Address,
                    x.AppealDetails.ReceiptDate,
                    x.AppealDetails.ControlPeriod,
                    Subjects = x.AppelCitsStatSubjectNames,
                    IsWarningControlPeriod =
                        config.RapidResponseSystemConfig.EnableSoprExpiringRecordsBacklight &&
                        !x.AppealDetails.State.FinalState &&
                        // Контрольный срок в рамках +2 рабочих дней от текущей даты
                        x.AppealDetails.ControlPeriod.Date >= nowDate &&
                        x.AppealDetails.ControlPeriod.Date <= controlPeriodMaxWorkDay.Date
                })
                .ToListDataResult(baseParams.GetLoadParam(), this.Container, usePersistentObjectOrdering: true, usePaging: !isExport);
        }
    }
}