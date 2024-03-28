namespace Bars.GkhGji.Regions.Tomsk.StateChange
{
    using System.Globalization;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class PresentationValidationNumberRule : BaseDocValidationNumberRule
    {

        public IDomainService<Presentation> PrescriptionDomain { get; set; }

        public override string Id { get { return "gji_presentation_validation_number"; } }

        public override string Name { get { return "Проверка возможности формирования номера представления Томска"; } }

        public override string TypeId { get { return "gji_document_presen"; } }

        public override string Description { get { return "Данное правило проверяет формирование номера представления в соответствии с правилами Томска"; } }

        protected override ValidateResult Action(DocumentGji document)
        {
            var result = new ValidateResult();

            // Если данное правило подставили под другой документ, то ничего не делаем
            if (document is Presentation)
            {
                var presentation = document as Presentation;

                if (presentation.DocumentNum == null && string.IsNullOrWhiteSpace(presentation.DocumentNumber))
                {
                    // Получаем номер родителя
                    var parentNumber = DocumentGjiChildrenService.GetAll()
                        .Where(x => x.Children.Id == presentation.Id)
                        .Select(x => x.Parent.DocumentNumber)
                        .AsEnumerable()
                        .FirstOrDefault(x => !string.IsNullOrWhiteSpace(x));

                    if (parentNumber == null)
                    {
                        result.Message = "Постановление, на основе которого создается представление, не имеет номера.";
                        result.Success = false;
                        return result;
                    }

                    var documentSubNum = (int?)null;

                    // Если есть аналогичные документы на данном уровне, то присвоим подномер равный max + 1 

                    var parentQuery = DocumentGjiChildrenService.GetAll()
                        .Where(x => x.Children.Id == presentation.Id)
                        .Select(x => x.Parent.Id);

                    var siblingsQuery = DocumentGjiChildrenService.GetAll()
                        .Where(x => parentQuery.Contains(x.Parent.Id))
                        .Select(x => x.Children.Id);

                    var siblings = PrescriptionDomain.GetAll()
                        .Where(x => x.Id != presentation.Id)
                        .WhereIf(presentation.Stage != null, x => x.Stage.Id == presentation.Stage.Id)
                        .Where(x => siblingsQuery.Contains(x.Id))
                        .Select(x => x.DocumentSubNum)
                        .ToList();

                    if (siblings.Any())
                    {
                        documentSubNum = (siblings.Max() ?? 0) + 1;
                    }

                    presentation.DocumentSubNum = documentSubNum;

                    presentation.DocumentNumber = parentNumber;
                    if (presentation.DocumentSubNum.ToInt() > 0)
                    {
                        presentation.DocumentNumber += "/" + presentation.DocumentSubNum.ToInt().ToString(CultureInfo.InvariantCulture);
                    }
                }
            }

            result.Success = true;

            return result;
        }
    }
}