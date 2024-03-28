namespace Bars.GkhGji.Regions.Chelyabinsk.ViewModel
{
    using B4;
    using Bars.B4.Utils;
    using Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CourtPracticeViewModel : BaseViewModel<CourtPractice>
    {
        public IDomainService<CourtPracticeInspector> InspectorDomain { get; set; }

        public override IDataResult List(IDomainService<CourtPractice> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var showCloseAppeals = baseParams.Params.GetAs("showCloseAppeals", true);
            var dateStart2 = baseParams.Params.GetAs("dateStart", new DateTime());
            var dateEnd2 = baseParams.Params.GetAs("dateEnd", new DateTime());
            var predata = domainService.GetAll()
              //  .Where(x => x.DateCourtMeeting == null || (x.DateCourtMeeting >= dateStart2 && x.DateCourtMeeting <= dateEnd2))
                .WhereIf(!showCloseAppeals, x => !x.State.FinalState)
                .Select(x => x.Id).ToList();
            Dictionary<long, string> lawyers = new Dictionary<long, string>();
            InspectorDomain.GetAll()
            .Where(x => predata.Contains(x.CourtPractice.Id))
            .Where(x=> x.Inspector != null)
            .Where(x => x.LawyerInspector == Enums.LawyerInspector.Lawyer).OrderBy(x=> x.Id).ForEach(x =>
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

            var data = domainService.GetAll()
                .Where(x => x.DateCourtMeeting == DateTime.MinValue || (x.DateCourtMeeting >= dateStart2 && x.DateCourtMeeting <= dateEnd2))
                .WhereIf(!showCloseAppeals, x => !x.State.FinalState)
                .AsEnumerable()
                .Select(x=> new
                {
                    x.Id,
                    x.State,
                    JurInstitution = x.JurInstitution != null? x.JurInstitution.Name: string.Empty,
                    x.DocumentNumber,
                    PlantiffChoice = x.ContragentPlaintiff!=null? x.ContragentPlaintiff.Name:x.PlaintiffFio,
                    Lawyer = lawyers.ContainsKey(x.Id) ? lawyers[x.Id]:"",
                    Inspector = inspectors.ContainsKey(x.Id) ? inspectors[x.Id] : "",
                    CourtMeetingTime = x.CourtMeetingTime.HasValue? x.CourtMeetingTime.Value.ToShortTimeString(): string.Empty,
                    DefendantChoice = x.ContragentDefendant != null ? x.ContragentDefendant.Name : x.DefendantFio,
                    DifferentChoice = x.DifferentContragent != null ? x.DifferentContragent.Name : x.DifferentFIo,
                    TypeFactViolation = x.TypeFactViolation != null? x.TypeFactViolation.Name: string.Empty,
                    x.CourtPracticeState,
                    x.DisputeType,
                    DateCourtMeeting = x.DateCourtMeeting.ToShortDateString() + (x.CourtMeetingTime.HasValue? (" " + x.CourtMeetingTime.Value.ToShortTimeString()): ""),
                    DateCourtMeetingDate = x.CourtMeetingTime.HasValue ? new DateTime(x.DateCourtMeeting.Year, x.DateCourtMeeting.Month, x.DateCourtMeeting.Day, x.CourtMeetingTime.Value.Hour, x.CourtMeetingTime.Value.Minute, 0): new DateTime(x.DateCourtMeeting.Year, x.DateCourtMeeting.Month, x.DateCourtMeeting.Day, 0, 0, 0),
                    InstanceGjiCode = x.InstanceGji!= null? x.InstanceGji.Code:"03",
                    x.CourtMeetingResult

                }).AsQueryable()
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        public override IDataResult Get(IDomainService<CourtPractice> domain, BaseParams baseParams)
        {

            var loadParams = baseParams.GetLoadParam();
            int id = Convert.ToInt32(baseParams.Params.Get("id"));

            var data = domain.GetAll()
                 .WhereIf(id != 0, x => x.Id == id)
                   .Select(x => new
                   {
                        x.Id,
                        ContragentDefendant = x.ContragentDefendant!= null? new {x.ContragentDefendant.Id, x.ContragentDefendant.Name}:null,
                        ContragentPlaintiff = x.ContragentPlaintiff != null? new { x.ContragentPlaintiff.Id, x.ContragentPlaintiff.Name}:null,
                        x.CourtCosts,
                        x.CourtCostsFact,
                        x.CourtCostsPlan,
                        x.CourtMeetingResult,
                        x.CourtMeetingTime,
                        FormatHour = x.CourtMeetingTime.HasValue ? x.CourtMeetingTime.Value.Hour:0,
                        FormatMinute = x.CourtMeetingTime.HasValue ? x.CourtMeetingTime.Value.Minute : 0,
                        x.CourtPracticeState,
                        x.DateCourtMeeting,
                        x.DefendantAddress,
                        x.DefendantFio,
                        x.DifferentAddress,
                        x.DifferentContragent,
                        x.DifferentFIo,
                        x.Discription,
                        x.Dispute,
                        x.DisputeCategory,
                        x.DisputeType,
                        x.DocumentNumber,
                        x.FileInfo,
                        x.InLaw,
                        x.InLawDate,
                        x.InstanceGji,
                        x.InterimMeasures,
                        x.InterimMeasuresDate,
                        JurInstitution = x.JurInstitution != null ? new { x.JurInstitution.Id, x.JurInstitution.Name} : null,
                        x.ObjectCreateDate,
                        x.ObjectEditDate,
                        x.ObjectVersion,
                        x.PausedComment,
                        x.PerformanceList,
                        x.PerformanceProceeding,
                        x.PlaintiffAddress,
                        x.PlaintiffFio,
                        x.State,
                        x.TypeFactViolation,
                        x.DocumentGji,
                       Admonition = x.Admonition != null ? new { x.Admonition.Id, x.Admonition.DocumentName, x.Admonition.DocumentNumber, x.Admonition.DocumentDate} : null,
                       x.MKDLicRequest,
                       x.ResolutionDecision,
                       x.AppealCitsDecision,
                       AppealNumber = x.ResolutionDecision.AppealNumber,
                       x.DisputeResult,
                       x.AppealCitsDefinition,
                       x.ResolutionDefinition
                   })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }



    }
}
