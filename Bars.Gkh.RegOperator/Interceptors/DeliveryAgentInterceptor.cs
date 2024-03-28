using System.Linq;
using Bars.B4;
using Bars.Gkh.RegOperator.Entities;

namespace Bars.Gkh.RegOperator.Interceptors
{
    using System;

    using Bars.B4.Utils;

    public class DeliveryAgentInterceptor : EmptyDomainInterceptor<DeliveryAgent>
    {
        public override IDataResult BeforeCreateAction(IDomainService<DeliveryAgent> service, DeliveryAgent entity)
        {
            return CheckContragent(service, entity)
                       ? Failure("Для указанного контрагента уже существует агент доставки.")
                       : Success();
        }

        public override IDataResult BeforeUpdateAction(IDomainService<DeliveryAgent> service, DeliveryAgent entity)
        {
            return CheckContragent(service, entity)
                       ? Failure("Для указанного контрагента уже существует агент доставки.")
                       : Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<DeliveryAgent> service, DeliveryAgent entity)
        {
            var delAgentMunicServ = Container.Resolve<IDomainService<DeliveryAgentMunicipality>>();
            var delAgentRoServ = Container.Resolve<IDomainService<DeliveryAgentRealObj>>();

            try
            {
                delAgentMunicServ.GetAll().Where(x => x.DeliveryAgent.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => delAgentMunicServ.Delete(x));

                delAgentRoServ.GetAll().Where(x => x.DeliveryAgent.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => delAgentRoServ.Delete(x));

                return Success();
            }
            catch (Exception)
            {
                return Failure("Не удалось удалить связанные записи");
            }
            finally
            {
                Container.Release(delAgentMunicServ);
                Container.Release(delAgentRoServ);
            }
        }

        private bool CheckContragent(IDomainService<DeliveryAgent> service, DeliveryAgent entity)
        {
            return
                service.GetAll()
                    .Any(x => x.Contragent.Id == entity.Contragent.Id && x.Id != entity.Id);
        }
    }
}