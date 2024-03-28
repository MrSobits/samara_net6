namespace Bars.GkhGji.Regions.Tomsk.StateChange
{
    using System.Globalization;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class ResolutionValidationNumberRule : BaseDocValidationNumberRule
    {
        public IDomainService<Resolution> ResolutionDomain { get; set; }

        public override string Id { get { return "gji_resolution_validation_number"; } }

        public override string Name { get { return "Проверка возможности формирования номера постановления Томска"; } }

        public override string TypeId { get { return "gji_document_resol"; } }

        public override string Description { get { return "Данное правило проверяет формирование номера постановления в соответствии с правилами Томска"; } }

        protected override ValidateResult Action(DocumentGji document)
        {
            var result = new ValidateResult();

            // Если данное правило подставили под другой документ, то ничего не делаем
            if (document is Resolution)
            {
                var resolution = document as Resolution;

                if (resolution.DocumentNum == null && string.IsNullOrWhiteSpace(resolution.DocumentNumber))
                {
                    var query = DocumentGjiChildrenService.GetAll()
                        .Where(x => x.Children.Id == resolution.Id);

                    string parentNumber;

                    // Получаем номер родителя
                    parentNumber = query
                        .Select(x => x.Parent.DocumentNumber)
                        .Where(x => x != null)
                        .AsEnumerable()
                        .FirstOrDefault(x => !string.IsNullOrWhiteSpace(x));

                    if (parentNumber == null)
                    {
                        result.Message = "Родительский документ, на основе которого создается постановление, не имеет номера.";
                        result.Success = false;
                        return result;
                    }

                    var documentSubNum = (int?)null;

                    // Если есть аналогичные документы на данном уровне, то присвоим подномер равный max + 1 
                    var siblings = ResolutionDomain.GetAll()
                        .Where(x => x.Id != resolution.Id)
                        .Where(x => x.Stage.Id == resolution.Stage.Id)
                        .Select(x => x.DocumentSubNum)
                        .ToList();

                    if (siblings.Any())
                    {
                        documentSubNum = (siblings.Max() ?? 0) + 1;
                    }
                    
                    resolution.DocumentSubNum = documentSubNum;

                    resolution.DocumentNumber = parentNumber;
                    if (resolution.DocumentSubNum.ToInt() > 0)
                    {
                        resolution.DocumentNumber += "/" + resolution.DocumentSubNum.ToInt().ToString(CultureInfo.InvariantCulture);
                    }
                }
            }

            result.Success = true;

            return result;
        }
    }
}