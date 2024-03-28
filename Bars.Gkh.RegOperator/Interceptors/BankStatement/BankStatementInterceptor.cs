namespace Bars.Gkh.RegOperator.Interceptors
{
    using System;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Utils;
    
    using Bars.Gkh.Authentification;
    using Bars.Gkh.DomainEvent.Events;
    using Bars.Gkh.DomainEvent.Infrastructure;

    using Castle.Core.Internal;

    using Entities;
    using Enums;

    using ServiceStack.Common;

    public class BankStatementInterceptor : EmptyDomainInterceptor<BankAccountStatement>
    {
        /// <summary>
        /// Метод вызывается перед созданием объекта
        /// </summary>
        /// <param name="service">Домен</param><param name="entity">Объект</param>
        /// <returns>
        /// Результат выполнения
        /// </returns>
        public override IDataResult BeforeCreateAction(IDomainService<BankAccountStatement> service, BankAccountStatement entity)
        {
            entity.OperationDate = DateTime.Today;
            entity.DistributeState = DistributionState.NotDistributed;

            //Прежде всего для создания через UI, где DateReceipt не вводится
            if (entity.DateReceipt == DateTime.MinValue)
            {
                entity.DateReceipt = entity.DocumentDate;
            }

            var firstResult = SetContragents(entity);
            if (!firstResult.Success)
            {
                return firstResult;
            }

            var secondResult = this.SetLogin(entity);
            if (!secondResult.Success)
            {
                return secondResult;
            }

            return this.Success();
        }

        /// <inheritdoc />
        public override IDataResult AfterCreateAction(IDomainService<BankAccountStatement> service, BankAccountStatement entity)
        {
            DomainEvents.Raise(new GeneralStateChangeEvent(entity, null, DistributionState.NotDistributed));
            return base.AfterCreateAction(service, entity);
        }

        /// <summary>
        /// Метод вызывается перед обновлением объекта
        /// </summary>
        /// <param name="service">Домен</param><param name="entity">Объект</param>
        /// <returns>
        /// Результат выполнения
        /// </returns>
        public override IDataResult BeforeUpdateAction(IDomainService<BankAccountStatement> service, BankAccountStatement entity)
        {
            var firstResult = SetContragents(entity);
            if (!firstResult.Success)
            {
                return firstResult;
            }

            var secondResult = this.SetLogin(entity);
            if (!secondResult.Success)
            {
                return secondResult;
            }

            return this.Success();
        }

        /// <summary>
        /// Метод вызывается перед удалением объекта
        /// </summary>
        /// <param name="service">Домен</param><param name="entity">Объект</param>
        /// <returns>
        /// Результат выполнения
        /// </returns>
        public override IDataResult BeforeDeleteAction(IDomainService<BankAccountStatement> service, BankAccountStatement entity)
        {
            if (entity.DistributeState != DistributionState.Deleted)
            {
                return this.Failure("Удалять можно только записи в статусе \"Удален\"");
            }

            var linkDomain = this.Container.ResolveDomain<BankAccountStatementLink>();

            linkDomain.GetAll()
                .Where(x => x.Statement.Id == entity.Id)
                .Select(x => x.Id)
                .ForEach(x => linkDomain.Delete(x));

            return this.Success();
        }

        private IDataResult SetLogin(BankAccountStatement entity)
        {
            var userManager = this.Container.Resolve<IGkhUserManager>();

            try
            {
                var login = userManager.GetActiveUser()?.Login;
                entity.UserLogin = login.IsNullOrEmpty() ? "anonymous" : login;
            }
            catch (Exception exception)
            {
                return this.Failure(exception.Message);
            }
            finally
            {
                this.Container.Release(userManager);
            }

            return this.Success();
        }

        private IDataResult SetContragents(BankAccountStatement entity)
        {
            var contragentService = this.Container.Resolve<IDomainService<Gkh.Entities.Contragent>>();

            try
            {
                if (entity.Payer == null && !string.IsNullOrEmpty(entity.PayerInn))
                {
                    entity.Payer = contragentService.GetAll().Where(x => x.Inn == entity.PayerInn).FirstOrDefault();
                }

                if (entity.Recipient == null && !string.IsNullOrEmpty(entity.RecipientInn))
                {
                    entity.Recipient = contragentService.GetAll().Where(x => x.Inn == entity.RecipientInn).FirstOrDefault();
                }
            }
            catch(Exception exception)
            {
                return this.Failure(exception.Message);
            }
            finally
            {
                this.Container.Release(contragentService);
            }

            return this.Success();
        }
    }
}