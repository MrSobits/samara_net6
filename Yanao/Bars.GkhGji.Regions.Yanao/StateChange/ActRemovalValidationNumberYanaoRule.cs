namespace Bars.GkhGji.StateChange
{
    public class ActRemovalValidationNumberYanaoRule : BaseDocValidationNumberYanaoRule
    {
        public override string Id { get { return "gji_yanao_actremoval_validation_number"; } }

        public override string Name { get { return "Проверка возможности формирования номера акта устранения нарушений ЯНАО"; } }

        public override string TypeId { get { return "gji_document_actrem"; } }

        public override string Description { get { return "Данное правило проверяет формирование номера акта устранения нарушений в соответствии с правилами ЯНАО"; } }
    }
}