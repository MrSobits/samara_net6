namespace Bars.GkhCr.DomainService
{
    using System.Linq;
    using B4;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.ConfigSections.Cr;
    using Bars.Gkh.ConfigSections.Cr.Enums;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Utils;

    using Entities;
    using Castle.Windsor;

    public class SpecialBuildContractService : ISpecialBuildContractService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddTypeWorks(BaseParams baseParams)
        {
            var buildContractTypeWorkDomain = this.Container.ResolveDomain<SpecialBuildContractTypeWork>();
            var typeWorkDomain = this.Container.ResolveDomain<SpecialTypeWorkCr>();
            var buildContractDomain = this.Container.ResolveDomain<SpecialBuildContract>();
            try
            {
                var buildContractId = baseParams.Params.GetAs<long>("buildContractId");

                var objectIds = baseParams.Params["objectIds"].ToStr().Split(',').Select(x => x.ToLong()).ToList();

                var exsistingTypeWorks = buildContractTypeWorkDomain.GetAll()
                    .Where(x => x.BuildContract.Id == buildContractId)
                    .Select(x => x.TypeWork.Id)
                    .ToList();

                foreach (var id in objectIds.Where(x => !exsistingTypeWorks.Contains(x)))
                {
                    var newBuildContractTypeWork = new SpecialBuildContractTypeWork
                    {
                        BuildContract = buildContractDomain.Load(buildContractId),
                        TypeWork = typeWorkDomain.Load(id),
                    };

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
                this.Container.Release(buildContractTypeWorkDomain);
                this.Container.Release(typeWorkDomain);
                this.Container.Release(buildContractDomain);
            }
        }

        public IDataResult ListAvailableBuilders(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var builderDomain = this.Container.ResolveDomain<Builder>();
            var lotBidDomain = this.Container.ResolveDomain<CompetitionLotBid>();
            var qualificationDomain = this.Container.ResolveDomain<SpecialQualification>();
            var lotTypeWorkDomain = this.Container.ResolveDomain<CompetitionLotSpecialTypeWork>();
            try
            {
                var objectCrId = baseParams.Params.GetAs<long>("objectCrId");
                if (objectCrId == 0)
                {
                    return new BaseDataResult(false, "Не указан объект КР");
                }

                var builderSelection = this.Container.GetGkhConfig<GkhCrConfig>().General.BuilderSelection;
                var query = builderDomain.GetAll().Select(x => new { ContragentName = x.Contragent.Name, x.Id });
                IQueryable<long> builderSelector = null;
                if (builderSelection == TypeBuilderSelection.Competition)
                {
                    var queryTypeWorks = lotTypeWorkDomain.GetAll().Where(x => x.TypeWork.ObjectCr.Id == objectCrId);
                    builderSelector = lotBidDomain.GetAll().Where(x => queryTypeWorks.Any(y => y.Lot.Id == x.Lot.Id)).Select(x => x.Builder.Id);
                }
                else
                {
                    builderSelector = qualificationDomain.GetAll().Where(x => x.ObjectCr.Id == objectCrId).Select(x => x.Builder.Id);
                }

                if (builderSelector.Any())
                {
                    query = query.Where(x => builderSelector.Contains(x.Id));
                }

                query = query.Filter(loadParams, this.Container);
                return new ListDataResult(query.Order(loadParams).Paging(loadParams), query.Count());
            }
            finally
            {
                this.Container.Release(builderDomain);
                this.Container.Release(lotBidDomain);
                this.Container.Release(qualificationDomain);
            }
        }
    }
}