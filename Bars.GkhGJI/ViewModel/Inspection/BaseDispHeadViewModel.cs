namespace Bars.GkhGji.ViewModel
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    /// <inheritdoc />
    public class BaseDispHeadViewModel : BaseDispHeadViewModel<BaseDispHead>
    {
    }

    /// <inheritdoc />
    public class BaseDispHeadViewModel<T> : BaseViewModel<T>
        where T: BaseDispHead
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<T> domainService, BaseParams baseParams)
        {
            var service = this.Container.ResolveDomain<ViewBaseDispHead>();
            var serviceInsRo = this.Container.ResolveDomain<InspectionGjiRealityObject>();

            try
            {
                var loadParams = baseParams.GetLoadParam();

                var dateStart = baseParams.Params.GetAs("dateStart", DateTime.MinValue);
                var dateEnd = baseParams.Params.GetAs("dateEnd", DateTime.MinValue);
                var realityObjectId = baseParams.Params.GetAs<long>("realityObjectId");
                var showCloseInspections = baseParams.Params.GetAs("showCloseInspections", true);

                return service.GetAll()
                    .WhereIf(dateStart != DateTime.MinValue, x => x.DispHeadDate >= dateStart)
                    .WhereIf(dateEnd != DateTime.MinValue, x => x.DispHeadDate <= dateEnd)
                    .WhereIf(realityObjectId > 0, y => serviceInsRo.GetAll()
                        .Any(x => x.RealityObject.Id == realityObjectId && x.Inspection.Id == y.Id))
                    .WhereIf(!showCloseInspections, x => x.State == null || !x.State.FinalState)
                    .Select(x => new
                    {
                        x.Id,
                        x.MunicipalityNames,
                        MoSettlement = x.MoNames,
                        PlaceName = x.PlaceNames,
                        x.ContragentName,
                        x.DispHeadDate,
                        x.RealityObjectCount,
                        Head = x.HeadFio,
                        x.InspectorNames,
                        x.InspectionNumber,
                        DispHeadNumber = x.DocumentNumber,
                        x.DisposalTypeSurveys,
                        x.PersonInspection,
                        TypeJurPerson = x.PersonInspection == PersonInspection.PhysPerson ? null : x.TypeJurPerson,
                        x.State
                    })
                    .ToListDataResult(loadParams, this.Container);
            }
            finally 
            {
                this.Container.Release(service);
                this.Container.Release(serviceInsRo);
            }
        }

        /// <inheritdoc />
        public override IDataResult Get(IDomainService<T> domainService, BaseParams baseParams)
        {
            var serviceDisposal = this.Container.ResolveDomain<Disposal>();
            var serviceInsDocRef = this.Container.ResolveDomain<InspectionDocGjiReference>();
            var inspectionRiskDomain = this.Container.ResolveDomain<InspectionRisk>();

            try
            {
                var id = baseParams.Params["id"].To<long>();
                var obj = domainService.Get(id);

                // Получаем Распоряжение
                var disposal = serviceDisposal.GetAll().FirstOrDefault(x => x.Inspection.Id == id);
                if (disposal != null)
                {
                    obj.DisposalId = disposal.Id;
                }

                //получаем предыдущий документ 
                var prevDoc = serviceInsDocRef.GetAll().FirstOrDefault(x => x.Inspection.Id == id);
                var risk = inspectionRiskDomain.GetAll().Where(x => x.Inspection.Id == id).FirstOrDefault(x => !x.EndDate.HasValue);

                obj.PrevDocument = prevDoc?.Document;
                // TODO: 
               // obj.RiskCategory = risk?.RiskCategory;
                obj.RiskCategoryStartDate = risk?.StartDate;
                
                return new BaseDataResult(obj);
            }
            finally 
            {
                this.Container.Release(inspectionRiskDomain);
                this.Container.Release(serviceDisposal);
                this.Container.Release(serviceInsDocRef);
            }
            
        }
    }
}