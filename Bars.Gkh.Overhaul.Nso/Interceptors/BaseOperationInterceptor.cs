namespace Bars.Gkh.Overhaul.Nso.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Nso.Entities;
    using Bars.Gkh.Overhaul.Nso.Enum;

    public class BaseOperationInterceptor : EmptyDomainInterceptor<BaseOperation>
    {
        public override IDataResult AfterUpdateAction(IDomainService<BaseOperation> service, BaseOperation entity)
        {
            return ChangeAccountInfo(service, entity);
        }

        public override IDataResult AfterCreateAction(IDomainService<BaseOperation> service, BaseOperation entity)
        {
            return ChangeAccountInfo(service, entity);
        }

        public override IDataResult AfterDeleteAction(IDomainService<BaseOperation> service, BaseOperation entity)
        {
            return ChangeAccountInfo(service, entity);
        }

        private IDataResult ChangeAccountInfo(IDomainService<BaseOperation> service, BaseOperation entity)
        {
            var operations = service.GetAll()
                                   .Where(x => x.BankStatement.Id == entity.BankStatement.Id)
                                   .ToList();

            entity.BankStatement.LastOperationDate = operations.Any() ? operations.Max(x => x.OperationDate) : (DateTime?)null;

            var totalIncome = operations.Any(x => x.Operation.Type == AccountOperationType.Income) ?
                 operations.Where(x => x.Operation.Type == AccountOperationType.Income).Sum(x => x.Sum.ToDecimal()) : 0;

            var totalOut = operations.Any(x => x.Operation.Type == AccountOperationType.Outcome) ?
                 operations.Where(x => x.Operation.Type == AccountOperationType.Outcome).Sum(x => x.Sum.ToDecimal()) : 0;

            entity.BankStatement.BalanceOut = entity.BankStatement.BalanceIncome.ToDecimal() + totalIncome - totalOut;

            return this.Success();
        }
    }
}