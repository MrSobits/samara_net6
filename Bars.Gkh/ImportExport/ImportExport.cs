namespace Bars.Gkh.ImportExport
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;

    public class ImportExport : BaseEntity
    {
        public virtual ImportExportType Type { get; set; }

        public virtual FileInfo FileInfo { get; set; }

        public virtual bool HasErrors { get; set; }

        public virtual bool HasMessages { get; set; }

        public virtual DateTime DateStart { get; set; }
    }
}