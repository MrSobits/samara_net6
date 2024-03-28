namespace Bars.Gkh1468.Entities.Passport
{
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities;
    using Bars.Gkh1468.Enums;

    /// <summary>
    /// Сводный паспорт дома
    /// </summary>
    public class HousePassport : BaseImportableEntity
    {
        public virtual int ReportYear { get; set; }

        public virtual int ReportMonth { get; set; }

        public virtual RealityObject RealityObject { get; set; }

        public virtual HouseType HouseType { get; set; }

        public virtual FileInfo Xml { get; set; }

        public virtual FileInfo Signature { get; set; }

        public virtual FileInfo Pdf { get; set; }

        public virtual decimal Percent { get; set; }
    }
}