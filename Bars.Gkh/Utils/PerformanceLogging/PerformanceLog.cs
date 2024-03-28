namespace Bars.Gkh.Utils.PerformanceLogging
{
    using System;

    /// <summary>
    /// Класс результата логгирования
    /// </summary>
    public class PerformanceLog
    { 
        /// <summary>
        /// Ключ лога
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Описание логгируемых данных
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Время, затраченное на выполнение
        /// </summary>
        public TimeSpan TimeSpan { get; set; }

        /// <summary>
        /// .ctor
        /// </summary>
        public PerformanceLog(string key, string description)
        {
            this.Key = key;
            this.Description = description;
        }
    }
}