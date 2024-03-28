namespace Bars.GkhCr.DomainService.Impl
{
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Utils;
    using Castle.Windsor;
    using Entities;
    using Gkh.Domain;

    public class WorksCrService : IWorksCrService
    {
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Получить список
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public IDataResult List(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var programIds = baseParams.Params.GetAs<string>("program").ToLongArray();
            var muIds = baseParams.Params.GetAs<string>("mu").ToLongArray();
            var workIds = baseParams.Params.GetAs<string>("work").ToLongArray();

            var repo = Container.ResolveRepository<TypeWorkCr>();
            try
            {
                var data = repo.GetAll()
                    .Where(x => x.IsActive)
                    .WhereIf(programIds.IsNotEmpty(), x => programIds.Contains(x.ObjectCr.ProgramCr.Id))
                    .WhereIf(muIds.IsNotEmpty(), x => muIds.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                    .WhereIf(workIds.IsNotEmpty(), x => workIds.Contains(x.Work.Id))
                    .Select(x => new
                    {
                        x.Id,
                        x.State,
                        ProgramCr = x.ObjectCr.ProgramCr.Name,
                        Municipality = x.ObjectCr.RealityObject.Municipality.Name,
                        x.ObjectCr.RealityObject.Address,
                        Work = x.Work.Name,
                        ObjectCr = x.ObjectCr.Id
                    })
                    .Filter(loadParams, Container);

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }
            finally
            {
                Container.Release(repo);
            }
        }
    }
}