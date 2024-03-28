namespace Bars.Gkh.Overhaul.Hmao.ViewModel
{
    using System.Collections;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.DataResult;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Overhaul.Hmao.ConfigSections;
    
    using Entities;
    using Gkh.Utils;

    public class DpkrCorrectionViewModel : BaseViewModel<DpkrCorrectionStage2>
    {
        public IDomainService<VersionRecord> VersionRecordDomain { get; set; }

        public override IDataResult List(IDomainService<DpkrCorrectionStage2> domainService, BaseParams baseParams)
        {
            var loadParam = GetLoadParam(baseParams);

            var moId = 0M;
            var versionId = loadParam.Filter.GetAs<long>("versionId");
            if (versionId == 0)
            {
                moId = baseParams.Params.GetAs<long>("mo_id");
            }

            var config = Container.GetGkhConfig<OverhaulHmaoConfig>();
            var groupByRoPeriod = config.GroupByRoPeriod;
            return new ListDataResult();

            //decimal summary;
            //int count;
            //IEnumerable data;
            
            //if (groupByRoPeriod == 0)
            //{
            //    var newData = domainService.GetAll()
            //        .WhereIf(moId > 0,x => x.Stage2.Stage3Version.ProgramVersion.IsMain && x.Stage2.Stage3Version.ProgramVersion.Municipality.Id == moId)
            //        .WhereIf(versionId > 0, x => x.Stage2.Stage3Version.ProgramVersion.Id == versionId)
            //        //.Where(x => x.Stage2.Stage3Version.IndexNumber > 0 || x.Stage2.Stage3Version.Year < periodEndYear) 
            //        // Не смейте суда сувать свои руки если не знаете как рабоатет
            //        // этой проверки быт ьнедолжно потому что деньги уже учтены в субсидировании и они должны заново пересчитыват ьвсе суммы в субсидировании через Расчет показателей
            //        // а то чт оделается по такой проверке это просто обман зрения. Например они сначала расчитали значения потом изменили год у какойто записи следовательно запись визуально пропадает из реестра но в суммах она остается 
            //        // им надо заново пересчитать субсидирвоание чтобы неоставалось в кореектировке записей которые ненужны
            //        .Select(x => new
            //        {
            //            x.Stage2.Id,
            //            Municipality = x.RealityObject.Municipality.Name,
            //            RealityObject = x.RealityObject.Address,
            //            CorrectionYear = x.PlanYear,
            //            PlanYear = x.Stage2.Stage3Version.Year,
            //            x.Stage2.Sum,
            //            CommonEstateObjectName = x.Stage2.CommonEstateObject.Name,
            //            x.Stage2.Stage3Version.IndexNumber
            //        })
            //        .Filter(loadParam, Container);

            //    summary = newData.Select(x => (decimal?) x.Sum).Sum().GetValueOrDefault(0m);
            //    count = newData.Count();

            //    data = newData
            //        .OrderIf(loadParam.Order.Length == 0, true, x => x.CorrectionYear)
            //        .Order(loadParam)
            //        .Paging(loadParam);
            //}
            //else
            //{
            //    var dataCorrection =
            //        domainService.GetAll()
            //            .WhereIf(moId > 0, x => x.Stage2.Stage3Version.ProgramVersion.IsMain && x.Stage2.Stage3Version.ProgramVersion.Municipality.Id == moId)
            //            .WhereIf(versionId > 0, x => x.Stage2.Stage3Version.ProgramVersion.Id == versionId)
            //            //.Where(x => x.Stage2.Stage3Version.IndexNumber > 0 || x.Stage2.Stage3Version.Year <= periodEndYear)
            //            // Не смейте суда сувать свои руки если не знаете как рабоатет
            //            //этой проверки быт ьнедолжно потому что деньги уже учтены в субсидировании и они должны заново пересчитыват ьвсе суммы в субсидировании через Расчет показателей
            //            // а то чт оделается по такой проверке это просто обман зрения. Например они сначала расчитали значения потом изменили год у какойто записи следовательно запись визуально пропадает из реестра но в суммах она остается 
            //            // им надо заново пересчитать субсидирвоание чтобы неоставалось в кореектировке записей которые ненужны
            //            .Select(x => new { x.Stage2.Stage3Version.Id, x.PlanYear })
            //            .AsEnumerable()
            //            .GroupBy(x => x.Id)
            //            .ToDictionary(x => x.Key, y => y.Select(x => x.PlanYear).FirstOrDefault());

            //    var newData =
            //        VersionRecordDomain.GetAll()
            //            .WhereIf(moId > 0, x => x.ProgramVersion.IsMain && x.ProgramVersion.Municipality.Id == moId)
            //            .WhereIf(versionId > 0, x => x.ProgramVersion.Id == versionId)
            //            .Where(x => domainService.GetAll().Any(y => y.Stage2.Stage3Version.Id == x.Id))
            //            .Select(x => new
            //            {
            //                x.Id,
            //                Municipality = x.RealityObject.Municipality.Name,
            //                RealityObject = x.RealityObject.Address,
            //                PlanYear = x.Year,
            //                x.Sum,
            //                CommonEstateObjectName = x.CommonEstateObjects,
            //                x.IndexNumber
            //            })
            //            .AsEnumerable()
            //            .Select(x => new
            //            {
            //                x.Id,
            //                x.Municipality,
            //                x.RealityObject,
            //                CorrectionYear = dataCorrection.ContainsKey(x.Id) ? dataCorrection[x.Id] : 0,
            //                x.PlanYear,
            //                x.Sum,
            //                x.CommonEstateObjectName,
            //                x.IndexNumber
            //            })
            //            .AsQueryable()
            //            .Filter(loadParam, Container);

            //    summary = newData.Select(x => (decimal?) x.Sum).Sum().GetValueOrDefault(0m);
            //    count = newData.Count();

            //    data = newData
            //        .OrderIf(loadParam.Order.Length == 0, true, x => x.CorrectionYear)
            //        .Order(loadParam)
            //        .Paging(loadParam);
            //}

            //return new ListSummaryResult(data, count, new { Sum = summary });
        }

        public IDataResult List1(IDomainService<DpkrCorrectionStage2> domainService, BaseParams baseParams)
        {
            var year = baseParams.Params.GetAs<int>("year");
            
            var dataCorrection =
                domainService.GetAll()
                    .Where(x => x.Stage2.Stage3Version.ProgramVersion.IsMain)
                    //.Where(x => x.Stage2.Stage3Version.IndexNumber > 0 || x.Stage2.Stage3Version.Year < periodEndYear)
                    //Эта херня неправильная потмоучто нужно заново персчяитывать субсидирвоание а эта проверка тольк овиузалнь оубирает записи которые насамм деле учтены но они непоказываются но в суммах они имеются
                    .Select(x => new { x.Stage2.Stage3Version.Id, x.PlanYear })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.Select(x => x.PlanYear).FirstOrDefault());

            var newData =
                VersionRecordDomain.GetAll()
                    .Where(x => x.ProgramVersion.IsMain)
                    .Where(x => domainService.GetAll().Any(y => y.Stage2.Stage3Version.Id == x.Id))
                    .Select(
                        x =>
                        new
                            {
                                x.Id,
                                MuId = x.RealityObject.Municipality.Id,
                                Area = x.RealityObject.AreaMkd,
                                Municipality = x.RealityObject.Municipality.Name,
                                x.Sum
                            })
                    .AsEnumerable()
                    .Select(
                        x =>
                        new
                            {
                                x.Id,
                                x.MuId,
                                Area = x.Area.HasValue ? x.Area.Value : 0,
                                x.Municipality,
                                CorrectionYear = dataCorrection.ContainsKey(x.Id) ? dataCorrection[x.Id] : 0,
                                x.Sum
                            })
                    .Where(x => x.CorrectionYear == year)
                    .GroupBy(x => x.MuId)
                    .Select(
                        x =>
                        new
                            {
                                Mu = x.FirstOrDefault().Return(y => y.Municipality),
                                Area = x.SafeSum(y => y.Area),
                                Sum = x.SafeSum(y => y.Sum),
                                Year = x.FirstOrDefault().Return(y => y.CorrectionYear)
                            });
            
            return new ListDataResult(newData, newData.Count());
        }
    }
}