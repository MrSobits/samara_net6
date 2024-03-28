namespace Bars.Gkh.ClaimWork.ViewModel.Dict
{
    using System.Linq;
    using B4;
    using Domain;
    using Modules.ClaimWork.Entities;

    public class DutyPetitionViewModel : BaseViewModel<StateDutyPetition>
    {
        #region Overrides of BaseViewModel<StateDutyPetition>

        /// <summary>
        /// Получить список
        /// </summary>
        /// <param name="domainService">Домен</param><param name="baseParams">Базовые параметры</param>
        /// <returns>
        /// Результат получения списка
        /// </returns>
        public override IDataResult List(IDomainService<StateDutyPetition> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();
            var dutyId = baseParams.Params.GetAsId("dutyId");

            var data = domainService.GetAll()
                .Where(x => x.StateDuty.Id == dutyId)
                .Filter(loadParam, Container);

            return new ListDataResult(data.Order(loadParam).Paging(loadParam), data.Count());
        }

        #endregion
    }
}