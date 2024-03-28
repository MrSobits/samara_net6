namespace Bars.Gkh.RegOperator.DataProviders.Meta
{
    using System.Collections.Generic;

    /// <summary>
    /// Источник данных для печатки распоряжение о заимствовании
    /// </summary>
    public class LoanDisposalInfo
    {
        /// <summary>
        /// Данные для распоряжения
        /// </summary>
        public List<DisposalInfo> Распоряжение { get; set; }
        /// <summary>
        /// Данные для таблицы займов
        /// </summary>
        public List<LoanInfo> Займы { get; set; }
    }
}
