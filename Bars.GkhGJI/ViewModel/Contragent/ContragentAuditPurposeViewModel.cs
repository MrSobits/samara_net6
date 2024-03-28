namespace Bars.GkhGji.ViewModel
{
    using System;
    using System.Linq;
    using B4.DataAccess;
    using B4.Utils;
    using Bars.B4;
    using Entities;
    using Gkh.Domain;
    using Gkh.Entities;
    using GkhGji.Entities.Dict;

    public class ContragentAuditPurposeViewModel : BaseViewModel<ContragentAuditPurpose>
    {
        public override IDataResult List(IDomainService<ContragentAuditPurpose> domainService, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var contrId = loadParams.Filter.GetAs<long>("contragentId");

            var existPurposes = domainService.GetAll()
                .Where(x => x.Contragent.Id == contrId)
                .Select(x => new
                {
                    x.AuditPurpose.Id,
                    x.Contragent,
                    x.AuditPurpose,
                    EntityId = x.Id,
                    AuditPurposeName = x.AuditPurpose.Name,
                    x.LastInspDate
                })
                .AsEnumerable()
                .GroupBy(x => x.AuditPurpose.Id)
                .ToDictionary(x => x.Key, y => y.FirstOrDefault());

            var auditPurposeDomain = Container.ResolveDomain<AuditPurposeGji>();
            var contragentDomain = Container.ResolveDomain<Contragent>();

            try
            {
                var contragent = contragentDomain.Load(contrId);

                var data = auditPurposeDomain.GetAll()
                    .ToArray()
                    .Select(x =>
                    {
                        var purpose = existPurposes.Get(x.Id) ?? new
                        {
                            x.Id,
                            Contragent = contragent,
                            AuditPurpose = x,
                            EntityId = 0L,
                            AuditPurposeName = x.Name,
                            LastInspDate = (DateTime?) null
                        };

                        return purpose;
                    })
                    .AsQueryable()
                    .Filter(loadParams, Container);

                 return new ListDataResult(data.Order(loadParams).Paging(loadParams), data.Count());
            }
            finally
            {
                Container.Release(auditPurposeDomain);
            }
        }
    }
}