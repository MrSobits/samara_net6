namespace Bars.Gkh.ExecutionAction.ExecutionActionScheduler.Impl
{
    using System;

    public class ExecutionActionJobInfo : IExecutionActionJobInfo
    {
        /// <inheritdoc />
        public string JobName { get; set; }

        /// <inheritdoc />
        public string JobGroup { get; set; }

        /// <inheritdoc />
        public DateTime? NextFireTime { get; set; }

        /// <inheritdoc />
        public DateTime? PreviousFireTime { get; set; }

        /// <inheritdoc />
        public long TaskId { get; set; }
    }
}