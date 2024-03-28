namespace Bars.GkhCr.Regions.Tatarstan.DomainService.Impl
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhCr.Regions.Tatarstan.Entities.ObjectOutdoorCr;

    using Castle.Windsor;

    public class ObjectOutdoorCrService : IObjectOutdoorCrService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult GetList(IDomainService<ObjectOutdoorCr> domainService, BaseParams baseParams)
        {
            var needDeleted = baseParams.Params.GetAs<bool>("needDeleted");
            var municipalityIds = baseParams.Params.GetAs<string>("municipalityIds").ToLongArray();
            var programIds = baseParams.Params.GetAs<string>("programIds")
                .ToLongArray()
                .Where(x => x != 0)
                .ToList();

            var userManager = this.Container.Resolve<IGkhUserManager>();
            using (this.Container.Using(userManager))
            {
                var unionMunicipalityIds =  municipalityIds.Union(userManager.GetMunicipalityIds()).Where(x => x != 0).ToList();
                
                return domainService.GetAll()
                    .WhereIf(needDeleted, x => x.BeforeDeleteRealityObjectOutdoorProgram != null && x.RealityObjectOutdoorProgram == null)
                    .WhereIf(!needDeleted, x => x.BeforeDeleteRealityObjectOutdoorProgram == null && x.RealityObjectOutdoorProgram != null)
                    .WhereIf(unionMunicipalityIds.Any(),
                        x => unionMunicipalityIds.Contains(x.RealityObjectOutdoor.MunicipalityFiasOktmo.Municipality.Id))
                    .WhereIf(programIds.Any() && !needDeleted, x => programIds.Contains(x.RealityObjectOutdoorProgram.Id))
                    .WhereIf(programIds.Any() && needDeleted, x => programIds.Contains(x.BeforeDeleteRealityObjectOutdoorProgram.Id))
                    .Select(x => new
                    {
                        x.Id,
                        x.State,
                        OutdoorProgramName = x.RealityObjectOutdoorProgram.Name,
                        BeforeDeleteOutdoorProgramName = x.BeforeDeleteRealityObjectOutdoorProgram.Name,
                        MunicipalityName = x.RealityObjectOutdoor.MunicipalityFiasOktmo.Municipality.Name,
                        RealityObjectOutdoorName = x.RealityObjectOutdoor.Name,
                        RealityObjectOutdoorCode = x.RealityObjectOutdoor.Code,
                        x.DateAcceptGji
                    })
                    .ToListDataResult(baseParams.GetLoadParam(), this.Container);
            }
        }

        /// <inheritdoc />
        public IDataResult Recover(BaseParams baseParams)
        {
            var ids = baseParams.Params.GetAs<string>("selectedIds").ToLongArray();

            using (var tr = this.Container.Resolve<IDataTransaction>())
            {
                var objectCrDomain = this.Container.ResolveDomain<ObjectOutdoorCr>();

                try
                {
                    var objectCrList = this.Container.ResolveDomain<ObjectOutdoorCr>().GetAll()
                        .Where(x => ids.Contains(x.Id))
                        .ToList();

                    foreach (var objectCr in objectCrList)
                    {
                        objectCr.RealityObjectOutdoorProgram = objectCr.BeforeDeleteRealityObjectOutdoorProgram;
                        objectCr.BeforeDeleteRealityObjectOutdoorProgram = null;

                        objectCrDomain.Update(objectCr);
                    }

                    tr.Commit();
                    return new BaseDataResult();
                }
                catch (Exception e)
                {
                    tr.Rollback();
                    return new BaseDataResult(false, e.Message);
                }
                finally
                {
                    this.Container.Release(objectCrDomain);
                }
            }
        }
    }
}
