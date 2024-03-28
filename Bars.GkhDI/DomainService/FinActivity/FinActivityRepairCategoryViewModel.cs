namespace Bars.GkhDi.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;

    using Bars.GkhDi.Enums;

    using Entities;

    public class FinActivityRepairCategoryViewModel : BaseViewModel<FinActivityRepairCategory>
    {
        public override IDataResult List(IDomainService<FinActivityRepairCategory> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var disclosureInfoId = baseParams.Params.GetAs<long>("disclosureInfoId");

            var dataFinActivityRepairCategory = domainService
                .GetAll()
                .Where(x => x.DisclosureInfo.Id == disclosureInfoId)
                .ToList();

            var dataNewFinActivityRepairCategory = new List<FinActivityRepairCategory>();

            foreach (TypeCategoryHouseDi type in Enum.GetValues(typeof(TypeCategoryHouseDi)))
            {
                var record = dataFinActivityRepairCategory.FirstOrDefault(x => x.TypeCategoryHouseDi == type);

                var finActivityRepairCategory = new FinActivityRepairCategory { Id = (int)type, TypeCategoryHouseDi = type };
                if (record != null)
                {
                    finActivityRepairCategory.WorkByRepair = record.WorkByRepair;
                    finActivityRepairCategory.WorkByBeautification = record.WorkByBeautification;
                    finActivityRepairCategory.PeriodService = record.PeriodService;
                    finActivityRepairCategory.DisclosureInfo = record.DisclosureInfo;
                }

                if (type == TypeCategoryHouseDi.Summury)
                {
                    finActivityRepairCategory.IsInvalid = this.GetSummuryString(dataFinActivityRepairCategory);
                }
                else
                {
                    finActivityRepairCategory.IsInvalid = "0;0;0";
                }

                dataNewFinActivityRepairCategory.Add(finActivityRepairCategory);
            }

            var totalCount = dataNewFinActivityRepairCategory.AsQueryable().Count();
            var data = dataNewFinActivityRepairCategory.AsQueryable().Order(loadParams).Paging(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }

        private string GetSummuryString(List<FinActivityRepairCategory> dataFinActivityRepairCategory)
        {
            var isWorkByRepair = 0;
            var isWorkByBeautification = 0;
            var isPeriodService = 0;

            var sumByWorkByRepair = dataFinActivityRepairCategory
                .Where(x => x.TypeCategoryHouseDi != TypeCategoryHouseDi.Summury)
                .Sum(x => x.WorkByRepair);
            var workByRepair = dataFinActivityRepairCategory
                .Where(x => x.TypeCategoryHouseDi == TypeCategoryHouseDi.Summury)
                .Sum(x => x.WorkByRepair);

            var sumByWorkByBeautification = dataFinActivityRepairCategory
                .Where(x => x.TypeCategoryHouseDi != TypeCategoryHouseDi.Summury)
                .Sum(x => x.WorkByBeautification);
            var workByBeautification = dataFinActivityRepairCategory
                .Where(x => x.TypeCategoryHouseDi == TypeCategoryHouseDi.Summury)
                .Sum(x => x.WorkByBeautification);

            var sumByPeriodService = dataFinActivityRepairCategory
                .Where(x => x.TypeCategoryHouseDi != TypeCategoryHouseDi.Summury)
                .Sum(x => x.PeriodService);
            var periodService = dataFinActivityRepairCategory
                .Where(x => x.TypeCategoryHouseDi == TypeCategoryHouseDi.Summury)
                .Sum(x => x.PeriodService);


            if (sumByWorkByRepair != workByRepair && sumByWorkByRepair > 0)
            {
                isWorkByRepair = 1;
            }

            if (sumByWorkByBeautification != workByBeautification && sumByWorkByBeautification > 0)
            {
                isWorkByBeautification = 1;
            }

            if (sumByPeriodService != periodService && sumByPeriodService > 0)
            {
                isPeriodService = 1;
            }

            var result = string.Format(
                "{0};{1};{2}",
                isWorkByRepair,
                isWorkByBeautification,
                isPeriodService);

            return result;
        }
    }
}