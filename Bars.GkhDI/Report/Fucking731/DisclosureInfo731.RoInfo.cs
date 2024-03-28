namespace Bars.GkhDi.Report.Fucking731
{
    using System.Collections.Generic;
    using System.Linq;
    using B4.DataAccess;
    using B4.Utils;
    using Entities;
    using Gkh.DomainService;

    public partial class DisclosureInfo731
    {
        protected void FillRobjectInfo(ManOrgRecord record, DisclosureInfo dinfo)
        {
            if (dinfo == null)
            {
                return;
            }

            var manorgRoService = Container.Resolve<IManagingOrgRealityObjectService>();
            var diroDomain = Container.ResolveDomain<DisclosureInfoRealityObj>();

            var filterRo =
                manorgRoService
                    .GetAllActive(dinfo.PeriodDi.DateStart.GetValueOrDefault(), dinfo.PeriodDi.DateEnd)
                    .Where(x => x.ManOrgContract.ManagingOrganization.Id == dinfo.ManagingOrganization.Id)
                    .Select(x => x.RealityObject);

            var diRobjects = diroDomain.GetAll()
                .Where(x => x.PeriodDi.Id == dinfo.PeriodDi.Id)
                .Where(y => filterRo.Any(x => x.Id == y.RealityObject.Id))
                .Select(x => new Robject
                {
                    DiRoId = x.Id,
                    RoId = x.RealityObject.Id,
                    Address = x.RealityObject.Address
                });

            WarmCache(diRobjects);

            record.Robject = diRobjects.OrderBy(x => x.Address).ToList();

            foreach (var robject in record.Robject)
            {
                FillRobject(robject);
            }
        }

        protected void FillRobject(Robject record)
        {
            record.ObjectManagService = DictManagService.Get(record.DiRoId) ?? new ObjectManagService[0];

            record.ObjectRepairProvider = DictRepairProvider.Get(record.DiRoId) ?? new ObjectRepairProvider[0];

            record.ObjectHousingProvider = DictHousingProvider.Get(record.DiRoId) ?? new ObjectHousingProvider[0];

            record.ObjectAdditionalProvider = DictAdditionalProvider.Get(record.DiRoId) ?? new ObjectAdditionalProvider[0];

            record.ObjectCommunalService = DictCommunalService.Get(record.DiRoId) ?? new ObjectCommunalService[0];

            record.ObjectCommonFacility = DictCommonFacilities.Get(record.DiRoId) ?? new ObjectCommonFacility[0];

            record.ObjectCommunalResource = DictCommunalResource.Get(record.DiRoId) ?? new ObjectCommunalResource[0];

            record.ObjectReductionWork = DictReductionWork.Get(record.DiRoId) ?? new ObjectReductionWork[0];

            record.ObjectReductionPayment = DictReductionPayment.Get(record.DiRoId) ?? new ObjectReductionPayment[0];
        }

        private static string FormatDecimal(decimal? dec, string defaultValue = null)
        {
            return dec.HasValue ? dec.Value.ToString("0.00") : defaultValue;
        }
    }

    internal static class SequenceNumberSetter
    {
        public static IEnumerable<T> SetNumber<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                return new List<T>();
            }

            var numberProperty = typeof(T).GetProperties().FirstOrDefault(x => x.Name.ToLower() == "number");

            if (numberProperty != null)
            {
                var i = 0;

                foreach (var item in enumerable)
                {
                    numberProperty.SetValue(item, ++i, null);
                }
            }

            return enumerable;
        }
    }
}