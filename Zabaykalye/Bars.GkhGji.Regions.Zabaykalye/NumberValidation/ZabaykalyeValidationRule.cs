using Bars.Gkh.Domain.CollectionExtensions;

namespace Bars.GkhGji.Regions.Zabaykalye.NumberValidation
{
    using System.Linq;
    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Castle.Windsor;
    using Bars.GkhGji.NumberValidation;
    using ValidateResult = Bars.B4.Modules.States.ValidateResult;

    public class ZabaykalyeValidationRule : INumberValidationRule
    {
        public IWindsorContainer Container { get; set; }

        public string Id
        {
            get { return "gji_zabaykalye_doc_number_validation"; }
        }

        public string Name
        {
            get { return "Правило проставления номера документа ГЖИ Забайкалья"; }
        }

        public ValidateResult Validate(DocumentGji document)
        {
            if (string.IsNullOrEmpty(document.DocumentNumber) || !document.DocumentDate.HasValue)
                return ValidateResult.Yes();

            var documentService = Container.Resolve<IDomainService<DocumentGji>>();

            var num = 0;

            switch (document.TypeDocumentGji)
            {
                default:
                {
                    num = documentService.GetAll()
                        .Where(x => x.TypeDocumentGji == document.TypeDocumentGji)
                        .Where(x => x.Id != document.Id)
                        .Where(
                            x =>
                                x.DocumentDate.HasValue && x.DocumentYear == document.DocumentYear)
                                .Select(x => x.DocumentNum.HasValue ? x.DocumentNum.Value : 0)
                        .SafeMax(x => x);

                }
                    break;
            }

            num++;

            document.DocumentNum = num;
            document.DocumentNumber = num.ToString();
            
            if (documentService.GetAll()
                .Any(x => x.Id != document.Id
                    && x.DocumentYear == document.DocumentYear
                    && x.DocumentNumber == document.DocumentNumber
                    && x.TypeDocumentGji == document.TypeDocumentGji))
            {
                return ValidateResult.No("Документ с таким номером уже существует");
            }

            return ValidateResult.Yes();
        }
    }
}