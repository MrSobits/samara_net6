namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.ControlList
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanControlList;

    public class ControlListQuestionViewModel : BaseViewModel<TatarstanControlListQuestion>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<TatarstanControlListQuestion> domainService, BaseParams baseParams)
        {
            var controlListId = baseParams.Params.GetAsId("controlListId");
            return domainService.GetAll()
                .Where(x => x.ControlList.Id == controlListId)
                .ToListDataResult(this.GetLoadParam(baseParams), this.Container);
        }
    }
}
