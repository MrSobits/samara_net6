namespace Bars.Gkh.RegOperator.DomainService.Impl
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using B4;
    using B4.DataAccess;
    using B4.Modules.FileStorage;
    using B4.Utils;

    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.DomainModelServices;
    using Bars.Gkh.Utils;

    using Castle.Windsor;
    using Entities;
    using Gkh.Domain;
    using Gkh.Domain.CollectionExtensions;
    using PersonalAccount;

    public class ExportPenaltyService : IExportPenaltyService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult Export(BaseParams baseParams)
        {
            var persAccFilterService = Container.Resolve<IPersonalAccountFilterService>();
            var periodDomain = Container.ResolveDomain<ChargePeriod>();
            var persAccDomain = Container.ResolveDomain<BasePersonalAccount>();
            var persAccPeriodSummaryDomain = Container.ResolveDomain<PersonalAccountPeriodSummary>();
            var fileManager = Container.Resolve<IFileManager>();

            try
            {
                var export = new StringBuilder();

                var ci = CultureInfo.InvariantCulture.Clone() as CultureInfo;
                NumberFormatInfo numberformat = null;
                if (ci != null)
                {
                    ci.NumberFormat.NumberDecimalSeparator = ".";
                    numberformat = ci.NumberFormat;
                }

                var periodId = baseParams.Params.GetAsId("periodId");
                var accountIds = baseParams.Params.GetAs<string>("accountIds").ToLongArray();
                var queryByFilters = persAccFilterService.GetQueryableByFilters(baseParams, persAccDomain.GetAll())
                    .WhereIf(accountIds.Length > 0, x => accountIds.Contains(x.Id));

                var period = periodDomain.Get(periodId);

                var penaltyDebts = persAccPeriodSummaryDomain.GetAll()
                    .Where(x => queryByFilters.Any(y => y.Id == x.PersonalAccount.Id))
                    .Where(x => x.Period.StartDate <= period.StartDate)
                    .Select(x => new
                    {
                        x.PersonalAccount.Id,
                        PenaltyDebt = x.Penalty + x.RecalcByPenalty - x.PenaltyPayment
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.SafeSum(x => x.PenaltyDebt));

                var accounts = queryByFilters.ToList();                

                var penaltyDebtSummary = 0M;

                export.AppendLine("");
                export.AppendLine("#SERVICE 9133");
                export.AppendLine("#TYPE 7");

                foreach (var account in accounts)
                {
                    var penaltyDebt = Math.Max(penaltyDebts.Get(account.Id), 0.00M);

                    penaltyDebtSummary += penaltyDebt;

                    export.AppendLine(string.Format("{0};{1},{2},{3};{4};{5};;;;", 
                        account.AccountOwner, 
                        account.Municipality,
                        account.Address, 
                        account.RoomNum, 
                        account.PersonalAccountNum, 
                        penaltyDebt.RegopRoundDecimal(2).ToString(numberformat)));
                }

                export.Insert(0, string.Format("#FILESUM {0}\n", penaltyDebtSummary.RegopRoundDecimal(2).ToString(numberformat)));

				var byteArray = Encoding.GetEncoding(1251).GetBytes(export.ToStr());

                var file = fileManager.SaveFile("Выгрузка начисления пени({0})".FormatUsing(period.Name), "txt", byteArray);

                return new BaseDataResult(file.Id);
            }
            finally
            {
                Container.Release(persAccPeriodSummaryDomain);
                Container.Release(persAccFilterService);
                Container.Release(persAccDomain);
                Container.Release(periodDomain);
                Container.Release(fileManager);
            }
        }

        public IDataResult ExportPenaltyExcel(BaseParams baseParams)
        {
            var persAccFilterService = Container.Resolve<IPersonalAccountFilterService>();
            var periodDomain = Container.ResolveDomain<ChargePeriod>();
            var persAccDomain = Container.ResolveDomain<BasePersonalAccount>();
            var persAccPeriodSummaryDomain = Container.ResolveDomain<PersonalAccountPeriodSummary>();
            var fileManager = Container.Resolve<IFileManager>();

            try
            {
                var export = new StringBuilder();

                var ci = CultureInfo.InvariantCulture.Clone() as CultureInfo;
                NumberFormatInfo numberformat = null;
                if (ci != null)
                {
                    ci.NumberFormat.NumberDecimalSeparator = ".";
                    numberformat = ci.NumberFormat;
                }

                var periodId = baseParams.Params.GetAsId("periodId");
                var accountIds = baseParams.Params.GetAs<string>("accountIds").ToLongArray();
                var queryByFilters = persAccFilterService.GetQueryableByFilters(baseParams, persAccDomain.GetAll())
                    .WhereIf(accountIds.Length > 0, x => accountIds.Contains(x.Id));

                var period = periodDomain.Get(periodId);

                var penaltyDebts = persAccPeriodSummaryDomain.GetAll()
                    .Where(x => queryByFilters.Any(y => y.Id == x.PersonalAccount.Id))
                    .Where(x => x.Period.StartDate <= period.StartDate)
                    .Select(x => new
                    {
                        x.PersonalAccount.Id,
                        PenaltyDebt = x.Penalty + x.RecalcByPenalty - x.PenaltyPayment
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.SafeSum(x => x.PenaltyDebt));

                var accounts = queryByFilters.ToList();

                var penaltyDebtSummary = 0M;

                foreach (var account in accounts)
                {
                    var penaltyDebt = Math.Max(penaltyDebts.Get(account.Id), 0.00M);

                    penaltyDebtSummary += penaltyDebt;

                    export.AppendLine(string.Format("{0};{1},{2},{3};{4};{5};;;;",
                        account.AccountOwner,
                        account.Municipality,
                        account.Address,
                        account.RoomNum,
                        account.PersonalAccountNum,
                        penaltyDebt.RegopRoundDecimal(2).ToString(numberformat)));
                }

                export.Insert(0, string.Format("#FILESUM {0}\n", penaltyDebtSummary.RegopRoundDecimal(2).ToString(numberformat)));

                var byteArray = Encoding.GetEncoding(1251).GetBytes(export.ToStr());

                var file = fileManager.SaveFile("Выгрузка начисления пени({0})".FormatUsing(period.Name), "csv", byteArray);

                return new BaseDataResult(file.Id);
            }
            finally
            {
                Container.Release(persAccPeriodSummaryDomain);
                Container.Release(persAccFilterService);
                Container.Release(persAccDomain);
                Container.Release(periodDomain);
                Container.Release(fileManager);
            }
        }
    }
}