namespace Bars.GkhGji.Regions.Tatarstan.DomainService.Decision.Impl
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Decision;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;

    using Castle.Windsor;

    using global::Quartz.Util;

    public class DecisionInspBaseService : IDecisionInspBaseService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult ListInspectionBaseType(BaseParams baseParams)
        {
            var kindCheckId = baseParams.Params.GetAsId("kindCheckId");
            var recordIds = baseParams.Params.GetAs<string>("recordIds").ToLongArray();

            var desInspBaseDomainService = this.Container.Resolve<IDomainService<DecisionInspectionBase>>();
            var inspBaseTypeDomainService = this.Container.Resolve<IDomainService<InspectionBaseType>>();
            var inspBaseTypeKindCheckDomainService = this.Container.Resolve<IDomainService<InspectionBaseTypeKindCheck>>();
            
            using (this.Container.Using(inspBaseTypeDomainService, inspBaseTypeKindCheckDomainService, desInspBaseDomainService))
            {
                // Получаем список идентификаторов типа InspectionBaseType, это нужно для того чтобы исключить уже существующие в текщуем "Решении" Основания Проведения
                var usedRecordIds = desInspBaseDomainService.GetAll()
                    .Where(x => recordIds.Contains(x.Id))
                    .Select(x => x.InspectionBaseType.Id)
                    .ToArray();

                // Получаем все записи у которых есть ErknmCode
                var inspBaseTypeQuery = inspBaseTypeDomainService.GetAll()
                    .Where(x => x.ErknmCode != null && x.ErknmCode != string.Empty)
                    .WhereIf(usedRecordIds.Any(), x => !usedRecordIds.Contains(x.Id));

                var kindCheckDict = inspBaseTypeKindCheckDomainService.GetAll()
                    .Where(x => inspBaseTypeQuery.Any(y => y.Id == x.InspectionBaseType.Id && x.KindCheck.Id == kindCheckId))
                    .ToDictionary(x => new Tuple<long, long>(x.InspectionBaseType.Id, x.KindCheck.Id));

                return inspBaseTypeQuery
                    .Select(x => new
                    {
                        x.Id,
                        x.ErknmCode,
                        x.Name,
                        x.HasTextField,
                        x.HasDate,
                        x.HasRiskIndicator,
                    })
                    .AsEnumerable()
                    .Select(x => new
                    {
                        x.Id,
                        x.ErknmCode,
                        x.Name,
                        x.HasTextField,
                        x.HasDate,
                        x.HasRiskIndicator,
                        KindCheck = kindCheckDict.TryGetAndReturn(Tuple.Create(x.Id, kindCheckId))
                    })
                    .Where(x => x.KindCheck != null)
                    .ToListDataResult(baseParams.GetLoadParam(), this.Container);
            }
        }
    }
}