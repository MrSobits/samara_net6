namespace Bars.GkhCr.DomainService
{
    using System.Linq;
    using B4;
    using B4.Utils;
    using Gkh.Domain;
    using Gkh.Enums;

    using Gkh.DataResult;
    using Entities;

    public class SpecialPerformedWorkActViewModel : BaseViewModel<SpecialPerformedWorkAct>
    {
        public override IDataResult List(IDomainService<SpecialPerformedWorkAct> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var objectCrId = baseParams.Params.GetAsId("objectCrId");

            var data = domainService.GetAll()
                .Where(x => x.ObjectCr.Id == objectCrId)
                .Select(x => new
                {
                    x.Id,
                    WorkName = x.TypeWorkCr.Work.Name,
                    Municipality = x.ObjectCr.RealityObject.Municipality.Name,
                    WorkFinanceSource = x.TypeWorkCr.FinanceSource.Name,
                    UnitMeasureName = x.TypeWorkCr.Work.UnitMeasure.Name,
                    x.Volume,
                    x.Sum,
                    x.DateFrom,
                    x.DocumentNum,
                    x.State,
                    x.RepresentativeSigned,
                    x.RepresentativeSurname,
                    x.RepresentativeName,
                    x.RepresentativePatronymic,
                    x.ExploitationAccepted,
                    x.WarrantyStartDate,
                    x.WarrantyEndDate
                })
                .Filter(loadParams, this.Container);

            var summary = data.Sum(x => x.Sum);
            var volumeSum = data.Sum(x => x.Volume);
            var totalCount = data.Count();

            data = data.Order(loadParams);

            return new ListSummaryResult(data.ToList(), totalCount, new { Sum = summary, Volume = volumeSum });
        }

        public override IDataResult Get(IDomainService<SpecialPerformedWorkAct> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();
            var obj = domainService.GetAll().FirstOrDefault(x => x.Id == id);

            return obj != null
                ? new BaseDataResult(
                    new
                    {
                        obj.Id,
                        obj.ObjectCr,
                        TypeWorkCr = obj.TypeWorkCr != null
                            ? new
                            {
                                obj.TypeWorkCr.Id,
                                WorkName =
                                    string.Format("{0} ({1})",
                                        obj.TypeWorkCr.Return(x => x.Work).Return(x => x.Name),
                                        obj.TypeWorkCr.Return(x => x.FinanceSource).Return(x => x.Name))
                            }
                            : null,
                        obj.DocumentNum,
                        obj.Volume,
                        obj.Sum,
                        obj.DateFrom,
                        obj.State,
                        obj.CostFile,
                        obj.DocumentFile,
                        obj.AdditionFile,
                        obj.UsedInExport,
                        obj.RepresentativeSigned,
                        obj.RepresentativeSurname,
                        obj.RepresentativeName,
                        obj.RepresentativePatronymic,
                        obj.ExploitationAccepted,
                        obj.WarrantyStartDate,
                        obj.WarrantyEndDate,
                        IsWork = obj.TypeWorkCr.Return(x => x.Work).Return(x => x.TypeWork) == TypeWork.Work
                    })
                : new BaseDataResult();
        }
    }
}