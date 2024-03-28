namespace Bars.GkhGji.Regions.Habarovsk.ViewModel
{
    using B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
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
                    DocumentGJINumber = x.DocumentGji != null? x.DocumentGji.DocumentNumber:"",
                    PlantiffChoice = x.ContragentPlaintiff!=null? x.ContragentPlaintiff.Name:x.PlaintiffFio,
                    Lawyer = lawyers.ContainsKey(x.Id) ? lawyers[x.Id]:"",
                    Inspector = inspectors.ContainsKey(x.Id) ? inspectors[x.Id] : "",
                    CourtMeetingTime = x.CourtMeetingTime.HasValue? x.CourtMeetingTime.Value.ToShortTimeString(): string.Empty,
                    DefendantChoice = x.ContragentDefendant != null ? x.ContragentDefendant.Name : x.DefendantFio,
                    DifferentChoice = x.DifferentContragent != null ? x.DifferentContragent.Name : x.DifferentFIo,
                    TypeFactViolation = x.TypeFactViolation != null? x.TypeFactViolation.Name: string.Empty,
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

        public override IDataResult Get(IDomainService<CourtPractice> domain, BaseParams baseParams)
        {

            var loadParams = baseParams.GetLoadParam();
            var id = baseParams.Params["id"].To<long>();

            var dataCp = domain.Get(id);
            

            var data = new
               {
                dataCp.Id,
                dataCp.ContragentDefendant,
                dataCp.ContragentPlaintiff,
                dataCp.CourtCosts,
                dataCp.CourtCostsFact,
                dataCp.CourtCostsPlan,
                dataCp.CourtMeetingResult,
                CourtMeetingTime = dataCp.CourtMeetingTime.HasValue ? dataCp.CourtMeetingTime.Value.ToShortTimeString() : string.Empty,
                FormatHour = dataCp.CourtMeetingTime.HasValue ? dataCp.CourtMeetingTime.Value.Hour:0,
                FormatMinute = dataCp.CourtMeetingTime.HasValue ? dataCp.CourtMeetingTime.Value.Minute : 0,
                dataCp.CourtPracticeState,
                dataCp.DateCourtMeeting,
                dataCp.DefendantAddress,
                dataCp.DefendantFio,
                dataCp.DifferentAddress,
                dataCp.DifferentContragent,
                dataCp.DifferentFIo,
                dataCp.Discription,
                dataCp.Dispute,
                dataCp.DisputeCategory,
                dataCp.DisputeType,
                dataCp.DocumentNumber,
                dataCp.FileInfo,
                dataCp.InLaw,
                dataCp.InLawDate,
                dataCp.InstanceGji,
                dataCp.InterimMeasures,
                dataCp.InterimMeasuresDate,
                dataCp.JurInstitution,
                dataCp.ObjectCreateDate,
                dataCp.ObjectEditDate,
                dataCp.ObjectVersion,
                dataCp.PausedComment,
                dataCp.PerformanceList,
                dataCp.PerformanceProceeding,
                dataCp.PlaintiffAddress,
                dataCp.PlaintiffFio,
                dataCp.State,
                dataCp.TypeFactViolation,
                dataCp.DocumentGji,
                dataCp.MKDLicRequest,
                TypeDocumentGji = dataCp.DocumentGji != null ? dataCp.DocumentGji.TypeDocumentGji:GkhGji.Enums.TypeDocumentGji.Prescription,
                TypeBase = dataCp.DocumentGji != null? dataCp.DocumentGji.Inspection.TypeBase:GkhGji.Enums.TypeBase.CitizenStatement,
                InspId = dataCp.DocumentGji != null ? dataCp.DocumentGji.Inspection.Id:0
            };

            return new BaseDataResult(data);
        }



    }
}
