namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount
{
    using Bars.B4;

    /// <summary>
    /// Интефейс сервиса разедения ЛС
    /// </summary>
    public interface IPersonalAccountSplitService
    {
        /// <summary>
        /// Метод возвращает итоговые задолженности на указанную дату
        /// </summary>
        /// <returns>Задолженности по базовому тарифу, тарифу решения и пени</returns>
        IDataResult GetDistributableDebts(BaseParams baseParams);

        /// <summary>
        /// Метод производит распределение задолженностей
        /// </summary>
        IDataResult DistributeDebts(BaseParams baseParams);

        /// <summary>
        /// Произвести распределение ЛС
        /// </summary>
        IDataResult ApplyDistribution(BaseParams baseParams);
    }
}