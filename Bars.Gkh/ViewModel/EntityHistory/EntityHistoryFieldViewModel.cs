namespace Bars.Gkh.ViewModel.EntityHistory
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Utils;

    public class EntityHistoryFieldViewModel : BaseViewModel<EntityHistoryField>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<EntityHistoryField> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();

            return domainService.GetAll()
                .Where(x => x.EntityHistoryInfo.Id == id)
                .ToListDataResult(baseParams.GetLoadParam());
        }
    }
}