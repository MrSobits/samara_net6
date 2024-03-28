namespace Bars.GkhCr.DomainService.Impl
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Utils;
    using Bars.GkhCr.Entities;

    using Castle.Windsor;

    /// <inheritdoc />
    public class SpecialObjectCrService : ISpecialObjectCrService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public IQueryable<ViewSpecialObjectCr> GetFilteredByOperator()
        {
            var userManager = this.Container.Resolve<IGkhUserManager>();

            var municipalityIds = userManager.GetMunicipalityIds();
            var contragentIds = userManager.GetContragentIds();

            var serviceManOrgContractRobjectDomain = this.Container.Resolve<IDomainService<ManOrgContractRealityObject>>();
            var viewSpecialObjectCrDomain = this.Container.Resolve<IDomainService<ViewSpecialObjectCr>>();

            using (this.Container.Using(serviceManOrgContractRobjectDomain, viewSpecialObjectCrDomain))
            {
                return viewSpecialObjectCrDomain.GetAll()
                    .WhereIf(municipalityIds.Count > 0, x => municipalityIds.Contains(x.MunicipalityId))
                    .WhereIf(contragentIds.Count > 0, y => serviceManOrgContractRobjectDomain.GetAll()
                    .Any(x => x.RealityObject.Id == y.RealityObjectId
                            && contragentIds.Contains(x.ManOrgContract.ManagingOrganization.Contragent.Id)
                            && x.ManOrgContract.StartDate <= DateTime.Now.Date
                            && (!x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate.Value >= DateTime.Now.Date)));
            }
        }

        /// <inheritdoc />
        public IDataResult GetBuilders(BaseParams baseParams)
        {
            var objectCrId = baseParams.Params.GetAsId("objectCrId");

            var specialBuildContractDomain = this.Container.Resolve<IDomainService<SpecialBuildContract>>();

            using (this.Container.Using(specialBuildContractDomain))
            {
                return specialBuildContractDomain.GetAll()
                    .Where(x => x.ObjectCr.Id == objectCrId)
                    .Select(x => x.Builder)
                    .Select(x => new
                    {
                        x.Id,
                        Municipality = x.Contragent.Municipality.Name,
                        ContragentName = x.Contragent.Name,
                        ContragentId = x.Contragent.Id,
                        x.Contragent.Inn,
                        x.Contragent.Kpp,
                        x.AdvancedTechnologies,
                        x.ConsentInfo,
                        x.WorkWithoutContractor,
                        x.Rating,
                        x.ActivityGroundsTermination,
                        x.File,
                        x.FileLearningPlan,
                        x.FileManningShedulle,
                        ContragentPhone = x.Contragent.Phone
                    })
                    .ToListDataResult(baseParams.GetLoadParam(), this.Container);
            }
        }

        /// <inheritdoc />
        public IDataResult Recover(BaseParams baseParams)
        {
            var ids = baseParams.Params.GetAs<string>("selected").ToLongArray();

            using (var tr = this.Container.Resolve<IDataTransaction>())
            {
                var objectCrDomain = this.Container.ResolveDomain<SpecialObjectCr>();

                using (this.Container.Using(objectCrDomain))
                {
                    var objectCrList = objectCrDomain.GetAll()
                        .Where(x => ids.Contains(x.Id))
                        .ToList();

                    foreach (var objectCr in objectCrList)
                    {
                        objectCr.ProgramCr = objectCr.BeforeDeleteProgramCr;
                        objectCr.BeforeDeleteProgramCr = null;

                        objectCrDomain.Update(objectCr);
                    }

                    tr.Commit();
                    return new BaseDataResult();
                }
            }
        }

        /// <inheritdoc />
        public IDataResult GetAdditionalParams(BaseParams baseParams)
        {
            var objectCrId = baseParams.Params.GetAsId("objectCrId");

            var domain = this.Container.ResolveDomain<SpecialAdditionalParameters>();

            SpecialAdditionalParameters rec;

            try
            {
                rec = domain.GetAll().FirstOrDefault(x => x.ObjectCr.Id == objectCrId);
            }
            catch (Exception exception)
            {
                return BaseDataResult.Error(exception.Message);
            }
            finally
            {
                this.Container.Release(domain);
            }

            return new BaseDataResult(rec);
        }
    }
}