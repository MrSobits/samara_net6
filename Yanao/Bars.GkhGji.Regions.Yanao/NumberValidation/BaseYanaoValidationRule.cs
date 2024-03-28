namespace Bars.GkhGji.Regions.Yanao.NumberValidation
{
    using System.Globalization;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.NumberValidation;

    using Castle.Windsor;

    using ValidateResult = Bars.B4.Modules.States.ValidateResult;

    public class BaseYanaoValidationRule : INumberValidationRule
    {
        public IWindsorContainer Container { get; set; }

        public string Id 
        {
            get { return "gji_yanao_doc_number_validation"; }
        }

        public string Name 
        {
            get { return "Правило проставления номера документа ГЖИ ЯНАО"; }
        }

        public ValidateResult Validate(DocumentGji document)
        {
            if (string.IsNullOrEmpty(document.DocumentNumber))
                return ValidateResult.Yes();


            var documentService = this.Container.Resolve<IDomainService<DocumentGji>>();
            try
            {

                // Строковый номер итоговый формируется как "Номер/ДополнительныйНомер/пр2017", где 2017 - год родительского документа
                // Если дополнительный номер = 0, то он не входит в стрковый номер и дробь не ставится
                document.DocumentNumber = document.DocumentNum.ToInt().ToString(CultureInfo.InvariantCulture);
                if (document.DocumentSubNum.ToInt() > 0)
                {
                    var findStage = document.Stage;
                    if (document.Stage.Parent != null)
                    {
                        findStage = document.Stage.Parent;
                    }

                    // получаем документ корневого этапа
                    var parentYear = documentService
                        .GetAll()
                        .Where(x => x.Stage.Id == findStage.Id)
                        .Select(x => x.DocumentYear)
                        .FirstOrDefault();

                    document.DocumentNumber += $"/{document.DocumentSubNum.ToInt().ToString(CultureInfo.InvariantCulture)}" +
                        $"/{(parentYear != null ? "пр" + parentYear : string.Empty)}";
                }

                if (documentService.GetAll()
                    .Any(
                        x => x.Id != document.Id
                            && x.DocumentYear == document.DocumentYear
                            && x.DocumentNumber == document.DocumentNumber
                            && x.TypeDocumentGji == document.TypeDocumentGji))
                {
                    return ValidateResult.No("Документ с таким номером уже существует!");
                }

                return ValidateResult.Yes();
            }
            finally
            {
                this.Container.Release(documentService);
            }
        }
    }
}