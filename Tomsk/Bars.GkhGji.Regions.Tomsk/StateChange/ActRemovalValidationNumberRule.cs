namespace Bars.GkhGji.Regions.Tomsk.StateChange
{
    using System.Globalization;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class ActRemovalValidationNumberRule : BaseDocValidationNumberRule
    {
        public IDomainService<Disposal> DisposalDomain { get; set; }

        public IDomainService<ActRemoval> ActRemoval { get; set; }

        public override string Id { get { return "gji_actremoval_validation_number"; } }

        public override string Name { get { return "Проверка возможности формирования номера акта устранения нарушений Томска"; } }

        public override string TypeId { get { return "gji_document_actrem"; } }

        public override string Description { get { return "Данное правило проверяет формирование номера акта устранения нарушений в соответствии с правилами Томска"; } }

        protected override ValidateResult Action(DocumentGji document)
        {
            var result = new ValidateResult();

            // Если данное правило подставили под другой документ, то ничего не делаем
            if (document is ActRemoval)
            {
                var actRemoval = document as ActRemoval;

                if (actRemoval.DocumentNum == null && string.IsNullOrWhiteSpace(actRemoval.DocumentNumber))
                {
                    var findStage = actRemoval.Stage;
                    if (actRemoval.Stage.Parent != null)
                    {
                        findStage = document.Stage.Parent;
                    }

                    // Теперь получаем документ корневого этапа
                    var disposalNumber = DisposalDomain.GetAll()
                        .Where(x => x.Stage.Id == findStage.Id)
                        .Select(x => x.DocumentNumber)
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
                        .Where(x => x.Children.Id == actRemoval.Id)
                        .Select(x => x.Parent.Id);

                    var siblingsQuery = this.DocumentGjiChildrenService.GetAll()
                        .Where(x => parentQuery.Contains(x.Parent.Id))
                        .Select(x => x.Children.Id);

                    var siblings = ActRemoval.GetAll()
                        .Where(x => x.Id != actRemoval.Id)
                        .WhereIf(actRemoval.Stage != null, x => x.Stage.Id == actRemoval.Stage.Id)
                        .Where(x => siblingsQuery.Contains(x.Id))
                        .Select(x => x.DocumentSubNum)
                        .ToList();

                    if (siblings.Any())
                    {
                        documentSubNum = (siblings.Max() ?? 0) + 1;
                    }

                    actRemoval.DocumentSubNum = documentSubNum;

                    actRemoval.DocumentNumber = disposalNumber;
                    if (actRemoval.DocumentSubNum.ToInt() > 0)
                    {
                        actRemoval.DocumentNumber += "-" + actRemoval.DocumentSubNum.ToInt().ToString(CultureInfo.InvariantCulture);
                    }
                }
            }

            result.Success = true;

            return result;
        }
    }
}