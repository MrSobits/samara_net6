namespace Bars.Gkh.Overhaul.Hmao.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;

    using Entities;

    public class HmaoWorkPriceViewModel : Bars.Gkh.Overhaul.ViewModel.WorkPriceViewModel<HmaoWorkPrice>
    {
        public IRepository<Municipality> MunicipalityRepo { get; set; }
        public override IDataResult Get(IDomainService<HmaoWorkPrice> domainService, BaseParams baseParams)
        {
            var obj = domainService.Get(baseParams.Params["id"].To<long>());

            var municipality =
                MunicipalityRepo.GetAll().FirstOrDefault(x => x.ParentMo != null && x.Id == obj.Municipality.Id);

            return new BaseDataResult(new
            {
                obj.Id,
                obj.Job,
                Municipality = municipality != null ? municipality.ParentMo : obj.Municipality,
                Settlement = obj.Municipality,
                obj.NormativeCost,
                obj.Year,
                obj.SquareMeterCost,
                obj.CapitalGroup,
                obj.RealEstateType
            });
        }
    }
}