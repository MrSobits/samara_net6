namespace Bars.Gkh.Overhaul.Entities
{
    using B4.Modules.FileStorage;
    using Gkh.Entities;

    public class ContragentBankCreditOrg : ContragentBank
    {
        public virtual CreditOrg CreditOrg { get; set; }

        public virtual FileInfo File { get; set; }
    }
}