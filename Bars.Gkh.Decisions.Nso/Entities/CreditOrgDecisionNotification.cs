namespace Bars.Gkh.Decisions.Nso.Entities
{
    using System;
    using B4.Modules.FileStorage;
    using B4.Modules.States;

    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums.Decisions;
    using Bars.Gkh.Overhaul.Entities;


    public class CreditOrgDecisionNotification : BaseImportableEntity, IStatefulEntity
    {
        public virtual State State { get; set; }

        public virtual CreditOrg CreditOrg { get; set; }

        public virtual AccountOwnerDecisionType OwnerType { get; set; }

        public virtual string BankAccountNumber { get; set; }

        public virtual DateTime? Date { get; set; }

        public virtual DateTime? RegistrationDate { get; set; }

        public virtual FileInfo BankFile { get; set; }

        public virtual bool HasOriginalNotification { get; set; }

        public virtual bool HasReferenceCopy { get; set; }

        public virtual bool HasProtocolCopy { get; set; }

        public virtual string IncomingGjiNumber { get; set; }
    }
}