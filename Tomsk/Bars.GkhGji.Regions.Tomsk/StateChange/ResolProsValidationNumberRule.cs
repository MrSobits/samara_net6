namespace Bars.GkhGji.Regions.Tomsk.StateChange
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class ResolProsValidationNumberRule : BaseDocValidationNumberRule
    {
        public IDomainService<AppealCits> AppealCitsDomain { get; set; }

        public IDomainService<DocumentGji> DocumentDomain { get; set; }

        public override string Id { get { return "gji_resolpros_validation_number"; } }

        public override string Name { get { return "Проверка возможности формирования номера постановления прокуратуры Томска"; } }

        public override string TypeId { get { return "gji_document_resolpros"; } }

        public override string Description { get { return "Данное правило проверяет формирование номера постановления прокуратуры в соответствии с правилами Томска"; } }

        protected override ValidateResult Action(DocumentGji document)
        {
            var result = new ValidateResult();

            // Если данное правило подставили под другой документ, то ничего не делаем
            if (document is ResolPros)
            {
                var resolPros = document as ResolPros;

                // Номер постановления прокуратуры сквозной с номером обращения 
                // Формируется путем получения максимального из вышеуказанных номеров за текущий год + 1

                if (!resolPros.DocumentYear.HasValue)
                {
                    // Год берется из даты документа
                    resolPros.DocumentYear = resolPros.DocumentDate.Value.Year;
                }

                if (resolPros.DocumentNum == null && string.IsNullOrWhiteSpace(resolPros.DocumentNumber))
                {
                    var appealNumbers = AppealCitsDomain.GetAll()
                        .Where(x => x.Year == resolPros.DocumentYear)
                        .Where(x => x.NumberGji != null)
                        .Select(x => x.NumberGji)
                        .AsEnumerable()
                        .Select(x => x.ToInt())
                        .ToArray();

                    var resolutionProsecutorMaxNumber = DocumentDomain.GetAll()
                        .Where(x => x.TypeDocumentGji == TypeDocumentGji.ResolutionProsecutor)
                        .Where(x => x.DocumentYear == resolPros.DocumentYear)
                        .Where(x => x.DocumentNum != null)
                        .Max(x => x.DocumentNum);

                    var appealMax = appealNumbers.Any() ? appealNumbers.Max() : 0;
                    var resolutionProsecutorMax = resolutionProsecutorMaxNumber ?? 0;

                    var newNumber = (appealMax > resolutionProsecutorMax ? appealMax : resolutionProsecutorMax) + 1;

                    resolPros.DocumentNum = newNumber;
                    resolPros.DocumentNumber = newNumber.ToStr();
                }

                var numberInUsageAtAppeal = AppealCitsDomain.GetAll()
                   .Any(x => x.Year == resolPros.DocumentYear && x.NumberGji == resolPros.DocumentNumber);

                var numberInUsageAtResolutionProsecutor = DocumentDomain.GetAll()
                    .Where(x => x.Id != resolPros.Id)
                    .Where(x => x.TypeDocumentGji == TypeDocumentGji.ResolutionProsecutor)
                    .Where(x => x.DocumentYear == resolPros.DocumentYear)
                    .Any(x => x.DocumentNumber == resolPros.DocumentNumber);

                if (numberInUsageAtAppeal || numberInUsageAtResolutionProsecutor)
                {
                    result.Message = "Указанный номер в этом году уже используется в другом документе.";
                    result.Success = false;
                    return result;
                }
            }

            result.Success = true;

            return result;
        }
    }
}