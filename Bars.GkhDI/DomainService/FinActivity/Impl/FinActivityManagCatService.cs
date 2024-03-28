namespace Bars.GkhDi.DomainService
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GkhDi.Entities;

    using Castle.Windsor;

    public class FinActivityManagCatService : IFinActivityManagCatService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddWorkMode(BaseParams baseParams)
        {
            var service = Container.ResolveDomain<FinActivityManagCategory>();
            var disclosureInfoDomain = Container.ResolveDomain<DisclosureInfo>();
            try
            {
                var disclosureInfoId = baseParams.Params.GetAs<long>("disclosureInfoId");
                var records = baseParams.Params["records"]
                    .As<List<object>>()
                    .Select(x => x.As<DynamicDictionary>().ReadClass<FinActivityManagCategory>())
                    .ToList();

                var existingFinActivityManagCategory = service.GetAll()
                        .Where(x => x.DisclosureInfo.Id == disclosureInfoId)
                        .AsEnumerable()
                        .GroupBy(x => x.TypeCategoryHouseDi)
                        .ToDictionary(x => x.Key, y => y.FirstOrDefault());

                var disclosureInfo = disclosureInfoDomain.GetAll().FirstOrDefault(x => x.Id == disclosureInfoId);

                foreach (var rec in records)
                {
                    FinActivityManagCategory existingManagCategory = null;
                    
                    if(existingFinActivityManagCategory.ContainsKey(rec.TypeCategoryHouseDi))
                        existingManagCategory = existingFinActivityManagCategory[rec.TypeCategoryHouseDi];

                    if (existingManagCategory != null)
                    {
                        existingManagCategory.IncomeManaging = rec.IncomeManaging;
                        existingManagCategory.IncomeUsingGeneralProperty = rec.IncomeUsingGeneralProperty;
                        existingManagCategory.ExpenseManaging = rec.ExpenseManaging;
                        existingManagCategory.ExactPopulation = rec.ExactPopulation;
                        existingManagCategory.DebtPopulationStart = rec.DebtPopulationStart;
                        existingManagCategory.DebtPopulationEnd = rec.DebtPopulationEnd;

                        service.Update(existingManagCategory);
                    }
                    else
                    {
                        var newFinActivityManagCategory = new FinActivityManagCategory
                        {
                            DisclosureInfo = disclosureInfo,
                            TypeCategoryHouseDi = rec.TypeCategoryHouseDi,
                            IncomeManaging = rec.IncomeManaging,
                            IncomeUsingGeneralProperty = rec.IncomeUsingGeneralProperty,
                            ExpenseManaging = rec.ExpenseManaging,
                            ExactPopulation = rec.ExactPopulation,
                            DebtPopulationStart = rec.DebtPopulationStart,
                            DebtPopulationEnd = rec.DebtPopulationEnd,
                        };

                        service.Save(newFinActivityManagCategory);
                    }
                }

                return new BaseDataResult { Success = true };
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
            finally
            {
                Container.Release(service);
                Container.Release(disclosureInfoDomain);
            }
        }
    }
}
