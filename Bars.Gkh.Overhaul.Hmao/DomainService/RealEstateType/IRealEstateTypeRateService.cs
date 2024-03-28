namespace Bars.Gkh.Overhaul.Hmao.DomainService
{
    using B4;

    /// <summary>
    /// Сервис для расчета тарифа по типам домов
    /// </summary>
    public interface IRealEstateTypeRateService
    {
        /// <summary>
        /// Расчет тарифа
        /// </summary>
        /// <param name="baseParams">
        /// The base Params.
        /// </param>
        /// <returns>
        /// The <see cref="IDataResult"/>.
        /// </returns>
        IDataResult CalculateRates(BaseParams baseParams);
    }
}