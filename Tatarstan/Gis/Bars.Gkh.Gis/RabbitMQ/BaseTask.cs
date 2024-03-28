namespace Bars.Gkh.Gis.RabbitMQ
{
    using System;

    /// <summary>
    /// Базовая сущность задачи
    /// </summary>
    [Serializable]
    public abstract class BaseTask
    {
        public Type Type { get; set; }
    }
}
