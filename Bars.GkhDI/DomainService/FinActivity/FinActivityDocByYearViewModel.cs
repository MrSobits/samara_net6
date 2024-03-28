namespace Bars.GkhDi.DomainService
{
    using System.Linq;
    using B4;

    using Entities;

    public class FinActivityDocByYearViewModel : BaseViewModel<FinActivityDocByYear>
    {
        public override IDataResult List(IDomainService<FinActivityDocByYear> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);
            var disclosureInfoId = baseParams.Params.GetAs<long>("disclosureInfoId");
            var serviceDi = this.Container.Resolve<IDomainService<DisclosureInfo>>();

            var disclosureInfo = serviceDi.Load(disclosureInfoId);

            if (disclosureInfo == null)
            {
                return new ListDataResult();
            }

            var data = domainService
                .GetAll()

                // Отображаем файлы только данного раскрытия по годам от меньше на 2 года периода раскрытия до равно году периода раскрытия
                .Where(x => x.ManagingOrganization.Id == disclosureInfo.ManagingOrganization.Id
                    && x.Year <= disclosureInfo.PeriodDi.DateStart.Value.Year && x.Year >= disclosureInfo.PeriodDi.DateStart.Value.Year - 2)
                .Filter(loadParams, this.Container);


            var totalCount = data.Count();
            data = data.Order(loadParams).Paging(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }
    }
}
