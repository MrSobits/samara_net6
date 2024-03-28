using Bars.B4.DataAccess;

namespace Bars.Gkh.Overhaul.Services.Impl
{
    using System.Linq;
    using Bars.B4.Config;
    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.DomainService;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Services.DataContracts;
    using Bars.Gkh.Services.DataContracts;

    public partial class Service
    {       
        public GetDataOfKonsResponse GetDataOfKons(string roId)
        {
            var dataOfKons = this.GetDataOfKons(roId.ToLong());

            return new GetDataOfKonsResponse
                       {
                           DataOfKons = dataOfKons,
                           Result = dataOfKons.Any() ? Result.NoErrors : Result.DataNotFound
                       };
        }

        public DataOfKonsWcfProxy[] GetDataOfKons(long roId)
        {
            var realObjStructElDomain = Container.ResolveDomain<RealityObjectStructuralElement>();
            var configProvider = Container.Resolve<IConfigProvider>();
            var realObjStructElService = Container.Resolve<IRealityObjectStructElementService>();

            try
            {
                var structElInOverhaul = configProvider.GetConfig().AppSettings.GetAs<bool>("Service_StructElForOverhaul");
                IQueryable<RealityObjectStructuralElement> structElInOverhaulQuery = realObjStructElService != null
                    ? realObjStructElService.GetElementsForLongProgram()
                    : null;

                return realObjStructElDomain.GetAll()
                    .Where(x => x.RealityObject.Id == roId)
                    .Where(x => x.State.StartState)
                    .WhereIf(structElInOverhaul && structElInOverhaulQuery != null, x => structElInOverhaulQuery.Any(y => y.Id == x.Id))
                    .Select(x => new DataOfKonsWcfProxy()
                    {
                        GroupKons = x.StructuralElement.Group.Name,
                        NameOfKons = x.StructuralElement.Name,
                        DateLastDid = x.LastOverhaulYear,
                        Wear = x.Wearout.RoundDecimal(2),
                        Value = x.Volume.RoundDecimal(2),
                        UnitMeasure = x.StructuralElement.UnitMeasure.Name
                    })
                    .ToArray();
            }
            finally
            {
                Container.Release(realObjStructElDomain);
                Container.Release(realObjStructElService);
                Container.Release(configProvider);
            }
        }
    }
}