namespace Bars.GkhGji.Regions.BaseChelyabinsk.StateChange.Rule
{
    public class ActSurveyNumberValidationChelyabinskRule : BaseDocNumberValidationChelyabinskRule
    {
        public override string Id { get { return "gji_nso_actsurvey_validation_number"; } }

        public override string Name { get { return "Челябинск - Присвоение номера акта обследования"; } }

        public override string TypeId { get { return "gji_document_actsur"; } }

        public override string Description { get { return "Данное правило присваивает номера актам обследования"; } }
    }
}
