namespace Bars.Gkh.RegOperator.Entities.PersonalAccount
{
    using System;
    
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities.Dict;

    public class PersonalAccountPrivilegedCategory : BaseImportableEntity
    {
        public virtual BasePersonalAccount PersonalAccount { get; set; }

        public virtual PrivilegedCategory PrivilegedCategory { get; set; }

        public virtual DateTime DateFrom { get; set; }

        public virtual DateTime? DateTo { get; set; }
    }
}