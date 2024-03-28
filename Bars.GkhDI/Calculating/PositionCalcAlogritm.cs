namespace Bars.GkhDi.Calculating
{
    using System.Collections.Generic;

    using Bars.B4.Utils;
    using Bars.GkhDi.Entities;

    /// <summary>
    /// Калькулятор, подсчитывающий проценты целиком на основании количества заполненных полей
    /// </summary>
    public class PositionCalcAlogritm : AbstractCalcPercentAlgoritm
    {
        /// <inheritdoc />
        protected override decimal CalcPercentInternal(BasePercent parentPercent, IList<BasePercent> childElements)
        {
            return (decimal.Divide(parentPercent.CompletePositionsCount, parentPercent.PositionsCount) * 100).RoundDecimal(2);
        }
    }
}