namespace Bars.Gkh.Regions.Voronezh.Domain
{
    using Bars.B4;
    using Bars.Gkh.RegOperator.Entities.Owner;

    /// <summary>
    /// Интерфейс  "Собственник в исковом заявлении"
    /// </summary>
    public interface IRosRegExtractOperationsService
    {      

        /// <summary>
        /// Получить собственников
        /// </summary>
        IDataResult GetOwners(BaseParams baseParams);
    }
}