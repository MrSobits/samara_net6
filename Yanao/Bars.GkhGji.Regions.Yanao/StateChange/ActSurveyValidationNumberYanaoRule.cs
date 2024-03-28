namespace Bars.GkhGji.StateChange
{
    public class ActSurveyValidationNumberYanaoRule : BaseDocValidationNumberYanaoRule
    {
        public override string Id { get { return "gji_yanao_actsurvey_validation_number"; } }

        public override string Name { get { return "Проверка возможности формирования номера акта обследования ЯНАО"; } }

        public override string TypeId { get { return "gji_document_actsur"; } }

        public override string Description { get { return "Данное правило проверяет формирование номера акта обследования в соответствии с правилами ЯНАО"; } }
    }
}