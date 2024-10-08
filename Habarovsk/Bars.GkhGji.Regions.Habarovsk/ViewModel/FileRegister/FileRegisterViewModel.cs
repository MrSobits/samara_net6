﻿namespace Bars.GkhGji.Regions.Habarovsk.ViewModel
{
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Entities;

    public class FileRegisterViewModel : BaseViewModel<FileRegister>
    {
        public override IDataResult List(IDomainService<FileRegister> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.RealityObject.Address,
                    x.DateFrom,
                    x.DateTo,
                    MuName = x.RealityObject.Municipality.Name,
                    x.File
                })
             .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}
