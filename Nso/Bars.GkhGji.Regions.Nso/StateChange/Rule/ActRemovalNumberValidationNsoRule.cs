using Bars.GkhGji.Regions.Nso.StateChange;

namespace Bars.GkhGji.StateChange
{
    public class ActRemovalNumberValidationNsoRule : BaseDocNumberValidationNsoRule
    {
        public override string Id { get { return "gji_nso_actremoval_validation_number"; } }

        public override string Name { get { return "НСО - Присвоение номера акта устранения нарушений"; } }

        public override string TypeId { get { return "gji_document_actrem"; } }

        public override string Description { get { return "НСО - Данное правило присваивает номера акта устранения нарушений"; } }
        
    }
}