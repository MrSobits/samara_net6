namespace Bars.GkhCr.DomainService
{
    using System.Collections.Generic;
    using System.Linq;
    using B4;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.ConfigSections.Cr;
    using Bars.Gkh.ConfigSections.Cr.Enums;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Utils;
    using Bars.GkhCr.Enums;

    using Entities;
    using Castle.Windsor;
    using Gkh.DomainService.GkhParam;
    using Bars.B4.Modules.States;
    using System;

    public class BuildContractService : IBuildContractService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddTypeWorks(BaseParams baseParams)
        {
            var buildContractTypeWorkDomain = Container.ResolveDomain<BuildContractTypeWork>();
            var typeWorkDomain = Container.ResolveDomain<TypeWorkCr>();
            var buildContractDomain = Container.ResolveDomain<BuildContract>();
            try
            {
                var buildContractId = baseParams.Params.GetAs<long>("buildContractId");

                var objectIds = baseParams.Params["objectIds"].ToStr().Split(',').Select(x => x.ToLong()).ToList();

                var exsistingTypeWorks = buildContractTypeWorkDomain.GetAll().Where(x => x.BuildContract.Id == buildContractId).Select(x => x.TypeWork.Id).ToList();

                foreach (var id in objectIds.Where(x => !exsistingTypeWorks.Contains(x)))
                {
                    var newBuildContractTypeWork = new BuildContractTypeWork { BuildContract = buildContractDomain.Load(buildContractId), TypeWork = typeWorkDomain.Load(id), };

                    buildContractTypeWorkDomain.Save(newBuildContractTypeWork);
                }

                return new BaseDataResult { Success = true };
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
            finally
            {
                Container.Release(buildContractTypeWorkDomain);
                Container.Release(typeWorkDomain);
                Container.Release(buildContractDomain);
            }
        }

        public IDataResult AddTermination(BaseParams baseParams)
        {
            var terminationDomain = Container.ResolveDomain<BuildContractTermination>();
            var buildContractDomain = Container.ResolveDomain<BuildContract>();
            try
            {
                var buildContractId = baseParams.Params.GetAs<long>("buildContractId");

                var newTermination = new BuildContractTermination
                {
                    BuildContract = buildContractDomain.Get(buildContractId),
                    TerminationDate = DateTime.Now,
                    Reason = "Н/Д",
                    DocumentNumber = "Н/Д"
                };

                terminationDomain.Save(newTermination);

                return new BaseDataResult { Success = true };
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
            finally
            {
                Container.Release(terminationDomain);
                Container.Release(buildContractDomain);
            }
        }

        public IDataResult ListAvailableStates(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var statesDomain = Container.ResolveDomain<State>();
            try
            {
                var data = statesDomain.GetAll()
                    .Where(x => x.TypeId == "cr_obj_build_contract")
                    .Select(x => x);

                data.Order(loadParams);
                var totalCount = data.Count();

                return new ListDataResult(data.ToList(), totalCount);
            }
            finally
            {
                Container.Release(statesDomain);
            }
        }

        public IDataResult ListAvailableBuilders(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var builderDomain = this.Container.ResolveDomain<Builder>();
            var lotBidDomain = this.Container.ResolveDomain<CompetitionLotBid>();
            var qualificationDomain = this.Container.ResolveDomain<Qualification>();
            var lotTypeWorkDomain = this.Container.ResolveDomain<CompetitionLotTypeWork>();

            try
            {
                var objectCrId = baseParams.Params.GetAs<long>("objectCrId");
                if (objectCrId == 0)
                {
                    return new BaseDataResult(false, "Не указан объект КР");
                }

                var builderSelection = this.Container.GetGkhConfig<GkhCrConfig>().General.BuilderSelection;

                var query = builderDomain.GetAll().
                    Select(x => new
                    {
                        x.Id,
                        ContragentName = x.Contragent.Name
                    });

                IQueryable<long> builderSelector = null;

                if (builderSelection == TypeBuilderSelection.Qualification)
                {
                    builderSelector = qualificationDomain.GetAll()
                        .Where(x => x.ObjectCr.Id == objectCrId)
                        .Select(x => x.Builder.Id);
                }
                else if (builderSelection == TypeBuilderSelection.Competition)
                {
                    var queryTypeWorks = lotTypeWorkDomain.GetAll()
                        .Where(x => x.TypeWork.ObjectCr.Id == objectCrId);

                    builderSelector = lotBidDomain.GetAll()
                        .Where(x => queryTypeWorks.Any(y => y.Lot.Id == x.Lot.Id))
                        .Select(x => x.Builder.Id);
                }

                return query.WhereIf(
                        builderSelector != null, 
                        x => builderSelector.Any(y => y == x.Id))
                    .ToListDataResult(loadParams, this.Container);
            }
            finally
            {
                this.Container.Release(builderDomain);
                this.Container.Release(lotBidDomain);
                this.Container.Release(qualificationDomain);
                this.Container.Release(lotTypeWorkDomain);
            }
        }

        public virtual IDataResult GetForMap(BaseParams baseParams)
        {
            var bcDomain = this.Container.ResolveDomain<BuildControlTypeWorkSmr>();

            try
            {
                var id = baseParams.Params.GetAs<long>("id");
                var dt = bcDomain.GetAll()
                    .Where(x => x.Id == id)
                    .Select(x => new { x = x.Latitude, y = x.Longitude }).FirstOrDefault();

                return new BaseDataResult(dt);
            }
            finally
            {
                this.Container.Release(bcDomain);
            }
        }
    }
}