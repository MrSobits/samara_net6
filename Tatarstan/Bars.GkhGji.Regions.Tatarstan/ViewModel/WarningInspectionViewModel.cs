namespace Bars.GkhGji.Regions.Tatarstan.ViewModel
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// ViewModel сущности <see cref="WarningInspection"/>
    /// </summary>
    public class WarningInspectionViewModel : BaseViewModel<WarningInspection>
    {
        public override IDataResult List(IDomainService<WarningInspection> domainService, BaseParams baseParams)
        {
            var service = this.Container.Resolve<IDomainService<ViewWarningInspection>>();
            var serviceInsRo = this.Container.Resolve<IDomainService<InspectionGjiRealityObject>>();

            try
            {
                var loadParams = this.GetLoadParam(baseParams);

                var dateStart = baseParams.Params.GetAs("dateStart", DateTime.MinValue);
                var dateEnd = baseParams.Params.GetAs("dateEnd", DateTime.MinValue);
                var realityObjectId = baseParams.Params.GetAs<long>("realityObjectId");
                var showCloseInspections = baseParams.Params.GetAs("showCloseInspections", true);

                return service.GetAll()
                    .WhereIf(dateStart.IsValid(), x => x.InspectionDate >= dateStart)
                    .WhereIf(dateEnd.IsValid(), x => x.InspectionDate <= dateEnd)
                    .WhereIf(realityObjectId > 0, y => serviceInsRo.GetAll()
                        .Any(x => x.RealityObject.Id == realityObjectId && x.Inspection.Id == y.Id))
                    .WhereIf(!showCloseInspections, x => x.State == null || !x.State.FinalState)
                    .Select(x => new ViewWarningInspection
                    {
                        Id = x.Id,
                        Municipality = x.Municipality,
                        ContragentName = x.ContragentName,
                        PersonInspection = x.PersonInspection,
                        TypeJurPerson = x.TypeJurPerson,
                        InspectionDate = x.InspectionDate,
                        RealityObjectCount = x.RealityObjectCount,
                        InspectionNumber = x.InspectionNumber,
                        RegistrationNumber = x.RegistrationNumber,
                        State = x.State,
                        Inspectors = x.Inspectors,
                        AppealCitsNumberDate = x.AppealCitsNumberDate
                    })
                    .ToListDataResult(loadParams);
            }
            finally 
            {
                this.Container.Release(service);
                this.Container.Release(serviceInsRo);
            }
        }

        public override IDataResult Get(IDomainService<WarningInspection> domainService, BaseParams baseParams)
        {
            var serviceDisposal = this.Container.ResolveDomain<Disposal>();

            using (this.Container.Using(serviceDisposal))
            {
                var id = baseParams.Params.GetAsId();
                var warningInspection = domainService.Get(id);

                // Получаем Распоряжение
                var disposal = serviceDisposal.FirstOrDefault(x => x.Inspection.Id == id);

                if (disposal != null)
                {
                    warningInspection.DisposalId = disposal.Id;
                }
                
                return new BaseDataResult(warningInspection);
            }
        }
    }
}