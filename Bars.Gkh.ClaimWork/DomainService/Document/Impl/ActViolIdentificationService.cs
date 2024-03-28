namespace Bars.Gkh.ClaimWork.DomainService.Document.Impl
{
    using System;
    using System.Collections;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.IoC;
    using B4.Utils;
    using Castle.Windsor;
    using Entities;

    /// <summary>
    /// Реализация интерфейса для акта проверки нарушения
    /// </summary>
    public class ActViolIdentificationService : IActViolIdentificationService
    {
        /// <summary>
        /// Container
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseParams"></param>
        /// <param name="usePaging"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IList GetList(BaseParams baseParams, bool usePaging, out int totalCount)
        {
            totalCount = 0;
            var dateStart = baseParams.Params.GetAs<DateTime?>("dateStart");
            var dateEnd = baseParams.Params.GetAs<DateTime?>("dateEnd");
            var roId = baseParams.Params.GetAs<long>("address");
            var loadParams = baseParams.GetLoadParam();

            var actViolIdentifDomain = Container.ResolveDomain<ActViolIdentificationClw>();

            using (Container.Using(actViolIdentifDomain))
            {
                var data = actViolIdentifDomain.GetAll()
                    .WhereIf(dateStart.HasValue, x => x.FormDate.Value.Date >= dateStart.Value.Date)
                    .WhereIf(dateEnd.HasValue, x => x.FormDate.Value.Date <= dateEnd.Value.Date)
                    .WhereIf(roId != 0, x => x.ClaimWork.RealityObject.Id == roId)
                    .Select(x => new
                    {
                        x.Id,
                        x.ClaimWork,
                        x.ClaimWork.ClaimWorkTypeBase,
                        x.FactOfSigning,
                        x.FormDate,
                        x.ClaimWork.BaseInfo,
                        Municipality = x.ClaimWork.RealityObject.Municipality.Name,
                        x.ClaimWork.RealityObject.Address
                    })
                    .Filter(loadParams, Container)
                    .Order(loadParams);
                
                if (usePaging)
                {
                    totalCount = data.Count();
                    data = data.Paging(loadParams);
                }

                return data.ToList();
            }
        }
    }
}