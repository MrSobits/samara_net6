namespace Bars.GkhGji.Regions.Chelyabinsk.DomainService
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

        public IDataResult GetListAppeals(BaseParams baseParams)
        {
            return null;
        }

        public IDataResult GetListSOPR(BaseParams baseParams)
        {

            return null;
        }
        public IDataResult GetListAdmonitions(BaseParams baseParams)
        {

            return null;
        }
        public IDataResult GetListPrescriptionsGji(BaseParams baseParams)
        {

            return null;
        }


        public IDataResult GetListDisposal(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();
            var dayId = baseParams.Params.GetAs<long>("dayId");
            var taskCalendDomain = this.Container.Resolve<IDomainService<TaskCalendar>>();
            var DisposalsDomain = this.Container.Resolve<IDomainService<Disposal>>();
            var taskCalate = taskCalendDomain.Get(dayId);
            var currentDate = new DateTime(taskCalate.DayDate.Year, taskCalate.DayDate.Month, taskCalate.DayDate.Day);
            //if (currentDate.Date < DateTime.Now.Date)
            //{
            //    return null;
            //}
            var DisposalDomain = this.Container.Resolve<IDomainService<Disposal>>();
            var ParentChildrenDomain = this.Container.Resolve<IDomainService<DocumentGjiChildren>>();
            List<long> disposalsWithAct = ParentChildrenDomain.GetAll()
                .Where(x => x.Parent.TypeDocumentGji == GkhGji.Enums.TypeDocumentGji.Disposal && x.Parent.DocumentDate.HasValue && x.Parent.DocumentDate.Value.Date > currentDate.AddMonths(-1).Date && x.Parent.DocumentDate <= currentDate.Date)
                .Select(x => x.Parent.Id).Distinct().ToList();
            var listDisposal = new List<long>();

            if (currentDate.Date == DateTime.Now.Date)
            {
                listDisposal = DisposalDomain.GetAll().Where(x => x.State.FinalState && x.DateStart.HasValue && x.DateEnd.HasValue)
              .Where(x => x.DateStart.Value.Date <= currentDate.Date && x.DateEnd.Value.Date >= currentDate.Date)
                 .Where(x => !disposalsWithAct.Contains(x.Id))
                 .Select(x => x.Id).Distinct().ToList();
            }
            else
            {
                listDisposal = DisposalDomain.GetAll().Where(x => x.State.FinalState && x.DateStart.HasValue && x.DateEnd.HasValue)
                    .Where(x => x.DateEnd.Value.Date == currentDate.Date)
                       .Where(x => !disposalsWithAct.Contains(x.Id))
                       .Select(x => x.Id).Distinct().ToList();
            }

            var serviceViewDisposal = Container.Resolve<IDomainService<ViewDisposal>>();

            var data = serviceViewDisposal.GetAll()
                .Where(x => x.State.FinalState)
                .Where(x => listDisposal.Contains(x.Id))
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
                      x.IssuedDisposal,
                      KindCheck = x.KindCheckName,
                      x.ContragentName,
                      x.TypeDisposal,
                      MunicipalityNames = x.MunicipalityId != null ? x.MunicipalityNames : x.ContragentMuName,
                      MoSettlement = x.MoNames,
                      PlaceName = x.PlaceNames,
                      MunicipalityId = x.MunicipalityId ?? x.ContragentMuId,
                      x.IsActCheckExist,
                      x.RealityObjectCount,
                      x.TypeSurveyNames,
                      x.InspectorNames,
                      x.InspectionId,
                      x.TypeDocumentGji,
                      x.TypeAgreementProsecutor,
                      x.ControlType,
                      x.KindKNDGJI

                  })
                    .Filter(loadParam, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }

        public IDataResult GetListResolPros(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();
            var dayId = baseParams.Params.GetAs<long>("dayId");
            var taskCalendDomain = this.Container.Resolve<IDomainService<TaskCalendar>>();
            var resolProsDomain = this.Container.Resolve<IDomainService<ResolPros>>();
            var taskCalate = taskCalendDomain.Get(dayId);
            var currentDate = new DateTime(taskCalate.DayDate.Year, taskCalate.DayDate.Month, taskCalate.DayDate.Day);

            var data = resolProsDomain.GetAll().Where(x=>x.DateResolPros == currentDate).Select(x=> new 
            {
                x.Id,
                DateResolPros = x.DateResolPros,
                ResolProsTime = $"{x.FormatHour}:{x.FormatMinute.ToString().PadLeft(2, '0')}",
                FIO = x.PhysicalPerson,
                DocumenNumber = x.DocumentNumber,
                ContrAgentName = x.Contragent.ShortName
            }).Filter(loadParam, Container); ;

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }

        public IDataResult GetListCourtPractice(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();
            var dayId = baseParams.Params.GetAs<long>("dayId");
            var taskCalendDomain = this.Container.Resolve<IDomainService<TaskCalendar>>();
            var CourtPracticeDomain = this.Container.Resolve<IDomainService<CourtPractice>>();
            var InspectorDomain = this.Container.Resolve<IDomainService<CourtPracticeInspector>>();
            var taskCalate = taskCalendDomain.Get(dayId);
            var currentDate = new DateTime(taskCalate.DayDate.Year, taskCalate.DayDate.Month, taskCalate.DayDate.Day);
            var predata = CourtPracticeDomain.GetAll().Where(x => !x.State.FinalState && x.DateCourtMeeting.Date == currentDate.Date)
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
            .Where(x => !x.State.FinalState && x.DateCourtMeeting.Date == currentDate.Date)
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
                 DateCourtMeeting = x.DateCourtMeeting.ToShortDateString() + (x.CourtMeetingTime.HasValue ? (" " + x.CourtMeetingTime.Value.ToShortTimeString()) : ""),
                 DateCourtMeetingDate = x.CourtMeetingTime.HasValue ? new DateTime(x.DateCourtMeeting.Year, x.DateCourtMeeting.Month, x.DateCourtMeeting.Day, x.CourtMeetingTime.Value.Hour, x.CourtMeetingTime.Value.Minute, 0) : new DateTime(x.DateCourtMeeting.Year, x.DateCourtMeeting.Month, x.DateCourtMeeting.Day, 0, 0, 0),
                 InstanceGjiCode = x.InstanceGji != null ? x.InstanceGji.Code : "03",
                 x.CourtMeetingResult,
                 InLawDate  = (x.DateCourtMeeting.ToShortDateString() + (x.CourtMeetingTime.HasValue ? (" " + x.CourtMeetingTime.Value.ToShortTimeString()) : "")).ToString()

             }).AsQueryable()
             .Filter(loadParam, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), data.Count());
           
        }

        public IDataResult GetListProtocolsGji(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();
            var dayId = baseParams.Params.GetAs<long>("dayId");
            var taskCalendDomain = this.Container.Resolve<IDomainService<TaskCalendar>>();
            var ProtocolDomain = this.Container.Resolve<IDomainService<Protocol>>();
            var Protocol197Domain = this.Container.Resolve<IDomainService<Protocol197>>();
            var taskCalate = taskCalendDomain.Get(dayId);
            var currentDate = new DateTime(taskCalate.DayDate.Year, taskCalate.DayDate.Month, taskCalate.DayDate.Day);
            //if (currentDate.Date < DateTime.Now.Date)
            //{
            //    return null;
            //}
            var protCnt = ProtocolDomain.GetAll()
          .Where(x => x.State.FinalState && x.DateOfProceedings.HasValue && x.DateOfProceedings.Value.Date == currentDate.Date)
          .Select(x=> x.Id).Distinct().ToList();
            var prot197Cnt = Protocol197Domain.GetAll()
        .Where(x => x.State.FinalState && x.DateOfProceedings.HasValue && x.DateOfProceedings.Value.Date == currentDate.Date)
        .Select(x => x.Id).Distinct().ToList();

            var serviceViewProtocol = Container.Resolve<IDomainService<ViewProtocol>>();
            var serviceViewProtocol197 = Container.Resolve<IDomainService<ViewProtocol197>>();

            var predata = serviceViewProtocol.GetAll()
              .Where(x => protCnt.Contains(x.Id))
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
              .Where(x => prot197Cnt.Contains(x.Id))
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
                ProtTime = $"{x.FormatHour}:{x.FormatMinute.ToString().PadLeft(2, '0')}"
            }).AsQueryable().Filter(loadParam, Container); ;

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }

        private List<TaskCalendar> GetMonthDays(DateTime tmpDate)
        {
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
                    day.TaskCount = GetTasksByDay(day.DayDate);
                }
                else if (day.DayDate == dtNow)
                {
                    day.TaskCount = GetTasksCurrentDay();
                }
                else
                {
                    day.TaskCount = GetTasksByDay(day.DayDate);
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

        private int GetTasksCurrentDay()
        {
            var currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var CourtPracticeDomain = this.Container.Resolve<IDomainService<CourtPractice>>();
            var DisposalDomain = this.Container.Resolve<IDomainService<Disposal>>();
            var ParentChildrenDomain = this.Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var ProtocolDomain = this.Container.Resolve<IDomainService<Protocol>>();
            var Protocol197Domain = this.Container.Resolve<IDomainService<Protocol197>>();
            var resolProsDomain = this.Container.Resolve<IDomainService<ResolPros>>();
            List<long> disposalsWithAct = ParentChildrenDomain.GetAll()
                .Where(x => x.Parent.TypeDocumentGji == GkhGji.Enums.TypeDocumentGji.Disposal && x.Parent.DocumentDate.HasValue && x.Parent.DocumentDate.Value.Date > currentDate.AddMonths(-1).Date)
                .Select(x => x.Parent.Id).Distinct().ToList();
            var cntCP = CourtPracticeDomain.GetAll().Where(x => !x.State.FinalState && x.DateCourtMeeting.Date == currentDate.Date)
                .Count();
            var cntDisposal = DisposalDomain.GetAll().Where(x => x.State.FinalState && x.DateStart.HasValue && x.DateEnd.HasValue)
                .Where(x=> x.DateStart.Value.Date<= currentDate.Date && x.DateEnd.Value.Date>= currentDate.Date)
                .Where(x=> !disposalsWithAct.Contains(x.Id))
               .Count(); 
            var protCnt = ProtocolDomain.GetAll()
            .Where(x => x.State.FinalState && x.DateOfProceedings.HasValue && x.DateOfProceedings.Value.Date == currentDate.Date)
            .Count();
            var prot197Cnt = Protocol197Domain.GetAll()
                .Where(x => x.State.FinalState && x.DateOfProceedings.HasValue && x.DateOfProceedings.Value.Date == currentDate.Date)
                .Count();
            var resolPros = resolProsDomain.GetAll()
                .Where(x => x.DateResolPros == currentDate)
                .Count();
            return cntCP + cntDisposal + protCnt + prot197Cnt + resolPros;
        }
        private int GetTasksByDay(DateTime dateValue)
        {
            var currentDate = new DateTime(dateValue.Year, dateValue.Month, dateValue.Day);
            var CourtPracticeDomain = this.Container.Resolve<IDomainService<CourtPractice>>();
            var DisposalDomain = this.Container.Resolve<IDomainService<Disposal>>();
            var ProtocolDomain = this.Container.Resolve<IDomainService<Protocol>>();
            var Protocol197Domain = this.Container.Resolve<IDomainService<Protocol197>>();
            var ParentChildrenDomain = this.Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var resolProsDomain = this.Container.Resolve<IDomainService<ResolPros>>();
            List<long> disposalsWithAct = ParentChildrenDomain.GetAll()
                .Where(x => x.Parent.TypeDocumentGji == GkhGji.Enums.TypeDocumentGji.Disposal && x.Parent.DocumentDate.HasValue && x.Parent.DocumentDate.Value.Date > currentDate.AddMonths(-1).Date && x.Parent.DocumentDate.Value.Date <= currentDate.Date)
                .Select(x => x.Parent.Id).Distinct().ToList();
            var cntCP = CourtPracticeDomain.GetAll().Where(x => !x.State.FinalState && x.DateCourtMeeting.Date == currentDate.Date)
                .Count();
            var cntDisposal = DisposalDomain.GetAll().Where(x => x.State.FinalState && x.DateStart.HasValue && x.DateEnd.HasValue)
                .Where(x => x.DateEnd.Value.Date == currentDate.Date)
                   .Where(x => !disposalsWithAct.Contains(x.Id))
               .Count();
            var protCnt = ProtocolDomain.GetAll()
                .Where(x => x.State.FinalState && x.DateOfProceedings.HasValue && x.DateOfProceedings.Value.Date == currentDate.Date)
                .Count();
            var prot197Cnt = Protocol197Domain.GetAll()
                .Where(x => x.State.FinalState && x.DateOfProceedings.HasValue && x.DateOfProceedings.Value.Date == currentDate.Date)
                .Count();
            var resolPros = resolProsDomain.GetAll()
                .Where(x => x.DateResolPros == currentDate)
                .Count();
            return cntCP + cntDisposal + protCnt + prot197Cnt + resolPros;
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