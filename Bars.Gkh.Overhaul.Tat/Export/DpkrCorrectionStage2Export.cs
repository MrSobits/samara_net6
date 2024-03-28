namespace Bars.Gkh.Overhaul.Tat.Export
{
    using System.Collections;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Tat.DomainService;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Bars.Gkh.Overhaul.Tat.Enum;

    public class DpkrCorrectionStage2Export : BaseDataExportService
    {
        public IDpkrCorrectionDataProvider _provider { get; set; }

        public IDomainService<VersionRecord> versionRecordDomain { get; set; }

        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParam = GetLoadParam(baseParams);

            var type = baseParams.Params.GetAs("type", 0);

            /* 
               Поскольку записи в 3 этапе могут быть сгруппированы то в корректировке необходимо показвать записи не 2этапа а 3го
               то есть должно быть так 'Лифт, Крыша' 
            */

            var correctionQuery = _provider.GetCorrectionData(baseParams)
                                           .WhereIf(type > 0, x => x.TypeResult == (TypeResultCorrectionDpkr)type);


            var corrDataBySt3 = correctionQuery.Select(x => new { st3Id = x.Stage2.Stage3Version.Id, x.PlanYear, x.TypeResult })
                              .AsEnumerable()
                              .GroupBy(x => x.st3Id)
                              .ToDictionary(
                                  x => x.Key,
                                  y =>
                                  new
                                  {
                                      PlanYear = y.Select(z => z.PlanYear).FirstOrDefault(),
                                      TypeResult = y.Select(z => z.TypeResult).FirstOrDefault()
                                  });


            return versionRecordDomain.GetAll()
                            .Where(x => correctionQuery.Any(y => y.Stage2.Stage3Version.Id == x.Id))

                            .Select(x => new
                            {
                                x.Id,
                                Municipality = x.RealityObject.Municipality.Name,
                                RealityObject = x.RealityObject.Address,
                                x.Sum,
                                CommonEstateObjectName = x.CommonEstateObjects,
                                FirstPlanYear = x.Year,
                                x.Point,
                                x.IndexNumber
                            })
                            .AsEnumerable()
                            .Select(x => new
                            {
                                x.Id,
                                x.Municipality,
                                x.RealityObject,
                                x.Sum,
                                x.CommonEstateObjectName,
                                x.FirstPlanYear,
                                x.Point,
                                x.IndexNumber,
                                PlanYear = corrDataBySt3.ContainsKey(x.Id) ? corrDataBySt3[x.Id].PlanYear : 0,
                                TypeResult = corrDataBySt3.ContainsKey(x.Id) ? corrDataBySt3[x.Id].TypeResult : TypeResultCorrectionDpkr.InLongTerm,
                            })
                            .AsQueryable()
                            .Filter(loadParam, Container)
                            .Order(loadParam)
                            .ToList();
        }
    }
}