using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.IoC;
using Bars.Gkh.Modules.ClaimWork.Interceptors;
using Bars.Gkh.RegOperator.Entities;
using Bars.Gkh.Repositories;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Bars.Gkh.Regions.Voronezh.Interceptors
{
    using Bars.Gkh.Regions.Voronezh.Controllers;

    public class DebtorClaimWorkInterceptor<T> : BaseClaimWorkInterceptor<T> where T : DebtorClaimWork
    {
        public override IDataResult AfterUpdateAction(IDomainService<T> service, T entity)
        {
            if (entity.IsDebtPaid && entity.DebtPaidDate.HasValue && entity.DebtPaidDate <= DateTime.Today)
            {
                var stateRepo = this.Container.Resolve<IStateRepository>();
                using (this.Container.Using(stateRepo))
                {
                    var finalState = stateRepo.GetAllStates<DebtorClaimWork>(x => x.FinalState).Take(2).ToList();
                    if (finalState.Count != 1)
                    {
                        return BaseDataResult.Error("Не найден конечный статус");
                    }

                    entity.State = finalState.First();
                }
            }

            return this.Success();
        }

        //TODO: Переписать
        public override IDataResult BeforeDeleteAction(IDomainService<T> service, T entity)
        {
            var domainService = this.Container.ResolveDomain<ClaimWorkAccountDetail>();
            var claimWorkController = new ArchivedClaimWorkController { Container = this.Container };
            claimWorkController.CreateArchiveEntries(entity.Id, true);
            using (this.Container.Using())
            {
                IQueryable<ClaimWorkAccountDetail> all = domainService.GetAll();
                Expression<Func<ClaimWorkAccountDetail, bool>> predicate = (Expression<Func<ClaimWorkAccountDetail, bool>>)(x => x.ClaimWork.Id == entity.Id);
                foreach (ClaimWorkAccountDetail workAccountDetail in all.Where<ClaimWorkAccountDetail>(predicate).ToList<ClaimWorkAccountDetail>())
                    domainService.Delete((object)workAccountDetail.Id);
            }
            return base.BeforeDeleteAction(service, entity);
        }
    }
}
