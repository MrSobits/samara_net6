namespace Bars.Gkh.Gis.RabbitMQ.Impl
{
    using Castle.Windsor;

    public class FileTaskHandler : ITaskHandler<FileTask>
    {
        public IWindsorContainer Container { get; set; }

        public void Run(FileTask task)
        {
        }
    }
}
