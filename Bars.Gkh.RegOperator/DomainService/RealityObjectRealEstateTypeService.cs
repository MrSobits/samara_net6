namespace Bars.Gkh.RegOperator.DomainService
{
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.ConfigSections.RegOperator;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.DomainService.Dict.RealEstateType;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Utils;
    using Castle.Windsor;
    using Gkh.Config;

    public class RealityObjectRealEstateTypeService : IRealityObjectRealEstateTypeService
    {
        public IWindsorContainer Container { get; set; }

        public IRealEstateTypeService realEstateTypeService { get; set; }

        public IDataResult GetAutoRealEstateType(RealityObject realityObject)
        {
            var realEstateTypeDomain = Container.ResolveDomain<RealEstateType>();
            var roDomain = Container.ResolveDomain<RealityObject>();

            var config = Container.Resolve<IGkhConfigProvider>().Get<RegOperatorConfig>().GeneralConfig;

            try
            {
                var usageRealEstateType = config.UsageRealEstateType;

                if (usageRealEstateType == UsageRealEstateType.Manually || realityObject == null)
                {
                    return new BaseDataResult(false, "");
                }

                var roTypes = string.Empty;

                if (realEstateTypeService != null)
                {
                    var roTypeIds = realEstateTypeService.GetRealEstateTypes(
                        roDomain.GetAll().Where(x => x.Id == realityObject.Id))
                        .Select(x => x.Value)
                        .FirstOrDefault() ?? new List<long>();

                    roTypes =
                        realEstateTypeDomain.GetAll()
                            .Where(x => roTypeIds.Contains(x.Id))
                            .Select(x => x.Name)
                            .AggregateWithSeparator(", ");
                }

                return new BaseDataResult(roTypes);
            }
            finally
            {
                Container.Release(realEstateTypeService);
                Container.Release(realEstateTypeDomain);
                Container.Release(roDomain);
                Container.Release(config);
            }
        }
    }
}