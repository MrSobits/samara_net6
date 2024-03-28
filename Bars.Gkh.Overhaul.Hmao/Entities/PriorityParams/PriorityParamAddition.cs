namespace Bars.Gkh.Overhaul.Hmao.Entities.PriorityParams
{
    using B4.DataAccess;

    using Bars.Gkh.Overhaul.Hmao.Enum;

    /// <summary> Параметры очередности. (Доп. поля) </summary>
    public class PriorityParamAddition : PersistentObject
    {
        /// <summary> Код </summary>
        public virtual string Code { get; set; }

        /// <summary> Доп.множитель </summary>
        public virtual PriorityParamAdditionFactor AdditionFactor { get; set; }

        /// <summary> Значение множителя </summary>
        public virtual decimal FactorValue { get; set; }

        /// <summary> Конечное значение </summary>
        public virtual PriorityParamFinalValue FinalValue { get; set; }
    }
}