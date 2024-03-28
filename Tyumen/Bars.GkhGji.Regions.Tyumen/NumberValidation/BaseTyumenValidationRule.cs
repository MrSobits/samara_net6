namespace Bars.GkhGji.Regions.Tyumen.NumberValidation
{
    using Bars.B4.Modules.States;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.NumberValidation;

    using Castle.Windsor;

    public class BaseTyumenValidationRule : INumberValidationRule
    {
        public IWindsorContainer Container { get; set; }

        public string Id
        {
            get { return "gji_tyumen_doc_number_validation"; }
        }

        public string Name
        {
            get { return "Правило проставления номера документа ГЖИ Тюмени"; }
        }

        public ValidateResult Validate(DocumentGji document)
        {
            return ValidateResult.Yes();
        }
    }
}
