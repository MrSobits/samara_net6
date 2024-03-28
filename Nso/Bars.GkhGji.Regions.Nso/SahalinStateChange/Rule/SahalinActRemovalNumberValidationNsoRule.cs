using Bars.GkhGji.Regions.Nso.StateChange;

namespace Bars.GkhGji.StateChange
{
    public class SahalinActRemovalNumberValidationNsoRule : SahalinBaseDocNumberValidationNsoRule
    {
        public override string Id { get { return "gji_sahalin_actremoval_validation_number"; } }

        public override string Name { get { return "САХАЛИН - Присвоение номера акта устранения нарушений"; } }

        public override string TypeId { get { return "gji_document_actrem"; } }

        public override string Description { get { return "САХАЛИН - Данное правило присваивает номера акта устранения нарушений"; } }
        
    }
}