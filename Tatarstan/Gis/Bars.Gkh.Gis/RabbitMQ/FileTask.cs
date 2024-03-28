namespace Bars.Gkh.Gis.RabbitMQ
{
    using System;

    [Serializable]
    public class FileTask : BaseTask
    {
        public long LoadedFileRegisterId { get; set; }
    }
}
