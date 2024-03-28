namespace Bars.GkhGji.Export
{
    using System.Collections;
    using System.Linq;

    using B4;
    using B4.DataAccess;
    using B4.Modules.DataExport.Domain;
    using B4.Utils;
    using Entities;
    using Gkh.Domain;

    public class BaseStatementDataExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParam = GetLoadParam(baseParams);

            /*
             * параметры:
             * realityObjectId - жилой дом
             */

            var realityObjectId = baseParams.Params.GetAsId("realityObjectId");

            var inspectionRoDomain = Container.ResolveDomain<InspectionGjiRealityObject>();

            return Container.Resolve<IDomainService<ViewBaseStatement>>().GetAll()
                .WhereIf(realityObjectId > 0, y => inspectionRoDomain.GetAll().Any(x => x.RealityObject.Id == realityObjectId && x.Inspection.Id == y.Id))
                .Select(x => new
                {
                    x.Id,
                    Municipality = x.MunicipalityNames,
                    x.RealityObjectCount,
                    x.ContragentName,
                    x.PersonInspection,
                    x.InspectionNumber,
                    TypeJurPerson = x.TypeJurPerson.Value,
                    x.IsDisposal,
                    x.RealObjAddresses,
                    x.State
                })
                .Filter(loadParam, Container)
                .Order(loadParam)
                .ToList();
        }
    }
}