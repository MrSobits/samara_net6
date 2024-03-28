namespace Bars.GkhDi.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using B4;

    using Bars.GkhDi.Enums;

    using Entities;

    public class FinActivityManagCatViewModel : BaseViewModel<FinActivityManagCategory>
    {
        public override IDataResult List(IDomainService<FinActivityManagCategory> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var disclosureInfoId = baseParams.Params.GetAs<long>("disclosureInfoId");

            // группируем по категории, т.к. по одной категории не может быть 2 значений
            var dataFinActivityManagCategory = domainService.GetAll()
                .Where(x => x.DisclosureInfo.Id == disclosureInfoId)
                .ToArray()
                .GroupBy(x => x.TypeCategoryHouseDi)
                .Select(x => x.First())
                .ToArray();

            var dataNewFinActivityManagCategory = new List<FinActivityManagCategory>();

            foreach (TypeCategoryHouseDi type in Enum.GetValues(typeof(TypeCategoryHouseDi)))
            {
                var record = dataFinActivityManagCategory.FirstOrDefault(x => x.TypeCategoryHouseDi == type);

                var finActivityManagCategory = new FinActivityManagCategory { Id = (int)type, TypeCategoryHouseDi = type };
                if (record != null)
                {
                    finActivityManagCategory.DisclosureInfo = record.DisclosureInfo;
                    finActivityManagCategory.TypeCategoryHouseDi = record.TypeCategoryHouseDi;
                    finActivityManagCategory.IncomeManaging = record.IncomeManaging;
                    finActivityManagCategory.IncomeUsingGeneralProperty = record.IncomeUsingGeneralProperty;
                    finActivityManagCategory.ExpenseManaging = record.ExpenseManaging;
                    finActivityManagCategory.ExactPopulation = record.ExactPopulation;
                    finActivityManagCategory.DebtPopulationStart = record.DebtPopulationStart;
                    finActivityManagCategory.DebtPopulationEnd = record.DebtPopulationEnd;
                }

                if (type == TypeCategoryHouseDi.Summury)
                {
                    finActivityManagCategory.IsInvalid = this.GetSummuryString(dataFinActivityManagCategory);
                }
                else
                {
                    finActivityManagCategory.IsInvalid = "0;0;0;0;0;0";
                }

                dataNewFinActivityManagCategory.Add(finActivityManagCategory);
            }

            var totalCount = dataNewFinActivityManagCategory.AsQueryable().Count();
            var data = dataNewFinActivityManagCategory.AsQueryable().Order(loadParams).Paging(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }

        private string GetSummuryString(FinActivityManagCategory[] dataFinActivityManagCategory)
        {
            var isInvalidIncome = 0;
            var isInvalidDebtStart = 0;
            var isInvalidDebtEnd = 0;
            var isInvalidIncomeUsingGeneralProperty = 0;
            var isInvalidExpenseManaging = 0;
            var isInvalidExactPopulation = 0;

            var sumByIncomeFromProviding = dataFinActivityManagCategory
                .Where(x => x.TypeCategoryHouseDi != TypeCategoryHouseDi.Summury)
                .Sum(x => x.IncomeManaging);
            var incomeFromProviding = dataFinActivityManagCategory
                .Where(x => x.TypeCategoryHouseDi == TypeCategoryHouseDi.Summury)
                .Sum(x => x.IncomeManaging);

            var sumByDebtStart = dataFinActivityManagCategory
                .Where(x => x.TypeCategoryHouseDi != TypeCategoryHouseDi.Summury)
                .Sum(x => x.DebtPopulationStart);
            var debtStart = dataFinActivityManagCategory
                .Where(x => x.TypeCategoryHouseDi == TypeCategoryHouseDi.Summury)
                .Sum(x => x.DebtPopulationStart);

            var sumByDebtEnd = dataFinActivityManagCategory
                .Where(x => x.TypeCategoryHouseDi != TypeCategoryHouseDi.Summury)
                .Sum(x => x.DebtPopulationEnd);
            var debtEnd = dataFinActivityManagCategory
                .Where(x => x.TypeCategoryHouseDi == TypeCategoryHouseDi.Summury)
                .Sum(x => x.DebtPopulationEnd);

            var sumByIncomeUsingGeneralProperty = dataFinActivityManagCategory
                .Where(x => x.TypeCategoryHouseDi != TypeCategoryHouseDi.Summury)
                .Sum(x => x.IncomeUsingGeneralProperty);
            var debtIncomeUsingGeneralProperty = dataFinActivityManagCategory
                .Where(x => x.TypeCategoryHouseDi == TypeCategoryHouseDi.Summury)
                .Sum(x => x.IncomeUsingGeneralProperty);

            var sumByExpenseManaging = dataFinActivityManagCategory
                .Where(x => x.TypeCategoryHouseDi != TypeCategoryHouseDi.Summury)
                .Sum(x => x.ExpenseManaging);
            var debtExpenseManaging = dataFinActivityManagCategory
                .Where(x => x.TypeCategoryHouseDi == TypeCategoryHouseDi.Summury)
                .Sum(x => x.ExpenseManaging);

            var sumByExactPopulation = dataFinActivityManagCategory
                .Where(x => x.TypeCategoryHouseDi != TypeCategoryHouseDi.Summury)
                .Sum(x => x.ExactPopulation);
            var debtExactPopulation = dataFinActivityManagCategory
                .Where(x => x.TypeCategoryHouseDi == TypeCategoryHouseDi.Summury)
                .Sum(x => x.ExactPopulation);

            if (sumByIncomeFromProviding != incomeFromProviding && sumByIncomeFromProviding > 0)
            {
                isInvalidIncome = 1;
            }

            if (sumByDebtStart != debtStart && sumByDebtStart > 0)
            {
                isInvalidDebtStart = 1;
            }

            if (sumByDebtEnd != debtEnd && sumByDebtEnd > 0)
            {
                isInvalidDebtEnd = 1;
            }

            if (sumByIncomeUsingGeneralProperty != debtIncomeUsingGeneralProperty && sumByIncomeUsingGeneralProperty > 0)
            {
                isInvalidIncomeUsingGeneralProperty = 1;
            }

            if (sumByExpenseManaging != debtExpenseManaging && sumByExpenseManaging > 0)
            {
                isInvalidExpenseManaging = 1;
            }

            if (sumByExactPopulation != debtExactPopulation && sumByExactPopulation > 0)
            {
                isInvalidExactPopulation = 1;
            }

            var result = string.Format(
                "{0};{1};{2};{3};{4};{5}",
                isInvalidIncome,
                isInvalidDebtStart,
                isInvalidDebtEnd,
                isInvalidIncomeUsingGeneralProperty,
                isInvalidExpenseManaging,
                isInvalidExactPopulation);

            return result;
        }
    }
}
