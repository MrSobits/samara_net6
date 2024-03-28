namespace Bars.GkhRf.Interceptors
{
    using System.Linq;
    using Bars.B4;
    using Bars.GkhRf.Entities;

    using Castle.Windsor;

    public class ContractRfObjectInterceptor : EmptyDomainInterceptor<ContractRfObject>
    {
        public override IDataResult BeforeCreateAction(IDomainService<ContractRfObject> service, ContractRfObject entity)
        {
            var transferRfObjService = Container.Resolve<IDomainService<TransferRfRecObj>>();
            var transferRfRecords = Container.Resolve<IDomainService<TransferRfRecord>>()
                                 .GetAll()
                                 .Where(x => x.TransferRf.ContractRf.Id == entity.ContractRf.Id)
                                 .ToList();

            foreach (var transferRfRecord in transferRfRecords)
            {
                if (entity.IncludeDate < transferRfRecord.TransferDate)
                {
                    transferRfObjService.Save(new TransferRfRecObj { TransferRfRecord = transferRfRecord, RealityObject = entity.RealityObject });
                }
            }

            return Success();
        }

        public override IDataResult BeforeUpdateAction(IDomainService<ContractRfObject> service, ContractRfObject entity)
        {
            var transferRfObjService = Container.Resolve<IDomainService<TransferRfRecObj>>();

            // выбираем все объекты перечисления, договор и жилой дом, которого  совпадает с изменяемым объектом договора
            var transferRfObjs = transferRfObjService.GetAll()
                 .Where(x => x.RealityObject.Id == entity.RealityObject.Id && (x.TransferRfRecord.TransferRf.ContractRf.Id == entity.ContractRf.Id))
                 .Select(x => new
                 {
                     x.Id,
                     x.TransferRfRecord
                 })
                 .ToList();

            // если после изменения месяц перечисления не входит в промежуток включения и исключения объекта, удаляем объект перечисления  
            foreach (var transferRfObj in transferRfObjs)
            {
                if (entity.IncludeDate > transferRfObj.TransferRfRecord.TransferDate || (entity.ExcludeDate.HasValue && entity.ExcludeDate < transferRfObj.TransferRfRecord.TransferDate))
                {
                    transferRfObjService.Delete(transferRfObj.Id);
                }
            }

            // выбираем все перечисления договора в котором производим изменения
            var transferRfRecords = Container.Resolve<IDomainService<TransferRfRecord>>()
                     .GetAll()
                     .Where(x => x.TransferRf.ContractRf.Id == entity.ContractRf.Id)
                     .ToList();

            // если в перечислении нет записи с данным объектом, то проверяем входит ли в промежуток включения и исключения объекта из договора месяц перечисления, если да то добавляем запись
            foreach (var transferRfRecord in transferRfRecords)
            {
                if (transferRfObjs.FirstOrDefault(x => x.TransferRfRecord == transferRfRecord) == null)
                {
                    if (!(entity.IncludeDate > transferRfRecord.TransferDate || (entity.ExcludeDate.HasValue && entity.ExcludeDate < transferRfRecord.TransferDate)))
                    {
                        transferRfObjService.Save(new TransferRfRecObj
                        {
                            Id = 0,
                            TransferRfRecord = transferRfRecord,
                            RealityObject = entity.RealityObject
                        });
                    }
                }
            }

            return Success();
        }
    }
}
