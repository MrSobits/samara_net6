namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.RegOperator.DomainService.RealityObjectAccount;
    using Bars.Gkh.RegOperator.Entities;

    using Castle.Windsor;

    public class RealityObjectChargeAccountNumbererAction : BaseExecutionAction
    {
        private readonly IWindsorContainer _container;
        private readonly IRealityObjectAccountGenerator _accountGenerator;

        public RealityObjectChargeAccountNumbererAction(
            IWindsorContainer container,
            IRealityObjectAccountGenerator accountGenerator)
        {
            this._container = container;
            this._accountGenerator = accountGenerator;
        }

        public override string Description => "Создание номера Счета для МКД. Для МКД, у которых номер счета не задан";

        public override string Name => "Создание номера Счета для МКД";

        public override Func<IDataResult> Action => this.CreateAccountNumbers;

        private BaseDataResult CreateAccountNumbers()
        {
            return this.InTransaction(
                () =>
                {
                    this.GenerateNumber<RealityObjectChargeAccount>();
                    this.GenerateNumber<RealityObjectPaymentAccount>();
                    this.GenerateNumber<RealityObjectSupplierAccount>();
                    this.GenerateNumber<RealityObjectSubsidyAccount>();
                });
        }

        private void GenerateNumber<T>() where T : PersistentObject, IRealityObjectAccount
        {
            var domain = this._container.ResolveDomain<T>();
            var accs = domain.GetAll()
                .Where(x => x.AccountNumber == null || x.AccountNumber == string.Empty)
                .ToList();

            accs.ForEach(
                x =>
                {
                    x.AccountNumber = this._accountGenerator.GenerateAccountNumber<T>(x.RealityObject);
                    domain.Update(x);
                });
        }

        private BaseDataResult InTransaction(Action action)
        {
            string errorMsg = null;

            using (var transaction = this._container.Resolve<IDataTransaction>())
            {
                try
                {
                    action();

                    transaction.Commit();
                }
                catch (Exception exc)
                {
                    errorMsg = exc.Message;

                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception e)
                    {
                        errorMsg = string.Format("{0}; {1}", errorMsg, e.Message);
                    }
                }
            }

            return new BaseDataResult(errorMsg == null, errorMsg);
        }
    }
}