namespace Bars.Gkh.RegOperator.Domain.Repository
{
    using Bars.B4;
    using Bars.Gkh.Contracts.Params;
    using Bars.Gkh.RegOperator.Entities;

    /// <summary>
    /// Репозиторий для абонентов
    /// </summary>
    public interface IPersonalAccountOwnerRepository
    {
        /// <summary>
        /// Получить юр абонентов
        /// </summary>
        /// <param name="params">Параметры</param>
        GenericListResult<LegalAccountOwner> ListLegalOwners(BaseParams @params);
    }
}