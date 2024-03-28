namespace Bars.GkhDi.DomainService
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;

    using Entities;

    public class GroupWorkPprViewModel : BaseViewModel<GroupWorkPpr>
    {
        public override IDataResult List(IDomainService<GroupWorkPpr> domainService, BaseParams baseParams)
        {
            // baseServiceId идентификатор базовой услуги в раскрытии, 
            // у нее получаем идентификатор шаблонной услуги, по которой сортируем группы работ
            var baseServiceId = baseParams.Params.GetAs<long>("baseServiceId");

            var templateServiceId = this.Container.Resolve<IDomainService<BaseService>>()
                                                  .GetAll()
                                                  .Where(x => x.Id == baseServiceId)
                                                  .Select(x => x.TemplateService.Id)
                                                  .FirstOrDefault();

            var loadParams = GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Where(x => !x.IsNotActual)
                .WhereIf(templateServiceId > 0, x => x.Service.Id == templateServiceId)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Code,
                    Service = x.Service.Name
                })
                .Filter(loadParams, Container);

            var totalCount = data.Count();

            data = data.Order(loadParams).Paging(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }
    }
}