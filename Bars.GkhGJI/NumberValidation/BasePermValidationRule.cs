namespace Bars.GkhGji.NumberValidation
{
    using System.Globalization;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    using Castle.Core.Internal;
    using Castle.Windsor;
    using ValidateResult = Bars.B4.Modules.States.ValidateResult;

    /// <summary>
    /// Правило проставления номера документа ГЖИ Пермь
    /// </summary>
    public class BasePermValidationRule : INumberValidationRule
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Идентификатор
        /// </summary>
        public string Id => "gji_perm_doc_number_validation";

        /// <summary>
        /// Название валидатора
        /// </summary>
        public string Name => "Правило проставления номера документа ГЖИ Пермь";

        /// <summary>
        /// Установка актуального номера документа ГЖИ
        /// </summary>
        /// <param name="document">Документ для проверки</param>
        /// <returns>Результат операции</returns>
        public ValidateResult Validate(DocumentGji document)
        {
            if (string.IsNullOrEmpty(document.DocumentNumber))
            {
                return ValidateResult.Yes();
            }

            var documentService = this.Container.Resolve<IDomainService<DocumentGji>>();
            var baseLicenseApplicantsService = this.Container.Resolve<IDomainService<BaseLicenseApplicants>>();

            using (this.Container.Using(baseLicenseApplicantsService))
            {
                this.SetDocumentNumber(document, baseLicenseApplicantsService);

                if (documentService.GetAll().Any(x => x.Id != document.Id
                   && x.DocumentYear == document.DocumentYear
                   && x.DocumentNumber == document.DocumentNumber
                   && x.TypeDocumentGji == document.TypeDocumentGji))
                {
                    return ValidateResult.No("Документ с таким номером уже существует");                
                }
            }

            return ValidateResult.Yes();
        }

        private void SetDocumentNumber(DocumentGji document, IDomainService<BaseLicenseApplicants> baseLicenseApplicantsService)
        {
            var muService = this.Container.Resolve<IDomainService<Municipality>>();

            var muCode = string.Empty;

            if (document.Inspection != null)
            {
                if (document.Inspection.TypeBase == TypeBase.ProsecutorsResolution)
                {
                    var resolPros = document as ResolPros;
                    if (resolPros != null)
                    {
                        muCode = resolPros.ReturnSafe(x => x.Municipality.Code);
                    }
                }
                else
                {
                    var mainDispId = this.Container.Resolve<IDomainService<Disposal>>().GetAll()
                            .Where(
                                x => x.Inspection.Id == document.Inspection.Id
                                    && x.TypeDisposal == TypeDisposalGji.Base)
                            .Select(x => x.Id)
                            .FirstOrDefault();

                    if (mainDispId > 0)
                    {
                        var mainDisp = this.Container.Resolve<IDomainService<ViewDisposal>>().Load(mainDispId);

                        if (mainDisp != null)
                        {
                            var mu = muService.Load(mainDisp.MunicipalityId);

                            if (mu != null)
                            {
                                muCode = mu.Code;
                            }
                        }
                    }
                }

                if (!string.IsNullOrEmpty(muCode))
                {
                    document.DocumentNumber = muCode + "-" + document.DocumentNum.ToLong().ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    document.DocumentNumber = document.DocumentNum.ToLong().ToString(CultureInfo.InvariantCulture);
                }

                if (!document.LiteralNum.IsNullOrEmpty())
                {
                    document.DocumentNumber += document.LiteralNum.Trim();
                }

                if (document.DocumentSubNum.ToLong() > 0)
                {
                    document.DocumentNumber += "/" + document.DocumentSubNum.ToLong().ToString(CultureInfo.InvariantCulture);
                }
            }

            // Для распоряжений в "Проверки соискателей лицензии" в зависимости от значения из поля "Тип проверки"(GKH-14900)
            if (document.TypeDocumentGji == TypeDocumentGji.Disposal
                || document.TypeDocumentGji == TypeDocumentGji.ActCheck)
            {
                var inspectionType = baseLicenseApplicantsService.GetAll()
                    .Where(x => x.Id == document.Inspection.Id)
                    .Select(x => x.InspectionType)
                    .FirstOrDefault();
                if (inspectionType == InspectionGjiType.MatchInformation)
                {
                    document.DocumentNumber = document.DocumentNumber + "д";
                }
            }
        }
    }
}