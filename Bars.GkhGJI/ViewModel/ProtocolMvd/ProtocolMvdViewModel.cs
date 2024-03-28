namespace Bars.GkhGji.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.GkhGji.Entities;

    public class ProtocolMvdViewModel: ProtocolMvdViewModel<ProtocolMvd>
    {
    }

    public class ProtocolMvdViewModel<T> : BaseViewModel<T>
        where T: ProtocolMvd
    {
        public override IDataResult List(IDomainService<T> domainService, BaseParams baseParams)
        {
            /*
             * параметры:
             * dateStart - период с
             * dateEnd - период по
             * realityObjectId - жилой дом
             */

            var serviceProtocolMvdRo = Container.Resolve<IDomainService<ProtocolMvdRealityObject>>();
            var userManager = Container.Resolve<IGkhUserManager>();

            try
            {
                var loadParam = baseParams.GetLoadParam();

                var dateStart = baseParams.Params.ContainsKey("dateStart") ? baseParams.Params["dateStart"].ToDateTime() : DateTime.MinValue;
                var dateEnd = baseParams.Params.ContainsKey("dateEnd") ? baseParams.Params["dateEnd"].ToDateTime() : DateTime.MinValue;
                var realityObjectId = baseParams.Params.ContainsKey("realityObjectId") ? baseParams.Params["realityObjectId"].ToLong() : 0;
                var stageId = baseParams.Params.ContainsKey("stageId") ? baseParams.Params["stageId"].ToLong() : 0;

                List<long> ids = null;

                if (realityObjectId > 0)
                {
                    ids = new List<long>();

                    ids.AddRange(serviceProtocolMvdRo.GetAll()
                        .Where(x => x.RealityObject.Id == realityObjectId)
                        .Select(x => x.ProtocolMvd.Id).ToList());
                }

                var municipalityList = userManager.GetMunicipalityIds();

                var data = domainService.GetAll()
                    //.WhereIf(municipalityList.Count > 0, x => municipalityList.Contains(x.Contragent.Municipality.Id))
                    .WhereIf(ids != null, x => ids.Contains(x.Id))
                    .WhereIf(dateStart != DateTime.MinValue, x => x.DocumentDate >= dateStart)
                    .WhereIf(dateEnd != DateTime.MinValue, x => x.DocumentDate <= dateEnd)
                    .WhereIf(stageId > 0, x => x.Stage.Id == stageId)
                    .Select(x => new
                    {
                        x.Id,
                        x.State,
                        x.DocumentNumber,
                        x.DocumentDate,
                        Executant = x.TypeExecutant,
                        InspectionId = x.Inspection.Id,
                        x.PhysicalPerson
                    })
                    //.OrderIf(loadParam.Order.Length == 0, true, x => x.Municipality)
                    .Filter(loadParam, Container);

                int totalCount = data.Count();

                return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
            }
            finally 
            {
                Container.Release(serviceProtocolMvdRo);
                Container.Release(userManager);
            }
            
        }

        public override IDataResult Get(IDomainService<T> domainService, BaseParams baseParams)
        {
            var intId = baseParams.Params.GetAs("id", 0L);
            var obj = domainService.GetAll().FirstOrDefault(x => x.Id == intId);
            
            obj.InspectionId = obj.Inspection.Id;
            obj.TimeOffense = obj.DateOffense.HasValue ? obj.DateOffense.Value.ToString("HH:mm") : string.Empty;

            return new BaseDataResult(obj);
        }
    }
}
