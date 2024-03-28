namespace Bars.GkhGji.Regions.Tomsk.StateChange
{
    using System.Globalization;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class ActSurveyValidationNumberRule : BaseDocValidationNumberRule
    {
        public IDomainService<ActSurvey> ActSurveyDomain { get; set; }

        public override string Id { get { return "gji_actsurvey_validation_number"; } }

        public override string Name { get { return "Проверка возможности формирования номера акта обследования Томска"; } }

        public override string TypeId { get { return "gji_document_actsur"; } }

        public override string Description { get { return "Данное правило проверяет формирование номера акта обследования в соответствии с правилами Томска"; } }

        protected override ValidateResult Action(DocumentGji document)
        {
            var result = new ValidateResult();

            // Если данное правило подставили под другой документ, то ничего не делаем
            if (document is ActSurvey)
            {
                var actSurvey = document as ActSurvey;

                if (actSurvey.DocumentNum == null && string.IsNullOrWhiteSpace(actSurvey.DocumentNumber))
                {
                    // Получаем номер распоряжения
                    var disposalNumber = DocumentGjiChildrenService.GetAll()
                        .Where(x => x.Children.Id == actSurvey.Id)
                        .Where(x => x.Parent.TypeDocumentGji == TypeDocumentGji.Disposal)
                        .Select(x => x.Parent.DocumentNumber)
                        .AsEnumerable()
                        .FirstOrDefault(x => !string.IsNullOrWhiteSpace(x));

                    if (disposalNumber == null)
                    {
                        result.Message = "Приказ, на основе которого создается акт обследования, не имеет номера.";
                        result.Success = false;
                        return result;
                    }

                    var documentSubNum = (int?)null;

                    // Если есть аналогичные документы на данном уровне, то присвоим подномер равный max + 1 
                    var parentQuery = DocumentGjiChildrenService.GetAll()
                        .Where(x => x.Children.Id == actSurvey.Id)
                        .Select(x => x.Parent.Id);

                    var siblingsQuery = DocumentGjiChildrenService.GetAll()
                        .Where(x => parentQuery.Contains(x.Parent.Id))
                        .Select(x => x.Children.Id);

                    var siblings = ActSurveyDomain.GetAll()
                        .Where(x => x.Id != actSurvey.Id)
                        .WhereIf(actSurvey.Stage != null, x => x.Stage.Id == actSurvey.Stage.Id)
                        .Where(x => siblingsQuery.Contains(x.Id))
                        .Select(x => x.DocumentSubNum)
                        .ToList();

                    if (siblings.Any())
                    {
                        documentSubNum = (siblings.Max() ?? 0) + 1;
                    }

                    actSurvey.DocumentSubNum = documentSubNum;

                    actSurvey.DocumentNumber = disposalNumber;
                    if (actSurvey.DocumentSubNum.ToInt() > 0)
                    {
                        actSurvey.DocumentNumber += "/" + actSurvey.DocumentSubNum.ToInt().ToString(CultureInfo.InvariantCulture);
                    }
                }
            }

            result.Success = true;

            return result;
        }
    }
}