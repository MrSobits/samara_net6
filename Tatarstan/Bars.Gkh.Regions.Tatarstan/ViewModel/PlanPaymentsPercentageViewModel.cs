namespace Bars.Gkh.Regions.Tatarstan.ViewModel
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Regions.Tatarstan.Entities.Dicts;

    /// <summary>
    /// Представление <see cref="PlanPaymentsPercentage"/>
    /// </summary>
    public class PlanPaymentsPercentageViewModel : BaseViewModel<PlanPaymentsPercentage>
    {
        /// <summary>
        /// Менеджер пользователей
        /// </summary>
        public IGkhUserManager UserManager { get; set; }

        /// <inheritdoc />
        public override IDataResult List(IDomainService<PlanPaymentsPercentage> domainService, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var contragentIds = this.UserManager.GetContragentIds();

            var query = domainService.GetAll()
                .WhereIf(contragentIds.IsNotEmpty(), x => contragentIds.Contains(x.PublicServiceOrg.Contragent.Id))
                .Select(x => new
                {
                    x.Id,
                    x.DateStart,
                    x.DateEnd,
                    x.Percentage,
                    Resource = x.Resource.Name,
                    Service = x.Service.Name,
                    PublicServiceOrg = x.PublicServiceOrg.Contragent.Name
                })
                .Filter(loadParams, this.Container);

            return new ListDataResult(query.Order(loadParams).Paging(loadParams).ToList(), query.Count());
        }

        /// <inheritdoc />
        public override IDataResult Get(IDomainService<PlanPaymentsPercentage> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();

            var entity = domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.DateStart,
                    x.DateEnd,
                    x.Percentage,
                    x.Resource,
                    x.Service,
                    PublicServiceOrg = new
                    {
                        Id = x.PublicServiceOrg.Id,
                        ContragentName = x.PublicServiceOrg.Contragent.Name
                    } 
                })
                .FirstOrDefault(x => x.Id == id);

            return new BaseDataResult(entity);
        }
    }
}