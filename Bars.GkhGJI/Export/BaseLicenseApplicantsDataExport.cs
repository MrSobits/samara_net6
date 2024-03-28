namespace Bars.GkhGji.Export
{
    using System.Collections;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Modules.DataExport.Domain;
    using B4.Utils;
    using Entities;
    using Enums;

    public class BaseLicenseApplicantsDataExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var serviceInspRobject = Container.ResolveDomain<InspectionGjiRealityObject>();
            var serviceView = Container.ResolveDomain<ViewBaseLicApplicants>();

            try
            {
                var contragentId = baseParams.Params.GetAs<long>("contragentId");
                var showCloseInspections = baseParams.Params.GetAs("showCloseInspections", true);

                return  serviceView.GetAll()
                    .WhereIf(contragentId > 0, y => y.ContragentId == contragentId)
                    .WhereIf(!showCloseInspections, x => x.State == null || !x.State.FinalState)
                    .Select(x => new
                    {
                        x.Id,
                        Municipality = x.MunicipalityNames,
                        x.ReqNumber,
                        x.ContragentName,
                        x.PersonInspection,
                        x.InspectionNumber,
                        TypeJurPerson = x.PersonInspection == PersonInspection.PhysPerson ? null : x.TypeJurPerson,
                        x.IsDisposal,
                        x.RealObjAddresses,
                        x.State
                    })
                    .ToList();

            }
            finally
            {
                Container.Release(serviceView);
                Container.Release(serviceInspRobject);
            }
        }
    }
}