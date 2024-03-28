namespace Bars.Gkh1468
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Entities;
    using Bars.Gkh1468.Entities;

    public class BaseProviderPassport : BaseEntity, IStatefulEntity
    {
        public virtual int ReportYear { get; set; }

        public virtual int ReportMonth { get; set; }

        public virtual State State { get; set; }

        public virtual ContragentType ContragentType { get; set; }

        public virtual Contragent Contragent { get; set; }

        public virtual FileInfo Xml { get; set; }

        public virtual FileInfo Signature { get; set; }

        public virtual FileInfo Pdf { get; set; }

        public virtual FileInfo Certificate { get; set; }

        public virtual decimal Percent { get; set; }

        public virtual PassportStruct PassportStruct { get; set; }

        public virtual DateTime? SignDate { get; set; }
    }
}