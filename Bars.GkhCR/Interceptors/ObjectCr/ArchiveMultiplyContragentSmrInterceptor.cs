namespace Bars.GkhCr.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.GkhCr.DomainService;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;

    using Castle.Windsor;

    public class ArchiveMultiplyContragentSmrInterceptor : EmptyDomainInterceptor<ArchiveMultiplyContragentSmr>
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<TypeWorkCr> TypeWorkCr { get; set; }

        public override IDataResult BeforeCreateAction(IDomainService<ArchiveMultiplyContragentSmr> service, ArchiveMultiplyContragentSmr entity)
        {
            var percentSum = service.GetAll()
                .Where(x => x.TypeWorkCr.Id == entity.TypeWorkCr.Id)
                .SafeSum(x => x.PercentOfCompletion);

            var costSum = service.GetAll()
                .Where(x => x.TypeWorkCr.Id == entity.TypeWorkCr.Id)
                .SafeSum(x => x.CostSum);

            var planSum = TypeWorkCr.GetAll()
                .Where(x => x.Id == entity.TypeWorkCr.Id)
                .Select(x => x.Sum).FirstOrDefault();

            var planVolume = TypeWorkCr.GetAll()
                .Where(x => x.Id == entity.TypeWorkCr.Id)
                .Select(x => x.Volume).FirstOrDefault();

            var volumeSum = service.GetAll()
                .Where(x => x.TypeWorkCr.Id == entity.TypeWorkCr.Id)
                .SafeSum(x => x.VolumeOfCompletion);

            //if (planSum < costSum + entity.CostSum)
            //{
            //    return Failure("Сумма расходов не может превышать плановую");
            //}

            if (entity.PercentOfCompletion > 100)
            {
                return Failure("Процент выполнения не может превышать 100");
            }

            //if (planVolume < volumeSum + entity.VolumeOfCompletion)
            //{
            //    return Failure("Объем не может превышать плановый");
            //}

            return Success();
        }

        public override IDataResult BeforeUpdateAction(IDomainService<ArchiveMultiplyContragentSmr> service, ArchiveMultiplyContragentSmr entity)
        {
            var percentSum = service.GetAll()
                .Where(x => x.TypeWorkCr.Id == entity.TypeWorkCr.Id)
                .SafeSum(x => x.PercentOfCompletion);

            var costSum = service.GetAll()
                .Where(x => x.TypeWorkCr.Id == entity.TypeWorkCr.Id)
                .SafeSum(x => x.CostSum);

            var planSum = TypeWorkCr.GetAll()
                .Where(x => x.Id == entity.TypeWorkCr.Id)
                .Select(x => x.Sum).FirstOrDefault();

            var planVolume = TypeWorkCr.GetAll()
                .Where(x => x.Id == entity.TypeWorkCr.Id)
                .Select(x => x.Volume).FirstOrDefault();

            var volumeSum = service.GetAll()
                .Where(x => x.TypeWorkCr.Id == entity.TypeWorkCr.Id)
                .SafeSum(x => x.VolumeOfCompletion);

            if (planSum < costSum)
            {
                return Failure("Сумма расходов не может превышать плановую");
            }

            if (percentSum > 100)
            {
                return Failure("Сумма процентов в сумме не может превышать 100");
            }

            if (planVolume < volumeSum)
            {
                return Failure("Объем не может превышать плановый");
            }

            return Success();
        }
    }
}