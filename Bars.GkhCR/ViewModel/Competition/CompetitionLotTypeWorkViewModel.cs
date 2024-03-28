namespace Bars.GkhCr.DomainService
{
    using System.Linq;

    using B4;
    using Entities;
    using Gkh.Domain;

    public class CompetitionLotTypeWorkViewModel : BaseViewModel<CompetitionLotTypeWork>
    {
        public override IDataResult List(IDomainService<CompetitionLotTypeWork> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var lotId = loadParams.Filter.GetAsId("lotId");

            var data = domainService.GetAll()
                .Where(x => x.Lot.Id == lotId)
                .Select(x => new
                {
                    x.Id,
                    TypeWork = x.TypeWork.Work.Name,
                    RoAddress = x.TypeWork.ObjectCr.RealityObject.Address,
                    RoMunicipality = x.TypeWork.ObjectCr.RealityObject.Municipality.Name,
                    ProgrammName = x.TypeWork.ObjectCr.ProgramCr.Name
                })
                .Filter(loadParams, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }
    }
}