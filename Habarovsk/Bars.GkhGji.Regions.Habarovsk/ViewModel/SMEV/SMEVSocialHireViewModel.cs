﻿namespace Bars.GkhGji.Regions.Habarovsk.ViewModel
{
    using Entities;
    using B4;
    using B4.Utils;
    using System;
    using Enums;
    using System.Linq;

    public class SMEVSocialHireViewModel : BaseViewModel<SMEVSocialHire>
    {
        public override IDataResult List(IDomainService<SMEVSocialHire> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    ReqId = x.Id,
                    RequestDate = x.ObjectCreateDate,
                    Inspector = x.Inspector.Fio,
                    Municipality = x.Municipality != null ? x.Municipality.Name : "",
                    x.RequestState,
                    x.CalcDate,
                    RealityObject = x.RealityObject.Address
                })            
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }


     
    }
}
