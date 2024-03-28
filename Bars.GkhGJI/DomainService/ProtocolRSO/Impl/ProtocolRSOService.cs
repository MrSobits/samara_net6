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

    public class ProtocolRSOService : IProtocolRSOService
    {
        public IWindsorContainer Container { get; set; }

        public IList GetViewModelList(BaseParams baseParams, bool paging, ref int totalCount)
        {
            var serviceRo = Container.Resolve<IDomainService<ProtocolRSORealityObject>>();
            var service = Container.Resolve<IDomainService<ProtocolRSO>>();
            var articleLawService = Container.Resolve<IDomainService<ProtocolRSOArticleLaw>>();

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

                var articleLawList = articleLawService.GetAll()
                   .Where(y => y.ProtocolRSO != null && y.ArticleLaw != null)
                   .GroupBy(x => x.ProtocolRSO.Id)
                           .ToDictionary(x => x.Key, y => y.Select(x => x.ArticleLaw.Name).ToList());

                var data =
                    service.GetAll()
                           .WhereIf(
                               realityObjectId > 0,
                               x =>
                               serviceRo.GetAll()
                                        .Any(y => y.ProtocolRSO.Id == x.Id && y.RealityObject.Id == realityObjectId))
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
                                       x.TypeSupplierProtocol,
                                       GasSupplier = x.GasSupplier.Name,
                                       Executant = x.Executant.Name,
                                       InspectionId = x.Inspection.Id,
                                       Contragent = x.Contragent.Name,
                                       x.PhysicalPerson,
                                       Koap = articleLawList.ContainsKey(x.Id) ? String.Join(", ", articleLawList[x.Id]) : ""
                               })
                           .Filter(loadParam, Container);

                if (paging)
                {
                    data = data.Paging(loadParam);
                }

                totalCount = data.Count();

                return data.OrderIf(loadParam.Order.Length == 0, true, x => x.GasSupplier).ToList(); 
            }
            finally
            {
                Container.Release(serviceRo);
                Container.Release(service);
            }
        }
    }
}