namespace Bars.Gkh.Overhaul.Nso.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Nso.Enum;

    public class ProgramVersion : BaseEntity
    {
        public virtual string Name { get; set; }

        public virtual Municipality Municipality { get; set; }

        public virtual DateTime VersionDate { get; set; }

        public virtual bool IsMain { get; set; }

        public virtual ProgramVersionCopyingState CopyingState { get; set; }
    }
}