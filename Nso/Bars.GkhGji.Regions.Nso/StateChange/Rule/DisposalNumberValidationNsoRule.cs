namespace Bars.GkhGji.StateChange
{
    using Contracts;
    using Regions.Nso.StateChange;

    public class DisposalNumberValidationNsoRule : BaseDocNumberValidationNsoRule
    {
        public IDisposalText DisposalText { get; set; }

        public override string Id { get { return "gji_nso_disposal_validation_number"; } }

        public override string TypeId { get { return "gji_document_disp"; } }

        public override string Name
        {
            get { return string.Format("НСО - Присвоение номера {0}", DisposalText.GenetiveCase.ToLower()); }
        }

        public override string Description
        {
            get { return string.Format("НСО - Данное правило присваивает номера {0}", DisposalText.GenetiveCase.ToLower()); }
        }
    }
}