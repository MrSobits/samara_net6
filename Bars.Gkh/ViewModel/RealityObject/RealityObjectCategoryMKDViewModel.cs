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

    public class RealityObjectCategoryMKDViewModel : BaseViewModel<RealityObjectCategoryMKD>
    {
        /// <summary>
        /// Получить список
        /// </summary>
        public override IDataResult List(IDomainService<RealityObjectCategoryMKD> domainService, BaseParams baseParams)
        {
            var roId = baseParams.Params.GetAsId("objectId");

            var loadParams = GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Where(x => x.RealityObject.Id == roId)
                  .Select(x => new
                  {
                      x.Id,
                      CategoryCSMKD = x.CategoryCSMKD.Name
                  })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
       
    }
}
