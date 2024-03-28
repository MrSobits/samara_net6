namespace Bars.GkhGji.Regions.Tula.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tula.Entities;
    using Bars.GkhGji.ViewModel;

    public class TulaActSurveyViewModel: ActSurveyViewModel<TulaActSurvey>
    {
        public override IDataResult Get(IDomainService<TulaActSurvey> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id");
                
            var obj =
                domainService.GetAll()
                                .Where(x => x.Id == id)
                                .Select(
                                    x =>
                                    new
                                    {
                                        x.Id,
                                        InspectionId = (x.Inspection != null ? x.Inspection.Id : 0),
                                        x.DocumentDate,
                                        x.DocumentNumber,
                                        x.DocumentYear,
                                        x.DocumentNum,
                                        x.DocumentSubNum,
                                        x.State,
                                        x.Flat,
                                        x.Area,
                                        x.DateOf,
                                        TimeStart =
                                    x.TimeStart.HasValue ? x.TimeStart.Value.ToShortTimeString() : "",
                                        TimeEnd =
                                    x.TimeEnd.HasValue ? x.TimeEnd.Value.ToShortTimeString() : "",
                                    x.FactSurveyed,
                                    x.Reason,
                                    x.ConclusionIssued,
                                    x.Description
                                    })
                                .FirstOrDefault();

            return new BaseDataResult(obj);
        }
    }
}