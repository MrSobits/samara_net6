namespace Bars.GkhGji.Regions.Tatarstan.ViewModel
{
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Base;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class KindCheckGjiViewModel : BaseViewModel<KindCheckGji>
    {
        public override IDataResult List(IDomainService<KindCheckGji> domainService, BaseParams baseParams)
        {
            var ids = baseParams.Params.GetAs<string>("Id");
            var typeEntityName = baseParams.Params.GetAs("typeEntity", string.Empty);
            var kindCheckIdsSelected = baseParams.Params.GetAs<string>("kindCheckIds").ToLongArray();
            var listIds = ids.ToLongArray();
            var typeEntity = Type.GetType(typeEntityName);
            HashSet<long> existKindCheckIds = null;

            if (typeEntity != null)
            {
                var type= typeof(IDomainService<>).MakeGenericType(typeEntity);
                var domainServ = (IDomainService)this.Container.Resolve(type);

                existKindCheckIds = domainServ.GetAll()
                    .To<IQueryable<IIncludeKindCheckGji>>()
                    .Where(x => !kindCheckIdsSelected.Contains(x.KindCheckGji.Id))
                    .Select(x => x.KindCheckGji.Id)
                    .ToHashSet();
            }

            return domainService.GetAll()
                .WhereIf(ids != null, x => listIds.Contains(x.Id))
                .WhereIf(existKindCheckIds != null, x => !existKindCheckIds.Contains(x.Id))
                .ToListDataResult(baseParams.GetLoadParam(), this.Container);
        }
    }
}