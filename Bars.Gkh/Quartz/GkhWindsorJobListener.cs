namespace Bars.Gkh.Quartz
{
    using System.Threading;
    using System.Threading.Tasks;

    using Bars.B4.IoC;

    using Castle.Windsor;

    using global::Quartz;

    /// <summary>
    /// Переопределение <see cref="Bars.B4.Modules.Quartz.WindsorJobListener"/>, чтобы не писало в лог инфо кучу сообщений
    /// </summary>
    public class GkhWindsorJobListener : IJobListener
    {
        private readonly IWindsorContainer container;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="container">Контейнер</param>
        public GkhWindsorJobListener(IWindsorContainer container)
        {
            this.container = container;
        }

        /// <inheritdoc />
        public Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            if (context.JobInstance != null)
            {
                this.container.BuildUp(context.JobInstance);
            }
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public string Name => "WindsorJobListener";
    }
}