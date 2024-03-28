namespace Bars.Gkh.RegOperator.DomainService.Interface
{
    using B4;

    /// <summary>
    /// Интерфейс начисления по Л/С
    /// </summary>
    public interface IPersonalAccountCharger
    {
        /// <summary>
        /// Создать неподтвержденное начисление
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IDataResult CreateUnacceptedCharges(BaseParams baseParams);

        /// <summary>
        /// Подтвердить неподтвержденное начисление
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IDataResult AcceptUnaccepted(BaseParams baseParams);
    }
}