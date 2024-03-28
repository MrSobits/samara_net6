namespace Bars.Gkh.Overhaul.Nso.ExecutionAction
{
    using System;
    using System.Data;
    using System.Diagnostics;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.Overhaul.Nso.Entities;
    using Bars.GkhCr.Entities;

    public class TypeWorkCrSetYearRepairAction : BaseExecutionAction
    {
        public IDomainService<DpkrCorrectionStage2> correctionDomain { get; set; }

        public override string Name => "ДПКР - Проставление года ремонта для видов работ";

        public override string Description => @"Для записей, которые созданы из ДПКР проставляется год ремонта из Результатов корректировки. При повторном выполнении действия старые значения будт затиратся значениями из ДПКР (То ест ьесли пользователб ввел уже другео значение то перетрется значением из ДПКР)";

        public override Func<IDataResult> Action => this.Execute;

        private BaseDataResult Execute()
        {
            /*
              Скорректированный год в ХМАО берется из корректирвоки а вид работы из 1 этапа
            */

            var correctionData = this.correctionDomain.GetAll()
                .Select(x => new {CorrectionYear = x.PlanYear, St2Id = x.Stage2.Id})
                .AsEnumerable()
                .GroupBy(x => x.St2Id)
                .ToDictionary(x => x.Key, y => y.Select(z => z.CorrectionYear).FirstOrDefault());

            var typeWorkVersionDomain = this.Container.Resolve<IDomainService<TypeWorkCrVersionStage1>>();
            var typeWorkDomain = this.Container.Resolve<IRepository<TypeWorkCr>>();

            try
            {
                // Получаем год неоюходимый для вида работы
                var dataQuery = typeWorkVersionDomain.GetAll()
                    .Where(x => x.Stage1Version != null)
                    .Where(x => !x.TypeWorkCr.YearRepair.HasValue);

                var yearsList = dataQuery
                    .Select(
                        x => new
                        {
                            TypeWorkId = x.TypeWorkCr.Id,
                            Stage2id = x.Stage1Version.Stage2Version.Id,
                            x.Stage1Version.Stage2Version.Stage3Version.Year
                        })
                    .AsEnumerable()
                    .Select(
                        x =>
                            new
                            {
                                x.TypeWorkId,
                                CorrectYear = correctionData.ContainsKey(x.Stage2id) ? correctionData[x.Stage2id] : x.Year
                            })
                    .ToList();

                if (!yearsList.Any())
                {
                    // Если не найдены записи то вообще выходим
                    return new BaseDataResult();
                }

                var savedCount = 0;

                while (savedCount <= yearsList.Count())
                {
                    var sessionProv = this.Container.Resolve<ISessionProvider>();

                    var transactionSize = 10000;
                    using (var stateLess = sessionProv.OpenStatelessSession())
                    {
                        stateLess.SetBatchSize(transactionSize);
                        var sw = Stopwatch.StartNew();
                        var toSave = yearsList.Skip(savedCount).Take(transactionSize).ToList();

                        savedCount += transactionSize;

                        using (var tr = stateLess.BeginTransaction(IsolationLevel.Serializable))
                        {
                            toSave.ForEach(
                                x =>
                                {
                                    stateLess.CreateQuery("update TypeWorkCr set YearRepair = :year where Id = :twid")
                                        .SetParameter("year", x.CorrectYear)
                                        .SetParameter("twid", x.TypeWorkId)
                                        .ExecuteUpdate();
                                });

                            toSave.Clear();

                            tr.Commit();
                        }
                        sw.Stop();
                    }
                }
            }
            finally
            {
                this.Container.Release(typeWorkVersionDomain);
                this.Container.Release(typeWorkDomain);
            }

            return new BaseDataResult();
        }
    }
}