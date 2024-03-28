namespace Bars.GkhGji.Regions.Tatarstan.InspectionRules
{
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.InspectionRules;
    using Bars.GkhGji.Regions.Tatarstan.Entities;

    /// <summary>
    /// Поскольку в РТ данный акт должен называтся просто АктПроверки, то переопределяем свойство и заменяем реализацию
    /// </summary>
    public class TatDisposalToActCheckByRoRule : DisposalToActCheckByRoRule
    {
        public IRepository<TatarstanDisposal> TatDisposalRepository { get; set; }

        public override string ResultName => "Акт проверки";

        public override IDataResult ValidationRule(DocumentGji document)
        {
            var result = base.ValidationRule(document);

            if (result.Success)
            {
                var disposal = this.TatDisposalRepository.FirstOrDefault(x => x.Id == document.Id);

                result.Success = disposal?.TypeDocumentGji == this.TypeDocumentInitiator;
            }

            return result;
        }
    }
}
