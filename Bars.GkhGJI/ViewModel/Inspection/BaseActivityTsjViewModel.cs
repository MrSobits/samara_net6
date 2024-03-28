namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using B4;
    using Entities;
    using Enums;

    public class BaseActivityTsjViewModel: BaseActivityTsjViewModel<BaseActivityTsj>
    {
    }

    public class BaseActivityTsjViewModel<T> : BaseViewModel<T>
        where T: BaseActivityTsj
    {
        public override IDataResult List(IDomainService<T> domainService, BaseParams baseParams)
        {
            var serviceDisposal = Container.Resolve<IDomainService<Disposal>>();
            try
            {
                var loadParam = baseParams.GetLoadParam();

                /*
                 * В данном методе по переданному activityTSJ (Деятельность ТСЖ) получаем все распоряжения ГЖИ с типом
                 */
                var activityTsjId = baseParams.Params.GetAs<long>("activityTSJ");

                // Получаем список идентификаторов проверок деятельности ТСЖ для данного activityTSJId
                var listActivityTsjIds = domainService.GetAll()
                    .Where(x => x.ActivityTsj.Id == activityTsjId)
                    .Select(x => x.Id)
                    .ToList();

                // Теперь получаем все основные распоряжения по проверкам деятельности ТСЖ
                var data = serviceDisposal.GetAll()
                    .Where(x => listActivityTsjIds.Contains(x.Inspection.Id) && x.TypeDisposal == TypeDisposalGji.Base)
                    .Select(x => new
                    {
                        x.Inspection.Id,
                        DisposalDocumentNumber = x.DocumentNumber,
                        DisposalDocumentDate = x.DocumentDate,
                        DisposalTypeCheck = x.KindCheck.Name
                    })
                    .Filter(loadParam, Container);

                int totalCount = data.Count();

                return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
            }
            finally 
            {
                Container.Release(serviceDisposal);
            }
            
        }
    }
}