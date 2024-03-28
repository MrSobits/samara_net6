namespace Bars.GkhGji.StateChange
{
    using Contracts;
    using Regions.Nso.StateChange;

    public class SahalinDisposalNumberValidationNsoRule : SahalinBaseDocNumberValidationNsoRule
    {
        public IDisposalText DisposalText { get; set; }

        public override string Id { get { return "gji_sahalin_disposal_validation_number"; } }

        public override string TypeId { get { return "gji_document_disp"; } }

        public override string Name
        {
            get { return string.Format("САХАЛИН - Присвоение номера {0}", DisposalText.GenetiveCase.ToLower()); }
        }

        public override string Description
        {
            get { return string.Format("САХАЛИН - Данное правило присваивает номера {0}", DisposalText.GenetiveCase.ToLower()); }
        }
    }
}