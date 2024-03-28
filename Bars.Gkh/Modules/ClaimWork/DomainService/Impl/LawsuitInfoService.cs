namespace Bars.Gkh.Modules.ClaimWork.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4.DataAccess;
    using B4.IoC;
    using Entities;
    using Castle.Windsor;

    /// <summary>
    /// </summary>
    public class LawsuitInfoService : ILawsuitInfoService
    {
        private readonly IWindsorContainer container;

        /// <summary>
        /// </summary>
        /// <param name="container"></param>
        public LawsuitInfoService(IWindsorContainer container)
        {
            this.container = container;
        }

        public Dictionary<long, DateTime> GetReviewDate(IQueryable<BaseClaimWork> claimWorkQuery)
        {
            var petitionRepo = container.ResolveRepository<Petition>();
            var courtOrderRepo = container.ResolveRepository<CourtOrderClaim>();
            var lawSuitRepo = container.ResolveRepository<Lawsuit>();

            using (container.Using(petitionRepo, courtOrderRepo, lawSuitRepo))
            {
                // Объединяем с завялениями о выдаче по основаниям, 
                // для которых нет искового завяления

                var petitionQuery = petitionRepo.GetAll()
                    .Where(x => claimWorkQuery.Any(y => y.Id == x.ClaimWork.Id))
                    .Where(x => x.DateOfRewiew.HasValue)
                    .Select(x => x.ClaimWork.Id);

                var courtClaimQuery = courtOrderRepo.GetAll()
                    .Where(x => petitionQuery.All(id => id != x.ClaimWork.Id))
                    .Where(x => claimWorkQuery.Any(y => y.Id == x.ClaimWork.Id))
                    .Where(x => x.DateOfRewiew.HasValue)
                    .Select(x => x.ClaimWork.Id);

                var ids = petitionQuery.ToArray().Union(courtClaimQuery.ToArray()).Distinct().ToArray();

                var result = lawSuitRepo.GetAll()
                    .Where(x => ids.Contains(x.ClaimWork.Id))
                    .GroupBy(x => x.ClaimWork.Id)
                    .Select(x => new { x.Key, DateOfRewiew = x.Max(m => m.DateOfRewiew) });

                return result.ToDictionary(x => x.Key, y => y.DateOfRewiew.GetValueOrDefault());
            }
        }

        public IQueryable<BaseClaimWork> GetClaimWorkQueryHasReviewDate()
        {
            var petitionRepo = container.ResolveRepository<Petition>();
            var courtOrderRepo = container.ResolveRepository<CourtOrderClaim>();
            var repo = container.ResolveRepository<BaseClaimWork>();

            using (container.Using(petitionRepo, courtOrderRepo, repo))
            {
                /*Приоритет отдается исковому заявлению*/

                var petitionQuery = petitionRepo.GetAll()
                    .Where(x => x.DateOfRewiew.HasValue)
                    .Where(x => !x.ClaimWork.State.FinalState && !x.ClaimWork.IsDebtPaid)
                    .Select(x => x.ClaimWork.Id);

                var courtClaimQuery = courtOrderRepo.GetAll()
                    .Where(x => petitionQuery.All(id => id != x.ClaimWork.Id))
                    .Where(x => x.DateOfRewiew.HasValue)
                    .Where(x => !x.ClaimWork.State.FinalState && !x.ClaimWork.IsDebtPaid)
                    .Select(x => x.ClaimWork.Id);

                var ids = petitionQuery.ToList().Union(courtClaimQuery.ToList()).Distinct();

                return repo.GetAll().Where(x => ids.Contains(x.Id));
            }
        }
    }
}