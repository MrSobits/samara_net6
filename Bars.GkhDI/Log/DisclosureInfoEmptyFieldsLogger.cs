namespace Bars.GkhDi.Log
{
    using Bars.Gkh.Log.Impl;
    using Bars.GkhDi.Entities;

    /// <summary>
    /// Асинхронный логгер незаполненных полей
    /// </summary>
    public class DisclosureInfoEmptyFieldsLogger : AsyncLoggerBase<DisclosureInfoEmptyFieldsBase>
    {
        /// <summary>
        /// Порог очереди для синхронизации
        /// <para>По умолчанию: 100</para>
        /// </summary>
        public int QueueThreeshold { get; set; }

        public DisclosureInfoEmptyFieldsLogger()
        {
            this.QueueThreeshold = 100;
            this.OnEnqueue += this.DisclosureInfoEmptyFieldsLoggerOnEnqueue;
        }

        private void DisclosureInfoEmptyFieldsLoggerOnEnqueue(int count)
        {
            if (count == this.QueueThreeshold)
            {
                this.Flush();
            }
        }
    }
}