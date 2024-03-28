namespace Bars.Gkh.ClaimWork.Formulas
{
    using System;
    using Bars.B4.Utils;
    using Bars.Gkh.Formulas;
    using Bars.Gkh.Modules.ClaimWork.Entities;

    /// <summary>
    /// Претензионная работа - Документы - документ "Исковое заявление" - поле "Сумма пени (руб.)".
    /// </summary>
    public class PenaltyAmountParameter : FormulaParameterBase
    {
        public static string Id = "PenaltyAmountParameter";
        private Lawsuit _lawsuit;

        #region Overrides of FormulaParameterBase

        /// <summary>
        /// Наименование параметра
        /// </summary>
        public override string DisplayName
        {
            get { return "Сумма пени (Исковое заявление)"; }
        }

        /// <summary>
        /// Код параметра для поиска
        /// </summary>
        public override string Code
        {
            get { return Id; }
        }

        /// <summary>
        /// Получить значение параметра
        /// </summary>
        public override object GetValue()
        {
            if (_lawsuit == null)
                throw new InvalidOperationException("Не передано исковое заявление!");

            return _lawsuit.PenaltyDebt;
        }
        
        /// <summary>
        /// Установить провадеры данных
        /// </summary>
        public override void SetDataProviders(DynamicDictionary providers)
        {
            if (providers.ContainsKey("Lawsuit"))
            {
                _lawsuit = providers.GetAs<Lawsuit>("Lawsuit", ignoreCase: true);
            }

            base.SetDataProviders(providers);
        }

        #endregion
    }
}