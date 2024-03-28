namespace Bars.Gkh.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Utils;
    using Domain;
    using Entities;
    using Repositories;

    public class EntranceViewModel : BaseViewModel<Entrance>
    {
        /// <summary>
        /// Получить список
        /// </summary>
        public override IDataResult List(IDomainService<Entrance> domainService, BaseParams baseParams)
        {
            var roId = baseParams.Params.GetAsId("objectId");

            var loadParams = GetLoadParam(baseParams);

            var tariffs = GetTariff(roId);

            var data = domainService.GetAll()
                .Where(x => x.RealityObject.Id == roId)
                .Select(x => new
                {
                    x.Id,
                    x.Number,
                    RealEstateType = x.RealEstateType.Name,
                    RoId = x.RealityObject.Id
                })
                .Order(loadParams)
                .Paging(loadParams)
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    x.Number,
                    x.RealEstateType,
                    Tariff = tariffs.Get(x.Id)
                })
                .ToList();

            return new ListDataResult(data, data.Count());
        }

        private Dictionary<long, decimal> GetTariff(long roId)
        {
            if (!Container.Kernel.HasComponent(typeof (IEntranceTariffRepository)))
            {
                return new Dictionary<long, decimal>();
            }

            var repo = Container.Resolve<IEntranceTariffRepository>();

            return repo.GetRobjectTariff(roId, DateTime.Today);
        }
    }
}
