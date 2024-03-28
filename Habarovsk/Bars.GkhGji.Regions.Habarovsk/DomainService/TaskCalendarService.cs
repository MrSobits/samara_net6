namespace Bars.GkhGji.Regions.Habarovsk.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using DomainService;
    using Entities;

    using Castle.Windsor;
    using Bars.GkhGji.Regions.BaseChelyabinsk.DomainService;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;
    using Bars.GkhCalendar.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Views;
    using Bars.GkhGji.Enums;
    using Bars.Gkh.Entities;
    using Bars.B4.Utils;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.AppealCits;
    using Bars.Gkh.Authentification;

    public class TaskCalendarService : ITaskCalendarService
    {
        public IWindsorContainer Container { get; set; }

        private static int GetRussianDayOfWeek(DayOfWeek dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case DayOfWeek.Monday:
                    return 1;
                case DayOfWeek.Tuesday:
                    return 2;
                case DayOfWeek.Wednesday:
                    return 3;
                case DayOfWeek.Thursday:
                    return 4;
                case DayOfWeek.Friday:
                    return 5;
                case DayOfWeek.Saturday:
                    return 6;
                case DayOfWeek.Sunday:
                    return 7;
            }

            return 7;
        }

        public IDataResult GetDays(BaseParams baseParams)
        {
            var date = baseParams.Params.GetAs<DateTime?>("date");
            var direction = baseParams.Params.GetAs<int>("direction");

            var ci = new CultureInfo("ru-RU");

            var tmpDate = (date ?? new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1)).AddMonths(direction);

            if (tmpDate != DateTime.MinValue)
            {
                var tmpDays = this.GetMonthDays(tmpDate);

                var tmpResult =
                    tmpDays.OrderBy(x => x.DayDate).Select(x => new DayProxy { id = x.Id, dayOfWeek = GetRussianDayOfWeek(x.DayDate.DayOfWeek), number = x.DayDate.Day, type = x.DayDate.Date == DateTime.Now.Date? DayType.Today: x.DayType, taskCount = x.TaskCount }).ToList();

                return tmpResult.Any() ? new BaseDataResult(new { days = tmpResult, date = tmpDate, dateText = tmpDate.ToString("MMMM yyyy", ci) }) : new BaseDataResult();

                //if (tmpResult.Any())
                //{
                //    // добавление дней в список (в начало списка и конец) для количества, кратного семи
                //    var needFirstDaysCount = tmpResult.First().dayOfWeek - 1;
                //    var needLastDaysCount = 7 - tmpResult.Last().dayOfWeek;

                //    var tmpFirstDays = new List<DayProxy>();
                //    for (var i = 1; i <= needFirstDaysCount; i++)
                //    {
                //        tmpFirstDays.Add(new DayProxy { id = -1, dayOfWeek = i });
                //    }

                //    var tmpLastDays = new List<DayProxy>();
                //    for (var i = 7 - needLastDaysCount + 1; i <= 7; i++)
                //    {
                //        tmpLastDays.Add(new DayProxy { id = -1, dayOfWeek = i });
                //    }

                //    var result = new List<DayProxy>();
                //    result.AddRange(tmpFirstDays);
                //    result.AddRange(tmpResult);
                //    result.AddRange(tmpLastDays.OrderBy(x => x.dayOfWeek));

                //    return new BaseDataResult(new { days = result, date = tmpDate, dateText = tmpDate.ToString("MMMM yyyy", ci) });
                //}
            }

            return new BaseDataResult();
        }

        public IDataResult GetListPrescriptionsGji(BaseParams baseParams)
        {
            var insplist = GetInspectorsList();
            if (insplist.Count <= 0)
            {
                return new ListDataResult();
            }
            var loadParam = baseParams.GetLoadParam();
            var dayId = baseParams.Params.GetAs<long>("dayId");
            var taskCalendDomain = this.Container.Resolve<IDomainService<TaskCalendar>>();
            var serviceViewPrescription = Container.Resolve<IDomainService<ViewPrescription>>();            
            var taskCalate = taskCalendDomain.Get(dayId);
            var currentDate = new DateTime(taskCalate.DayDate.Year, taskCalate.DayDate.Month, taskCalate.DayDate.Day);
            var serviceDocInsp = Container.Resolve<IDomainService<DocumentGjiInspector>>();
            var avaliableDocs = serviceDocInsp.GetAll()
                .Where(x=> x.DocumentGji.TypeDocumentGji == TypeDocumentGji.Prescription)
                .Where(x => insplist.Contains(x.Inspector.Id) && x.DocumentGji.DocumentDate.HasValue && x.DocumentGji.DocumentDate.Value.Date >= currentDate.Date.AddYears(-1))
                .Select(x => x.DocumentGji.Id).Distinct().ToList();

            var data = serviceViewPrescription.GetAll()
                .Where(x=> avaliableDocs.Contains(x.Id))
               .Where(x => x.DateRemoval.HasValue && x.DateRemoval.Value.Date == currentDate)                
                  .Filter(loadParam, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), data.Count());
        }

        public IDataResult GetListAdmonitions(BaseParams baseParams)
        {
            var insplist = GetInspectorsList();
            if (insplist.Count <= 0)
            {
                return new ListDataResult();
            }
            var loadParam = baseParams.GetLoadParam();
            var dayId = baseParams.Params.GetAs<long>("dayId");
            var taskCalendDomain = this.Container.Resolve<IDomainService<TaskCalendar>>();
            var AdmonitionDomain = this.Container.Resolve<IDomainService<AppealCitsAdmonition>>();
            var AppealCitsRealityObjectDomain = this.Container.Resolve<IDomainService<AppealCitsRealityObject>>();
            var appealCitsRealityObject = AppealCitsRealityObjectDomain.GetAll();
            var taskCalate = taskCalendDomain.Get(dayId);
            var currentDate = new DateTime(taskCalate.DayDate.Year, taskCalate.DayDate.Month, taskCalate.DayDate.Day);

            var data = AdmonitionDomain.GetAll()
                .Where(x=> insplist.Contains(x.Inspector.Id) || insplist.Contains(x.Executor.Id))
                .Where(x=> x.PerfomanceDate.HasValue && x.PerfomanceDate.Value.Date == currentDate)
                   .Join(
                       appealCitsRealityObject.AsEnumerable(),
                       x => x.AppealCits.Id,
                       y => y.AppealCits.Id,
                       (x, y) => new
                       {
                           x.Id,
                           x.DocumentName,
                           x.PerfomanceDate,
                           x.PerfomanceFactDate,
                           Contragent = x.Contragent.Name,
                           x.AppealCits.Number,
                           Inspector = x.Inspector.Fio,
                           y.RealityObject.Address,
                           x.KindKND,
                           Executor = x.Executor.Fio,
                           x.DocumentNumber,
                           x.DocumentDate
                       })
                   .Filter(loadParam, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), data.Count());

        }

        public IDataResult GetListDisposal(BaseParams baseParams)
        {
            var insplist = GetInspectorsList();
            if (insplist.Count <= 0)
            {
                return new ListDataResult();
            }
            var loadParam = baseParams.GetLoadParam();
            var dayId = baseParams.Params.GetAs<long>("dayId");
            var taskCalendDomain = this.Container.Resolve<IDomainService<TaskCalendar>>();
            var DisposalsDomain = this.Container.Resolve<IDomainService<Decision>>();          
            var taskCalate = taskCalendDomain.Get(dayId);
            var currentDate = new DateTime(taskCalate.DayDate.Year, taskCalate.DayDate.Month, taskCalate.DayDate.Day);
            var serviceDocInsp = Container.Resolve<IDomainService<DocumentGjiInspector>>();
            var avaliableDocs = serviceDocInsp.GetAll()
                .Where(x => x.DocumentGji.TypeDocumentGji == TypeDocumentGji.Decision)
                .Where(x => insplist.Contains(x.Inspector.Id) && x.DocumentGji.DocumentDate.HasValue && x.DocumentGji.DocumentDate.Value.Date >= currentDate.Date.AddYears(-1))
                .Select(x => x.DocumentGji.Id).Distinct().ToList();
            if (avaliableDocs.Count <= 0)
            {
                return new ListDataResult();
            }

            var DisposalDomain = this.Container.Resolve<IDomainService<Decision>>();
            var ParentChildrenDomain = this.Container.Resolve<IDomainService<DocumentGjiChildren>>();
            List<long> disposalsWithAct = ParentChildrenDomain.GetAll()
                .Where(x=> avaliableDocs.Contains(x.Parent.Id))
                .Where(x => x.Parent.TypeDocumentGji == GkhGji.Enums.TypeDocumentGji.Decision && x.Parent.DocumentDate.HasValue && x.Parent.DocumentDate.Value.Date > currentDate.AddMonths(-1).Date && x.Parent.DocumentDate<= currentDate.Date)
                .Select(x => x.Parent.Id).Distinct().ToList();
            var listDisposal = new List<long>();

            if (currentDate.Date == DateTime.Now.Date)
            {
                listDisposal = DisposalDomain.GetAll().Where(x => x.State.FinalState && x.DateStart.HasValue && x.DateEnd.HasValue)
               .Where(x => avaliableDocs.Contains(x.Id))
              .Where(x => x.DateStart.Value.Date <= currentDate.Date && x.DateEnd.Value.Date >= currentDate.Date)
                 .Where(x => !disposalsWithAct.Contains(x.Id))
                 .Select(x => x.Id).Distinct().ToList();
            }
            else
            {
                listDisposal = DisposalDomain.GetAll().Where(x => x.State.FinalState && x.DateStart.HasValue && x.DateEnd.HasValue)
                     .Where(x => avaliableDocs.Contains(x.Id))
                    .Where(x => x.DateEnd.Value.Date == currentDate.Date)
                       .Where(x => !disposalsWithAct.Contains(x.Id))
                       .Select(x => x.Id).Distinct().ToList();
            }

            var serviceViewDisposal = Container.Resolve<IDomainService<ViewDecision>>();

            var data = serviceViewDisposal.GetAll()
                .Where(x => x.State.FinalState)
                .Where(x=> listDisposal.Contains(x.Id))
                  .Select(x => new
                  {
                      x.Id,
                      x.State,
                      x.DateEnd,
                      x.DateStart,
                      x.DocumentDate,
                      x.DocumentNumber,
                      x.DocumentNum,
                      x.ERPID,
                      x.TypeBase,
                      x.IssuedDecision,
                      KindCheck = x.KindCheckName,
                      x.ContragentName,
                      x.TypeDisposal,
                      MunicipalityNames = x.MunicipalityId != null ? x.MunicipalityNames : "",
                      MoSettlement = x.MoNames,
                      PlaceName = x.PlaceNames,
                      MunicipalityId = x.MunicipalityId ?? null,
                      x.RealityObjectCount,
                      x.InspectorNames,
                      x.InspectionId,
                      x.TypeDocumentGji,
                      x.TypeAgreementProsecutor,
                      x.KindKNDGJI

                  })
                    .Filter(loadParam, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }

        public IDataResult GetListResolPros(BaseParams baseParams)
        {
            return null;
        }
        public IDataResult GetListCourtPractice(BaseParams baseParams)
        {
            var insplist = GetInspectorsList();
            if (insplist.Count <= 0)
            {
                return new ListDataResult();
            }

            var loadParam = baseParams.GetLoadParam();
            var dayId = baseParams.Params.GetAs<long>("dayId");
            var taskCalendDomain = this.Container.Resolve<IDomainService<TaskCalendar>>();
            var CourtPracticeDomain = this.Container.Resolve<IDomainService<CourtPractice>>();
            var InspectorDomain = this.Container.Resolve<IDomainService<CourtPracticeInspector>>();
            var taskCalate = taskCalendDomain.Get(dayId);
            var currentDate = new DateTime(taskCalate.DayDate.Year, taskCalate.DayDate.Month, taskCalate.DayDate.Day);
            var avalCp = InspectorDomain.GetAll()
                .Where(x => insplist.Contains(x.Inspector.Id))
                .Where(x => !x.CourtPractice.State.FinalState && x.CourtPractice.DateCourtMeeting.Date == currentDate.Date)
                .Select(x => x.CourtPractice.Id).Distinct().ToList();
            if (avalCp.Count <= 0)
            {
                return new ListDataResult();
            }
            var predata = CourtPracticeDomain.GetAll().Where(x => avalCp.Contains(x.Id))
                .Select(x=> x.Id).Distinct().ToList();
            Dictionary<long, string> lawyers = new Dictionary<long, string>();
            InspectorDomain.GetAll()
            .Where(x => predata.Contains(x.CourtPractice.Id))
            .Where(x => x.Inspector != null)
            .Where(x => x.LawyerInspector == Enums.LawyerInspector.Lawyer).OrderBy(x => x.Id).ForEach(x =>
            {
                if (!lawyers.ContainsKey(x.CourtPractice.Id))
                {
                    lawyers.Add(x.CourtPractice.Id, x.Inspector.Fio);
                }
                else
                {
                    lawyers[x.CourtPractice.Id] = x.Inspector.Fio;
                }
            });
            Dictionary<long, string> inspectors = new Dictionary<long, string>();
            InspectorDomain.GetAll()
            .Where(x => predata.Contains(x.CourtPractice.Id))
            .Where(x => x.Inspector != null)
            .Where(x => x.LawyerInspector == Enums.LawyerInspector.Inspector).OrderBy(x => x.Id).ForEach(x =>
            {
                if (!inspectors.ContainsKey(x.CourtPractice.Id))
                {
                    inspectors.Add(x.CourtPractice.Id, x.Inspector.Fio);
                }
                else
                {
                    inspectors[x.CourtPractice.Id] = x.Inspector.Fio;
                }
            });

            var data = CourtPracticeDomain.GetAll()
             .Where(x => avalCp.Contains(x.Id))
             .AsEnumerable()
             .Select(x => new
             {
                 x.Id,
                 x.State,
                 JurInstitution = x.JurInstitution != null ? x.JurInstitution.Name : string.Empty,
                 x.DocumentNumber,
                 PlantiffChoice = x.ContragentPlaintiff != null ? x.ContragentPlaintiff.Name : x.PlaintiffFio,
                 Lawyer = lawyers.ContainsKey(x.Id) ? lawyers[x.Id] : "",
                 Inspector = inspectors.ContainsKey(x.Id) ? inspectors[x.Id] : "",
                 CourtMeetingTime = x.CourtMeetingTime.HasValue ? x.CourtMeetingTime.Value.ToShortTimeString() : string.Empty,
                 DefendantChoice = x.ContragentDefendant != null ? x.ContragentDefendant.Name : x.DefendantFio,
                 DifferentChoice = x.DifferentContragent != null ? x.DifferentContragent.Name : x.DifferentFIo,
                 TypeFactViolation = x.TypeFactViolation != null ? x.TypeFactViolation.Name : string.Empty,
                 x.CourtPracticeState,
                 x.DisputeType,
                 x.InLawDate,
                 DateCourtMeeting = x.DateCourtMeeting.ToShortDateString() + (x.CourtMeetingTime.HasValue ? (" " + x.CourtMeetingTime.Value.ToShortTimeString()) : ""),
                 DateCourtMeetingDate = x.CourtMeetingTime.HasValue ? new DateTime(x.DateCourtMeeting.Year, x.DateCourtMeeting.Month, x.DateCourtMeeting.Day, x.CourtMeetingTime.Value.Hour, x.CourtMeetingTime.Value.Minute, 0) : new DateTime(x.DateCourtMeeting.Year, x.DateCourtMeeting.Month, x.DateCourtMeeting.Day, 0, 0, 0),
                 InstanceGjiCode = x.InstanceGji != null ? x.InstanceGji.Code : "03",
                 x.CourtMeetingResult

             }).AsQueryable()
             .Filter(loadParam, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), data.Count());
           
        }

        public IDataResult GetListProtocolsGji(BaseParams baseParams)
        {
            var insplist = GetInspectorsList();
            if (insplist.Count <= 0)
            {
                return new ListDataResult();
            }
            var loadParam = baseParams.GetLoadParam();
            var dayId = baseParams.Params.GetAs<long>("dayId");
            var taskCalendDomain = this.Container.Resolve<IDomainService<TaskCalendar>>();
            var ProtocolDomain = this.Container.Resolve<IDomainService<Protocol>>();
            var Protocol197Domain = this.Container.Resolve<IDomainService<Protocol197>>();
            var taskCalate = taskCalendDomain.Get(dayId);
            var currentDate = new DateTime(taskCalate.DayDate.Year, taskCalate.DayDate.Month, taskCalate.DayDate.Day);

            var serviceDocInsp = Container.Resolve<IDomainService<DocumentGjiInspector>>();
            var avaliableDocs = serviceDocInsp.GetAll()
                .Where(x => x.DocumentGji.TypeDocumentGji == TypeDocumentGji.Protocol || x.DocumentGji.TypeDocumentGji == TypeDocumentGji.Protocol197)
                .Where(x => insplist.Contains(x.Inspector.Id) && x.DocumentGji.DocumentDate.HasValue && x.DocumentGji.DocumentDate.Value.Date == currentDate.Date)
                .Select(x => x.DocumentGji.Id).Distinct().ToList();
            if (avaliableDocs.Count <= 0)
            {
                return new ListDataResult();
            }
            //if (currentDate.Date < DateTime.Now.Date)
            //{
            //    return null;
            //}
          //  var protCnt = ProtocolDomain.GetAll()
          //.Where(x => avaliableDocs.Contains(x.Id))
          //.Select(x=> x.Id).Distinct().ToList();
          //  var prot197Cnt = Protocol197Domain.GetAll()
          //.Where(x => avaliableDocs.Contains(x.Id))
          //.Select(x => x.Id).Distinct().ToList();

            var serviceViewProtocol = Container.Resolve<IDomainService<ViewProtocol>>();
            var serviceViewProtocol197 = Container.Resolve<IDomainService<ViewProtocol197>>();

            var predata = serviceViewProtocol.GetAll()
              .Where(x => avaliableDocs.Contains(x.Id))
                .Select(x => new ProtocolProxy
                {
                    Id = x.Id,
                    ContragentName = x.ContragentName,
                    MunicipalityNames = x.MunicipalityId != null ? x.MunicipalityNames : x.ContragentMuName,
                    TypeExecutant = x.TypeExecutant,
                    CountViolation = x.CountViolation.HasValue ? x.CountViolation.Value : 0,
                    InspectorNames = x.InspectorNames,
                    DocumentDate = x.DocumentDate,
                    DocumentNumber = x.DocumentNumber,
                    InspectionId = x.InspectionId,
                    TypeBase = x.TypeBase,
                    ControlType = x.ControlType,
                    TypeDocumentGji = x.TypeDocumentGji,
                    ArticleLaw = x.ArticleLaw,
                    FormatHour = x.FormatHour,
                    FormatMinute = x.FormatMinute
                }).ToList();

            predata.AddRange(serviceViewProtocol197.GetAll()
              .Where(x => avaliableDocs.Contains(x.Id))
                .Select(x => new ProtocolProxy
                {
                    Id = x.Id,
                    ContragentName = x.ContragentName,
                    MunicipalityNames = x.MunicipalityId != null ? x.MunicipalityNames : x.ContragentMuName,
                    TypeExecutant = x.TypeExecutant,
                    CountViolation = x.CountViolation.HasValue ? x.CountViolation.Value : 0,
                    InspectorNames = x.InspectorNames,
                    DocumentDate = x.DocumentDate,
                    DocumentNumber = x.DocumentNumber,
                    InspectionId = x.InspectionId,
                    TypeBase = x.TypeBase,
                    ControlType = x.ControlType,
                    TypeDocumentGji = x.TypeDocumentGji,
                    ArticleLaw = x.ArticleLaw,
                    FormatHour = x.FormatHour,
                    FormatMinute = x.FormatMinute
                }).ToList());
            var data = predata.Select(x => new
            {
                x.Id,
                x.ContragentName,
                x.MunicipalityNames,
                x.TypeExecutant,
                x.CountViolation,
                x.InspectorNames,
                x.DocumentDate,
                x.DocumentNumber,
                x.InspectionId,
                x.TypeBase,
                x.ControlType,
                x.TypeDocumentGji,
                x.ArticleLaw,
                ProtTime = $"{x.FormatHour}:{x.FormatMinute.ToString().PadLeft(2,'0')}"
            }).AsQueryable().Filter(loadParam, Container); ;

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }

        public IDataResult GetListSOPR(BaseParams baseParams)
        {
            var insplist = GetInspectorsList();
            if (insplist.Count <= 0)
            {
                return new ListDataResult();
            }
            var loadParam = baseParams.GetLoadParam();
            var dayId = baseParams.Params.GetAs<long>("dayId");
            var taskCalendDomain = this.Container.Resolve<IDomainService<TaskCalendar>>();
            var appealOrderDomain = this.Container.Resolve<IDomainService<AppealOrder>>();
            var AppealCitsExecDomain = this.Container.Resolve<IDomainService<AppealCitsExecutant>>();


            var taskCalate = taskCalendDomain.Get(dayId);
            var chosenDate = new DateTime(taskCalate.DayDate.Year, taskCalate.DayDate.Month, taskCalate.DayDate.Day);
            var currentDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            var avalAppeal = AppealCitsExecDomain.GetAll()
                .Where(x => x.Executant != null && insplist.Contains(x.Executant.Id) && x.AppealCits.DateFrom.HasValue && x.AppealCits.DateFrom.Value > chosenDate.AddMonths(-2))
                .Select(x => x.AppealCits.Id).Distinct().ToList();
            if (avalAppeal.Count <= 0)
            {
                return new ListDataResult();
            }

            var data = appealOrderDomain.GetAll()
                .Where(x=> avalAppeal.Contains(x.AppealCits.Id))
                .Where(x=> x.OrderDate > chosenDate.AddDays(-30))
                     .Where(x => x.AppealCits.CaseDate.HasValue && x.AppealCits.CaseDate.Value.Date == chosenDate.Date)                    
                                      .Select(x => new
                                      {
                                          x.Id,
                                          Executant = x.Executant.Name,
                                          ContragentINN = x.Executant.Inn,
                                          x.Person,
                                          x.AppealCits.DocumentNumber,
                                          x.AppealCits.DateFrom,
                                          x.OrderDate,
                                          x.AppealCits.State,
                                          x.PerformanceDate,
                                          x.YesNoNotSet,
                                          x.Confirmed,
                                          x.Description,
                                          x.ConfirmedGJI
                                      })
                                      .Filter(loadParam, this.Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }

        public IDataResult GetListAppeals(BaseParams baseParams)
        {
            var insplist = GetInspectorsList();
            if (insplist.Count <= 0)
            {
                return new ListDataResult();
            }
            var loadParam = baseParams.GetLoadParam();
            var dayId = baseParams.Params.GetAs<long>("dayId");
            var taskCalendDomain = this.Container.Resolve<IDomainService<TaskCalendar>>();
            var AppealCitsExecDomain = this.Container.Resolve<IDomainService<AppealCitsExecutant>>();

            var taskCalate = taskCalendDomain.Get(dayId);
            var chosenDate = new DateTime(taskCalate.DayDate.Year, taskCalate.DayDate.Month, taskCalate.DayDate.Day);
            var currentDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            var avalAppeal = AppealCitsExecDomain.GetAll()
              .Where(x => x.Executant != null && insplist.Contains(x.Executant.Id) && x.AppealCits.DateFrom.HasValue && x.AppealCits.DateFrom.Value > chosenDate.AddMonths(-2))
              .Select(x => x.AppealCits.Id).Distinct().ToList();
            if (avalAppeal.Count<=0)
            {
                return new ListDataResult();
            }
            if (chosenDate == currentDay)
            {
                return GetAppealsByDays(chosenDate,5, loadParam, avalAppeal);
            }
            else
            {
                return GetAppealsByDays(chosenDate, 0, loadParam, avalAppeal);
            }
           
        }

        private IDataResult GetAppealsByDays(DateTime chosenDate, int numberDays,LoadParam loadParam, List<long> aval)
        {
            var AppealCitsDomain = this.Container.Resolve<IDomainService<AppealCits>>();
            var executantDomainService = this.Container.Resolve<IDomainService<AppealCitsExecutant>>();
            var query2 = AppealCitsDomain.GetAll()
                .Where(x=> aval.Contains(x.Id))
                .Where(x=> !x.State.FinalState && x.CheckTime.HasValue && x.CheckTime.Value <= chosenDate.AddDays(numberDays) && x.CheckTime.Value>= chosenDate)
                   .Select(x => new
                       {
                           x.Id,
                           x.State,
                           Name = $"{x.Number} ({x.NumberGji})",
                           ManagingOrganization = x.ManagingOrganization != null ? x.ManagingOrganization.Contragent.Name : "",
                           x.DocumentNumber,
                           x.Number,
                           x.NumberGji,
                           x.DateFrom,
                           x.CheckTime,
                           x.QuestionsCount,
                           x.QuestionStatus,
                           Executant = x.Executant != null ? x.Executant.Fio : string.Empty,
                           Tester = x.Tester != null ? x.Tester.Fio : "",
                           SuretyResolve = x.SuretyResolve != null ? x.SuretyResolve.Name : "",
                           x.ExecuteDate,
                           StateCode = x.State.Code,
                           x.SuretyDate,
                           x.CaseDate,
                           x.ZonalInspection,
                           ZoneName = x.ZonalInspection != null ? x.ZonalInspection.ZoneName : string.Empty,
                           x.Correspondent,
                           x.CorrespondentAddress,
                           x.SpecialControl,
                           KindStatement = x.KindStatement != null ? x.KindStatement.Name : string.Empty,
                           HasExecutant = executantDomainService.GetAll().Any(y => y.AppealCits.Id == x.Id),
                           x.ExtensTime,
                           x.Description,
                           x.Municipality,
                           x.MunicipalityId,
                           x.AppealUid,
                           x.IncomingSourcesName,
                           x.Phone,
                           SoprDate = x.CaseDate,
                           x.Executors,
                           x.Testers,
                           x.SSTUExportState,
                           x.AnswerDate,
                           x.RealityAddresses,
                           x.IncomingSources,
                           x.FastTrack
                   }).Filter(loadParam, this.Container); ;
          


            var totalCount = query2.Count();           

            query2 = query2
                .OrderIf(loadParam.Order.Length == 0, true, x => x.DateFrom)
                .Order(loadParam);

            return new ListDataResult(query2.Paging(loadParam).ToList(), totalCount);
        }

        private List<TaskCalendar> GetMonthDays(DateTime tmpDate)
        {
            var insplist = GetInspectorsList();
            if (insplist.Count <= 0)
            {
                return null;
            }
            var firstDayCurMonth = new DateTime(tmpDate.Year, tmpDate.Month, 1);
            var firstDayNextMonth = firstDayCurMonth.AddMonths(1);

            var daysCount = (firstDayNextMonth - firstDayCurMonth).Days;

            var dayList = this.GetRecords(firstDayCurMonth, firstDayNextMonth);

            if (dayList.Count == 0)
            {
                var createResult = this.CreateDays(tmpDate, daysCount);

                if (createResult.Success)
                {
                    dayList = this.GetRecords(firstDayCurMonth, firstDayNextMonth);
                }
            }
            List<TaskCalendar> listInfo = new List<TaskCalendar>();
            dayList.ForEach(day =>
            {
                var dtNow = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                if (day.DayDate < dtNow)
                {
                    day.TaskCount = GetTasksByDay(day.DayDate, insplist);
                }
                else if (day.DayDate == dtNow)
                {
                    day.TaskCount = GetTasksCurrentDay(insplist);
                }
                else
                {
                    day.TaskCount = GetTasksByDay(day.DayDate, insplist);
                }
                listInfo.Add(day);

            });
            //добавлем информацию о проверках


            //if (dayList.Count == 0)
            //{
            //    createResult = this.CreateDays(tmpDate, daysCount, new List<int>());
            //}

            //if (dayList.Count > 0 && dayList.Count < daysCount)
            //{
            //    createResult = this.CreateDays(tmpDate, daysCount, dayList.Select(x => x.DayDate.Day).ToList());
            //}

            //if (createResult != null && !createResult.Success)
            //{
            //    return new List<Day>();
            //}

            return listInfo;
        }

        private List<TaskCalendar> GetRecords(DateTime firstDayCurMonth, DateTime firstDayNextMonth)
        {
            return
                this.Container.Resolve<IDomainService<TaskCalendar>>()
                    .GetAll()
                    .Where(x => x.DayDate >= firstDayCurMonth)
                    .Where(x => x.DayDate < firstDayNextMonth)
                    .OrderByDescending(x => x.ObjectCreateDate)
                    .AsEnumerable()
                    .GroupBy(x => x.DayDate)
                    .Select(x => x.Select(y => y).First())
                    .ToList();
        }

        private int GetTasksCurrentDay(List<long> insplist)
        {
            var currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var CourtPracticeDomain = this.Container.Resolve<IDomainService<CourtPractice>>();
            var DisposalDomain = this.Container.Resolve<IDomainService<Decision>>();
            var ParentChildrenDomain = this.Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var ProtocolDomain = this.Container.Resolve<IDomainService<Protocol>>();
            var Protocol197Domain = this.Container.Resolve<IDomainService<Protocol197>>();
            var AppealDomain = this.Container.Resolve<IDomainService<AppealCits>>();
            var AppealOrderDomain = this.Container.Resolve<IDomainService<AppealOrder>>();
            var AdmonitionDomain = this.Container.Resolve<IDomainService<AppealCitsAdmonition>>();
            var serviceViewPrescription = Container.Resolve<IDomainService<ViewPrescription>>();
            var InspectorDomain = this.Container.Resolve<IDomainService<CourtPracticeInspector>>();
            var AppealCitsExecDomain = this.Container.Resolve<IDomainService<AppealCitsExecutant>>();

            var avalAppeal = AppealCitsExecDomain.GetAll()
               .Where(x => x.Executant != null && insplist.Contains(x.Executant.Id) && x.AppealCits.DateFrom.HasValue && x.AppealCits.DateFrom.Value > currentDate.AddMonths(-2))
               .Select(x => x.AppealCits.Id).Distinct().ToList();


            var serviceDocInsp = Container.Resolve<IDomainService<DocumentGjiInspector>>();
            var avaliableDocs = serviceDocInsp.GetAll()
                .Where(x => insplist.Contains(x.Inspector.Id) && x.DocumentGji.DocumentDate.HasValue && x.DocumentGji.DocumentDate.Value.Date > currentDate.Date.AddMonths(-12))
                .Select(x => x.DocumentGji.Id).Distinct().ToList();

            List<long> disposalsWithAct = ParentChildrenDomain.GetAll()
                .Where(x => x.Parent.TypeDocumentGji == GkhGji.Enums.TypeDocumentGji.Decision && x.Parent.DocumentDate.HasValue && x.Parent.DocumentDate.Value.Date > currentDate.AddMonths(-12).Date)
                .Select(x => x.Parent.Id).Distinct().ToList();

            var avalCp = InspectorDomain.GetAll()
               .Where(x => insplist.Contains(x.Inspector.Id))
               .Where(x => !x.CourtPractice.State.FinalState && x.CourtPractice.DateCourtMeeting.Date == currentDate.Date)
               .Select(x => x.CourtPractice.Id).Distinct().ToList();

            var cntCP = CourtPracticeDomain.GetAll().Where(x => !x.State.FinalState && x.DateCourtMeeting.Date == currentDate.Date)
                .Where(x => avalCp.Contains(x.Id))
                .Count();
            var cntDisposal = DisposalDomain.GetAll().Where(x => x.State.FinalState && x.DateStart.HasValue && x.DateEnd.HasValue)
                .Where(x=> avaliableDocs.Contains(x.Id))
                .Where(x=> x.DateStart.Value.Date<= currentDate.Date && x.DateEnd.Value.Date>= currentDate.Date)
                .Where(x=> !disposalsWithAct.Contains(x.Id))
               .Count(); 
            var protCnt = ProtocolDomain.GetAll()
                .Where(x => avaliableDocs.Contains(x.Id))
            .Where(x => x.DocumentDate.HasValue && x.DocumentDate.Value.Date == currentDate.Date)
            .Count();
            var prot197Cnt = Protocol197Domain.GetAll()
                .Where(x => avaliableDocs.Contains(x.Id))
                .Where(x => x.DocumentDate.HasValue && x.DocumentDate.Value.Date == currentDate.Date)
                .Count();
            var appcitCnt = AppealDomain.GetAll()
             .Where(x => avalAppeal.Contains(x.Id))
             .Where(x => !x.State.FinalState && x.CheckTime.HasValue && x.CheckTime.Value.Date >= currentDate.Date && x.CheckTime.Value.Date <= currentDate.Date.AddDays(5))
             .Count();
            var soprcnt = AppealOrderDomain.GetAll()
                 .Where(x => avalAppeal.Contains(x.AppealCits.Id))
               .Where(x => x.OrderDate > currentDate.AddDays(-30))
               .Where(x => x.AppealCits.CaseDate.HasValue && x.AppealCits.CaseDate.Value.Date == currentDate.Date).Count();
            var admonCount = AdmonitionDomain.GetAll()
                .Where(x => insplist.Contains(x.Inspector.Id) || insplist.Contains(x.Executor.Id))
                .Where(x => x.PerfomanceDate.HasValue && x.PerfomanceDate.Value.Date == currentDate.Date).Count();
            var prescrCount = serviceViewPrescription.GetAll()
                .Where(x => avaliableDocs.Contains(x.Id))
                .Where(x => x.DateRemoval.HasValue && x.DateRemoval.Value.Date == currentDate.Date).Count();
            return cntCP + cntDisposal + protCnt + prot197Cnt + appcitCnt + soprcnt + admonCount + prescrCount;
        }
        private int GetTasksByDay(DateTime dateValue,List<long> insplist)
        {          
            var currentDate = new DateTime(dateValue.Year, dateValue.Month, dateValue.Day);
            if (currentDate.Date == new DateTime(2022, 6, 9).Date)
            {
                string str = "";
            }
            var CourtPracticeDomain = this.Container.Resolve<IDomainService<CourtPractice>>();
            var DisposalDomain = this.Container.Resolve<IDomainService<Decision>>();
            var ProtocolDomain = this.Container.Resolve<IDomainService<Protocol>>();
            var AppealDomain = this.Container.Resolve<IDomainService<AppealCits>>();
            var Protocol197Domain = this.Container.Resolve<IDomainService<Protocol197>>();
            var ParentChildrenDomain = this.Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var AppealOrderDomain = this.Container.Resolve<IDomainService<AppealOrder>>();
            var AdmonitionDomain = this.Container.Resolve<IDomainService<AppealCitsAdmonition>>();
            var serviceViewPrescription = Container.Resolve<IDomainService<ViewPrescription>>();

            var InspectorDomain = this.Container.Resolve<IDomainService<CourtPracticeInspector>>();
            var AppealCitsExecDomain = this.Container.Resolve<IDomainService<AppealCitsExecutant>>();

            var avalAppeal = AppealCitsExecDomain.GetAll()
               .Where(x => x.Executant != null && insplist.Contains(x.Executant.Id) && x.AppealCits.DateFrom.HasValue && x.AppealCits.DateFrom.Value > currentDate.AddMonths(-2))
               .Select(x => x.AppealCits.Id).Distinct().ToList();

            var serviceDocInsp = Container.Resolve<IDomainService<DocumentGjiInspector>>();
            var avaliableDocs = serviceDocInsp.GetAll()
                .Where(x => insplist.Contains(x.Inspector.Id) && x.DocumentGji.DocumentDate.HasValue && x.DocumentGji.DocumentDate.Value.Date > currentDate.Date.AddMonths(-3))
                .Select(x => x.DocumentGji.Id).Distinct().ToList();

            List<long> disposalsWithAct = ParentChildrenDomain.GetAll()
                .Where(x => x.Parent.TypeDocumentGji == GkhGji.Enums.TypeDocumentGji.Decision && x.Parent.DocumentDate.HasValue && x.Parent.DocumentDate.Value.Date > currentDate.AddMonths(-1).Date && x.Parent.DocumentDate.Value.Date <= currentDate.Date)
                .Select(x => x.Parent.Id).Distinct().ToList();
            var avalCp = InspectorDomain.GetAll()
              .Where(x => insplist.Contains(x.Inspector.Id))
              .Where(x => !x.CourtPractice.State.FinalState && x.CourtPractice.DateCourtMeeting.Date == currentDate.Date)
              .Select(x => x.CourtPractice.Id).Distinct().ToList();
            var cntCP = CourtPracticeDomain.GetAll().Where(x => !x.State.FinalState && x.DateCourtMeeting.Date == currentDate.Date)
                   .Where(x => avalCp.Contains(x.Id))
                .Count();
            var cntDisposal = DisposalDomain.GetAll().Where(x => x.State.FinalState && x.DateStart.HasValue && x.DateEnd.HasValue)
                  .Where(x => avaliableDocs.Contains(x.Id))
                .Where(x => x.DateEnd.Value.Date == currentDate.Date)
                   .Where(x => !disposalsWithAct.Contains(x.Id))
               .Count();
            var protCnt = ProtocolDomain.GetAll()
                  .Where(x => avaliableDocs.Contains(x.Id))
                .Where(x => x.FormatDate.HasValue && x.FormatDate.Value.Date == currentDate.Date)
                .Count();
            var prot197Cnt = Protocol197Domain.GetAll()
                  .Where(x => avaliableDocs.Contains(x.Id))
                .Where(x => x.FormatDate.HasValue && x.FormatDate.Value.Date == currentDate.Date)
                .Count();
            var appcitCnt = AppealDomain.GetAll()
              .Where(x => avalAppeal.Contains(x.Id))
              .Where(x => !x.State.FinalState && x.CheckTime.HasValue && x.CheckTime.Value.Date == currentDate.Date)
              .Count();
            var soprcnt = AppealOrderDomain.GetAll()
             .Where(x => avalAppeal.Contains(x.AppealCits.Id))
             .Where(x => x.OrderDate > currentDate.AddDays(-30))
             .Where(x => x.AppealCits.CaseDate.HasValue && x.AppealCits.CaseDate.Value.Date == currentDate.Date).Count();
            var admonCount = AdmonitionDomain.GetAll()
                .Where(x => insplist.Contains(x.Inspector.Id) || insplist.Contains(x.Executor.Id))
                .Where(x => x.PerfomanceDate.HasValue && x.PerfomanceDate.Value.Date == currentDate.Date).Count();
            var prescrCount = serviceViewPrescription.GetAll()
               .Where(x => avaliableDocs.Contains(x.Id))
               .Where(x => x.DateRemoval.HasValue && x.DateRemoval.Value.Date == currentDate.Date).Count();
            return cntCP + cntDisposal + protCnt + prot197Cnt + appcitCnt + soprcnt + admonCount + prescrCount;
        }
        /// <summary>
        /// Создает дни месяца
        /// </summary>
        private BaseDataResult CreateDays(DateTime tmpDate, int daysCount) //, List<int> daysNumberList)
        {
            var serviceDays = this.Container.Resolve<IDomainService<TaskCalendar>>();

            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                try
                {
                    for (var i = 1; i <= daysCount; i++)
                    {
                        //if (daysNumberList.Contains(i))
                        //{
                        //    continue;
                        //}

                        var dayDate = new DateTime(tmpDate.Year, tmpDate.Month, i);
                        var isWorkDay = dayDate.DayOfWeek != DayOfWeek.Saturday && dayDate.DayOfWeek != DayOfWeek.Sunday;

                        var newObj = new TaskCalendar { DayDate = dayDate, DayType = isWorkDay ? DayType.Workday : DayType.DayOff };

                        serviceDays.Save(newObj);
                    }

                    transaction.Commit();
                    return new BaseDataResult { Success = true };
                }
                catch
                {
                    transaction.Rollback();
                    return new BaseDataResult { Success = false };
                }
            }
        }

        public IGkhUserManager UserManager { get; set; }
        public IDomainService<InspectorSubscription> InspectorSubscriptionDomain { get; set; }
        public IDomainService<Inspector> InspectorDomain { get; set; }

        //Получаем список инспекторов
        private List<long> GetInspectorsList()
        {
            Operator thisOperator = UserManager.GetActiveOperator();
            if (thisOperator?.Inspector == null)
                return null;
            List<long> insplist = new List<long>();
            insplist.Add(thisOperator.Inspector.Id);
            insplist.AddRange(InspectorSubscriptionDomain.GetAll()
                .Where(x => x.SignedInspector == thisOperator.Inspector)
                .Select(x => x.Inspector.Id).ToList());
            return insplist;
        }
    }

    internal class DayProxy
    {
        public long id;

        public int dayOfWeek;

        public int number;

        public DayType type;

        public int taskCount;
    }
    internal class ProtocolProxy
    {
        public long Id;
        public string ContragentName;
        public string MunicipalityNames;
        public string TypeExecutant;
        public string InspectorNames;
        public int CountViolation;
        public DateTime? DocumentDate;
        public string DocumentNumber;
        public long? InspectionId;
        public TypeBase TypeBase;
        public ControlType ControlType;
        public TypeDocumentGji TypeDocumentGji;
        public string ArticleLaw;
        public int? FormatHour;
        public int? FormatMinute;
    }

}