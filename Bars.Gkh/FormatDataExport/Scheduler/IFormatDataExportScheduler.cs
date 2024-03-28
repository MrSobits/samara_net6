namespace Bars.Gkh.FormatDataExport.Scheduler
{
    using Bars.Gkh.Quartz.Scheduler;

    public interface IFormatDataExportScheduler : IGkhQuartzScheduler
    {
        /// <summary>
        /// Код планировщика
        /// </summary>
        string Code { get; }
    }
}