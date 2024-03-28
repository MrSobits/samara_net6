namespace Bars.Gkh.Modules.ClaimWork.Repository
{
    using Bars.Gkh.Modules.ClaimWork.Entities;

    /// <summary>
    /// ИНтерфейс репозитория получения информации об исковой работе
    /// </summary>
    public interface ILawsuitRepository
    {
        /// <summary>
        /// Найти исковое заявление или завяление о выдаче искового заявления
        /// </summary>
        /// <param name="claimWork">Основание</param>
        /// <returns>Если есть исковое заявление, то возвращает его, иначе заявление о выдаче</returns>
        Lawsuit FindPetitionOrCourtOrderClaim(BaseClaimWork claimWork);
    }
}