namespace Bars.Gkh.RegOperator.Interceptors
{
    using System.Linq;
    using B4;
    using B4.Utils;
    using Gkh.Entities;

    public class PaymentAgentInterceptor : EmptyDomainInterceptor<PaymentAgent>
    {
        public override IDataResult BeforeCreateAction(IDomainService<PaymentAgent> service, PaymentAgent entity)
        {
            var uniqueResult = CheckUniqueContractId(service, entity);

            if (!uniqueResult.Success)
            {
                return uniqueResult;
            }

            return base.BeforeCreateAction(service, entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<PaymentAgent> service, PaymentAgent entity)
        {
            var uniqueResult = CheckUniqueContractId(service, entity);

            if (!uniqueResult.Success)
            {
                return uniqueResult;
            }

            return base.BeforeUpdateAction(service, entity);
        }

        private IDataResult CheckUniqueContractId(IDomainService<PaymentAgent> service, PaymentAgent entity)
        {
            var contractIds = service.GetAll()
                .Where(x => x.Id != entity.Id)
                .Select(x => new
                {
                    x.PenaltyContractId,
                    x.SumContractId
                })
                .ToList();

            if (!entity.PenaltyContractId.IsEmpty())
            {
                if (contractIds.Any(x => x.PenaltyContractId == entity.PenaltyContractId)
                    || contractIds.Any(x => x.SumContractId == entity.PenaltyContractId))
                {
                    return Failure("Id договора загрузки пени должен быть уникальным");
                }
            }

            if (!entity.SumContractId.IsEmpty())
            {
                if (contractIds.Any(x => x.PenaltyContractId == entity.SumContractId)
                    || contractIds.Any(x => x.SumContractId == entity.SumContractId))
                {
                    return Failure("Id договора загрузки суммы должен быть уникальным");
                }
            }

            return Success();
        }
    }
}