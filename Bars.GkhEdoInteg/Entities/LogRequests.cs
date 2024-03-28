namespace Bars.GkhEdoInteg.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;

    public class LogRequests : BaseEntity
    {
        public virtual DateTime DateStart { get; set; }

        public virtual DateTime DateEnd { get; set; }

        public virtual int CountAdded { get; set; }

        public virtual int CountUpdated { get; set; }

        public virtual decimal? TimeExecution { get; set; }

        public virtual int Count { get; set; }

        public virtual string Uri { get; set; }

        public virtual FileInfo File { get; set; }
    }
}
