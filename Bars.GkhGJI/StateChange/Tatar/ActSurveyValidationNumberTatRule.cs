namespace Bars.GkhGji.StateChange
{
    public class ActSurveyValidationNumberTatRule : BaseDocValidationNumberTatRule
    {
        public override string Id { get { return "gji_tatar_actsurvey_validation_number"; } }

        public override string Name { get { return "Проверка возможности формирования номера акта обследования РТ"; } }

        public override string TypeId { get { return "gji_document_actsur"; } }

        public override string Description { get { return "Данное правило проверяет формирование номера акта обследования в соответствии с правилами РТ"; } }
    }
}