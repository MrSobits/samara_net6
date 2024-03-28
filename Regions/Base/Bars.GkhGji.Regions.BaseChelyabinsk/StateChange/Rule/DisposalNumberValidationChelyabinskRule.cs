namespace Bars.GkhGji.Regions.BaseChelyabinsk.StateChange.Rule
{
    using Bars.GkhGji.Contracts;

    public class DisposalNumberValidationChelyabinskRule : BaseDocNumberValidationChelyabinskRule
    {
        public IDisposalText DisposalText { get; set; }

        public override string Id { get { return "gji_nso_disposal_validation_number"; } }

        public override string TypeId { get { return "gji_document_disp"; } }

        public override string Name
        {
            get { return string.Format("Челябинск - Присвоение номера {0}", this.DisposalText.GenetiveCase.ToLower()); }
        }

        public override string Description
        {
            get { return string.Format("Данное правило присваивает номера {0}", this.DisposalText.GenetiveCase.ToLower()); }
        }
    }
}