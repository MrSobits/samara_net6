namespace Bars.Gkh.RegOperator.DomainModelServices
{
    using B4;
    using B4.Modules.Quartz;
    using Entities;

    /// <summary>Интерфейс для погашения займов</summary>
    public interface IRealityObjectLoanRepayment : ITask
    {
        /// <summary>
        /// Запустить возврат по всем займам
        /// </summary>
        /// <returns>
        /// Экземпляр <see cref="IDataResult"/>.
        /// </returns>
        /// <param name="baseParams">
        /// Параметры запроса
        /// </param>
        IDataResult RepaymentAll(BaseParams baseParams);

        /// <summary>
        /// Возврат займов дома
        /// </summary>
        /// <param name="baseParams">
        /// Параметры запроса
        /// </param>
        /// <returns>
        /// Экземпляр <see cref="IDataResult"/>.
        /// </returns>
        IDataResult Repayment(BaseParams baseParams);
    }
}