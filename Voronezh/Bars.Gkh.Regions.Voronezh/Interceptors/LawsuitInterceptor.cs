namespace Bars.Gkh.Regions.Voronezh.Interceptors
{
    using B4;
    using B4.Modules.States;
    using Bars.Gkh.RegOperator.Entities;
    using Domain;
    using Modules.ClaimWork.Entities;
    using Regions.Voronezh.Entities;
    using System.Collections.Generic;

    /// <summary>
    /// Интерцептор для ПИР
    /// </summary>
    /// <typeparam name="TLawsuit">ПИР</typeparam>
    public class LawsuitInterceptor : EmptyDomainInterceptor<Lawsuit>
    {  //DebtorClaimWork
        public IDomainService<DebtorClaimWork> DebtorClaimWorkDomain { get; set; }

        public IDomainService<Lawsuit> LawsuitDomain { get; set; }

        /// <summary>Метод вызывается перед созданием объекта</summary>
        /// <param name="service">Домен</param>
        /// <param name="entity">Объект</param>
        /// <returns>Результат выполнения</returns>
        public override IDataResult AfterCreateAction(IDomainService<Lawsuit> service, Lawsuit entity)
        {   
            /*var lai = new List<LawsuitAddInfo>();
            var stateProvider = this.Container.Resolve<IStateProvider>();
            lai.Add(new LawsuitAddInfo
            {
                ClaimworkId = (DebtorClaimWork)entity.ClaimWork,
                LawsuitId = entity,
                DutyPostponed = true
            });
            TransactionHelper.InsertInManyTransactions(this.Container,lai);*/
            return this.Success();
        }

        public override IDataResult AfterUpdateAction(IDomainService<Lawsuit> service, Lawsuit entity)
        {
            return base.AfterUpdateAction(service, entity);
        }
        public override IDataResult BeforeUpdateAction(IDomainService<Lawsuit> service, Lawsuit entity)
        {
            var thisLawSuit = LawsuitDomain.Get(entity.Id);

            if (!string.IsNullOrEmpty(thisLawSuit.JudgeName) && string.IsNullOrEmpty(entity.JudgeName))
            {
                entity.JudgeName = thisLawSuit.JudgeName;
                entity.NumberCourtBuisness = thisLawSuit.NumberCourtBuisness;
            }

          

            return this.Success();
        }
    }
}