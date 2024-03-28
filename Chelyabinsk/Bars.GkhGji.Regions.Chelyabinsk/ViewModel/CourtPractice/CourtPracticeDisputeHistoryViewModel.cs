namespace Bars.GkhGji.Regions.Chelyabinsk.ViewModel
{
    using B4;
    using Bars.B4.Utils;
    using Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CourtPracticeDisputeHistoryViewModel : BaseViewModel<CourtPracticeDisputeHistory>
    {
        public IDomainService<CourtPracticeInspector> InspectorDomain { get; set; }

        public override IDataResult List(IDomainService<CourtPracticeDisputeHistory> domainService, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var id = loadParams.Filter.GetAs("courtpracticeId", 0L);          

            var data = domainService.GetAll()
               .Where(x => x.CourtPractice.Id == id)
                .AsEnumerable()
                .Select(x=> new
                {
                    x.Id,
                    JurInstitution = x.JurInstitution != null? x.JurInstitution.Name: string.Empty,
                    InstanceGji = x.InstanceGji.Name,
                    CourtMeetingTime = x.CourtMeetingTime.HasValue? x.CourtMeetingTime.Value.ToShortTimeString(): string.Empty,                  
                    x.CourtPracticeState,
                    DateCourtMeeting = x.DateCourtMeeting.ToShortDateString() + (x.CourtMeetingTime.HasValue? (" " + x.CourtMeetingTime.Value.ToShortTimeString()): ""),
                    DateCourtMeetingDate = x.CourtMeetingTime.HasValue ? new DateTime(x.DateCourtMeeting.Year, x.DateCourtMeeting.Month, x.DateCourtMeeting.Day, x.CourtMeetingTime.Value.Hour, x.CourtMeetingTime.Value.Minute, 0): new DateTime(x.DateCourtMeeting.Year, x.DateCourtMeeting.Month, x.DateCourtMeeting.Day, 0, 0, 0),
                    InstanceGjiCode = x.InstanceGji!= null? x.InstanceGji.Code:"03",
                    x.CourtMeetingResult

                }).AsQueryable()
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        public override IDataResult Get(IDomainService<CourtPracticeDisputeHistory> domain, BaseParams baseParams)
        {

            var loadParams = baseParams.GetLoadParam();
            int id = Convert.ToInt32(baseParams.Params.Get("id"));

            var data = domain.GetAll()
                 .WhereIf(id != 0, x => x.Id == id)
                   .Select(x => new
                   {
                        x.Id,                       
                        x.CourtCosts,
                        x.CourtCostsFact,
                        x.CourtCostsPlan,
                        x.CourtMeetingResult,
                        x.CourtMeetingTime,
                        FormatHour = x.CourtMeetingTime.HasValue ? x.CourtMeetingTime.Value.Hour:0,
                        FormatMinute = x.CourtMeetingTime.HasValue ? x.CourtMeetingTime.Value.Minute : 0,
                        x.CourtPracticeState,
                        x.DateCourtMeeting,
                        x.Discription,
                        x.Dispute,
                        x.CourtPractice.DisputeCategory,
                        x.CourtPractice.DisputeType,
                        x.CourtPractice.DocumentNumber,
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
                        x.CourtPractice.State,
                        x.CourtPractice.TypeFactViolation,
                        x.CourtPractice.DocumentGji,
                       Admonition = x.CourtPractice.Admonition != null ? new { x.CourtPractice.Admonition.Id, x.CourtPractice.Admonition.DocumentName, x.CourtPractice.Admonition.DocumentNumber, x.CourtPractice.Admonition.DocumentDate} : null,
                   })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }



    }
}
