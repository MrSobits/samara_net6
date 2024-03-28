namespace Bars.Gkh1468.DomainService.Passport
{
    /// <summary>
    /// Интерфейс обобщенного сервиса подсчета процента заполненности паспорта
    /// </summary>
    /// <typeparam name="TPassport"></typeparam>
    /// <typeparam name="TPassportRow"></typeparam>
    public interface IPassportFillPercentService<TPassport, TPassportRow>
        where TPassport : BaseProviderPassport where TPassportRow : BaseProviderPassportRow<TPassport>
    {
        /// <summary>
        /// Подсчет процента заполнения паспорта
        /// </summary>
        /// <param name="providerPassportId"></param>
        /// <returns></returns>
        decimal CountFillPercentage(long providerPassportId);

        /// <summary>
        /// Подсчет процента заполнения паспорта
        /// </summary>
        /// <param name="providerPassport"></param>
        /// <returns></returns>
        decimal CountFillPercentage(TPassport providerPassport);
    }
}