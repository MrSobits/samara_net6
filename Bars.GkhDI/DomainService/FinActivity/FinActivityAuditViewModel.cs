namespace Bars.GkhDi.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhDi.Entities;

    public class FinActivityAuditViewModel : BaseViewModel<FinActivityAudit>
    {
        public override IDataResult List(IDomainService<FinActivityAudit> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var disclosureInfoId = baseParams.Params.GetAs<long>("disclosureInfoId");

            var serviceDi = Container.Resolve<IDomainService<DisclosureInfo>>();
            var disclosureInfo = serviceDi.Load(disclosureInfoId);

            if (disclosureInfo == null)
            {
                return new ListDataResult();
            }

            // отображаем загруженные документы данного раскрытия по годам (вплоть до 2 лет назад)
            var data = domainService
                .GetAll()
                .Where(x => x.ManagingOrganization.Id == disclosureInfo.ManagingOrganization.Id
                    && x.Year <= disclosureInfo.PeriodDi.DateStart.Value.Year && x.Year >= disclosureInfo.PeriodDi.DateStart.Value.Year - 2)
                .Select(x => new
                             {
                                  x.Id,
                                  x.TypeAuditStateDi,
                                  x.Year,
                                  x.File
                             })
                .Filter(loadParams, Container);

            var totalCount = data.Count();
            data = data.Order(loadParams).Paging(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }
    }
}