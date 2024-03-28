namespace Bars.Gkh1468.Entities
{
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Сводный паспорт ОКИ
    /// </summary>
    public class OkiPassport : BaseImportableEntity
    {
        public virtual int ReportYear { get; set; }

        public virtual int ReportMonth { get; set; }

        public virtual Municipality Municipality { get; set; }

        public virtual FileInfo Xml { get; set; }

        public virtual FileInfo Signature { get; set; }

        public virtual FileInfo Pdf { get; set; }

        public virtual decimal Percent { get; set; }
    }
}