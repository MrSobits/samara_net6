namespace Bars.Gkh.ViewModel
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;

    public class CSCalculationViewModel : BaseViewModel<CSCalculation>
    {
        public override IDataResult List(IDomainService<CSCalculation> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domain.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    CSFormula = x.CSFormula.Name,
                    x.Description,
                    Municipality = x.RealityObject.Municipality.Name,
                    RealityObject = x.RealityObject.Address,
                    Room = x.Room != null? x.Room.RoomNum:"",
                    x.CalcDate,
                    x.Result,
                    x.ObjectEditDate
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        } 
    }
}