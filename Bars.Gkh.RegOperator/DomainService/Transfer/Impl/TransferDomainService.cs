namespace Bars.Gkh.RegOperator.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;

    /// <summary>
    /// Домен-сервис трансферов
    /// </summary>
    public class TransferDomainService : BaseDomainService<Transfer>, ITransferDomainService
    {
        private readonly object syncRoot = new object();
        private IDictionary<Type, IDomainService> domainServices; 

        /// <summary>
        /// Домен-сервис <see cref="RealityObjectTransfer"/>
        /// </summary>
        public IDomainService<RealityObjectTransfer> RealityObjectTransferDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="PersonalAccountPaymentTransfer"/>
        /// </summary>
        public IDomainService<PersonalAccountPaymentTransfer> PersonalAccountPaymentTransferDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="PersonalAccountChargeTransfer"/>
        /// </summary>
        public IDomainService<PersonalAccountChargeTransfer> PersonalAccountChargeTransferDomain { get; set; }

        /// <inheritdoc />
        [Obsolete("Use implicit transfer type domain service or Generic GetAll<T>")]
        public override IQueryable<Transfer> GetAll()
        {
            throw new InvalidOperationException("Use implicit transfer type domain service or Generic GetAll<T>");
        }

        /// <inheritdoc />
        [Obsolete("Use implicit transfer type domain service or Generic Get<T>")]
        public override Transfer Get(object id)
        {
            throw new InvalidOperationException("Use implicit transfer type domain service or Generic Get<T>");
        }

        /// <inheritdoc />
        public override void Save(Transfer value)
        {
            this.GetDomainService(value).Save(value);
        }

        /// <inheritdoc />
        public override void Update(Transfer value)
        {
            this.GetDomainService(value).Update(value);
        }
         
        /// <inheritdoc />
        public IQueryable<TTransfer> GetAll<TTransfer>() where TTransfer : Transfer
        {
            return this.GetDomainService<TTransfer>().GetAll();
        }

        /// <inheritdoc />
        public TTransfer Get<TTransfer>(object id) where TTransfer : Transfer
        {
            return this.GetDomainService<TTransfer>().Get(id);
        }

        private IDomainService<TTransfer> GetDomainService<TTransfer>() where TTransfer : Transfer
        {
            this.EnsureInitialized();
            return (IDomainService<TTransfer>)this.domainServices[typeof(TTransfer)];
        }

        private IDomainService GetDomainService(Transfer transfer)
        {
            this.EnsureInitialized();
            return this.domainServices[transfer.GetType()];
        }

        private void EnsureInitialized()
        {
            lock (this.syncRoot)
            {
                if (this.domainServices.IsEmpty())
                {
                    this.domainServices = new Dictionary<Type, IDomainService>
                    {
                        { typeof(RealityObjectTransfer), this.RealityObjectTransferDomain },
                        { typeof(PersonalAccountPaymentTransfer), this.PersonalAccountPaymentTransferDomain },
                        { typeof(PersonalAccountChargeTransfer), this.PersonalAccountChargeTransferDomain }
                    };
                }
            }
        }
    }
}