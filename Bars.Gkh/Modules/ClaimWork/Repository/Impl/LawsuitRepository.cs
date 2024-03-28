namespace Bars.Gkh.Modules.ClaimWork.Repository.Impl
{
    using System.Linq;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.B4.Utils.Annotations;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Modules.ClaimWork.Repository;

    public class LawsuitRepository : ILawsuitRepository
    {
        private readonly IRepository<Petition> _petitionRepo;
        private readonly IRepository<CourtOrderClaim> _courtOrderRepo;

        public LawsuitRepository(
            IRepository<Petition> petitionRepo,
            IRepository<CourtOrderClaim> courtOrderRepo)
        {
            _petitionRepo = petitionRepo;
            _courtOrderRepo = courtOrderRepo;
        }

        #region Implementation of ILawsuitRepository

        /// <summary>
        /// Найти исковое заявление или завяление о выдаче искового заявления
        /// </summary>
        /// <param name="claimWork">Основание</param>
        /// <returns>Если есть исковое заявление, то возвращает его, иначе заявление о выдаче</returns>
        public Lawsuit FindPetitionOrCourtOrderClaim(BaseClaimWork claimWork)
        {
            ArgumentChecker.NotNull(claimWork, "claimWork");

            return _petitionRepo.GetAll().FirstOrDefault(x => x.ClaimWork.Id == claimWork.Id).As<Lawsuit>()
                   ?? _courtOrderRepo.GetAll().FirstOrDefault(x => x.ClaimWork.Id == claimWork.Id).As<Lawsuit>();
        }

        #endregion
    }
}