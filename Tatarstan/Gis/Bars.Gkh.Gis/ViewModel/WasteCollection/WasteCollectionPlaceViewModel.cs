namespace Bars.Gkh.Gis.ViewModel.WasteCollection
{
    using System.Linq;
    using B4;
    using B4.Utils;
    using Entities.WasteCollection;
    using Gkh.Domain;

    public class WasteCollectionPlaceViewModel : BaseViewModel<WasteCollectionPlace>
    {
        public override IDataResult List(IDomainService<WasteCollectionPlace> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    Municipality = x.RealityObject.Municipality.Name,
                    Settlement = x.RealityObject.MoSettlement.Name,
                    x.RealityObject.Address,
                    Customer = x.Customer.Name,
                    x.TypeWaste,
                    x.TypeWasteCollectionPlace,
                    x.ContainersCount,
                    x.LandfillDistance,
                    Contractor = x.Contractor.Name,
                    x.NumberContract
                })
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                .OrderThenIf(loadParams.Order.Length == 0, true, x => x.Settlement)
                .OrderThenIf(loadParams.Order.Length == 0, true, x => x.Address)
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        public override IDataResult Get(IDomainService<WasteCollectionPlace> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();
            var wasteCollPlace = domainService.GetAll().FirstOrDefault(x => x.Id == id);

            if (wasteCollPlace == null)
            {
                return new BaseDataResult(false, "Не удалось получить площадку сбора БО");
            }

            var obj = new
            {
                wasteCollPlace.Id,
                Municipality = wasteCollPlace.RealityObject.Municipality.Name,
                wasteCollPlace.RealityObject,
                wasteCollPlace.RealityObject.Address,
                wasteCollPlace.Customer,
                wasteCollPlace.TypeWaste,
                wasteCollPlace.TypeWasteCollectionPlace,
                wasteCollPlace.PeopleCount,
                wasteCollPlace.ContainersCount,
                wasteCollPlace.WasteAccumulationDaily,
                wasteCollPlace.LandfillDistance,
                wasteCollPlace.Comment,
                wasteCollPlace.ExportDaysWinter,
                wasteCollPlace.ExportDaysSummer,
                wasteCollPlace.Contractor,
                JuridicalAddress = wasteCollPlace.Contractor != null 
                    ? wasteCollPlace.Contractor.JuridicalAddress
                    : string.Empty,
                Inn = wasteCollPlace.Contractor != null
                    ? wasteCollPlace.Contractor.Inn
                    : string.Empty,
                Kpp = wasteCollPlace.Contractor != null
                    ? wasteCollPlace.Contractor.Kpp
                    : string.Empty,
                wasteCollPlace.NumberContract,
                wasteCollPlace.DateContract,
                wasteCollPlace.FileContract,
                wasteCollPlace.LandfillAddress
            };

            return new BaseDataResult(obj);
        }
    }
}