namespace Bars.GkhGji.StateChange
{
    public class ActRemovalValidationNumberTatRule : BaseDocValidationNumberTatRule
    {
        public override string Id { get { return "gji_tatar_actremoval_validation_number"; } }

        public override string Name { get { return "Проверка возможности формирования номера акта устранения нарушений РТ"; } }

        public override string TypeId { get { return "gji_document_actrem"; } }

        public override string Description { get { return "Данное правило проверяет формирование номера акта устранения нарушений в соответствии с правилами РТ"; } }
    }
}