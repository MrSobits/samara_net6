namespace Bars.Gkh.Gis.Entities.Dict
{
    using System;
    using B4.DataAccess;
    using B4.Modules.FileStorage;

    using Bars.Gkh.Entities.Dicts;

    using Gkh.Entities;

    public class GisNormativDict : BaseEntity
    {
        public virtual Municipality Municipality { get; set; }

        public virtual ServiceDictionary Service { get; set; }

        public virtual decimal? Value { get; set; }

        public virtual string Measure { get; set; }

        public virtual DateTime DateStart { get; set; }

        public virtual DateTime? DateEnd { get; set; }

        public virtual string Description { get; set; }

        public virtual string DocumentName { get; set; }

        public virtual FileInfo DocumentFile { get; set; }
    }
}