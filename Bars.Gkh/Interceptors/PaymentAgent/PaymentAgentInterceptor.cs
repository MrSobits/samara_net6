namespace Bars.Gkh.Interceptors
{
    using System;
    using System.Linq;

    using B4;

    using Bars.B4.Utils;

    using Entities;

    public class PaymentAgentInterceptor : PaymentAgentInterceptor<PaymentAgent>
    {
        // Внимание !!! все override и ноые методы делать в Generic классе
    }

    public class PaymentAgentInterceptor<T> : EmptyDomainInterceptor<T>
        where T : PaymentAgent
    {
        public override IDataResult BeforeUpdateAction(IDomainService<T> service, T entity)
        {
            if (service.GetAll().Any(x => x.Contragent.Id == entity.Contragent.Id && x.Id != entity.Id))
            {
                return Failure("Платежный агент с таким контрагентом уже создан");
            }

	        if (!entity.SumContractId.IsEmpty() && !entity.PenaltyContractId.IsEmpty())
	        {
		        if (string.Equals(entity.SumContractId, entity.PenaltyContractId, StringComparison.InvariantCultureIgnoreCase))
		        {
			        return Failure(
				        "Id договора загрузки пени - Уникально в рамках системы с учетом значений поля «Id договора загрузки суммы»"
					        + "<br />"
					        + "Id договора загрузки суммы - Уникально в рамках системы с учетом значений поля «Id договора загрузки пени»");
		        }
	        }
	        return Success();
        }

        public override IDataResult BeforeCreateAction(IDomainService<T> service, T entity)
        {
            return BeforeUpdateAction(service, entity);
        }
    }
}