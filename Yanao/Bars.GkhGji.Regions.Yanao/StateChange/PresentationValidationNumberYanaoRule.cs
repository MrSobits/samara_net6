namespace Bars.GkhGji.StateChange
{
    public class PresentationValidationNumberYanaoRule : BaseDocValidationNumberYanaoRule
    {
        public override string Id { get { return "gji_yanao_presentation_validation_number"; } }

        public override string Name { get { return "Проверка возможности формирования номера представления ЯНАО"; } }

        public override string TypeId { get { return "gji_document_presen"; } }

        public override string Description { get { return "Данное правило проверяет формирование номера представления в соответствии с правилами ЯНАО"; } }
    }
}