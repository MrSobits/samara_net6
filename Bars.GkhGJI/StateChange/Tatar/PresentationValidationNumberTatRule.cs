namespace Bars.GkhGji.StateChange
{
    public class PresentationValidationNumberTatRule : BaseDocValidationNumberTatRule
    {
        public override string Id { get { return "gji_tatar_presentation_validation_number"; } }

        public override string Name { get { return "Проверка возможности формирования номера представления РТ"; } }

        public override string TypeId { get { return "gji_document_presen"; } }

        public override string Description { get { return "Данное правило проверяет формирование номера представления в соответствии с правилами РТ"; } }
    }
}