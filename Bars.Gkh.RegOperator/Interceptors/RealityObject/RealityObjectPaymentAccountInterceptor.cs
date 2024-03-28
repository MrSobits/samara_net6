namespace Bars.Gkh.RegOperator.Interceptors
{
    using System.Linq;
    using B4.DataAccess;
    using B4;

    using Entities;
    using Entities.Wallet;
    using FastMember;
    using NHibernate;

    public class RealityObjectPaymentAccountInterceptor : EmptyDomainInterceptor<RealityObjectPaymentAccount>
    {
        private TypeAccessor _accessor;
        private FlushMode _oldFlush;
        public IDomainService<Wallet> WalletDomain { get; set; }
        public ISessionProvider Sessions { get; set; }

        public RealityObjectPaymentAccountInterceptor()
        {
            _accessor = TypeAccessor.Create(typeof(RealityObjectPaymentAccount));
        }

        /// <summary>
        /// Метод вызывается перед созданием объекта
        /// </summary>
        /// <param name="service">Домен</param><param name="entity">Объект</param>
        /// <returns>
        /// Результат выполнения
        /// </returns>
        public override IDataResult BeforeCreateAction(IDomainService<RealityObjectPaymentAccount> service, RealityObjectPaymentAccount entity)
        {
            _oldFlush = Sessions.GetCurrentSession().FlushMode;
            Sessions.GetCurrentSession().FlushMode = FlushMode.Never;

            CreateWalletsIfNeeded(entity);

            return base.BeforeCreateAction(service, entity);
        }

        /// <summary>
        /// Метод вызывается после создания объекта
        /// </summary>
        /// <param name="service">Домен</param><param name="entity">Объект</param>
        /// <returns>
        /// Результат выполнения
        /// </returns>
        public override IDataResult AfterCreateAction(IDomainService<RealityObjectPaymentAccount> service, RealityObjectPaymentAccount entity)
        {
            Sessions.GetCurrentSession().FlushMode = _oldFlush;

            return base.AfterCreateAction(service, entity);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<RealityObjectPaymentAccount> service, RealityObjectPaymentAccount entity)
        {
            var realityObjectPaymentAccountOperationDomain = Container.Resolve<IDomainService<RealityObjectPaymentAccountOperation>>();

            try
            {
                if (realityObjectPaymentAccountOperationDomain.GetAll().Count(x => x.Account == entity) > 0)
                {
                    return Failure("Существуют зависимые записи: Операции по счету оплат дома");
                }
            }
            finally
            {
                Container.Release(realityObjectPaymentAccountOperationDomain);
            }

            return this.Success();
        }

        private void CreateWalletsIfNeeded(RealityObjectPaymentAccount entity)
        {
            using (var tr = Container.Resolve<IDataTransaction>())
            {
                foreach (var walletProp in _accessor.GetMembers())
                {
                    var wallet = _accessor[entity, walletProp.Name] as Wallet;
                    if (wallet != null && wallet.Id == 0)
                    {
                        WalletDomain.Save(wallet);
                    }
                }

                try
                {
                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                    throw;
                }
            }
        }
    }
}