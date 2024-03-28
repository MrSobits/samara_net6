namespace Bars.GkhGji.Regions.Tatarstan.DomainService.Decision.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Decision;

    using Castle.Windsor;

    public class DecisionControlObjectInfoService : IDecisionControlObjectInfoService
    {
        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public IDataResult ListControlObjectKind(BaseParams baseParams)
        {
            var controlTypeId = baseParams.Params.GetAsId("controlTypeId");

            var domainService = this.Container.ResolveDomain<ControlObjectKind>();

            using (this.Container.Using(domainService))
            {
                return domainService.GetAll()
                    .Where(x => x.ControlType.Id == controlTypeId)
                    .Select(x => new
                    {
                        x.Id,
                        x.Name,
                        x.ControlObjectType,
                        x.ControlType,
                        x.ErvkId
                    })
                    .ToListDataResult(baseParams.GetLoadParam());
            }
        }

        /// <inheritdoc />
        public IDataResult ListInspGjiRealityObject(BaseParams baseParams)
        {
            var inspectionId = baseParams.Params.GetAsId("inspectionId");
            var rObjRecordsId = baseParams.Params.GetAs<string>("realityObjIds").ToLongArray();

            var controlObjInfoService = this.Container.ResolveDomain<DecisionControlObjectInfo>();
            var inspRealityObjService = this.Container.ResolveDomain<InspectionGjiRealityObject>();

            using (this.Container.Using(inspRealityObjService, controlObjInfoService))
            {
                var usedRecordIds = controlObjInfoService.GetAll()
                    .Where(x => rObjRecordsId.Contains(x.Id))
                    .Select(x => x.InspGjiRealityObject.Id)
                    .ToArray();

                var rObjectQuery = inspRealityObjService.GetAll()
                    .Where(x => x.Inspection.Id == inspectionId)
                    .WhereIf(usedRecordIds.Any(), x => !usedRecordIds.Contains(x.Id));

                return rObjectQuery
                    .Select(x => new
                    {
                        x.Id,
                        x.RealityObject.Address
                    })
                    .ToListDataResult(baseParams.GetLoadParam(), this.Container);
            }
        }
    }
}