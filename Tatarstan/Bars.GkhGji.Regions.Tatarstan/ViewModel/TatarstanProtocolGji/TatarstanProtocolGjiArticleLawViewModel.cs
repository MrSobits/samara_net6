namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.TatarstanProtocolGji
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanProtocolGji;

    public class TatarstanProtocolGjiArticleLawViewModel : BaseViewModel<TatarstanProtocolGjiArticleLaw>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<TatarstanProtocolGjiArticleLaw> domainService, BaseParams baseParams)
        {
            var protocolId = baseParams.Params.GetAsId("documentId");
            return domainService.GetAll()
                .WhereIf(protocolId != default(long), x => x.TatarstanProtocolGji.Id == protocolId)
                .Select(x => new
                {
                    x.Id,
                    ArticleLaw = x.ArticleLaw.Name,
                    x.ArticleLaw.Description
                })
                .ToListDataResult(baseParams.GetLoadParam(), this.Container);
        }
    }
}
