namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.TatarstanProtocolGji
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanProtocolGji;

    public class TatarstanProtocolGjiViolationViewModel : BaseViewModel<TatarstanProtocolGjiViolation>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<TatarstanProtocolGjiViolation> domainService, BaseParams baseParams)
        {
            var protocolId = baseParams.Params.GetAsId("documentId");

            return domainService.GetAll()
                .WhereIf(protocolId != default(long), x => x.TatarstanProtocolGji.Id == protocolId)
                .Select(x => new
                {
                    x.Id,
                    ViolationGji = x.ViolationGji.Name,
                    NormativeDoc = x.ViolationGji.NormativeDocNames
                }).ToListDataResult(baseParams.GetLoadParam(), this.Container);
        }
    }
}
