namespace Bars.Gkh.RegOperator.Entities.ValueObjects
{
    using System;

    using Bars.B4.Application;
    using Bars.Gkh.Authentification;
    
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.DomainModelServices;
    using Bars.Gkh.RegOperator.DomainService.PartialOperationCancellation;

    using Enums;

    /// <summary>
    /// Детализация распределения
    /// </summary>
    public class DistributionDetail : BaseImportableEntity
    {
        /// <summary>
        /// Идентификатор сущности
        /// </summary>
        public virtual long EntityId { get; set; }

        /// <summary>
        /// Источник распределения
        /// </summary>
        public virtual DistributionSource Source { get; set; }

        /// <summary>
        /// Получатель денег
        /// </summary>
        public virtual string Object { get; set; }

        /// <summary>
        /// Р/С получателя
        /// </summary>
        public virtual string PaymentAccount { get; set; }

        /// <summary>
        /// Сумма
        /// </summary>
        public virtual decimal Sum { get; set; }

        /// <summary>
        /// Назначение
        /// </summary>
        public virtual string Destination { get; set; }

        /// <summary>
        /// Логин пользователя, который инициировал распределение
        /// </summary>
        public virtual string UserLogin { get; set; }

        private IGkhUserManager userManager;

        private IGkhUserManager UserManager
        {
            get
            {
                return this.userManager ?? (this.userManager = ApplicationContext.Current.Container.Resolve<IGkhUserManager>());
            }
        }

        public DistributionDetail(long entityId, string Object, decimal sum)
        {
            this.EntityId = entityId;
            this.Object = Object;
            this.Sum = sum;
            this.UserLogin = this.UserManager.GetActiveUser()?.Login;
        }

        public DistributionDetail() { }
    }

    internal class DistributionDetailWrapper : ICancelablePayment
    {
        public DistributionDetailWrapper(IDistributable statement, DistributionDetail detail)
        {
            if (detail.EntityId != statement.Id || detail.Source != statement.Source)
            {
                throw new ArgumentException();
            }

            this.Source = detail;
            this.PaymentDate = statement.DateReceipt;
            this.Sum = detail.Sum;
        }

        /// <summary>
        /// Идентификатор
        /// </summary>
        public DistributionDetail Source { get; set; }

        /// <inheritdoc />
        public decimal Sum { get; }

        /// <inheritdoc />
        public DateTime PaymentDate { get; }
    }
}