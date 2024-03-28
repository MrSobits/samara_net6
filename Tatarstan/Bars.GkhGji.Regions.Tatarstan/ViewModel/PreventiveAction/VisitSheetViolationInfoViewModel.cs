namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.PreventiveAction
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;

    using NHibernate.Linq;

    /// <summary>
    /// View-модель для <see cref="VisitSheetViolationInfo"/>
    /// </summary>
    public class VisitSheetViolationInfoViewModel : BaseViewModel<VisitSheetViolationInfo>
    {
        /// <inheritdoc />
        public override IDataResult Get(IDomainService<VisitSheetViolationInfo> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();

            var entity = domainService.Get(id);

            return new BaseDataResult(new
            {
                entity.Id,
                entity.VisitSheet,
                RealityObject = new
                {
                    entity.RealityObject.Id,
                    FullAddress = entity.RealityObject?.FiasAddress.AddressName
                },
                entity.Description
            });
        }

        /// <inheritdoc />
        public override IDataResult List(IDomainService<VisitSheetViolationInfo> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();

            var visitSheetViolationDomain = this.Container.ResolveDomain<VisitSheetViolation>();

            using (this.Container.Using(visitSheetViolationDomain))
            {
                return domainService.GetAll()
                    .Where(x => x.VisitSheet.Id == id)
                    .LeftJoin(visitSheetViolationDomain.GetAll(),
                        x => x.Id,
                        y => y.ViolationInfo.Id,
                        (x, y) => new
                        {
                            ViolationInfo = x,
                            Violation = y
                        })
                    .AsEnumerable()
                    .GroupBy(x => x.ViolationInfo,
                        (x, y) => new
                        {
                            x.Id,
                            RealityObjectId = x.RealityObject.Id,
                            Address = x.RealityObject.FiasAddress?.AddressName,
                            Violation = string.Join("<br>", y.Select(z => z.Violation?.Violation.Name)),
                        })
                    .ToListDataResult(baseParams.GetLoadParam());
            }
        }
    }
}