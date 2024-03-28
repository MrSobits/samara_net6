using Bars.GkhGji.Regions.Nso.StateChange;

namespace Bars.GkhGji.StateChange
{
    public class ProtocolNumberValidationNsoRule : BaseDocNumberValidationNsoRule
    {
        public override string Id { get { return "gji_nso_protocol_validation_number"; } }

        public override string Name { get { return "НСО - Присвоение номера протокола"; } }

        public override string TypeId { get { return "gji_document_prot"; } }

        public override string Description { get { return "НСО - Данное правило присваивает номера протокола"; } }
        
    }
}