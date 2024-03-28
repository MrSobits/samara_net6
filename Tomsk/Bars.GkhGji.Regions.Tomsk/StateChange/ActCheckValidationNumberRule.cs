namespace Bars.GkhGji.Regions.Tomsk.StateChange
{
    using System.Globalization;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class ActCheckValidationNumberRule : BaseDocValidationNumberRule
    {
        public IDomainService<ActCheck> ActCheckDomain { get; set; }

        public override string Id { get { return "gji_actcheck_validation_number"; } }

        public override string Name { get { return "Проверка возможности формирования номера акта проверки Томска"; } }

        public override string TypeId { get { return "gji_document_actcheck"; } }

        public override string Description { get { return "Данное правило проверяет формирование номера акта проверки в соответствии с правилами Томска"; } }

        protected override ValidateResult Action(DocumentGji document)
        {
            var result = new ValidateResult();

            // Если данное правило подставили под другой документ, то ничего не делаем
            if (document is ActCheck)
            {
                var actCheck = document as ActCheck;

                if (actCheck.DocumentNum == null && string.IsNullOrWhiteSpace(actCheck.DocumentNumber))
                {
                    // Получаем номер распоряжения
                    var disposalNumber = DocumentGjiChildrenService.GetAll()
                        .Where(x => x.Children.Id == actCheck.Id)
                        .Where(x => x.Parent.TypeDocumentGji == TypeDocumentGji.Disposal)
                        .Select(x => x.Parent.DocumentNumber)
                        .AsEnumerable()
                        .FirstOrDefault(x => !string.IsNullOrWhiteSpace(x));

                    if (disposalNumber == null)
                    {
                        result.Message = "Приказ, на основе которого создается акт проверки, не имеет номера.";
                        result.Success = false;
                        return result;
                    }

                    var documentSubNum = (int?)null;

                    // Если есть аналогичные документы на данном уровне, то присвоим подномер равный max + 1 

                    var parentQuery = this.DocumentGjiChildrenService.GetAll()
                            .Where(x => x.Children.Id == actCheck.Id)
                            .Select(x => x.Parent.Id);

                    var siblingsQuery = this.DocumentGjiChildrenService.GetAll()
                        .Where(x => parentQuery.Contains(x.Parent.Id))
                        .Select(x => x.Children.Id);

                    var siblings = ActCheckDomain.GetAll()
                        .Where(x => x.Id != actCheck.Id)
                        .Where(x => x.Stage.Id == actCheck.Stage.Id)
                        .Where(x => siblingsQuery.Contains(x.Id))
                        .Select(x => x.DocumentSubNum)
                        .ToList();

                    if (siblings.Any())
                    {
                        documentSubNum = (siblings.Max() ?? 0) + 1;
                    }

                    actCheck.DocumentSubNum = documentSubNum;
                    
                    actCheck.DocumentNumber = disposalNumber;
                    if (actCheck.DocumentSubNum.ToInt() > 0)
                    {
                        actCheck.DocumentNumber += "/" + actCheck.DocumentSubNum.ToInt().ToString(CultureInfo.InvariantCulture);
                    }
                }
            }

            result.Success = true;

            return result;
        }
    }
}