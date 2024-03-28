namespace Bars.Gkh.Overhaul.Tat.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Tat.DomainService;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Bars.Gkh.Overhaul.Tat.Enum;

    public class DpkrCorrectionViewModel : BaseViewModel<DpkrCorrectionStage2>
    {
        private readonly IDpkrCorrectionDataProvider _provider;

        public IDomainService<PublishedProgramRecord> publishedRecordDomain { get; set; }

        public IDomainService<VersionRecord> versionRecordDomain { get; set; }

        public DpkrCorrectionViewModel(IDpkrCorrectionDataProvider provider)
        {
            _provider = provider;
        }

        public override IDataResult Get(IDomainService<DpkrCorrectionStage2> domainService, BaseParams baseParams)
        {
            /* 
               Поскольку записи в 3 этапе могут быть сгруппированы, то суда буде тприходить id не корректировки а 3 этапа
               Так делаю. потомучто корректировка теперь показывает не свои записи а схлопывает по 3 этапу соовтетсвенно и редактирвоание будет также схлопнуто по 3 этапу
            */

            var value =
                 domainService.GetAll()
                     .Where(x => x.Stage2.Stage3Version.Id == baseParams.Params["id"].To<long>())
                     .Select(x => new
                     {
                         x.Id,
                         St3Id = x.Stage2.Stage3Version.Id,
                         x.Stage2.Stage3Version.IndexNumber,
                         FirstPlanYear = x.Stage2.Stage3Version.Year,
                         x.PlanYear,
                         x.Stage2.Stage3Version.FixedYear
                     })
                     .FirstOrDefault();

            return new BaseDataResult(value);
        }

        public override IDataResult List(IDomainService<DpkrCorrectionStage2> domainService, BaseParams baseParams)
        {
            var loadParam = GetLoadParam(baseParams);

            var type = baseParams.Params.GetAs("type", 0);

            // параметркоторый отвечает за то, будет ли группировка по 3 этапу или нет
            // поскольку реестр корректировки должен показывт ьтольк опо 3 этапу но если провалится в редатирвоание 
            // чтобы изменить год там будет детализация по записям корректировки
            var st3Id = loadParam.Filter.GetAs("st3Id", 0l);

            /* 
               Поскольку записи в 3 этапе могут быть сгруппированы то в корректировке необходимо показвать записи не 2этапа а 3го
               то есть должно быть так 'Лифт, Крыша' 
            */

            var correctionQuery = _provider.GetCorrectionData(baseParams)
                .WhereIf(st3Id > 0, x => x.Stage2.Stage3Version.Id == st3Id)
                .WhereIf(type > 0, x => x.TypeResult == (TypeResultCorrectionDpkr)type);

            if ( st3Id > 0 )
            {
                // если нужно получить записи по 3 этапу, то получаем все корректировки 3 этапа , так как их может быть несколько 

                var dictPublishBySt2 = publishedRecordDomain.GetAll()
                    .Where(x => correctionQuery.Any(y => y.Stage2.Id == x.Stage2.Id))
                    .Select(x => new { st2Id = x.Stage2.Id, x.PublishedYear })
                    .AsEnumerable()
                    .GroupBy(x => x.st2Id)
                    .ToDictionary(x => x.Key, y => y.Select(z => z.PublishedYear).FirstOrDefault());

                var data = correctionQuery
                    .Select( x => new {
                        x.Id,
                        x.Stage2.Sum,
                        CommonEstateObjectName = x.Stage2.CommonEstateObject.Name,
                        FirstPlanYear = x.Stage2.Stage3Version.Year,
                        st2Id = x.Stage2.Id,
                        x.PlanYear
                    })
                    .AsEnumerable()
                    .Select( x => new {
                        x.Id,
                        x.Sum,
                        x.CommonEstateObjectName,
                        x.FirstPlanYear,
                        x.PlanYear,
                        PublishYear = dictPublishBySt2.ContainsKey(x.st2Id) ? dictPublishBySt2[x.st2Id] : 0
                    })
                    .AsQueryable()
                    .Filter(loadParam, Container);

                var count = data.Count();

                return new ListDataResult(data.Order(loadParam)
                    .OrderIf(loadParam.Order.Length == 0, true, x => x.PlanYear)
                    .Paging(loadParam), count);

            }
            else
            {
                // получаем те записи корректировки у которых скорректированный год превышает год публикации
                // и фиксируем в словаре только Id 3 этапа
                // потомучто в 3м этапе могут быть несколько ООи и какаято одна из них может превышат ьгод публикации а какаят оможет не превышать
                // тоест ьнаходит хотябы одну и фиксируем Id 3 этапа
                var dictPublishBySt3 = correctionQuery
                    .Where(x => publishedRecordDomain.GetAll().Any(y => y.Stage2.Id == x.Stage2.Id && x.PlanYear > y.PublishedYear))
                    .Select(x => x.Stage2.Stage3Version.Id)
                    .AsEnumerable()
                    .Distinct()
                    .GroupBy(x => x)
                    .ToDictionary(x => x.Key, y => y.FirstOrDefault());

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

                var st3Data = versionRecordDomain.GetAll()
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
                        x.IndexNumber,
                        x.FixedYear
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
                        PublishYearExceeded = dictPublishBySt3.ContainsKey(x.Id),
                        x.FixedYear
                    })
                    .AsQueryable()
                    .Filter(loadParam, Container);

                var count = st3Data.Count();

                return new ListDataResult(st3Data.Order(loadParam)
                    .OrderIf(loadParam.Order.Length == 0, true, x => x.PlanYear)
                    .Paging(loadParam), count);
            }
            
        }
    }
}