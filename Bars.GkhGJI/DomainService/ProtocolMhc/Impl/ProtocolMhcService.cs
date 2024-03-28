namespace Bars.GkhGji.DomainService
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    using Castle.Windsor;

    public class ProtocolMhcService : IProtocolMhcService
    {
        public IWindsorContainer Container { get; set; }

        public IList GetViewModelList(BaseParams baseParams, bool paging, ref int totalCount)
        {
            var serviceRo = Container.Resolve<IDomainService<ProtocolMhcRealityObject>>();
            var service = Container.Resolve<IDomainService<ProtocolMhc>>();

            try
            {
                var loadParam = baseParams.GetLoadParam();

                /*
                 * параметры:
                 * dateStart - период с
                 * dateEnd - период по
                 * realityObjectId - жилой дом
                 */

                var dateStart = baseParams.Params.GetAs("dateStart", DateTime.MinValue);
                var dateEnd = baseParams.Params.GetAs("dateEnd", DateTime.MinValue);
                var realityObjectId = baseParams.Params.GetAs("realityObjectId", 0L);
                var stageId = baseParams.Params.GetAs("stageId", 0L);

                var data =
                    service.GetAll()
                           .WhereIf(
                               realityObjectId > 0,
                               x =>
                               serviceRo.GetAll()
                                        .Any(y => y.ProtocolMhc.Id == x.Id && y.RealityObject.Id == realityObjectId))
                           .WhereIf(dateStart != DateTime.MinValue, x => x.DocumentDate >= dateStart)
                           .WhereIf(dateEnd != DateTime.MinValue, x => x.DocumentDate <= dateEnd)
                           .WhereIf(stageId > 0, x => x.Stage.Id == stageId)
                           .Select(
                               x =>
                               new
                                   {
                                       x.Id,
                                       x.State,
                                       x.DocumentNumber,
                                       x.DocumentDate,
                                       Municipality = x.Municipality.Name,
                                       Executant = x.Executant.Name,
                                       InspectionId = x.Inspection.Id,
                                       Contragent = x.Contragent.Name,
                                       x.PhysicalPerson
                                   })
                           .Filter(loadParam, Container);

                if (paging)
                {
                    data = data.Paging(loadParam);
                }

                totalCount = data.Count();

                return data.OrderIf(loadParam.Order.Length == 0, true, x => x.Municipality).ToList(); 
            }
            finally
            {
                Container.Release(serviceRo);
                Container.Release(service);
            }
        }
    }
}