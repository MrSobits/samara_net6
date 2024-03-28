using Bars.B4;

namespace Bars.Gkh.RegOperator.Regions.Chelyabinsk.DomainService
{
    public interface IAgentPIRExecuteService
    {
        /// <summary>
        /// Получить ЛС
        /// </summary>
        IDataResult GetListPersonalAccountDebtor(BaseParams baseParams);

        /// <summary>
        /// Добавление лс
        /// </summary>
        IDataResult AddPersonalAccountDebtor(BaseParams baseParams);

        /// <summary>
        /// Получение списка оплат
        /// </summary>
        IDataResult GetListPayment(BaseParams baseParams);

        /// <summary>
        /// Расчитать дату начала задолженности
        /// </summary>
        IDataResult DebtStartDateCalculate(BaseParams baseParams);
    }
}
