namespace Bars.GkhCr.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Extensions;
    using Bars.GkhCr.Entities;

    using Castle.Windsor;

    public class ContractCrService : IContractCrService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddTypeWorks(BaseParams baseParams)
        {
            var contractCrTypeWorkDomain = this.Container.ResolveDomain<ContractCrTypeWork>();
            var contractCrDomain = this.Container.ResolveDomain<ContractCr>();
            try
            {
                var contractCrId = baseParams.Params.GetAs<long>("contractCrId");

                var objectIds = baseParams.Params.GetAs<long[]>("objectIds").ToList();

                var exsistingTypeWorks = contractCrTypeWorkDomain.GetAll().Where(x => x.ContractCr.Id == contractCrId).Select(x => x.TypeWork.Id)
                    .ToList();

                return this.Container.InTransactionWithResult(() =>
                {
                    foreach (var id in objectIds.Except(exsistingTypeWorks))
                    {
                        var newContractCrTypeWork = new ContractCrTypeWork
                        {
                            ContractCr = contractCrDomain.Load(contractCrId),
                            TypeWork = new TypeWorkCr {Id = id}
                        };

                        contractCrTypeWorkDomain.Save(newContractCrTypeWork);
                    }
                    return new BaseDataResult();
                });

            }
            catch (ValidationException exc)
            {
                return BaseDataResult.Error(exc.Message);
            }
            finally
            {
                this.Container.Release(contractCrTypeWorkDomain);
                this.Container.Release(contractCrDomain);
            }
        }
    }
}