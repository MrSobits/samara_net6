namespace Bars.Gkh.RegOperator.Interceptors.PersonalAccount.PayDoc
{
    using System.Linq;
    using Bars.B4;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;
    using NHibernate.Util;

    internal class PaymentDocumentSnapshotInterceptor : EmptyDomainInterceptor<PaymentDocumentSnapshot>
    {
        private readonly IDomainService<AccountPaymentInfoSnapshot> _accountInfoDmn;

        public PaymentDocumentSnapshotInterceptor(IDomainService<AccountPaymentInfoSnapshot> accountInfoDmn)
        {
            _accountInfoDmn = accountInfoDmn;
        }
        
        /// <summary>
        /// Метод вызывается перед удалением объекта
        /// </summary>
        /// <param name="service">Домен</param><param name="entity">Объект</param>
        /// <returns>
        /// Результат выполнения
        /// </returns>
        public override IDataResult BeforeDeleteAction(IDomainService<PaymentDocumentSnapshot> service, PaymentDocumentSnapshot entity)
        {
            _accountInfoDmn.GetAll().Where(x => x.Snapshot.Id == entity.Id)
                .Select(x => x.Id).ForEach(id => _accountInfoDmn.Delete(id));

            return base.BeforeDeleteAction(service, entity);
        }
    }
}