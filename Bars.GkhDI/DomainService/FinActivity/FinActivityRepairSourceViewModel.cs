namespace Bars.GkhDi.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;

    using Bars.GkhDi.Enums;

    using Entities;

    public class FinActivityRepairSourceViewModel : BaseViewModel<FinActivityRepairSource>
    {
        public override IDataResult List(IDomainService<FinActivityRepairSource> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var disclosureInfoId = baseParams.Params.GetAs<long>("disclosureInfoId");

            var dataFinActivityRepairSource = domainService
                .GetAll()
                .Where(x => x.DisclosureInfo.Id == disclosureInfoId)
                .ToList();

            var dataNewFinActivityRepairSource = new List<FinActivityRepairSource>();

            foreach (TypeSourceFundsDi type in Enum.GetValues(typeof(TypeSourceFundsDi)))
            {
                var record = dataFinActivityRepairSource.FirstOrDefault(x => x.TypeSourceFundsDi == type);

                var finActivityRepairSource = new FinActivityRepairSource { Id = (int)type, TypeSourceFundsDi = type };
                if (record != null)
                {
                    finActivityRepairSource.Sum = record.Sum;
                    finActivityRepairSource.DisclosureInfo = record.DisclosureInfo;
                }

                if (type == TypeSourceFundsDi.Summury)
                {
                    finActivityRepairSource.IsInvalid = this.GetSummuryString(dataFinActivityRepairSource);
                }
                else
                {
                    finActivityRepairSource.IsInvalid = "0;";
                }

                dataNewFinActivityRepairSource.Add(finActivityRepairSource);
            }

            var totalCount = dataNewFinActivityRepairSource.AsQueryable().Count();
            var data = dataNewFinActivityRepairSource.AsQueryable().Order(loadParams).Paging(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }

        private string GetSummuryString(List<FinActivityRepairSource> finActivityRepairSource)
        {
            var isSum = 0;

            var sumBySum = finActivityRepairSource
                .Where(x => x.TypeSourceFundsDi != TypeSourceFundsDi.Summury)
                .Sum(x => x.Sum);
            var sum = finActivityRepairSource
                .Where(x => x.TypeSourceFundsDi == TypeSourceFundsDi.Summury)
                .Sum(x => x.Sum);


            if (sumBySum != sum && sumBySum > 0)
            {
                isSum = 1;
            }

            var result = string.Format("{0};", isSum);

            return result;
        }
    }
}