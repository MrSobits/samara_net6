namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.TatarstanProtocolGji
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanProtocolGji;

    public class TatarstanDocumentWitnessViewModel : BaseViewModel<TatarstanDocumentWitness>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<TatarstanDocumentWitness> domainService, BaseParams baseParams)
        {
            var documentId = baseParams.Params.GetAsId("documentId");

            return domainService.GetAll()
                .Where(x => x.DocumentGji.Id == documentId)
                .ToListDataResult(baseParams.GetLoadParam(), this.Container);
        }
    }
}
