using Bars.Gkh.Overhaul.Enum;

namespace Bars.Gkh.Overhaul.Nso.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Nso.Entities;
    using Bars.Gkh.Overhaul.Nso.Entities;
    using Bars.Gkh.Overhaul.Nso.Enum;

    public class AccBankStatementInterceptor : EmptyDomainInterceptor<AccBankStatement>
    {
        public override IDataResult BeforeCreateAction(IDomainService<AccBankStatement> service, AccBankStatement entity)
        {
            var stateProvider = Container.Resolve<IStateProvider>();
            stateProvider.SetDefaultState(entity);

            if (entity.BankAccount != null)
            {
                var prevBalanceOut =
                    service.GetAll()
                           .Where(x => x.BankAccount.Id == entity.BankAccount.Id && x.DocumentDate < entity.DocumentDate)
                           .OrderByDescending(x => x.DocumentDate)
                           .Select(x => x.BalanceOut)
                           .FirstOrDefault();

                entity.BalanceIncome = prevBalanceOut.ToDecimal();
                entity.BalanceOut = prevBalanceOut.ToDecimal();
            }
            else
            {
                entity.BalanceIncome = 0;
                entity.BalanceOut = 0;
            }

            return this.Success();
        }

        public override IDataResult AfterUpdateAction(IDomainService<AccBankStatement> service, AccBankStatement entity)
        {
            return ChangeAccountInfo(service, entity);
        }

        public override IDataResult AfterCreateAction(IDomainService<AccBankStatement> service, AccBankStatement entity)
        {
            return ChangeAccountInfo(service, entity);
        }

        public override IDataResult AfterDeleteAction(IDomainService<AccBankStatement> service, AccBankStatement entity)
        {
            return ChangeAccountInfo(service, entity);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<AccBankStatement> service, AccBankStatement entity)
        {
            if (Container.Resolve<IDomainService<BaseOperation>>().GetAll().Any(x => x.BankStatement.Id == entity.Id))
            {
                return Failure("В банковской выписке существуют платежные поручения. Удаление невозможно.");
            }

            return Success();
        }

        private IDataResult ChangeAccountInfo(IDomainService<AccBankStatement> service, AccBankStatement entity)
        {

            //if (entity.BankAccount != null && entity.BankAccount.AccountType == AccountType.CalcAccount)
            //{
            //    var calcAccountServ = Container.Resolve<IDomainService<RegOpCalcAccount>>();
            //    var operAccountServ = Container.Resolve<IDomainService<BaseOperation>>();

            //    var calcAccount = calcAccountServ.Load(entity.BankAccount.Id);

            //    var operations = operAccountServ.GetAll().Where(x => x.BankStatement.BankAccount.Id == entity.BankAccount.Id).ToList();

            //    calcAccount.LastOperationDate = operations.Any()
            //                                                 ? operations.Max(x => x.OperationDate)
            //                                                 : (DateTime?)null;

            //    calcAccount.TotalIncome = operations.Any(x => x.Operation.Type == AccountOperationType.Income)
            //                          ? operations.Where(x => x.Operation.Type == AccountOperationType.Income)
            //                                      .Sum(x => x.Sum.ToDecimal())
            //                          : 0;

            //    calcAccount.TotalOut = operations.Any(x => x.Operation.Type == AccountOperationType.Outcome)
            //                       ? operations.Where(x => x.Operation.Type == AccountOperationType.Outcome)
            //                                   .Sum(x => x.Sum.ToDecimal())
            //                       : 0;

            //    calcAccount.BalanceOut = service.GetAll().OrderByDescending(x => x.DocumentDate).First().BalanceOut;

            //    calcAccountServ.Update(calcAccount);
            //}

            return this.Success();
        }
    }
}