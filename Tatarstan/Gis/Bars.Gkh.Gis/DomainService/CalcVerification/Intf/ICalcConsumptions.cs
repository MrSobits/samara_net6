using Bars.B4;

namespace Bars.Gkh.Gis.DomainService.CalcVerification.Intf
{
    using Bars.Gkh.Gis.KP_legacy;

    /// <summary>
    /// Подсчет домовых расходов (ОДН)
    /// </summary>
    public interface ICalcConsumptions
    {
        /// <summary>
        /// Расчет домовых расходов
        /// </summary>
        /// <param name="SourceTable"></param>
        /// <returns></returns>
        IDataResult CalcHouseConsumptions(CalcTypes.ParamCalc param, string SourceTable);

        /// <summary>
        /// Получение домовых расходов из ЦХД
        /// </summary>
        /// <param name="TargetTable"></param>
        /// <returns></returns>
        IDataResult ApplyHouseConsumption(string TargetTable);
    }
}