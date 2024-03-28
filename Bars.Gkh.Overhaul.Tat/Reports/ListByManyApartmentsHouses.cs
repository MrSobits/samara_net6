namespace Bars.Gkh.Overhaul.Tat.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.Utils;
    using GkhCr.Entities;

    /// <summary>
    /// Перечень многоквартирных домов
    /// </summary>
    internal class ListByManyApartmentsHouses : GkhCr.Report.ListByManyApartmentsHouses
    {
        protected override Dictionary<long, Dictionary<long, DataProxy>> GetTypeWorkDataByObjectCrDict(IDomainService<TypeWorkCr> serviceTypeWork)
        {
            var typeWorkDataByObjectCrDict = serviceTypeWork.GetAll()
                .Where(x => x.ObjectCr.ProgramCr.Id == this.ProgramCrId)
                .WhereIf(this.MunicipalityIds.Length > 0, x => this.MunicipalityIds.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                .WhereIf(this.FinanceIds.Length > 0, x => this.FinanceIds.Contains(x.FinanceSource.Id))
                .Select(x => new
                {
                    ObjectCrId = x.ObjectCr.Id,
                    MunicipId = x.ObjectCr.RealityObject.Municipality.Id,
                    MunicipName = x.ObjectCr.RealityObject.Municipality.Name,
                    Address = x.ObjectCr.RealityObject.Address,
                    DateEnterExp = x.ObjectCr.RealityObject.DateCommissioning ?? DateTime.MinValue,
                    DateLastOverhaul = x.ObjectCr.RealityObject.DateLastOverhaul ?? DateTime.MinValue,
                    WallMaterial = x.ObjectCr.RealityObject.WallMaterial.Name,
                    CountFloor = x.ObjectCr.RealityObject.MaximumFloors ?? 0,
                    CountEntr = x.ObjectCr.RealityObject.NumberEntrances,
                    AreaMkd = x.ObjectCr.RealityObject.AreaMkd ?? 0M,
                    AreaLivingNotLivingMkd = x.ObjectCr.RealityObject.AreaLivingNotLivingMkd ?? 0M,
                    AreaLivingOwned = x.ObjectCr.RealityObject.AreaLivingOwned ?? 0M,
                    CountPerson = x.ObjectCr.RealityObject.NumberLiving ?? 0,
                    Sum = x.Sum ?? 0M,
                    WorkName = x.Work.Name,
                    DateEndWork = x.DateEndWork ?? DateTime.MinValue
                })
                .AsEnumerable()
                .OrderBy(x => x.MunicipName)
                .ThenBy(x => x.Address)
                .GroupBy(x => x.MunicipId)
                .ToDictionary(
                    x => x.Key,
                    x => x.GroupBy(y => y.ObjectCrId)
                        .ToDictionary(
                            y => y.Key,
                            y => new DataProxy
                            {
                                Address = y.Select(z => z.Address).FirstOrDefault(),
                                DateEnterExp = y.Select(z => z.DateEnterExp).FirstOrDefault(),
                                DateLastOverhaul = y.Select(z => z.DateLastOverhaul).FirstOrDefault(),
                                WallMat = y.Select(z => z.WallMaterial).FirstOrDefault(),
                                CountFloor = y.Select(z => z.CountFloor).FirstOrDefault(),
                                CountEnt = y.Select(z => z.CountEntr).FirstOrDefault(),
                                AreaMkd = y.Select(z => z.AreaMkd).FirstOrDefault(),
                                AreaLivNotLivMkd = y.Select(z => z.AreaLivingNotLivingMkd).FirstOrDefault(),
                                AreaLivOwned = y.Select(z => z.AreaLivingOwned).FirstOrDefault(),
                                CountPerson = y.Select(z => z.CountPerson).FirstOrDefault(),
                                Sum = y.Sum(z => z.Sum.RoundDecimal(2)),
                                WorkNames = string.Join(", ", y.Select(z => z.WorkName).Where(z => !string.IsNullOrEmpty(z))),
                                WorksDateEnd = y.Max(z => z.DateEndWork)
                            }));
            return typeWorkDataByObjectCrDict;
        }

        protected override Dictionary<long, Dictionary<long, FinanceSourceDataProxy>> GetFinSrcResourceByObjectCrDict(IDomainService<FinanceSourceResource> serviceFinanceSourceResource)
        {
            var finSrcResourceByObjectCrDict = serviceFinanceSourceResource.GetAll()
                .Where(x => x.ObjectCr.ProgramCr.Id == this.ProgramCrId)
                .WhereIf(this.MunicipalityIds.Length > 0, x => this.MunicipalityIds.Contains(x.ObjectCr.RealityObject.MoSettlement.Id))
                .WhereIf(this.FinanceIds.Length > 0, x => this.FinanceIds.Contains(x.FinanceSource.Id))
                .Select(x => new
                {
                    objectCrId = x.ObjectCr.Id,
                    municipId = x.ObjectCr.RealityObject.Municipality.Id,
                    x.ObjectCr.RealityObject.Municipality.Name,
                    x.ObjectCr.RealityObject.Address,
                    BudgetMu = x.BudgetMu ?? 0M,
                    BudgetSubject = x.BudgetSubject ?? 0M,
                    FundResource = x.FundResource ?? 0M,
                    OwnerResource = x.OwnerResource ?? 0M
                })
                .AsEnumerable()
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Address)
                .GroupBy(x => x.municipId)
                .ToDictionary(
                    x => x.Key,
                    x => x.GroupBy(y => y.objectCrId)
                        .ToDictionary(
                            y => y.Key,
                            y => new FinanceSourceDataProxy
                            {
                                BudgetMu = y.Sum(z => z.BudgetMu.RoundDecimal(2)),
                                BudgetSubject = y.Sum(z => z.BudgetSubject.RoundDecimal(2)),
                                FundResource = y.Sum(z => z.FundResource.RoundDecimal(2)),
                                OwnerResource = y.Sum(z => z.OwnerResource.RoundDecimal(2)),
                            }));
            return finSrcResourceByObjectCrDict;
        }
    }
}
