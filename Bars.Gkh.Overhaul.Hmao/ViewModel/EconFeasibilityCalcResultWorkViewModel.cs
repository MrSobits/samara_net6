namespace Bars.Gkh.Overhaul.Hmao.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    /// <summary>
    /// Представление записи 3 этапа
    /// </summary>
    public class EconFeasibilityCalcResultWorkViewModel : BaseViewModel<EconFeasibilitiWork>
    {
        /// <summary>
        /// Получить список
        /// </summary>
        /// <param name="domainService">Домен сервис <see cref="EconFeasibilitiWork" /></param>
        /// <param name="baseParams">Базовое параметры</param>
        /// <returns>Список</returns>
        public override IDataResult List(IDomainService<EconFeasibilitiWork> domainService, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var id = loadParams.Filter.GetAs("ResultId", 0L);

            var data = domainService.GetAll()
                .Where(x => x.ResultId.Id == id)
                .Select(x => new
                {
                    x.Id,                
                    x.RecorWorkdId.Year,
                    x.RecorWorkdId.Sum,
                    x.RecorWorkdId.CommonEstateObjects,
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Paging(loadParams).ToList(), data.Count());
           
        }
    }
}