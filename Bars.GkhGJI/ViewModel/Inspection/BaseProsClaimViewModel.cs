namespace Bars.GkhGji.ViewModel
{
    using System;
    using System.Linq;

    using B4;
    using B4.DataAccess;
    using B4.Utils;

    using Bars.Gkh.Utils;

    using Entities;
    using Enums;

    /// <inheritdoc />
    public class BaseProsClaimViewModel: BaseProsClaimViewModel<BaseProsClaim>
    {
    }

    /// <inheritdoc />
    public class BaseProsClaimViewModel<T> : BaseViewModel<T>
        where T : BaseProsClaim
    {
        /// <inheritdoc />
        /// <remarks>
        /// параметры:
        ///   dateStart - период с
        ///   dateEnd - период по
        /// </remarks>
        public override IDataResult List(IDomainService<T> domainService, BaseParams baseParams)
        {
            /*
            * параметры:
            * dateStart - период с
            * dateEnd - период по
            */

            var serviceView = this.Container.ResolveDomain<ViewBaseProsClaim>();

            try
            {
                var loadParams = this.GetLoadParam(baseParams);

                var dateStart = baseParams.Params.GetAs<DateTime>("dateStart");
                var dateEnd = baseParams.Params.GetAs<DateTime>("dateEnd");
                var showCloseInspections = baseParams.Params.GetAs("showCloseInspections", true);

                return serviceView.GetAll()
                    .WhereIf(dateStart != DateTime.MinValue, x => x.ProsClaimDateCheck >= dateStart)
                    .WhereIf(dateEnd != DateTime.MinValue, x => x.ProsClaimDateCheck <= dateEnd)
                    .WhereIf(!showCloseInspections, x => x.State == null || !x.State.FinalState)
                    .Select(x => new
                    {
                        x.Id,
                        Municipality = x.MunicipalityNames,
                        MoSettlement = x.MoNames,
                        PlaceName = x.PlaceNames,
                        x.ContragentName,
                        x.ProsClaimDateCheck,
                        x.RealityObjectCount,
                        x.DocumentNumber,
                        x.InspectionNumber,
                        x.InspectorNames,
                        x.PersonInspection,
                        TypeJurPerson = x.PersonInspection == PersonInspection.PhysPerson ? null : x.TypeJurPerson,
                        x.State
                    })
                    .ToListDataResult(loadParams, this.Container);
            }
            finally 
            {
                this.Container.Release(serviceView);
            }
            
        }

        /// <inheritdoc />
        public override IDataResult Get(IDomainService<T> domainService, BaseParams baseParams)
        {
            var serviceDisposal = this.Container.ResolveDomain<Disposal>();
            var inspectionRiskDomain = this.Container.ResolveDomain<InspectionRisk>();

            try
            {
                var id = baseParams.Params["id"].To<long>();

                var obj = domainService.Get(id);

                // Получаем Распоряжение
                var disposal = serviceDisposal.GetAll().FirstOrDefault(x => x.Inspection.Id == id && x.TypeDisposal == TypeDisposalGji.Base);
                var risk = inspectionRiskDomain.GetAll().Where(x => x.Inspection.Id == id).FirstOrDefault(x => !x.EndDate.HasValue);

                obj.DisposalId = disposal?.Id;
                // TODO: 
                //obj.RiskCategory = risk?.RiskCategory;
                obj.RiskCategoryStartDate = risk?.StartDate;

                return new BaseDataResult(obj);
            }
            finally 
            {
                this.Container.Release(serviceDisposal);
            }
        }
    }
}