namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.RegOperator.DomainService.RealityObjectAccount;

    using Castle.Windsor;

    public class RoAccountsGeneratorAction : BaseExecutionAction
    {
        private readonly IDomainService<RealityObject> _roDomain;
        private readonly IRealityObjectAccountGenerator _generator;
        private readonly IWindsorContainer _container;

        public RoAccountsGeneratorAction(
            IDomainService<RealityObject> roDomain,
            IRealityObjectAccountGenerator generator,
            IWindsorContainer container)
        {
            this._roDomain = roDomain;
            this._generator = generator;
            this._container = container;
        }

        public override string Description => "Создание счетов домов (начислений, оплат, расчета с поставщиками)";

        public override string Name => "Создание счетов домов";

        public override Func<IDataResult> Action => this.CreateAccounts;

        private BaseDataResult CreateAccounts()
        {
            var sessionProvider = this.Container.Resolve<ISessionProvider>();

            var allRo = this._roDomain.GetAll().OrderBy(x => x.Id);

            var totalCount = allRo.Count();

            var take = 1000;

            var session = sessionProvider.GetCurrentSession();

            for (int skip = 0; skip < totalCount; skip += take)
            {
                var ros = allRo.Skip(skip).Take(take);

                var error = this.CreateChargeAccounts(ros);

                if (error.IsNotEmpty())
                {
                    return new BaseDataResult(false, error);
                }

                error = this.CreatePaymentAccounts(ros);

                if (error.IsNotEmpty())
                {
                    return new BaseDataResult(false, error);
                }

                error = this.CreateSupplierAccounts(ros);

                if (error.IsNotEmpty())
                {
                    return new BaseDataResult(false, error);
                }

                error = this.CreateSubsidyAccounts(ros);

                if (error.IsNotEmpty())
                {
                    return new BaseDataResult(false, error);
                }

                session.Clear();
            }

            return new BaseDataResult();
        }

        private string CreateChargeAccounts(IQueryable<RealityObject> allRo)
        {
            string error = string.Empty;
            this._container.UsingForResolved<IDataTransaction>(
                (c, tr) =>
                {
                    try
                    {
                        this._generator.GenerateChargeAccounts(allRo);
                        tr.Commit();
                    }
                    catch (Exception e)
                    {
                        error = string.Format("Ошибка при создании счетов начислений: {0}", e.Message);
                    }
                });

            return error;
        }

        private string CreatePaymentAccounts(IQueryable<RealityObject> allRo)
        {
            string error = string.Empty;
            this._container.UsingForResolved<IDataTransaction>(
                (c, tr) =>
                {
                    try
                    {
                        this._generator.GeneratePaymentAccounts(allRo);
                        tr.Commit();
                    }
                    catch (Exception e)
                    {
                        error = string.Format("Ошибка при создании счетов оплат: {0}", e.Message);
                    }
                });

            return error;
        }

        private string CreateSupplierAccounts(IQueryable<RealityObject> allRo)
        {
            string error = string.Empty;
            this._container.UsingForResolved<IDataTransaction>(
                (c, tr) =>
                {
                    try
                    {
                        this._generator.GenerateSupplierAccounts(allRo);
                        tr.Commit();
                    }
                    catch (Exception e)
                    {
                        error = string.Format("Ошибка при создании счетов расчтеа с поставщиками: {0}", e.Message);
                    }
                });

            return error;
        }

        private string CreateSubsidyAccounts(IQueryable<RealityObject> allRo)
        {
            string error = string.Empty;
            this._container.UsingForResolved<IDataTransaction>(
                (c, tr) =>
                {
                    try
                    {
                        this._generator.GenerateSubsidyAccounts(allRo);
                        tr.Commit();
                    }
                    catch (Exception e)
                    {
                        error = string.Format("Ошибка при создании счетов субсидий: {0}", e.Message);
                    }
                });

            return error;
        }
    }
}