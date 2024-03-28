namespace Bars.GkhGji.StateChange
{
    public class ResolutionValidationNumberYanaoRule : BaseDocValidationNumberYanaoRule
    {
        public override string Id { get { return "gji_yanao_resolution_validation_number"; } }

        public override string Name { get { return "Проверка возможности формирования номера постановления ЯНАО"; } }

        public override string TypeId { get { return "gji_document_resol"; } }

        public override string Description { get { return "Данное правило проверяет формирование номера постановления в соответствии с правилами ЯНАО"; } }
    }
}