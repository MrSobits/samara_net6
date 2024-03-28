namespace Bars.Gkh.Domain.ParameterVersioning.Proxy
{
    using System;

    public class EntityLogRecord
    {
        public DateTime ObjectCreateDate { get; set; }

        public long LogId { get; set; }

        public long EntityId { get; set; }

        public DateTime? DateApplied { get; set; }

        public DateTime DateActualChange { get; set; }

        public string ParameterName { get; set; }

        public string PropertyValue { get; set; }
    }
}