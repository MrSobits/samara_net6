namespace Bars.Gkh.RegOperator.Imports.BankStatement
{
    using System;
    
    /// <summary>
    /// Класс посредник для основной информации о расчетном счете
    /// </summary>
    public class BankAccountBaseInfoProxyCharge
    {
        /// <summary>
        /// Поле дата начала
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Поле дата конца
        /// </summary>
        public DateTime? EndDate { get; set; }


        /// <summary>
        /// Поле начальный остаток
        /// </summary>
        public decimal? StartBalance { get; set; }


        /// <summary>
        /// Поле расчетный счет
        /// </summary>
        public string AccountNum { get; set; }


        /// <summary>
        /// Поле всего списано
        /// </summary>
        public decimal? AllWrittenOff { get; set; }

        /// <summary>
        /// Поле всего поступило
        /// </summary>
        public decimal? AllReceived { get; set; }

        /// <summary>
        /// Поле конечный остаток
        /// </summary>
        public decimal? EndBalance { get; set; }
    }
}
