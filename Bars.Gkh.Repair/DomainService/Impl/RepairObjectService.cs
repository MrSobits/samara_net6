namespace Bars.Gkh.Repair.DomainService.Impl
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Repair.Entities;

    using Castle.Windsor;

    public class RepairObjectService : IRepairObjectService
    {
        public IWindsorContainer Container { get; set; }

        public IGkhUserManager GkhUserManagerService { get; set; }

        public IDomainService<RepairObject> RepairObjectDomainService { get; set; }

        public IDomainService<ManOrgContractRealityObject> ManOrgContractRealityObjectDomainService { get; set; }

        public IQueryable<RepairObject> GetFilteredByOperator()
        {
            var municipalityIds = this.GkhUserManagerService.GetMunicipalityIds();
            var contragentIds = this.GkhUserManagerService.GetContragentIds();

            return this.RepairObjectDomainService.GetAll()
                .WhereIf(municipalityIds.Count > 0, x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                .WhereIf(
                    contragentIds.Count > 0,
                    y => this.ManOrgContractRealityObjectDomainService.GetAll()
                        .Any(
                            x => x.RealityObject.Id == y.RealityObject.Id
                                && contragentIds.Contains(x.ManOrgContract.ManagingOrganization.Contragent.Id)
                                && x.ManOrgContract.StartDate <= DateTime.Now.Date
                                && (!x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate.Value >= DateTime.Now.Date)));
        }

        public IDataResult MassStateChange(BaseParams baseParams)
        {
            var ids = baseParams.Params.GetAs<long[]>("ids");
            var newStateId = baseParams.Params.GetAs<long>("newStateId");
            using (var tr = this.Container.Resolve<IDataTransaction>())
            {
                var stateProvider = this.Container.Resolve<IStateProvider>();
                var serviceState = this.Container.Resolve<IDomainService<State>>();

                try
                {
                    var newState = serviceState.Load(newStateId);

                    foreach (var id in ids)
                    {
                        stateProvider.ChangeState(id, "repair_object", newState, "Массовый перевод статуса", true);
                    }

                    tr.Commit();
                    return new BaseDataResult();
                }
                catch (ValidationException e)
                {
                    tr.Rollback();
                    return new BaseDataResult(false, e.Message);
                }
                catch
                {
                    tr.Rollback();
                    throw;
                }
                finally
                {
                    this.Container.Release(stateProvider);
                    this.Container.Release(serviceState);
                }
            }
        }
    }
}