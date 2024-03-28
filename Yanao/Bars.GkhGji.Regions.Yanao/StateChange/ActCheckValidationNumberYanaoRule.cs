namespace Bars.GkhGji.StateChange
{
    public class ActCheckValidationNumberYanaoRule : BaseDocValidationNumberYanaoRule
    {
        public override string Id { get { return "gji_yanao_actcheck_validation_number"; } }

        public override string Name { get { return "Проверка возможности формирования номера акта проверки ЯНАО"; } }

        public override string TypeId { get { return "gji_document_actcheck"; } }

        public override string Description { get { return "Данное правило проверяет формирование номера акта проверки в соответствии с правилами ЯНАО"; } }
    }
}