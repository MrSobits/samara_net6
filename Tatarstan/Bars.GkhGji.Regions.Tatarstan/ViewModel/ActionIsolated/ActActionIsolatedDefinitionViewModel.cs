namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.ActionIsolated
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;

    using NLog.LayoutRenderers;

    public class ActActionIsolatedDefinitionViewModel : BaseViewModel<ActActionIsolatedDefinition>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<ActActionIsolatedDefinition> domainService, BaseParams baseParams)
        {
            var actId = baseParams.Params.GetAsId("documentId");

            return domainService
                .GetAll()
                .Where(x => x.Act.Id == actId)
                .Select(x => new
                {
                    x.Id,
                    x.Number,
                    x.Date,
                    x.ExecutionDate,
                    Official = x.Official.Fio,
                    x.DefinitionType
                })
                .ToListDataResult(baseParams.GetLoadParam(), this.Container);
        }
    }
}