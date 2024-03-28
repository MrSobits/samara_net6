namespace Bars.GkhGji.Regions.Tomsk.StateChange
{
    using System.Globalization;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class PrescriptionValidationNumberRule : BaseDocValidationNumberRule
    {
        public IDomainService<Prescription> PrescriptionDomain { get; set; }

        public override string Id { get { return "gji_prescription_validation_number"; } }

        public override string Name { get { return "Проверка возможности формирования номера предписания Томска"; } }

        public override string TypeId { get { return "gji_document_prescr"; } }

        public override string Description { get { return "Данное правило проверяет формирование номера предписания в соответствии с правилами Томска"; } }

        protected override ValidateResult Action(DocumentGji document)
        {
            var result = new ValidateResult();

            // Если данное правило подставили под другой документ, то ничего не делаем
            if (document is Prescription)
            {
                var prescription = document as Prescription;

                if (prescription.DocumentNum == null && string.IsNullOrWhiteSpace(prescription.DocumentNumber))
                {
                    var query = DocumentGjiChildrenService.GetAll()
                        .Where(x => x.Children.Id == prescription.Id);

                    string parentNumber;

                    // Получаем номер родителя
                    parentNumber = query
                        .Select(x => x.Parent.DocumentNumber)
                        .Where(x => x != null)
                        .AsEnumerable()
                        .FirstOrDefault(x => !string.IsNullOrWhiteSpace(x));

                    if (parentNumber == null)
                    {
                        result.Message = "Документ, на основе которого создается предписание, не имеет номера.";
                        result.Success = false;
                        return result;
                    }

                    var documentSubNum = (int?)null;

                    // Если есть аналогичные документы на данном уровне, то присвоим подномер равный max + 1 
                    var parentQuery = DocumentGjiChildrenService.GetAll()
                        .Where(x => x.Children.Id == prescription.Id)
                        .Select(x => x.Parent.Id);

                    var siblingsQuery = DocumentGjiChildrenService.GetAll()
                        .Where(x => parentQuery.Any(y => y == x.Parent.Id))
                        .Select(x => x.Children.Id);

                    var siblings = PrescriptionDomain.GetAll()
                        .Where(x => x.Id != prescription.Id)
                        .WhereIf(prescription.Stage != null, x => x.Stage.Id == prescription.Stage.Id)
                        .Where(x => siblingsQuery.Contains(x.Id))
                        .Select(x => x.DocumentSubNum)
                        .ToList();

                    if (siblings.Any())
                    {
                        documentSubNum = (siblings.Max() ?? 0) + 1;
                    }

                    prescription.DocumentSubNum = documentSubNum;

                    prescription.DocumentNumber = parentNumber;
                    if (prescription.DocumentSubNum.ToInt() > 0)
                    {
                        prescription.DocumentNumber += "/" + prescription.DocumentSubNum.ToInt().ToString(CultureInfo.InvariantCulture);
                    }
                }
            }

            result.Success = true;

            return result;
        }
    }
}