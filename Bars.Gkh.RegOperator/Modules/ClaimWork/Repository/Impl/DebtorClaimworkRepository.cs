namespace Bars.Gkh.RegOperator.Modules.ClaimWork.Repository.Impl
{
    using System.Linq;
    using Bars.B4.DataAccess;
    using Bars.Gkh.RegOperator.Entities;

    public class DebtorClaimworkRepository : IDebtorClaimworkRepository
    {
        private readonly IRepository<DebtorClaimWork> repo;

        public DebtorClaimworkRepository(IRepository<DebtorClaimWork> repo)
        {
            this.repo = repo;
        }

        public IQueryable<DebtorClaimWork> GetByOwnerId(long ownerId)
        {
            return this.repo.GetAll().Where(x => x.AccountOwner.Id == ownerId);
        }
    }
}