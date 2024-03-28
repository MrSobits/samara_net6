namespace Bars.GkhGji.Regions.Nnovgorod.StateChange
{
    public class ActSurveyValidationNumberRule : BaseDocValidationNumberRule
    {
        public override string Id { get { return "gji_actsurvey_validation_number"; } }

        public override string Name { get { return "Проверка возможности формирования номера акта обследования"; } }

        public override string TypeId { get { return "gji_document_actsur"; } }

        public override string Description { get { return "Данное правило проверяет формирование номера акта обследования в соответствии с правилами"; } }
    }
}