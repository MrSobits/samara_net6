namespace Bars.GkhGji.Regions.Chelyabinsk.DataExport
{
    using System;
    using System.Collections;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Utils;
    using Bars.GkhGji.DomainService;
    using Entities;
    using GkhGji.Entities;
    using System.Collections.Generic;
    using GkhGji.Enums;

    public class CourtPracticeDataExport : BaseDataExportService
    {
        public IDomainService<CourtPractice> domainService { get; set; }
        public IDomainService<CourtPracticeInspector> InspectorDomain { get; set; }

        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParam = GetLoadParam(baseParams);
            var showCloseAppeals = baseParams.Params.GetAs("showCloseAppeals", true);
            var dateStart2 = baseParams.Params.GetAs("dateStart", new DateTime());
            var dateEnd2 = baseParams.Params.GetAs("dateEnd", new DateTime());
            var predata = domainService.GetAll()
                .Where(x => x.DateCourtMeeting == null || (x.DateCourtMeeting >= dateStart2 && x.DateCourtMeeting <= dateEnd2))
                .WhereIf(!showCloseAppeals, x => !x.State.FinalState)
                .Select(x => x.Id).ToList();
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

            var data = domainService.GetAll()
                 .Where(x => x.DateCourtMeeting == DateTime.MinValue || (x.DateCourtMeeting >= dateStart2 && x.DateCourtMeeting <= dateEnd2))
                 .WhereIf(!showCloseAppeals, x => !x.State.FinalState)
                 .AsEnumerable()
                 .Select(x => new
                 {
                     x.Id,
                     x.State,
                     JurInstitution = x.JurInstitution.Name,
                     x.DocumentNumber,
                     PlantiffChoice = x.ContragentPlaintiff != null ? x.ContragentPlaintiff.Name : x.PlaintiffFio,
                     Lawyer = lawyers.ContainsKey(x.Id) ? lawyers[x.Id] : "",
                     Inspector = inspectors.ContainsKey(x.Id) ? inspectors[x.Id] : "",
                     DefendantChoice = x.ContragentDefendant != null ? x.ContragentDefendant.Name : x.DefendantFio,
                     DifferentChoice = x.DifferentContragent != null ? x.DifferentContragent.Name : x.DifferentFIo,
                     TypeFactViolation = x.TypeFactViolation.Name,
                     x.CourtPracticeState,
                     x.DateCourtMeeting,
                     InstanceGjiCode = x.InstanceGji != null ? x.InstanceGji.Code : "03",
                     x.CourtMeetingResult

                 }).AsQueryable()
                 .Filter(loadParam, Container);

            return data.ToList();
        }
    }
}