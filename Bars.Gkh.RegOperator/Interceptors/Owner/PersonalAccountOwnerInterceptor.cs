namespace Bars.Gkh.RegOperator.Interceptors
{
    using System.Linq;
    using B4;

    using Bars.B4.DataAccess;
    using Bars.Gkh.DomainEvent.Infrastructure;
    using Bars.Gkh.RegOperator.DomainEvent;
    using Bars.Gkh.RegOperator.DomainEvent.Events.PersonalAccountDto;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;

    using Entities;
    using NHibernate.Util;

    /// <summary>
    /// Интерцептор для сущности Абонент
    /// </summary>
    public class PersonalAccountOwnerInterceptor : PersonalAccountOwnerInterceptor<PersonalAccountOwner>
    {
    }

    /// <summary>
    /// Базовый интерцептор для сущности Абонент
    /// </summary>
    public class PersonalAccountOwnerInterceptor<T> : EmptyDomainInterceptor<T>
        where T : PersonalAccountOwner
    {
        /// <summary>
        /// Домен-сервис лицевых счетов
        /// </summary>
        public IDomainService<BasePersonalAccount> BasePersonalAccountDomainService { get; set; }

        /// <summary>
        /// Сервис абонентов
        /// </summary>
        public IPersonalAccountOwnerService PersonalAccountOwnerService { get; set; }

        /// <summary>
        /// Метод вызывается перед созданием объекта
        /// </summary>
        /// <param name="service">Домен</param><param name="entity">Объект</param>
        /// <returns>
        /// Результат выполнения
        /// </returns>
        public override IDataResult BeforeCreateAction(IDomainService<T> service, T entity)
        {
            var result = base.BeforeCreateAction(service, entity);

            if (result.Success)
            {
                this.PersonalAccountOwnerService.UpdateName(entity);
            }

            return result;
        }

        /// <summary>
        /// Действие, выполняемое до обновления сущности
        /// </summary>
        /// <param name="service">Домен-сервис</param>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат операции</returns>
        public override IDataResult BeforeUpdateAction(IDomainService<T> service, T entity)
        {
            var result = base.BeforeUpdateAction(service, entity);

            if (result.Success)
            {
                this.PersonalAccountOwnerService.OnUpdateOwner(entity);
            }

            return result;
        }

        /// <summary>
        /// Действие, выполняемое до удаления сущности
        /// </summary>
        /// <param name="service">Домен-сервис</param>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат операции</returns>
        public override IDataResult BeforeDeleteAction(IDomainService<T> service, T entity)
        {
            var result = base.BeforeDeleteAction(service, entity);

            if (result.Success)
            {
                var hasAccounts = this.BasePersonalAccountDomainService.GetAll()
                    .Any(x => x.AccountOwner.Id == entity.Id);

                if (hasAccounts)
                {
                    result = this.Failure("У данного абонента существуют лицевые счета. Удаление невозможно!");
                }

                var accountOwnershipHistoryDomain = this.Container.ResolveDomain<AccountOwnershipHistory>();
                try
                {
                    accountOwnershipHistoryDomain.GetAll()
                        .Where(x => x.AccountOwner.Id == entity.Id)
                        .Select(x => x.Id)
                        .ForEach(x => accountOwnershipHistoryDomain.Delete(x));
                }
                finally
                {
                    this.Container.Release(accountOwnershipHistoryDomain);
                }
            }

            return result;
        }

        /// <inheritdoc />
        public override IDataResult AfterUpdateAction(IDomainService<T> service, T entity)
        {
            DomainEvents.Raise(new PersonalAccountOwnerUpdateEvent(entity));
            return base.AfterUpdateAction(service, entity);
        }
    }
}