using System.Linq;
using Bars.B4.Utils;

namespace Bars.GkhCr.Interceptors
{
    using Bars.B4;
    using Bars.GkhCr.Entities;

    public class BuildContractTypeWorkInterceptor : EmptyDomainInterceptor<BuildContractTypeWork>
    {
        public IDomainService<TypeWorkCr> TypeWorkCrDomain { get; set; }

        public override IDataResult BeforeCreateAction(IDomainService<BuildContractTypeWork> service, BuildContractTypeWork entity)
        {
            return this.Success();
            //  return CheckSum(service, entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<BuildContractTypeWork> service, BuildContractTypeWork entity)
        {
            var buildContractTypeWorkSum = service.GetAll()
             .Where(x => x.BuildContract.Id == entity.BuildContract.Id && x.Id != entity.Id)
             .Select(x => x.Sum)
             .Sum();

            if (buildContractTypeWorkSum.ToDecimal() + entity.Sum.ToDecimal() > entity.BuildContract.Sum.ToDecimal())
            {
                return this.Failure("Сумма по работам превышает общую сумму по договору.");
            }

            if (buildContractTypeWorkSum != null)
            {
                entity.BuildContract.Sum = entity.Sum + buildContractTypeWorkSum;
            }
            else entity.BuildContract.Sum = entity.Sum;
            Container.Resolve<IDomainService<BuildContract>>().Update(entity.BuildContract);

            return this.Success();
            //return CheckSum(service, entity);
        }

        private IDataResult CheckSum(IDomainService<BuildContractTypeWork> service, BuildContractTypeWork entity)
        {
            var typeWorkSum = this.TypeWorkCrDomain.GetAll()
                .Where(x => x.Work.Id == entity.TypeWork.Work.Id && x.ObjectCr.Id == entity.BuildContract.ObjectCr.Id)
                .Select(x => x.Sum)
                .Sum();

            var buildContractTypeWorkSum = service.GetAll()
                .Where(x => x.TypeWork.Work.Id == entity.TypeWork.Work.Id && x.BuildContract.ObjectCr.Id == entity.BuildContract.ObjectCr.Id && x.Id != entity.Id)
                .Select(x => x.Sum)
                .Sum();

            if (buildContractTypeWorkSum.ToDecimal() + entity.Sum.ToDecimal() > typeWorkSum.ToDecimal())
            {
                return this.Failure("Введенное значение превышает сумму, указанную в разделе \"Виды работ\"");
            }

            return this.Success();
        }
    }

}