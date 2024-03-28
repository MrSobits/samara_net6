namespace Bars.GkhGji.Regions.Voronezh.ViewModel
{
    using Entities;
    using B4;
    using B4.Utils;
    using System;
    using Enums;
    using System.Linq;
    using Bars.GkhGji.Regions.Voronezh.Entities.SMEVRedevelopment;
    using System.Xml.Linq;
    using System.Xml.Serialization;
    using System.IO;
    using System.Xml;
    using Bars.B4.Modules.FileStorage;

    public class SMEVRedevelopmentViewModel : BaseViewModel<SMEVRedevelopment>
    {
        public override IDataResult List(IDomainService<SMEVRedevelopment> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Select(x=> new
                {
                    x.Id,
                    ReqId = x.Id,
                    RequestDate = x.ObjectCreateDate,
                    Inspector = x.Inspector.Fio,
                    Municipality = x.Municipality != null ? x.Municipality.Name : "",
                    x.RequestState,
                    x.CalcDate,
                    RealityObject = x.RealityObject.Address,
                    x.Answer
                })               
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

    }
}

