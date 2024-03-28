namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.Dict
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict.KnmActions;
    using Bars.GkhGji.Regions.Tatarstan.Enums;

    public class KnmActionViewModel : BaseViewModel<KnmAction>
    {
        public override IDataResult List(IDomainService<KnmAction> domainService, BaseParams baseParams)
        {
            var ids = baseParams.Params.GetAs<string>("Id").ToLongArray();
            var kindAction = baseParams.Params.GetAs<KindAction?>("kindAction");
            var currentDictOnly = baseParams.Params.GetAs<bool>("currentDictOnly");

            var kindCheckId = baseParams.Params.GetAsId("kindCheckId");
            var controlTypeId = baseParams.Params.GetAsId("controlTypeId");

            var knmTypesDict = new Dictionary<long, IEnumerable<KnmTypeDto>>();
            var controlTypesDict = new Dictionary<long, IEnumerable<KnmControlTypeDto>>();
            var kindActionDict = new Dictionary<long, IEnumerable<KindAction>>();

            var knmActionKindActionDomain = this.Container.ResolveDomain<KnmActionKindAction>();

            using (this.Container.Using(knmActionKindActionDomain))
            {
                if (!currentDictOnly)
                {
                    var knmActionKnmTypeDomain = this.Container.ResolveDomain<KnmActionKnmType>();
                    var knmTypeKindCheckDomain = this.Container.ResolveDomain<KnmTypeKindCheck>();
                    var knmActionControlTypeDomain = this.Container.ResolveDomain<KnmActionControlType>();

                    using (this.Container.Using(knmActionKnmTypeDomain, knmTypeKindCheckDomain, knmActionControlTypeDomain))
                    {
                        knmTypesDict = knmActionKnmTypeDomain.GetAll()
                            .Join(knmTypeKindCheckDomain.GetAll(),
                                a => a.KnmTypes.Id,
                                b => b.KnmTypes.Id,
                                (a, b) => new
                                {
                                    KnmActionId = a.KnmAction.Id,
                                    KnmTypesId = a.KnmTypes.Id,
                                    Name = string.Join(", ", b.KindCheckGji.Name),
                                    KindCheckId = b.KindCheckGji.Id
                                })
                            .AsEnumerable()
                            .GroupBy(x => x.KnmActionId)
                            .ToDictionary(x => x.Key,
                                y => y.Select(z => new KnmTypeDto
                                {
                                    KnmTypesId = z.KnmTypesId,
                                    Name = z.Name,
                                    KindCheckId = z.KindCheckId
                                }));

                        controlTypesDict = knmActionControlTypeDomain.GetAll()
                            .GroupBy(x => x.KnmAction.Id)
                            .ToDictionary(x => x.Key,
                                y => y.Select(z => new KnmControlTypeDto { Id = z.ControlType.Id, Name = z.ControlType.Name }));
                    }
                }

                kindActionDict = knmActionKindActionDomain.GetAll()
                    .GroupBy(x => x.KnmAction.Id)
                    .ToDictionary(x => x.Key,
                        y => y.Select(z => z.KindAction));

                return domainService.GetAll()
                    .WhereIf(ids.Any(), x => ids.Contains(x.Id))
                    .AsEnumerable()
                    .Select(x => new
                    {
                        x.Id,
                        Name = x.ActCheckActionType?.GetDisplayName() ?? string.Empty,
                        x.ActCheckActionType,
                        x.ErvkId,
                        KindAction = kindActionDict.Get(x.Id),
                        KnmType = knmTypesDict.Get(x.Id),
                        ControlType = controlTypesDict.Get(x.Id)
                    })
                    .WhereIf(kindAction != null, x => x.KindAction?.Any(y => y == kindAction) ?? false)
                    .WhereIf(kindCheckId > 0, x => x.KnmType?.Any(y => y.KindCheckId == kindCheckId) ?? false)
                    .WhereIf(controlTypeId > 0, x => x.ControlType?.Any(y => y.Id == controlTypeId) ?? false)
                    .ToListDataResult(baseParams.GetLoadParam(), this.Container);
            }
        }

        /// <summary>
        /// Информация о виде КНМ
        /// </summary>
        private class KnmTypeDto
        {
            /// <summary>
            /// Идентификатор справочника
            /// </summary>
            public long KnmTypesId { get; set; }

            /// <summary>
            /// Наименование
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Идентификатор вида проверки
            /// </summary>
            public long KindCheckId { get; set; }
        }

        /// <summary>
        /// Информация о виде контроля
        /// </summary>
        private class KnmControlTypeDto
        {
            /// <summary>
            /// Идентификатор справочника
            /// </summary>
            public long Id { get; set; }

            /// <summary>
            /// Наименование
            /// </summary>
            public string Name { get; set; }
        }
    }
}