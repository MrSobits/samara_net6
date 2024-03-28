namespace Bars.GkhCr.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Extensions;
    using Bars.GkhCr.Entities;

    using Castle.Windsor;

    public class SpecialContractCrService : ISpecialContractCrService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddTypeWorks(BaseParams baseParams)
        {
            var contractCrTypeWorkDomain = this.Container.ResolveDomain<SpecialContractCrTypeWork>();
            var contractCrDomain = this.Container.ResolveDomain<SpecialContractCr>();
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
                        var newContractCrTypeWork = new SpecialContractCrTypeWork
                        {
                            ContractCr = contractCrDomain.Load(contractCrId),
                            TypeWork = new SpecialTypeWorkCr { Id = id}
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