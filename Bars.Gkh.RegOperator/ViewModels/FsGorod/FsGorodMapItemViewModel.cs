namespace Bars.Gkh.RegOperator.ViewModels.FsGorod
{
    using System.Linq;
    using B4;
    using Entities;

    public class FsGorodMapItemViewModel : BaseViewModel<FsGorodMapItem>
    {
        /// <summary>
        /// Получить список
        /// </summary>
        /// <param name="domainService">Домен</param><param name="baseParams">Базовые параметры</param>
        /// <returns>
        /// Результат получения списка
        /// </returns>
        public override IDataResult List(IDomainService<FsGorodMapItem> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();
            var infoId = baseParams.Params.GetAs<long>("infoId", ignoreCase: true);

            var data = domainService.GetAll()
                .Where(x => x.ImportInfo.Id == infoId)
                .Filter(loadParam, Container);

            return new ListDataResult(data.Order(loadParam).Paging(loadParam), data.Count());
        }
    }
}