namespace Bars.Gkh.ClaimWork.ViewModel.Dict
{
    using System.Linq;
    using B4;
    using B4.Utils;
    using Domain;
    using Modules.ClaimWork.Entities;

    public class PetitionToCourtViewModel : BaseViewModel<PetitionToCourtType>
    {
        private readonly IDomainService<StateDutyPetition> _dutyPetitionDomain;

        /// <summary>
        /// .ctor
        /// </summary>
        public PetitionToCourtViewModel(IDomainService<StateDutyPetition> dutyPetitionDomain)
        {
            _dutyPetitionDomain = dutyPetitionDomain;
        }

        #region Overrides of BaseViewModel<PetitionToCourt>

        /// <summary>
        /// Получить список
        /// </summary>
        /// <param name="domainService">Домен</param><param name="baseParams">Базовые параметры</param>
        /// <returns>
        /// Результат получения списка
        /// </returns>
        public override IDataResult List(IDomainService<PetitionToCourtType> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();
            var dutyId = baseParams.Params.GetAsId("dutyId");

            var data = domainService.GetAll()
                .WhereIf(dutyId > 0, z => !_dutyPetitionDomain.GetAll()
                    .Where(x => x.StateDuty.Id == dutyId)
                    .Any(x => x.PetitionToCourtType.Id == z.Id))
                .Filter(loadParam, Container);

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), data.Count());
        }

        #endregion
    }
}