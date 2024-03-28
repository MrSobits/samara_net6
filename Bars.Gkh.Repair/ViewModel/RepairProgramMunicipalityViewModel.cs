namespace Bars.Gkh.Repair.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Repair.Entities;

    public class RepairProgramMunicipalityViewModel : BaseViewModel<RepairProgramMunicipality>
    {
        public override IDataResult List(IDomainService<RepairProgramMunicipality> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var repairProgramId = baseParams.Params.GetAs("repairProgramId", 0);

            var data = domainService.GetAll()
                .Where(x => x.RepairProgram.Id == repairProgramId)
                .Select(x => new
                {
                    x.Id,
                    MunicipalityName = x.Municipality.Name
                })
                .Filter(loadParams, this.Container);

            var totalCount = data.Count();

            data = data.Order(loadParams).Paging(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }
    }
}