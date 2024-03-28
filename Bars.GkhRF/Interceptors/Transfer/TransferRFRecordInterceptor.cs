namespace Bars.GkhRf.Interceptors
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.GkhRf.Entities;
    using Bars.GkhRf.Enums;

    public class TransferRfRecordInterceptor : EmptyDomainInterceptor<TransferRfRecord>
    {
        public override IDataResult BeforeCreateAction(IDomainService<TransferRfRecord> service, TransferRfRecord entity)
        {
            if (entity.TransferDate >= entity.TransferRf.ContractRf.DateBegin)
            {
                // Перед сохранением проставляем начальный статус
                var stateProvider = this.Container.Resolve<IStateProvider>();
                stateProvider.SetDefaultState(entity);
            }
            else
            {
                throw new ValidationException("Дата перечисления должна быть больше даты начала договора");
            }

            return Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<TransferRfRecord> service, TransferRfRecord entity)
        {
            var transferRfRecordService = Container.Resolve<IDomainService<TransferRfRecObj>>();
            var list = transferRfRecordService.GetAll().Where(x => x.TransferRfRecord.Id == entity.Id).Select(x => x.Id).ToList();
            foreach (var value in list)
            {
                transferRfRecordService.Delete(value);
            }

            return this.Success();
        }

        public override IDataResult AfterCreateAction(IDomainService<TransferRfRecord> service, TransferRfRecord entity)
        {
            // Сохраняем для создаваемой сущности дочерние объекты по каждому дому из договора, где дата включения дома в договор <= месяцу перечисления
            var contractRfObjectService = this.Container.Resolve<IDomainService<ContractRfObject>>();

            if (!entity.TransferDate.HasValue)
            {
                return Success();
            }

            var contractRfObjects = contractRfObjectService
                .GetAll()
                .Where(x => x.ContractRf.Id == entity.TransferRf.ContractRf.Id
                            && (x.IncludeDate.HasValue && x.IncludeDate.Value <= entity.TransferDate.Value)
                            && (x.TypeCondition == TypeCondition.Include || (x.ExcludeDate.Value >= entity.TransferDate.Value && x.TypeCondition == TypeCondition.Exclude)))
                .ToList();

            var transferRfRecObjService = this.Container.Resolve<IDomainService<TransferRfRecObj>>();

            foreach (var contractRfObject in contractRfObjects)
            {
                var transferRfRecObj = new TransferRfRecObj
                {
                    TransferRfRecord = entity,
                    RealityObject = contractRfObject.RealityObject
                };
                transferRfRecObjService.Save(transferRfRecObj);
            }

            return Success();
        }
    }
}
