namespace Bars.GkhDi.DomainService.Service
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Utils;
    using Bars.GkhDi.Entities.Service;

    public class ProviderOtherServiceViewModel : BaseViewModel<ProviderOtherService>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<ProviderOtherService> domainService, BaseParams baseParams)
        {
            var otherServiceId = baseParams.Params.GetAs<long>("otherServiceId");

            return domainService.GetAll()
                .Where(x => x.OtherService.Id == otherServiceId)
                .Select(x => new
                {
                    x.Id,
                    Name = x.Provider == null ? x.ProviderName : x.Provider.Name,
                    DateStartContract = x.DateStartContract <= DateTime.MinValue ? null : x.DateStartContract,
                    x.Description,
                    IsActive = x.IsActive ? "Да" : "Нет",
                    x.ProviderName
                })
                .ToListDataResult(this.GetLoadParam(baseParams), this.Container);
        }
    }
}
