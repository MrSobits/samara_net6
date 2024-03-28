namespace Bars.GkhGji.Regions.BaseChelyabinsk.SahalinStateChange.Rule
{
    public class SahalinProtocolNumberValidationChelyabinskRule : SahalinBaseDocNumberValidationChelyabinskRule
    {
        public override string Id { get { return "gji_sahalin_protocol_validation_number"; } }

        public override string Name { get { return "САХАЛИН - Присвоение номера протокола"; } }

        public override string TypeId { get { return "gji_document_prot"; } }

        public override string Description { get { return "САХАЛИН - Данное правило присваивает номера протокола"; } }
        
    }
}