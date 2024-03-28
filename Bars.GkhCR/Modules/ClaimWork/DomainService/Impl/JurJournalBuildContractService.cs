namespace Bars.GkhCr.Modules.ClaimWork.DomainService.Impl
{
    using System;
    using System.Collections;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Utils;
    using Castle.Windsor;
    using Entities;

    /// <summary>
    /// 
    /// </summary>
    public class JurJournalBuildContractService : IJurJournalBuildContractService
    {
        /// <summary>
        /// 
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

            var viewContractDomain = Container.ResolveDomain<ViewBuildContract>();

            var dateStart = baseParams.Params.GetAs<DateTime?>("dateStart");
            var dateEnd = baseParams.Params.GetAs<DateTime?>("dateEnd");
            var address = baseParams.Params.GetAs<string>("address");

            var loadParams = baseParams.GetLoadParam();

            var data = viewContractDomain.GetAll()
                .WhereIf(dateStart.HasValue, x => x.LawsuitDocDate.Value.Date >= dateStart.Value)
                .WhereIf(dateEnd.HasValue, x => x.LawsuitDocDate.Value.Date <= dateEnd.Value)
                .WhereIf(!address.IsEmpty(), x => x.Address.ToLower().Contains(address.ToLower()))
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