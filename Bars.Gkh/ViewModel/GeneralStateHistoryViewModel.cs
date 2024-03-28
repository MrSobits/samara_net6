namespace Bars.Gkh.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils.Annotations;
    using Bars.Gkh.Entities;
    using Bars.Gkh.GeneralState;

    public class GeneralStateHistoryViewModel : BaseViewModel<GeneralStateHistory>
    {
        public IGeneralStateHistoryService StateService { get; set; }

        /// <inheritdoc />
        public override IDataResult List(IDomainService<GeneralStateHistory> domainService, BaseParams baseParams)
        {
            var stateCode = baseParams.Params.GetAs<string>("stateCode");
            var entityId = baseParams.Params.GetAs<long>("entityId");

            ArgumentChecker.NotNull(stateCode, "Код состояния");
            ArgumentChecker.NotNull(entityId, "Id сущности");

            var loadParam = this.GetLoadParam(baseParams);

            var info = this.StateService.GetStateHistoryInfo(stateCode);

            if (info == null)
            {
                return new BaseDataResult(false, "Тип  логируемого свойства не найден");
            }

            var data = domainService.GetAll()
                .Where(x => x.Code == stateCode && x.EntityId == entityId)
                .AsEnumerable()
                .Select(
                    x => new
                    {
                        x.ChangeDate,
                        x.UserLogin,
                        x.UserName,
                        StartState = this.StateService.GetDisplayValue(info, x.StartState),
                        FinalState = this.StateService.GetDisplayValue(info, x.FinalState),
                        x.Id
                    })
                .AsQueryable()
                .Filter(loadParam, this.Container)
                .ToList();

            return new ListDataResult(data.AsQueryable().Order(loadParam).Paging(loadParam).ToList(), data.Count);
        }
    }
}