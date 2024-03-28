namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using B4;
    using B4.Utils;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;

    using Entities;
    using Enums;

    /// <inheritdoc />
    public class BaseStatementViewModel: BaseStatementViewModel<BaseStatement>
    {
    }

    /// <inheritdoc />
    public class BaseStatementViewModel<T> : BaseViewModel<T>
        where T: BaseStatement
    {
        /// <inheritdoc />
        /// <param name="domainService"></param>
        /// <param name="baseParams">
        /// realityObjectId - жилой дом <para/>
        /// showCloseInspections - признак bool показывать или нет закрытые проверки
        /// </param>
        public override IDataResult List(IDomainService<T> domainService, BaseParams baseParams)
        {
            var serviceInspRobject = this.Container.ResolveDomain<InspectionGjiRealityObject>();
            var serviceView = this.Container.ResolveDomain<ViewBaseStatement>();

            try
            {
                var realityObjectId = baseParams.Params.GetAs<long>("realityObjectId");
                var showCloseInspections = baseParams.Params.GetAs("showCloseInspections", true);

                return serviceView.GetAll()
                    .WhereIf(realityObjectId > 0, y => serviceInspRobject.GetAll().Any(x => x.RealityObject.Id == realityObjectId && x.Inspection.Id == y.Id))
                    .WhereIf(!showCloseInspections, x => x.State == null || !x.State.FinalState)
                    .Select(x => new
                    {
                        x.Id,
                        Municipality = x.MunicipalityNames,
                        MoSettlement = x.MoNames,
                        PlaceName = x.PlaceNames,
                        x.RealityObjectCount,
                        x.ContragentName,
                        x.PersonInspection,
                        x.InspectionNumber,
                        TypeJurPerson = x.PersonInspection == PersonInspection.PhysPerson ? null : x.TypeJurPerson,
                        x.IsDisposal,
                        x.RealObjAddresses,
                        x.DocumentNumber,
                        x.State,
                        x.RequestType
                    })
                    .ToListDataResultWithPaging(baseParams.GetLoadParam());
            }
            finally 
            {
                this.Container.Release(serviceView);
                this.Container.Release(serviceInspRobject);
            }
        }

        /// <inheritdoc />
        public override IDataResult Get(IDomainService<T> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();
            var obj = domainService.Get(id);

            var serviceDiposal = this.Container.ResolveDomain<Disposal>();
            var inspectionRiskDomain = this.Container.ResolveDomain<InspectionRisk>();

            try
            {
                var disposal = serviceDiposal.GetAll().FirstOrDefault(x => x.Inspection.Id == id && x.TypeDisposal == TypeDisposalGji.Base);
                var risk = inspectionRiskDomain.GetAll().Where(x => x.Inspection.Id == id).FirstOrDefault(x => !x.EndDate.HasValue);

                obj.DisposalId = disposal?.Id;
                obj.RiskCategory = risk?.RiskCategory;
                obj.RiskCategoryStartDate = risk?.StartDate;

                return new BaseDataResult(obj);
            }
            finally 
            {
                this.Container.Release(inspectionRiskDomain);
                this.Container.Release(serviceDiposal);
            }
        }
    }
}