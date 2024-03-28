namespace Bars.Gkh.Overhaul.Tat.Reports
{
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.Utils;
    using GkhCr.Entities;

    public class PlanedProgramIndicatorsReport : GkhCr.Report.PlanedProgramIndicatorsReport
    {
        protected override Dictionary<MunicipalityDataProxy, Record> GetObjectCrDict(IDomainService<ObjectCr> objectCrService, IDomainService<TypeWorkCr> typeWorkCrService)
        {
            //Для Татарстана необходимо, чтобы группировка было по МР
            return objectCrService.GetAll()
                .WhereIf(municipalityListId.Length > 0, x => municipalityListId.Contains(x.RealityObject.Municipality.Id))
                .Where(x => x.ProgramCr.Id == programCrId
                            && typeWorkCrService.GetAll().Any(y => y.ObjectCr.Id == x.Id && y.FinanceSource.Code == "1"))
                .Select(x => new
                {
                    x.RealityObject.AreaMkd,
                    x.RealityObject.NumberLiving,
                    MunicipalityId = x.RealityObject.Municipality.Id,
                    MunicipalityName = x.RealityObject.Municipality.Name,
                    MunicipalityGroup = x.RealityObject.Municipality.Group,
                    SumByTypeWorkCr = typeWorkCrService.GetAll().Where(y => y.ObjectCr.Id == x.Id && y.FinanceSource.Code == "1").Sum(y => y.Sum)
                })
                .AsEnumerable()
                .GroupBy(x => new MunicipalityDataProxy { Id = x.MunicipalityId, Name = x.MunicipalityName, Group = x.MunicipalityGroup })
                .ToDictionary(x => x.Key,
                    x => new Record
                    {
                        AreaMkd = x.Sum(y => y.AreaMkd),
                        NumberLiving = x.Sum(y => y.NumberLiving),
                        SumByTypeWorkCr = x.Sum(y => y.SumByTypeWorkCr),
                        CountHouses = x.Count()
                    });
        }
    }
}
