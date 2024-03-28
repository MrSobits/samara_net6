namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Refactor.TransferOwner;
    using Bars.Gkh.RegOperator.Entities.Wallet;
    using Bars.Gkh.RegOperator.Extenstions;

    using FastMember;

    /// <summary>
    /// Действие по созданию кошельков
    /// </summary>
    public class WalletsCreateAction : BaseExecutionAction
    {
        /// <summary>
        /// Поставщик сессий
        /// </summary>
        public ISessionProvider SessionProvider { get; set; }

        /// <inheritdoc />
        public override string Description => "Обновляет лицевые счета, и счета оплат домов, чтобы появились кошельки (в случае их отсутсвия)";

        /// <inheritdoc />
        public override string Name => "РегОператор - Создание кошельков на Лицевых счетах и Счетах оплат домов";

        /// <inheritdoc />
        public override Func<IDataResult> Action => this.Execute;

        private BaseDataResult Execute()
        {
            this.CreateWalletsFor<BasePersonalAccount>();
            this.CreateWalletsFor<RealityObjectPaymentAccount>();

            return new BaseDataResult();
        }

        private void CreateWalletsFor<THolder>() where THolder : ITransferOwner, IEntity
        {
            var repo = this.Container.ResolveRepository<THolder>();
            var sessionProvider = this.Container.Resolve<ISessionProvider>();
            var accessor = TypeAccessor.Create(typeof(THolder), true);

            var parameter = Expression.Parameter(typeof(THolder));
            var props = typeof(THolder).GetProperties().Where(x => x.PropertyType == typeof(Wallet)).ToArray();

            if (props.Length == 0)
            {
                return;
            }

            Expression where = null;

            foreach (var propertyInfo in props)
            {
                var walletExpr = Expression.Property(parameter, propertyInfo);

                if (where == null)
                {
                    where = Expression.Equal(walletExpr, Expression.Constant(null));
                }
                else
                {
                    where = Expression.OrElse(where, Expression.Equal(walletExpr, Expression.Constant(null)));
                }
            }

            var lambda = Expression.Lambda<Func<THolder, bool>>(where, parameter);

            while (true)
            {
                var items = repo.GetAll().Where(lambda).Take(10000).ToList();

                if (!items.Any())
                {
                    break;
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();

                using (var session = sessionProvider.OpenStatelessSession())
                {
                    using (var tr = session.BeginTransaction())
                    {
                        try
                        {
                            foreach (var item in items)
                            {
                                foreach (var walletProp in props)
                                {
                                    var wallet = (Wallet) accessor[item, walletProp.Name];

                                    if (wallet == null)
                                    {
                                        wallet =
                                            (Wallet)
                                                Activator.CreateInstance(
                                                    typeof(Wallet),
                                                    Guid.NewGuid().ToString(),
                                                    item,
                                                    WalletHelper.GetWalletTypeByPropertyName(walletProp.Name));

                                        accessor[item, walletProp.Name] = wallet;
                                    }

                                    if (wallet.Id == 0)
                                    {
                                        session.Insert(wallet);
                                    }
                                }

                                session.Update(item);
                            }

                            tr.Commit();
                        }
                        catch (Exception)
                        {
                            tr.Rollback();
                            throw;
                        }
                    }
                }

                sessionProvider.GetCurrentSession().Clear();
            }
        }
    }
}