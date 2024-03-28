using System.Linq;
using Bars.B4.Utils;

namespace Bars.GkhCr.Interceptors
{
    using Bars.B4;
    using Bars.GkhCr.Entities;

    public class SpecialBuildContractTypeWorkInterceptor : EmptyDomainInterceptor<SpecialBuildContractTypeWork>
    {
        public IDomainService<TypeWorkCr> TypeWorkCrDomain { get; set; }

        public override IDataResult BeforeCreateAction(IDomainService<SpecialBuildContractTypeWork> service, SpecialBuildContractTypeWork entity)
        {
            return this.CheckSum(service, entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<SpecialBuildContractTypeWork> service, SpecialBuildContractTypeWork entity)
        {
            var buildContractTypeWorkSum = service.GetAll()
             .Where(x => x.BuildContract.Id == entity.BuildContract.Id && x.Id != entity.Id)
             .Select(x => x.Sum)
             .Sum();

            if (buildContractTypeWorkSum.ToDecimal() + entity.Sum.ToDecimal() > entity.BuildContract.Sum.ToDecimal())
            {
                return this.Failure("Сумма по работам превышает общую сумму по договору.");
            }

            return this.CheckSum(service, entity);
        }

        private IDataResult CheckSum(IDomainService<SpecialBuildContractTypeWork> service, SpecialBuildContractTypeWork entity)
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