namespace Bars.Gkh.Modules.ClaimWork.Interceptors
{
    using System;
    using System.Linq;
    using B4.DataAccess;
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Repositories;

    /// <summary>
    /// Интерцептор для базовой ПИР
    /// </summary>
    public class BaseClaimWorkInterceptor : BaseClaimWorkInterceptor<BaseClaimWork>
    {
        // Внимание !!! override делать в Generic классе
    }

    // 
    /// <summary>
    /// Интерцептор для базовой ПИР.
    /// <remarks>Generic чтобы лучше наследоватся и заменять в других модулях</remarks>
    /// </summary>
    /// <typeparam name="T">Претензионная работа</typeparam>
    public class BaseClaimWorkInterceptor<T> : EmptyDomainInterceptor<T>
        where T : BaseClaimWork
    {
        /// <summary>
        /// Действие, выполняемое после обновления сущности
        /// </summary>
        /// <param name="service">Домен-сервис</param>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат работы</returns>
        public override IDataResult AfterUpdateAction(IDomainService<T> service, T entity)
        {
            if (entity.IsDebtPaid && entity.DebtPaidDate.HasValue && entity.DebtPaidDate <= DateTime.Now)
            {
                var stateRepo = this.Container.Resolve<IStateRepository>();

                try
                {
                    var finalState = stateRepo.GetAllStates<T>(x => x.FinalState).FirstOrDefault();
                    entity.State = finalState;
                }
                finally
                {
                    this.Container.Release(stateRepo);
                }
            }

            return this.Success();
        }

        #region Overrides of EmptyDomainInterceptor<T>

        /// <summary>
        /// Метод вызывается перед удалением объекта
        /// </summary>
        /// <param name="service">Домен</param><param name="entity">Объект</param>
        /// <returns>
        /// Результат выполнения
        /// </returns>
        public override IDataResult BeforeDeleteAction(IDomainService<T> service, T entity)
        {
            var documentDomain = this.Container.ResolveDomain<DocumentClw>();
            using (this.Container.Using(documentDomain))
            {
                documentDomain.GetAll()
                    .Where(x => x.ClaimWork.Id == entity.Id)
                    .ForEach(
                    x =>
                    {
                        var type = x.GetType();
                        var domainServiceType = typeof(IDomainService<>).MakeGenericType(type);
                        var domainServiceImpl = (IDomainService)this.Container.Resolve(domainServiceType);
                        try
                        {
                            domainServiceImpl.Delete(x.Id);
                        }
                        finally
                        {
                            this.Container.Release(domainServiceImpl);
                        }
                    });
            }

            return base.BeforeDeleteAction(service, entity);
        }

        #endregion
    }
}
