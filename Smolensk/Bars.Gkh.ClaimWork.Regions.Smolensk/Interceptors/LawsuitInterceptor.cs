namespace Bars.Gkh.ClaimWork.Regions.Smolensk.Interceptors
{
    using B4;

    using Bars.Gkh.RegOperator.Entities;

    using Modules.ClaimWork.Entities;

    using Bars.Gkh.Authentification;

    /// <summary>
    /// Интерцептор для исковых заявлений
    /// </summary>
    /// <typeparam name="TLawsuit">Тип искового заявления</typeparam>
    public class LawsuitInterceptorSml<TLawsuit> : EmptyDomainInterceptor<TLawsuit>
        where TLawsuit : Lawsuit
    {
        public IDomainService<DebtorClaimWork> DebtorClaimWorkDomain { get; set; }
      
        /// <summary>Метод вызывается после обновления объекта</summary>
        /// <param name="service">Домен</param>
        /// <param name="entity">Объект</param>
        /// <returns>Результат выполнения</returns>
        public override IDataResult AfterUpdateAction(IDomainService<TLawsuit> service, TLawsuit entity)
        {
            var debtor = this.GetDebtor(entity);
            // присваиваем пользователя, который сохранил изменения
            debtor.User =  this.Container.Resolve<IGkhUserManager>().GetActiveUser();
            this.DebtorClaimWorkDomain.Update(debtor);
            return this.Success();
        }

        private DebtorClaimWork GetDebtor(TLawsuit entity)
        {
            return this.DebtorClaimWorkDomain.Load(entity.ClaimWork.Id);
        }
    }
}