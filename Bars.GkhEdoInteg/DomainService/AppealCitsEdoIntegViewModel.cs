namespace Bars.GkhEdoInteg.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhEdoInteg.Entities;

    public class AppealCitsEdoIntegViewModel : BaseViewModel<AppealCitsCompareEdo>
    {
        public override IDataResult List(IDomainService<AppealCitsCompareEdo> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Select(x => new
                {
                    x.AppealCits.Id,
                    x.AppealCits.State,
                    Name = string.Format("{0} ({1})", x.AppealCits.Number, x.AppealCits.NumberGji),  // Для отображения в строке масового выбора
                    x.AppealCits.Number,
                    x.AppealCits.NumberGji,
                    x.AppealCits.DateFrom,
                    x.AppealCits.CheckTime,
                    x.AppealCits.QuestionsCount
                })
                .Order(loadParams)
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            data = data.Order(loadParams).Paging(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }
    }
}
