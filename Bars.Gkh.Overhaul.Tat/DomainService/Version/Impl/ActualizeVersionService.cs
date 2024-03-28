namespace Bars.Gkh.Overhaul.Tat.DomainService.Impl
{
    using System.Data;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Tat.Enum;
    using Bars.Gkh.Overhaul.Tat.PriorityParams;
    using Bars.Gkh.Overhaul.Tat.PriorityParams.Impl;
    using Bars.Gkh.Utils;

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Overhaul.Tat.ConfigSections;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;

    using Castle.Windsor;
    using Gkh.Entities.CommonEstateObject;
    using NHibernate.Linq;

    /// <summary>
    /// Сервис для актуализации версии
    /// </summary>
    public class ActualizeVersionService : IActualizeVersionService
    {
        #region

        // объявляем Код для услуги ПСД расзработка
        private readonly string servicePsdRazrabotka = "1018";

        // Объявляем Код для услуги ПСД экспертиза
        private readonly string servicePsdExpertiza = "1019";

        // Объявляем Код для Услуги Технадзор
        private readonly string serviceTehnadzor = "1020";

        #endregion

        public IWindsorContainer Container { get; set; }

        public IDomainService<ProgramCr> ProgramCrDomain { get; set; }

        public IDomainService<VersionActualizeLog> LogDomain { get; set; }

        public IActualizeVersionLogService LogService { get; set; }

        public IUserIdentity User { get; set; }

        public IDataResult ActualizeFromShortCr(BaseParams baseParams)
        {
            var logDomain = Container.Resolve<IDomainService<VersionActualizeLog>>();
            var versSt3Domain = Container.Resolve<IDomainService<VersionRecord>>();
            var versSt2Domain = Container.Resolve<IDomainService<VersionRecordStage2>>();
            var versSt1Domain = Container.Resolve<IDomainService<VersionRecordStage1>>();
            var twpHistoryDomain = Container.Resolve<IDomainService<TypeWorkCrHistory>>();
            var versionDomain = Container.Resolve<IDomainService<ProgramVersion>>();
            var programCrDomain = Container.Resolve<IDomainService<ProgramCr>>();
            var workDomain = Container.Resolve<IDomainService<Work>>();
            var roDomain = Container.Resolve<IDomainService<RealityObject>>();
            var objectCrDomain = Container.Resolve<IDomainService<ObjectCr>>();
            var roStElDomain = Container.Resolve<IDomainService<RealityObjectStructuralElement>>();
            var typeWorkCrHistoryDomain = Container.Resolve<IDomainService<TypeWorkCrHistory>>();
            var typeWorkCrRemovalDomain = Container.Resolve<IDomainService<TypeWorkCrRemoval>>();
            var typeWorkCrRefDomain = Container.Resolve<IDomainService<TypeWorkCrVersionStage1>>();
            var typeWorkCrDomain = Container.Resolve<IRepository<TypeWorkCr>>();// иуи нужен именно IRepository чтобы получить всеработы и существующие и удаленные
            var shortProgramRecService = Container.Resolve<IShortProgramRecordService>();
            var strElWorksDomain = Container.Resolve<IDomainService<StructuralElementWork>>();
            var shortPrgRecordDomain = Container.Resolve<IRepository<ShortProgramRecord>>();
            var correctionDomain = Container.Resolve<IDomainService<DpkrCorrectionStage2>>();
            var publishPrgDomain = Container.Resolve<IDomainService<PublishedProgramRecord>>();

            try
            {

                var versionId = baseParams.Params.GetAs<long>("versionId");
                var municipalityId = baseParams.Params.GetAs<long>("municipalityId");

                var programCrId = baseParams.Params.GetAs("programCrId", 0);
                var config = Container.GetGkhConfig<OverhaulTatConfig>();
                var endPeriod = config.ProgrammPeriodEnd;

                var version = versionDomain.FirstOrDefault(x => x.Id == versionId);

                if (version == null)
                {
                    return new BaseDataResult(false, string.Format("Не удалось получить версию по Id {0}", versionId));
                }

                var programCr = programCrDomain.FirstOrDefault(x => x.Id == programCrId);

                if (programCr == null)
                {
                    return new BaseDataResult(false, string.Format("Не удалось получить программу по Id {0}", programCrId));
                }

                if (municipalityId == 0)
                {
                    return new BaseDataResult(false, "Не выбрано МО");
                }

                // поулчаем есть ли в справочнике услуги с кодами 1018, 1019, 1020
                var services = workDomain.GetAll()
                    .Where(x => x.TypeWork == TypeWork.Service &&
                        (x.Code == serviceTehnadzor || x.Code == servicePsdExpertiza || x.Code == servicePsdRazrabotka))
                    .AsEnumerable()
                    .GroupBy(x => x.Code)
                    .ToDictionary(x => x.Key, y => y.First());

                if (!services.ContainsKey(serviceTehnadzor))
                {
                    return new BaseDataResult(false, "В справочнике отсутствует услуга 'Технадзор'");
                }

                if (!services.ContainsKey(servicePsdExpertiza))
                {
                    return new BaseDataResult(false, "В справочнике отсутствует услуга 'ПСД экспертиза'");
                }

                if (!services.ContainsKey(servicePsdRazrabotka))
                {
                    return new BaseDataResult(false, "В справочнике отсутствует услуга 'ПСД разработка'");
                }

                var stage1ToSave = new List<VersionRecordStage1>();
                var stage2ToSave = new List<VersionRecordStage2>();
                var stage3ToSave = new List<VersionRecord>();
                var stage1ToDelete = new List<long>();
                var stage2ToDelete = new List<long>();
                var stage3ToDelete = new List<long>();
                var typeWorkStage1ToSave = new Dictionary<VersionRecordStage1, TypeWorkCrVersionStage1>();

                #region Подготовка данных
                // если в какойто из работ все таки небудет указан плановый год ремонта, то тогда 
                var defaultYear = programCr.Period.DateStart.Year;

                // Запрос на записи добавленные в программе
                var addHistoryQuery =
                    twpHistoryDomain.GetAll().Where(x =>
                        x.TypeWorkCr.ObjectCr.ProgramCr.Id == programCrId && x.TypeAction == TypeWorkCrHistoryAction.Creation);



                var ocrQuery = objectCrDomain.GetAll()
                    .Where(y => y.ProgramCr.Id == programCrId)
                    .Where(y => y.RealityObject.Municipality.Id == municipalityId);

                var typeWorkQuery = typeWorkCrDomain.GetAll()
                    .Where(x => ocrQuery.Any(y => y.Id == x.ObjectCr.Id));

                // Получаем только работы собсвенно, которые и будут либо изменены, либо добавлены, либо удалены из ДПКР 
                var dataWorks = typeWorkQuery
                    .Where(x => x.Work.TypeWork == TypeWork.Work && x.IsActive)
                    .Select(
                        x =>
                            new ShortRecordProxy
                            {
                                Id = x.Id,
                                RealityObjectId = x.ObjectCr.RealityObject.Id,
                                WorkId = x.Work.Id,
                                WorkName = x.Work.Name,
                                Cost = x.Sum.HasValue ? x.Sum.Value : 0m,
                                TotalCost = x.Sum.HasValue ? x.Sum.Value : 0m,
                                Volume = x.Volume.HasValue ? x.Volume.Value : 0m,
                                TypeDpkrRecord = TypeDpkrRecord.UserRecord,
                                ShortYearStart = x.YearRepair.HasValue ? x.YearRepair.Value : defaultYear
                            })
                    .AsEnumerable()
                    .GroupBy(x => x.RealityObjectId)
                    .ToDictionary(x => x.Key, y => y.ToList());

                // полчучаем все записи 1 этапа
                var st1query =
                    versSt1Domain.GetAll()
                        .Where(
                            x =>
                                x.Stage2Version.Stage3Version.ProgramVersion.Id == versionId
                                && x.Stage2Version.Stage3Version.RealityObject.Municipality.Id == municipalityId);

                // получаем id записей которые были добавлены ы краткосрочке
                var st1DictHistoryCreation =
                    typeWorkCrRefDomain.GetAll()
                        .Where(x => addHistoryQuery.Any(y => x.TypeWorkCr.Id == y.TypeWorkCr.Id))
                        .Select(x => x.Stage1Version.Id)
                        .Distinct()
                        .AsEnumerable()
                        .GroupBy(x => x)
                        .ToDictionary(x => x.Key, y => y.FirstOrDefault());

                var st1Data = versSt1Domain.GetAll()
                    .Where(x => st1query.Any(y => y.Id == x.Id))
                    .Select(x => new
                    {
                        st2Id = x.Stage2Version.Id,
                        st3Id = x.Stage2Version.Stage3Version.Id,
                        x.Volume,
                        address = x.RealityObject.Address,
                        ceoNames = x.Stage2Version.Stage3Version.CommonEstateObjects,
                        x.Stage2Version.Stage3Version.Sum,
                        x.Stage2Version.Stage3Version.Year,
                        x.Stage2Version.Stage3Version.CorrectYear,
                        x.Stage2Version.Stage3Version.IndexNumber,
                        x.Stage2Version.Stage3Version.FixedYear
                    })
                    .AsEnumerable();

                // значения старые по 3 этапу
                var oldData = st1Data
                    .GroupBy(x => x.st3Id)
                    .ToDictionary(
                        x => x.Key,
                        y => new
                        {
                            ceoNames = y.Select(z => z.ceoNames).FirstOrDefault(),
                            volume = y.Sum(z => z.Volume),
                            sum = y.Select(z => z.Sum).FirstOrDefault(),
                            number = y.Select(z => z.IndexNumber).FirstOrDefault(),
                            year = y.Select(z => z.Year).FirstOrDefault(),
                            correctionYear = y.Select(z => z.CorrectYear).FirstOrDefault(),
                            address = y.Select(z => z.address).FirstOrDefault(),
                            fixedYear = y.Select(z => z.FixedYear).FirstOrDefault()
                        });

                var st1List = st1query.ToList();

                // получаем все записи 1 этапа сгруппированные по КЭ дома нужно тчобы понимат ьпо каким записям бежат ьесли нужно удалить все элементы
                var st1dictByRoStrEl =
                    st1query
                        .Where(x => x.StructuralElement != null)
                        .Select(x => new { x.Id, roStrElId = x.StructuralElement.Id })
                        .AsEnumerable()
                        .GroupBy(x => x.roStrElId)
                        .ToDictionary(x => x.Key, y => y.Select(z => z.Id).ToList());

                // Перечень домов с неоднозначно сопоставленными с КЭ дома работами
                var roWithoutStrElRefList = st1query
                    .Where(x => x.StructuralElement == null)
                    .Select(x => x.RealityObject.Id + "_" + x.StrElement.Id)
                    .Distinct()
                    .ToList();

                var st1Dict = st1List.ToDictionary(x => x.Id);

                var st2Query = versSt2Domain.GetAll().Where(x =>
                    x.Stage3Version.ProgramVersion.Id == versionId && x.Stage3Version.RealityObject.Municipality.Id == municipalityId);

                var st3Query = versSt3Domain.GetAll().Where(x => x.ProgramVersion.Id == versionId && x.RealityObject.Municipality.Id == municipalityId);

                var st2Data =
                    st2Query.Select(
                        x =>
                            new
                            {
                                x.Id,
                                roId = x.Stage3Version.RealityObject.Id,
                                ceoId = x.CommonEstateObject.Id,
                                ceoWeight = x.CommonEstateObject.Weight,
                                x.Stage3Version.CorrectYear,
                                st2 = x
                            }).ToList();

                var correctionDictBySt2 =
                    correctionDomain.GetAll()
                        .Where(x => st2Query.Any(y => y.Id == x.Stage2.Id))
                        .Select(x => new { st2Id = x.Stage2.Id, x.Id })
                        .AsEnumerable()
                        .GroupBy(x => x.st2Id)
                        .ToDictionary(x => x.Key, y => y.Select(z => z.Id).First());

                var publishPrgDictBySt2 =
                    publishPrgDomain.GetAll()
                        .Where(x => st2Query.Any(y => y.Id == x.Stage2.Id))
                        .Select(x => new { st2Id = x.Stage2.Id, x.Id })
                        .AsEnumerable()
                        .GroupBy(x => x.st2Id)
                        .ToDictionary(x => x.Key, y => y.Select(z => z.Id).First());

                var st2List = st2Data.Select(x => x.st2).ToList();

                // получаем запись 2 этапа по ключу Дом+ООИ+Год
                var st2DictByCeo = st2Data
                    .GroupBy(x => x.roId + "_" + x.ceoId + "_" + x.CorrectYear)
                    .ToDictionary(x => x.Key, y => y.Select(z => z.st2).First());

                // получаем запись второго этапа по Id 
                var st2DictById = st2Data
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.Select(z => z.st2).First());

                var st3DictById = st3Query.AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.FirstOrDefault());

                // Получаем только работы которые удалены из краткосрочки
                var dataWorksRemoval = typeWorkQuery
                    .Where(x => x.Work.TypeWork == TypeWork.Work && !x.IsActive)
                    .Select(
                        x =>
                            new
                            {
                                x.Id,
                                RealityObjectId = x.ObjectCr.RealityObject.Id,
                                WorkId = x.Work.Id,
                                Cost = x.Sum.HasValue ? x.Sum.Value : 0m,
                                Volume = x.Volume.HasValue ? x.Volume.Value : 0m,
                                TypeDpkrRecord = TypeDpkrRecord.UserRecord,
                                ShortYearStart = x.YearRepair.HasValue ? x.YearRepair.Value : defaultYear
                            })
                    .AsEnumerable()
                    .GroupBy(x => x.RealityObjectId)
                    .ToDictionary(x => x.Key, y => y.ToList());

                // Получаем только услуги которые необходимы ,чтобы получить Стоимость работы с учетом услуг
                // если в объекте КР имеем следуюещие виды работ:
                //
                // Крыша = 240   тыс/руб
                // Фасад = 105,6 тыс/руб
                // ПСД экспертиза = 20   тыс/руб
                // ПСД разработка = 8   тыс/руб
                // Технадзор = 13   тыс/руб
                // Ремонт подъезда = 175  тыс/руб
                //
                // то в результате в ДПКР уйдут стоимости следующие:
                //
                // Крыша = 240   + 14 + 5,6 + 5,98 = 265,58 тыс руб
                // Фасад = 105,6 + 6 + 2,4 + 2,6 = 116,6
                // Ремонт подъезда = 175  + 4,42 = 179,42
                var dataServcies = typeWorkQuery
                    .Where(x => x.Work.TypeWork == TypeWork.Service)
                    .Where(x => x.Work.Code == serviceTehnadzor || x.Work.Code == servicePsdExpertiza || x.Work.Code == servicePsdRazrabotka)
                    .Select(
                        x =>
                            new
                            {
                                x.Id,
                                RoId = x.ObjectCr.RealityObject.Id,
                                WorkCode = x.Work.Code,
                                Cost = x.Sum.HasValue ? x.Sum.Value : 0m
                            })
                    .AsEnumerable()
                    .GroupBy(x => x.RoId)
                    .ToDictionary(x => x.Key, y => y.GroupBy(z => z.WorkCode).ToDictionary(z => z.Key, x => x.Sum(k => k.Cost)));

                // поулчаем связку вида работ и записи 1 этапа ДПКР
                // может быть либо 1 либо несколкь оссылок на один вид работы
                // поскольку может быть 4 лифта в ДПКР по одному году, но в Объекте кр будет только 1 вид работы по Ремонту лифта
                // следовательно для удобства дальнейшей обработки я привожу либо в Тип  либ ок списку
                // чаще всег обудет Id в 1 экземпляре тоесть будет Тип
                var refQuery = typeWorkCrRefDomain.GetAll()
                    .Where(
                        x =>
                            x.Stage1Version.Stage2Version.Stage3Version.ProgramVersion.Id == versionId
                            && x.TypeWorkCr.ObjectCr.ProgramCr.Id == programCrId);

                var typeRefList = refQuery.Select(
                    x =>
                        new
                        {
                            x.Id,
                            TwcrId = x.TypeWorkCr.Id,
                            st1Id = x.Stage1Version.Id,
                            x.Stage1Version.TypeDpkrRecord
                        }).ToList();

                var typeWorkCrRefDpkr = typeRefList.Select(x => new
                    {
                        x.TwcrId,
                        x.TypeDpkrRecord,
                        Stage1Version = st1Dict[x.st1Id]
                    })
                    .GroupBy(x => x.TwcrId)
                    .ToDictionary(
                        x => x.Key,
                        y =>
                            new
                            {
                                TypeDpkrRecord = y.Select(z => z.TypeDpkrRecord).FirstOrDefault(),
                                Stage1 = y.Select(z => z.Stage1Version).FirstOrDefault(),
                                Stage1List = y.Select(z => z.Stage1Version).ToList()
                            });

                // Получаем все КЭ по дому, на случай если придется создвать запись 1 этапа
                // то надо будет по виду работы опредеят ьна какой КЭ сажать запись
                // Иначе придется создавать без связи с КЭ по дому 
                var roStrElDict = roStElDomain.GetAll()
                    .Where(x => ocrQuery.Any(y => y.RealityObject.Id == x.RealityObject.Id))
                    .Where(x => x.State.StartState) // берем только КЭ со статусом Актуальный
                    .Select(
                        x => new
                        {
                            x.Id,
                            x.Volume,
                            strElId = x.StructuralElement.Id,
                            roId = x.RealityObject.Id
                        })
                    .AsEnumerable()
                    .GroupBy(x => x.roId + "_" + x.strElId)
                    .ToDictionary(x => x.Key, y => y.FirstOrDefault());

                // получаем по WorkId Сруктурный Элемент и ООИ в котором он находится 
                // страшная жесть но сказали что на одну работу должен быть 1 КЭ и 1 ООИ
                var strElEnumerable =
                    strElWorksDomain.GetAll()
                        .Select(
                            x =>
                                new WorkStrElInfo
                                {
                                    WorkId = x.Job.Work.Id,
                                    StrElId = x.StructuralElement.Id,
                                    CeoId = x.StructuralElement.Group.CommonEstateObject.Id,
                                    CeoName = x.StructuralElement.Group.CommonEstateObject.Name,
                                    CeoWeight = x.StructuralElement.Group.CommonEstateObject.Weight
                                })
                        .AsEnumerable();

                // словарь для быстрого получения по виду работ нужные КЭ
                var infoStructElementsDict =
                    strElEnumerable
                        .GroupBy(x => x.WorkId)
                        .ToDictionary(x => x.Key,
                            y => new
                            {
                                list = y.ToList(),
                                firstElement = y.FirstOrDefault()
                            });

                // слвоарь для быстрого получения по КЭ нужное ООИ
                var ceoByStrEl =
                    strElEnumerable
                        .GroupBy(x => x.StrElId)
                        .ToDictionary(x => x.Key,
                            y => new
                            {
                                CeoWeight = y.Select(z => z.CeoWeight).FirstOrDefault(),
                                CeoId = y.Select(z => z.CeoId).FirstOrDefault(),
                                CeoName = y.Select(z => z.CeoName).FirstOrDefault()
                            });

                // словарь для получения причины удаления вида работ из объекта кр
                var removalDict =
                    typeWorkCrRemovalDomain.GetAll()
                        .Where(x => x.TypeWorkCr.ObjectCr.ProgramCr.Id == programCrId)
                        .AsEnumerable()
                        .GroupBy(x => x.TypeWorkCr.Id)
                        .ToDictionary(x => x.Key, y => y.FirstOrDefault());

                #endregion

                using (var transaction = Container.Resolve<IDataTransaction>())
                {
                    try
                    {

                        #region Создаем или изменяем работы в ДПКР
                        foreach (var kvp in dataWorks)
                        {
                            var roId = kvp.Key;

                            // Проставляем ссылку на VersionStage1 либо создаем новые ссылки
                            foreach (var workData in kvp.Value)
                            {
                                // Если ссылки на ДПКР есть то берем их
                                // иначе создаем новую запись в ДПКР
                                if (typeWorkCrRefDpkr.ContainsKey(workData.Id))
                                {
                                    var refData = typeWorkCrRefDpkr[workData.Id];

                                    // Для того чтобы при расчете услуги понимать 
                                    // какие записи относятся к добавленым в ручную, а какие расчитаны в ДПКР
                                    workData.TypeDpkrRecord = refData.TypeDpkrRecord;

                                    if (refData.Stage1List.Count == 1)
                                    {
                                        workData.Stage1 = refData.Stage1;
                                        workData.Stage1Id = refData.Stage1.Id;
                                    }

                                    workData.Stage1List = refData.Stage1List;

                                }
                                else
                                {

                                    // поскольку работы еще не существовало то необходимо определить на какой КЭ сядет данная работа
                                    // стоит знать то что кЭ для одного вида работы может быть несколько
                                    // например работа может быть Ремонт крыши , а КЭ могут быть 'Черепичная крыша' ,'деревянная крыша'

                                    if (!infoStructElementsDict.ContainsKey(workData.WorkId))
                                    {
                                        return new BaseDataResult(false,
                                            string.Format(
                                                "Для вида работ {0} нет ни одной работы, связанной с конструктивным элементом. В ДПКР попадают только виды работ, связанные с КЭ и ООИ.",
                                                workData.WorkName));
                                    }

                                    var StrEls = infoStructElementsDict[workData.WorkId];

                                    foreach (var strEl in StrEls.list)
                                    {
                                        var roStrLeKey = roId + "_" + strEl.StrElId;

                                        // проверяем если данный кэ существует в Доме, то для него создраем запись
                                        if (roStrElDict.ContainsKey(roStrLeKey))
                                        {
                                            var roStrEl = roStrElDict[roStrLeKey];

                                            VersionRecordStage1 st1;
                                            VersionRecordStage2 st2 = null;
                                            TypeWorkCrVersionStage1 twRef;
                                            CreateStage1(out st1,
                                                out twRef,
                                                workData.Id,
                                                roId,
                                                roStrEl.strElId,
                                                roStrEl.Id,
                                                workData.ShortYearStart,
                                                roStrEl.Volume);

                                            var ceoId = 0l;
                                            var ceoWeight = 0;
                                            var ceoName = string.Empty;
                                            if (ceoByStrEl.ContainsKey(roStrEl.strElId))
                                            {
                                                ceoId = ceoByStrEl[roStrEl.strElId].CeoId;
                                                ceoWeight = ceoByStrEl[roStrEl.strElId].CeoWeight;
                                                ceoName = ceoByStrEl[roStrEl.strElId].CeoName;
                                            }

                                            var keySt2 = roId + "_" + ceoId + "_" + st1.Year;

                                            if (st2DictByCeo.ContainsKey(keySt2))
                                            {
                                                st2 = st2DictByCeo[keySt2];
                                            }
                                            else
                                            {
                                                st2 = new VersionRecordStage2
                                                {
                                                    CommonEstateObject = new CommonEstateObject { Id = ceoId, Name = ceoName },
                                                    CommonEstateObjectWeight = ceoWeight,
                                                    TypeCorrection = TypeCorrection.NotDone,
                                                    TypeDpkrRecord = TypeDpkrRecord.ActualizeAddRecord
                                                };

                                                var st3 = new VersionRecord
                                                {
                                                    Year = workData.ShortYearStart,
                                                    CorrectYear = workData.ShortYearStart,
                                                    FixedYear = true, // Фиксируем год  потому что запись добавляется
                                                    ProgramVersion = new ProgramVersion { Id = versionId },
                                                    RealityObject = new RealityObject { Id = roId },
                                                    TypeDpkrRecord = TypeDpkrRecord.ActualizeAddRecord
                                                };

                                                st2.Stage3Version = st3;

                                                st2List.Add(st2);

                                                st2DictByCeo.Add(keySt2, st2);
                                            }

                                            st1.Stage2Version = st2;

                                            // добавляем в список на сохранение
                                            stage1ToSave.Add(st1);

                                            // добавляем также в общий список записей 1 этапа так как потом придется п онему бежат ьи суммировать значения для 2 этапа 
                                            st1List.Add(st1);

                                            // запоминаем в словарь
                                            typeWorkStage1ToSave.Add(st1, twRef);

                                            if (workData.Stage1List == null)
                                            {
                                                workData.Stage1List = new List<VersionRecordStage1>();
                                            }

                                            workData.Stage1List.Add(st1);

                                        }
                                    }

                                    // если не удалось найти нужные КЭ по дому, то создаем всего 1 запись 1 этапа  Без связки с КЭ дома
                                    if (workData.Stage1List == null)
                                    {
                                        var first = StrEls.firstElement;

                                        VersionRecordStage1 st1;
                                        VersionRecordStage2 st2 = null;
                                        TypeWorkCrVersionStage1 twRef;
                                        CreateStage1(out st1, out twRef, workData.Id, roId, first.StrElId, null, workData.ShortYearStart, workData.Volume);

                                        // добавляем в список на сохранение
                                        stage1ToSave.Add(st1);

                                        // добавляем также в общий список записей 1 этапа так как потом придется п онему бежат ьи суммировать значения для 2 этапа 
                                        st1List.Add(st1);

                                        var ceoId = 0l;
                                        var ceoWeight = 0;
                                        var ceoName = string.Empty;

                                        if (ceoByStrEl.ContainsKey(first.StrElId))
                                        {
                                            ceoId = ceoByStrEl[first.StrElId].CeoId;
                                            ceoWeight = ceoByStrEl[first.StrElId].CeoWeight;
                                            ceoName = ceoByStrEl[first.StrElId].CeoName;
                                        }

                                        var keySt2 = roId + "_" + ceoId + "_" + st1.Year;

                                        if (st2DictByCeo.ContainsKey(keySt2))
                                        {
                                            st2 = st2DictByCeo[keySt2];
                                        }
                                        else
                                        {
                                            st2 = new VersionRecordStage2
                                            {
                                                CommonEstateObject = new CommonEstateObject { Id = ceoId, Name = ceoName },
                                                CommonEstateObjectWeight = ceoWeight,
                                                TypeCorrection = TypeCorrection.NotDone,
                                                TypeDpkrRecord = TypeDpkrRecord.ActualizeAddRecord
                                            };

                                            var st3 = new VersionRecord
                                            {
                                                Year = workData.ShortYearStart,
                                                CorrectYear = workData.ShortYearStart,
                                                ProgramVersion = new ProgramVersion { Id = versionId },
                                                RealityObject = new RealityObject { Id = roId },
                                                TypeDpkrRecord = TypeDpkrRecord.ActualizeAddRecord
                                            };

                                            st2.Stage3Version = st3;

                                            st2List.Add(st2);
                                            st2DictByCeo.Add(keySt2, st2);
                                        }

                                        st1.Stage2Version = st2;

                                        // запоминаем в словарь
                                        typeWorkStage1ToSave.Add(st1, twRef);

                                        if (workData.Stage1List == null)
                                        {
                                            workData.Stage1List = new List<VersionRecordStage1>();
                                        }

                                        workData.Stage1List.Add(st1);
                                    }

                                    if (workData.Stage1List != null && workData.Stage1List.Count == 1)
                                    {
                                        workData.Stage1 = workData.Stage1List.First();
                                    }
                                }
                            }

                            // если по дому были услуги то не обходимо по томуже алгоритму, что и в ДПКР 
                            // итоговую стоимость расчитать как = СтоиомстьРаботы + Стоимость услуги 
                            if (dataServcies.ContainsKey(roId))
                            {
                                shortProgramRecService.CalculationCosts(dataServcies[roId], kvp.Value);
                            }

                            // После того как подсчитаны Итоговые стоимости, нужно либо изменить существующие записи
                            // Либо создать их 
                            foreach (var workData in kvp.Value)
                            {
                                if (workData.Stage1List != null && workData.Stage1List.Count == 1)
                                {
                                    var st1 = workData.Stage1;

                                    var oldSum = st1.Sum;
                                    var oldService = st1.SumService;
                                    var oldVolume = st1.Volume;

                                    st1.Sum = workData.Cost;
                                    st1.SumService = workData.ServiceCost;
                                    st1.Volume = workData.Volume;

                                    if (st1.Id > 0)
                                    {
                                        var st2 = st2DictById[st1.Stage2Version.Id];
                                        var st3 = st3DictById[st2.Stage3Version.Id];
                                        var newYear = 0;

                                        if (st3.CorrectYear != workData.ShortYearStart || st1DictHistoryCreation.ContainsKey(st1.Id))
                                        {
                                            newYear = workData.ShortYearStart;

                                            st3.CorrectYear = newYear;
                                            st3.FixedYear = true;
                                            st2.Stage3Version = st3;
                                        }

                                        if (st1.Sum != oldSum || st1.SumService != oldService || st1.Volume != oldVolume ||
                                            (newYear > 0 && st1.Year != newYear) || (!string.IsNullOrEmpty(st3.CommonEstateObjects) &&
                                                st3.CommonEstateObjects.Contains("Grouping")))
                                        {
                                            // добавляем в список на сохранение
                                            if (newYear > 0)
                                            {
                                                st1.Year = newYear;
                                            }

                                            stage1ToSave.Add(st1);
                                        }
                                    }

                                }
                                else if (workData.Stage1List != null && workData.Stage1List.Count > 1)
                                {
                                    // КЭ больше чем 1 по данному виду работы то тогда начинается самое прикольное в этом методе 
                                    // Поскольку объемы ставят на 1 работу а в каждом КЭ есть свои объемы
                                    // то не обходимо получить долю объема по КЭ относителньо объема по работе 
                                    var cnt = workData.Stage1List.Count();

                                    var i = 0;
                                    var tempCost = 0m;
                                    var tempSumService = 0m;
                                    var tempVolume = 0m;

                                    var volumeByStrEl = workData.Stage1List.Sum(x => x.Volume); // объемы забитые в домах в КЭ
                                    var proportionVolume = 0m;

                                    if (workData.Volume != volumeByStrEl)
                                    {
                                        proportionVolume =
                                            workData.Volume / cnt; // поскольку объем забитый ы ыиде работ не соответсвует объемам забитым в КЭ
                                    }

                                    // нужно разделить пропорционально стоиомсть между всеми записями
                                    foreach (var st1 in workData.Stage1List)
                                    {
                                        var oldSum = st1.Sum;
                                        var oldService = st1.SumService;
                                        var oldVolume = st1.Volume;

                                        i++;

                                        var currentCost = 0m;
                                        var currentServiceCost = 0m;
                                        var curretVolume = 0m;

                                        if (i == cnt)
                                        {
                                            // Если работа последняя, то избегаем ошибок округления
                                            curretVolume = workData.Volume - tempVolume;
                                            currentCost = workData.Cost - tempCost;
                                            currentServiceCost = workData.ServiceCost - tempSumService;
                                        }
                                        else
                                        {
                                            // вычисляем сумму услуги исходя из доли каждой работы в общей стоимости
                                            curretVolume = proportionVolume > 0 ? proportionVolume : st1.Volume;
                                            currentCost = ((curretVolume / workData.Volume) * workData.Cost).RoundDecimal(2);
                                            currentServiceCost = ((curretVolume / workData.Volume) * workData.ServiceCost).RoundDecimal(2);
                                        }

                                        st1.Sum = currentCost;
                                        st1.SumService = currentServiceCost;
                                        st1.Volume = curretVolume;

                                        if (st1.Id > 0)
                                        {
                                            var st2 = st2DictById[st1.Stage2Version.Id];
                                            var st3 = st3DictById[st2.Stage3Version.Id];
                                            var newYear = 0;

                                            if (st3.CorrectYear != workData.ShortYearStart || st1DictHistoryCreation.ContainsKey(st1.Id) ||
                                                (!string.IsNullOrEmpty(st3.CommonEstateObjects) && st3.CommonEstateObjects.Contains("Grouping")))
                                            {
                                                newYear = workData.ShortYearStart;

                                                st3.CorrectYear = newYear;
                                                st3.FixedYear = true;
                                                st2.Stage3Version = st3;
                                            }

                                            if (st1.Sum != oldSum || st1.SumService != oldService || st1.Volume != oldVolume ||
                                                (newYear > 0 && st1.Year != newYear))
                                            {
                                                // добавляем в список на сохранение
                                                if (newYear > 0)
                                                {
                                                    st1.Year = newYear;
                                                }

                                                stage1ToSave.Add(st1);
                                            }

                                        }

                                        // Для того чтобы не было ошибок округления накапливаем Стоимость услуги
                                        tempCost += currentCost;
                                        tempSumService += currentServiceCost;
                                        tempVolume += curretVolume;
                                    }
                                }
                            }
                        }
                        #endregion

                        #region Удаляем работы из Дпкр
                        var newYearsDict = new Dictionary<VersionRecord, int>();

                        foreach (var kvp in dataWorksRemoval)
                        {
                            var roId = kvp.Key;

                            foreach (var work in kvp.Value)
                            {
                                if (!removalDict.ContainsKey(work.Id))
                                {
                                    continue;
                                }

                                var removal = removalDict[work.Id];

                                switch (removal.TypeReason)
                                {
                                    // Перенос срока ремонта на более поздний срок
                                    case TypeWorkCrReason.NewYear:
                                    {
                                        if (typeWorkCrRefDpkr.ContainsKey(work.Id))
                                        {
                                            var refData = typeWorkCrRefDpkr[work.Id];

                                            var newYearRepair = removal.NewYearRepair.HasValue ? removal.NewYearRepair.Value : 0;

                                            foreach (var refSt1 in refData.Stage1List)
                                            {
                                                // получаем записи котоыре хотят перекинут ьна новый год
                                                // соответсвенно получаем что необходимо сдвинуть на нужное количество лет вск КЭ программы, 
                                                // а это значит, что возможно нужно будет и перегруппирвоать 1, 2, 3 этапы

                                                var oldSt2 = st2DictById[refSt1.Stage2Version.Id];
                                                var oldSt3 = st3DictById[oldSt2.Stage3Version.Id];

                                                var oldYear = oldSt3.CorrectYear;
                                                var delta = newYearRepair - oldYear;
                                                var fixedYear = true;

                                                if (delta == 0)
                                                {
                                                    continue;
                                                }

                                                // У дома есть несопоставленные с КЭ дома работы
                                                if (roWithoutStrElRefList.Contains(roId + "_" + refSt1.StrElement.Id))
                                                {
                                                    return new BaseDataResult(false, "При сопоставлении работ ДПКР с КПКР произошла ошибка. Обратитесь к администратору");
                                                }

                                                if (!st1dictByRoStrEl.TryGetValue(refSt1.StructuralElement.Id, out var st1Ids))
                                                {
                                                    continue;
                                                }

                                                var ceoId = 0l;
                                                var ceoWeight = 0;
                                                var ceoName = string.Empty;
                                                if (ceoByStrEl.ContainsKey(refSt1.StrElement.Id))
                                                {
                                                    ceoId = ceoByStrEl[refSt1.StrElement.Id].CeoId;
                                                    ceoWeight = ceoByStrEl[refSt1.StrElement.Id].CeoWeight;
                                                    ceoName = ceoByStrEl[refSt1.StrElement.Id].CeoName;
                                                }

                                                foreach (var st1Id in st1Ids)
                                                {
                                                    var st1 = st1Dict[st1Id];

                                                    oldSt2 = st2DictById[st1.Stage2Version.Id];
                                                    oldSt3 = st3DictById[oldSt2.Stage3Version.Id];
                                                    oldYear = st1Id == refSt1.Id ? oldSt3.CorrectYear : st1.Year;
                                                    fixedYear = st1Id == refSt1.Id;

                                                    var newYear = oldYear + delta;

                                                    if (oldYear == newYear)
                                                        continue;

                                                    if (newYear > endPeriod)
                                                    {
                                                        stage1ToDelete.Add(st1Id);
                                                    }

                                                    var keySt2 = roId + "_" + ceoId + "_" + newYear;

                                                    VersionRecordStage2 st2;
                                                    if (st2DictByCeo.ContainsKey(keySt2))
                                                    {
                                                        st2 = st2DictByCeo[keySt2];

                                                        if (st3DictById.ContainsKey(st2.Stage3Version.Id))
                                                        {
                                                            var st3 = st3DictById[st2.Stage3Version.Id];
                                                            st3.Year = newYear;
                                                            st3.CorrectYear = newYear;
                                                            st3.FixedYear = fixedYear;

                                                            if (!newYearsDict.ContainsKey(st3))
                                                            {
                                                                newYearsDict.Add(st3, newYear);
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        st2 = new VersionRecordStage2
                                                        {
                                                            CommonEstateObject = new CommonEstateObject { Id = ceoId, Name = ceoName },
                                                            CommonEstateObjectWeight = ceoWeight,
                                                            TypeCorrection = TypeCorrection.NotDone,
                                                            TypeDpkrRecord = TypeDpkrRecord.ActualizeAddRecord
                                                        };

                                                        var st3 = new VersionRecord
                                                        {
                                                            Year = newYear,
                                                            CorrectYear = newYear,
                                                            FixedYear = fixedYear,
                                                            ProgramVersion = new ProgramVersion { Id = versionId },
                                                            RealityObject = new RealityObject { Id = roId },
                                                            TypeDpkrRecord = TypeDpkrRecord.ActualizeAddRecord
                                                        };

                                                        st2.Stage3Version = st3;

                                                        st2List.Add(st2);

                                                        st2DictByCeo.Add(keySt2, st2);
                                                    }

                                                    st1.Stage2Version = st2;
                                                }

                                            }
                                        }
                                    }
                                        break;

                                    // Не требуется ремонт в рамках Долгосрочной программы
                                    // Значит необходимо найти все КЭ в ДПКР и удалить их
                                    case TypeWorkCrReason.NotRequiredDpkr:
                                    {
                                        if (typeWorkCrRefDpkr.ContainsKey(work.Id))
                                        {
                                            var refData = typeWorkCrRefDpkr[work.Id];

                                            foreach (var refSt1 in refData.Stage1List)
                                            {
                                                // У дома есть несопоставленные с КЭ дома работы
                                                if (roWithoutStrElRefList.Contains(roId + "_" + refSt1.StrElement.Id))
                                                {
                                                    return new BaseDataResult(false, "При сопоставлении работ ДПКР с КПКР произошла ошибка. Обратитесь к администратору");
                                                }

                                                // для каждого КЭ дома получаем все записи 1 этапа и удаляем их
                                                if (st1dictByRoStrEl.TryGetValue(refSt1.StructuralElement.Id, out var st1Ids))
                                                {
                                                    stage1ToDelete.AddRange(st1Ids);
                                                }
                                            }
                                        }
                                    }
                                        break;

                                    // Не требуется ремонт в рамках текущей Краткосрочной программы
                                    // Значит необходимо удалиить только запись по ссылке на ДПКР ,но удалять другие КЭ  других годах ненужно
                                    case TypeWorkCrReason.NotRequiredShortProgram:
                                    {
                                        if (typeWorkCrRefDpkr.ContainsKey(work.Id))
                                        {
                                            var refData = typeWorkCrRefDpkr[work.Id];

                                            foreach (var refSt1 in refData.Stage1List)
                                            {
                                                stage1ToDelete.Add(refSt1.Id);
                                            }
                                        }
                                    }
                                        break;

                                }
                            }
                        }
                        #endregion

                        // после того как определили полный список добавляемых, изменяемых и удаляемых объектов 1 этапа 
                        // не обходимо либо обновить или удалить записи 2го и 3го этапав

                        #region обрабатываем записи 2 этапа
                        var st2NewData = st1List
                            .Where(x => !stage1ToDelete.Contains(x.Id))
                            .GroupBy(x => x.Stage2Version)
                            .ToDictionary(x => x.Key,
                                y => new
                                {
                                    sum = y.Sum(z => z.Sum + z.SumService),
                                    volume = y.Sum(z => z.Volume)
                                });

                        foreach (var kvp in st2NewData)
                        {
                            var st2 = kvp.Key;
                            var oldSum = st2.Sum;
                            st2.Sum = kvp.Value.sum;
                            st2.Volume = kvp.Value.volume;

                            if (st2.Id == 0)
                            {
                                stage2ToSave.Add(st2);
                            }
                            else if (oldSum != st2.Sum)
                            {
                                stage2ToSave.Add(st2);
                            }
                        }

                        stage2ToDelete.AddRange(st2DictById.Keys.Where(x => !st2NewData.Keys.Any(y => y.Id == x)).ToList());
                        #endregion

                        // формируем логи актуализации
                        var log = new VersionActualizeLog();
                        log.ActualizeType = VersionActualizeType.ActualizeFromShortCr;
                        log.DateAction = DateTime.Now;
                        log.Municipality = municipalityId > 0 ? new Municipality() { Id = municipalityId } : null;
                        log.ProgramVersion = new ProgramVersion() { Id = versionId };
                        log.UserName = User.Name;

                        var logRecords = new List<ActualizeVersionLogRecord>();

                        #region обрабатываем записи 3 этапа
                        var st3NewData = st2List
                            .Where(x => !stage2ToDelete.Contains(x.Id))
                            .GroupBy(x => x.Stage3Version)
                            .ToDictionary(x => x.Key,
                                y => new
                                {
                                    sum = y.Sum(z => z.Sum),
                                    ceoNames = y.Select(z => z.CommonEstateObject.Name).ToList().Any()
                                        ? y.Select(z => z.CommonEstateObject.Name).ToList().Distinct()
                                            .Aggregate((str, res) => !string.IsNullOrEmpty(res) ? res + ", " + str : res)
                                        : string.Empty,
                                    volume = y.Sum(z => z.Volume)
                                });

                        foreach (var kvp in st3NewData)
                        {
                            var st3 = kvp.Key;

                            st3.Sum = kvp.Value.sum;
                            st3.CommonEstateObjects = kvp.Value.ceoNames;
                            var volume = kvp.Value.volume;

                            if (st3.Id == 0)
                            {
                                stage3ToSave.Add(st3);

                                var ro = roDomain.Load(st3.RealityObject.Id);

                                var logRecord = new ActualizeVersionLogRecord
                                {
                                    TypeAction = VersionActualizeType.ActualizeFromShortCr,
                                    Action = "Добавление",
                                    Description = string.Format(
                                        "Добавление при Актуализации из КПКР"),
                                    Address = ro.Address,
                                    Ceo = st3.CommonEstateObjects,
                                    PlanYear = st3.Year,
                                    CorrectYear = st3.CorrectYear,
                                    Volume = volume,
                                    Sum = st3.Sum
                                };

                                logRecords.Add(logRecord);
                            }
                            else if (oldData.ContainsKey(st3.Id) &&
                                (
                                    oldData[st3.Id].sum != st3.Sum
                                    || oldData[st3.Id].ceoNames != st3.CommonEstateObjects
                                    || oldData[st3.Id].year != st3.Year
                                    || oldData[st3.Id].correctionYear != st3.CorrectYear
                                    || oldData[st3.Id].fixedYear != st3.FixedYear
                                )
                                || newYearsDict.ContainsKey(st3))
                            {
                                stage3ToSave.Add(st3);

                                var old = oldData[kvp.Key.Id];

                                var logRecord = new ActualizeVersionLogRecord
                                {
                                    TypeAction = VersionActualizeType.ActualizeFromShortCr,
                                    Action = "Изменение",
                                    Description = string.Format(
                                        "Изменение при Актуализации из КПКР"),
                                    Address = old.address,
                                    Ceo = old.ceoNames,
                                    PlanYear = old.year,
                                    CorrectYear = old.correctionYear,
                                    Volume = old.volume,
                                    Sum = old.sum,
                                    Number = old.number,
                                    ChangeNumber = st3.IndexNumber != old.number ? st3.IndexNumber : 0,
                                    ChangeSum = st3.Sum != old.sum ? st3.Sum : 0m,
                                    ChangeCeo = st3.CommonEstateObjects != old.ceoNames ? st3.CommonEstateObjects : null,
                                    ChangePlanYear = st3.Year != old.year ? st3.Year : 0,
                                    ChangeCorrectYear = st3.CorrectYear != old.correctionYear ? st3.CorrectYear : 0,
                                    ChangeVolume = volume != old.volume ? volume : 0
                                };

                                logRecords.Add(logRecord);
                            }
                        }

                        var notExistSt3Ids = st3DictById.Keys.Where(x => !st3NewData.Keys.Any(y => y.Id == x)).ToList();

                        foreach (var delId in notExistSt3Ids)
                        {
                            if (oldData.ContainsKey(delId))
                            {
                                var old = oldData[delId];

                                var logRecord = new ActualizeVersionLogRecord
                                {
                                    TypeAction = VersionActualizeType.ActualizeFromShortCr,
                                    Action = "Удаление",
                                    Description = string.Format(
                                        "Удаление при Актуализации из КПКР"),
                                    Address = old.address,
                                    Ceo = old.ceoNames,
                                    PlanYear = old.year,
                                    CorrectYear = old.correctionYear,
                                    Volume = old.volume,
                                    Sum = old.sum,
                                    Number = old.number
                                };

                                logRecords.Add(logRecord);
                            }
                        }

                        stage3ToDelete.AddRange(notExistSt3Ids);
                        #endregion

                        if (!stage3ToSave.Any() && !stage2ToSave.Any() && !stage1ToSave.Any() && !typeWorkStage1ToSave.Any() && !stage1ToDelete.Any() &&
                            !stage2ToDelete.Any() && !stage3ToDelete.Any())
                        {
                            return new BaseDataResult(false, "Нет записей для сохранения");
                        }

                        stage3ToSave.ForEach(x =>
                        {
                            if (x.Id > 0) versSt3Domain.Update(x);
                            else versSt3Domain.Save(x);
                        });

                        stage2ToSave.ForEach(x =>
                        {
                            if (x.Id > 0) versSt2Domain.Update(x);
                            else versSt2Domain.Save(x);
                        });

                        stage1ToSave.ForEach(x =>
                        {
                            if (x.Id > 0) versSt1Domain.Update(x);
                            else versSt1Domain.Save(x);
                        });

                        typeWorkStage1ToSave.Values.ForEach(x => { typeWorkCrRefDomain.Save(x); });

                        if (stage1ToDelete.IsNotEmpty())
                        {
                            typeWorkCrRefDomain.GetAll()
                                .Where(x => stage1ToDelete.Contains(x.Stage1Version.Id))
                                .Select(x => x.Id)
                                .AsEnumerable()
                                .ForEach(x => typeWorkCrRefDomain.Delete(x));

                            shortPrgRecordDomain.GetAll()
                                .Where(x => stage1ToDelete.Contains(x.Stage1.Id))
                                .Select(x => x.Id)
                                .AsEnumerable()
                                .ForEach(x => shortPrgRecordDomain.Delete(x));
                            
                            stage1ToDelete.ForEach(x => versSt1Domain.Delete(x));
                        }
                        
                        stage2ToDelete.ForEach(id =>
                        {
                            // сначала удаляем связи
                            if (correctionDictBySt2.ContainsKey(id))
                            {
                                correctionDomain.Delete(correctionDictBySt2[id]);
                            }

                            if (publishPrgDictBySt2.ContainsKey(id))
                            {
                                publishPrgDomain.Delete(publishPrgDictBySt2[id]);
                            }

                            versSt2Domain.Delete(id);
                        });

                        stage3ToDelete.ForEach(id => { versSt3Domain.Delete(id); });

                        log.CountActions = logRecords.Count();
                        log.LogFile = LogService.CreateLogFile(
                            logRecords.OrderBy(x => x.Address).OrderBy(x => x.Number),
                            baseParams);

                        logDomain.Save(log);

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            finally
            {
                Container.Release(logDomain);
                Container.Release(programCrDomain);
                Container.Release(versSt3Domain);
                Container.Release(versSt2Domain);
                Container.Release(versSt1Domain);
                Container.Release(versionDomain);
                Container.Release(typeWorkCrDomain);
                Container.Release(typeWorkCrRefDomain);
                Container.Release(typeWorkCrRemovalDomain);
                Container.Release(typeWorkCrHistoryDomain);
                Container.Release(shortProgramRecService);
                Container.Release(strElWorksDomain);
                Container.Release(roStElDomain);
                Container.Release(objectCrDomain);
                Container.Release(correctionDomain);
                Container.Release(publishPrgDomain);
                Container.Release(shortPrgRecordDomain);
            }

            return new BaseDataResult();
        }

        private void CreateStage1(out VersionRecordStage1 st1, out TypeWorkCrVersionStage1 twRef, long typeWorkCrId,  long roId, long strElId, long? roStrElId, int year, decimal volume)
        {
            st1 = new VersionRecordStage1
            {
                RealityObject = new RealityObject { Id = roId },
                StrElement = new StructuralElement { Id = strElId },
                StructuralElement = roStrElId.HasValue && roStrElId.Value > 0 ? new RealityObjectStructuralElement {Id = roStrElId.Value}: null,
                Volume = volume,
                Year = year,
                TypeDpkrRecord = TypeDpkrRecord.ActualizeAddRecord
            };

            twRef = new TypeWorkCrVersionStage1
            {
                Stage1Version = st1,
                TypeWorkCr = new TypeWorkCr { Id = typeWorkCrId },
                Volume = st1.Volume
            };
        }

        private class WorkStrElInfo
        {
            public long WorkId { get; set; }
            public long StrElId { get; set; }
            public long CeoId { get; set; }
            public int CeoWeight { get; set; }
            public string CeoName { get; set; }
        }
        
        public IDataResult ActualizeGroup(BaseParams baseParams)
        {
/* Требование 57368
   Требуется: в существующей версии сделать группировку ООИ в одну запись. Сгруппировать надо только ООИ: ХВС, ГВС, Водоотведение, Подвалы. 
   А остальные записи оставить без группировки. Но это будет регулироваться включен ООИ в группу или нет.
Реализовать:
1. В версию программы добавить действие актуализации ДПКР - Группировка ООИ.
2. При нажатии на действие выполнять:
    •	Выводить сообщение: В результате выполнения действия записи будут сгруппированы по группам ООИ. Продолжить? Варианты: Да или Нет.
    •	В случае выбора "Нет" ничего не проводить.
    •	В случае выбора "Да" проводить группировку записей по группе ООИ за заданный период по одному дому.
        o	Группа ООИ. Определяется в: Справочники--Капитальный ремонт--ООИ--форма ООИ--поле "Тип группы". Если у ООИ равен Тип группы, то эти записи относятся 
             к одной группе и их надо группировать. !!!Примечание: Я не знаю, используется это поле еще где-то или нет. Вроде не должно учитваться. Если учитывается, сообщите мне.
        o	Период Группировки задается в: Капитальный ремонт--Параметры программы капитального ремонта--Настройка параметров--поле "Период группировки ООИ (год)".
        o	Новый год записи = минимальный год из всех записей.
        o	Запись отображать как: 1 запись в версии, адрес дома, ООИ отображать через запятую.
        o	Алгоритм группировки: 
            	по дому по каждой группе ООИ найти записи. 
            	Среди записей ООИ найти запись с наименьшим годом.
            	Проверить наличие записей в следующих годах. Период проверки: [Год ООИ; ((Год ООИ + Период группировки ООИ из настроек)-1)]. Если записи есть, то их сгруппировать.
            	Найти запись следующую за последней сгруппированной записью.
            	Проверить наличие записей в следующих годах. Период проверки: [Год ООИ; ((Год ООИ + Период группировки ООИ из настроек)-1)]. Если записи есть, то их сгруппировать.
            	и т.д.
Пример: у ООИ = Крыша и ООИ = Фасад Тип группы = Основные КЭ, Период группировки = 3. У дома Ленина, д. 2 Год ремонта крыши = 2014, Год ремонта фасада = 2016.
Такие записи надо объединить в одну запись и присвоить им год = 2016.

             */
            var versionId = baseParams.Params.GetAs<long>("versionId");
            var municipalityId = baseParams.Params.GetAs<long>("municipalityId");
            var actualizeStart = baseParams.Params.GetAs("yearStart", 0);
            var config = Container.GetGkhConfig<OverhaulTatConfig>();
            var versSt3Domain = Container.Resolve<IDomainService<VersionRecord>>();
            var versSt2Domain = Container.Resolve<IDomainService<VersionRecordStage2>>();
            var versSt1Domain = Container.Resolve<IDomainService<VersionRecordStage1>>();
            var ceoDomain = Container.Resolve<IDomainService<CommonEstateObject>>();
            var sessionProvider = Container.Resolve<ISessionProvider>();
            var unProxy = Container.Resolve<IUnProxy>();
            
            try
            {
                var st3ToSave = new Dictionary<long, VersionRecord>();
                var st2ToSave = new Dictionary<long, VersionRecordStage2>();
                var newRef = new Dictionary<long, VersionRecord>();
                var stage3Period = config.GroupByCeoPeriod;

                // берем тлкьо те записи, которых несколько по типу группы
                var typeIds = new List<long>();
                var deoGroup = ceoDomain.GetAll()
                .Where(x => x.GroupType != null)
                .Select(x => new { x.Id, GroupTypeId = x.GroupType.Id })
                .AsEnumerable()
                .GroupBy(x => x.GroupTypeId)
                .ToDictionary(x => x.Key, y => y.Select(z => z.Id).Count());

                foreach (var gr in deoGroup)
                {
                    if (gr.Value > 1)
                    {
                        // если по типу больше чем 1 запись ООИ то берем ее в наш список 
                        typeIds.Add(gr.Key);
                    }
                }

                // получаем просто те записи у которых есть группы  
                var groupQuery = versSt2Domain.GetAll()
                .Where(x => x.Stage3Version.ProgramVersion.Id == versionId && x.Stage3Version.RealityObject.Municipality.Id == municipalityId)
                .Where(x => x.Stage3Version.CorrectYear >= actualizeStart)
                .Where(x => typeIds.Contains(x.CommonEstateObject.GroupType.Id));

                var st1Data = versSt1Domain.GetAll()
                .Where(x => groupQuery.Any(y => y.Id == x.Stage2Version.Id))
                .Select( x => new
                {
                    st2Id = x.Stage2Version.Id,
                    st3Id = x.Stage2Version.Stage3Version.Id,
                    x.Volume,
                    address = x.RealityObject.Address,
                    ceoNames = x.Stage2Version.Stage3Version.CommonEstateObjects,
                    x.Stage2Version.Stage3Version.Sum,
                    x.Stage2Version.Stage3Version.Year,
                    x.Stage2Version.Stage3Version.IndexNumber
                })
                .AsEnumerable();

                // значения старые по 3 этапу
                var oldData = st1Data
                .GroupBy(x => x.st3Id)
                .ToDictionary(
                    x => x.Key,
                    y => new
                    {
                        ceoNames = y.ToList().Select(z => z.ceoNames).Any() ?
                                        y.ToList().Select(z => z.ceoNames)
                                        .Distinct()
                                        .Aggregate(
                                            (str, result) => string.IsNullOrEmpty(result) ? str : result + ", " + str) : string.Empty,
                        volume = y.Sum(z => z.Volume),
                        sum = y.Select(z => z.Sum).FirstOrDefault(),
                        number = y.Select(z => z.IndexNumber).FirstOrDefault(),
                        year = y.Select(z => z.Year).FirstOrDefault(),
                        address = y.Select(z => z.address).FirstOrDefault()
                    });

                var st2Dict = groupQuery.ToDictionary(x => x.Id);

                var st3Dict = versSt3Domain.GetAll()
                .Where(x => groupQuery.Any(y => y.Stage3Version.Id == x.Id))
                .ToDictionary(x => x.Id);

                var st2Data = groupQuery
                .Select( x => new
                {
                    x.Id,
                    st3Id = x.Stage3Version.Id,
                    x.Stage3Version.IndexNumber,
                    x.Stage3Version.Point,
                    x.Stage3Version.Year,
                    x.Stage3Version.CorrectYear,
                    ceoId = x.CommonEstateObject.Id,
                    roId = x.Stage3Version.RealityObject.Id,
                    groupType = x.CommonEstateObject.GroupType.Id
                })
                .AsEnumerable()
                .GroupBy(x => x.roId + "_" + x.groupType)
                .ToDictionary(x => x.Key, y => y.OrderBy(z => z.Year)
                                                .ThenBy(x => x.IndexNumber)
                                                .ToList());

                var st2TempDict = groupQuery
                    .Select(
                        x =>
                        new
                        {
                            x.Id,
                            st3Id = x.Stage3Version.Id
                            
                        })
                        .AsEnumerable()
                        .GroupBy(x => x.st3Id)
                        .ToDictionary(x => x.Key, y => y.Select(x => x.Id).ToList());

                foreach (var st2kvp in st2Data)
                {
                    var groupYear = 0;
                    var st3groups = new Dictionary<long, List<long>>();
                    var st3Id = 0L;
                    
                    // сначала поулчаем какие записи будут сгруппированы в какую 
                    foreach (var item in st2kvp.Value)
                    {
                        if (groupYear > 0 && item.Year > groupYear + stage3Period - 1)
                        {
                            // если какая то группа уже была пройдена, а текущий элемент уже вышел за пределы группы
                            // тогда обнуляем и заново делаем группировку
                            st3Id = 0;
                        }

                        if (st3Id == 0)
                        {
                            st3Id = item.st3Id;
                            groupYear = item.Year;

                            st3groups.Add(st3Id, new List<long>() { item.Id });
                        }
                        else
                        {
                            st3groups[st3Id].Add(item.Id);
                        }
                    }

                    foreach (var st3kvp in st3groups)
                    {
                        // Сначала делаем проверку изменился ли состав записей 2го этапа 
                        var oldList = st2TempDict[st3kvp.Key];

                        var changeExists = false;

                        // нужно понять есть ли хоть какие то изменения чтобы неделать лишних действий
                        // для этог опросто сравниваем список Id который был раньше и тот который пришел на сохранение
                        oldList.ForEach(x =>
                            {
                                if (!st3kvp.Value.Contains(x))
                                {
                                    changeExists = true;
                                }
                            });

                        st3kvp.Value.ForEach(x =>
                        {
                            if (!oldList.Contains(x))
                            {
                                changeExists = true;
                            }
                        });

                        if (!changeExists)
                        {
                            continue; // выходим в таком случае из итерации
                        }
                        
                        var st3rec = st3Dict[st3kvp.Key];
                        var sum = 0m;
                        var ceoNames = new List<string>();

                        foreach (var st2_id in st3kvp.Value)
                        {
                            var st2 = st2Dict[st2_id];
                            sum += st2.Sum;

                            ceoNames.Add(st2.CommonEstateObject.Name);

                            if (st2.Stage3Version.Id != st3rec.Id)
                            {
                                var excludeSt3 = st3Dict[st2.Stage3Version.Id];

                                if (excludeSt3.Point > st3rec.Point)
                                {
                                    // берем самый большой балл для группы 
                                    st3rec.Point = excludeSt3.Point;
                                }

                                st2.Stage3Version = st3rec;

                                if (!newRef.ContainsKey(excludeSt3.Id))
                                {
                                    newRef.Add(excludeSt3.Id, st3rec);
                                }

                                if (!st2ToSave.ContainsKey(st2.Id))
                                {
                                    st2ToSave.Add(st2.Id, st2);
                                }
                            }
                        }

                        // фиксируем общую сумму для группы
                        if (ceoNames.Any())
                        {
                            st3rec.CommonEstateObjects = ceoNames.Distinct().Aggregate((x, y) => string.IsNullOrEmpty(y) ? y : y + ", " + x);
                        }

                        st3rec.Sum = sum;

                        if (!st3ToSave.ContainsKey(st3rec.Id))
                        {
                            st3ToSave.Add(st3rec.Id, st3rec);
                        }
                    }
                }

                // теперь после того как переставили ссылки надо понять какие записи совсем нужно удалить из 3го этапа
                var allSt3Ids = st3Dict.Keys.ToList();
                var needSt3Ids = st2Dict.Values.Select(x => x.Stage3Version.Id).Distinct().ToList();
                var deleteSt3Ids = allSt3Ids.Where(x => !needSt3Ids.Contains(x)).ToList();

                if (!st3ToSave.Any() && !st2ToSave.Any())
                {
                    return new BaseDataResult(false, "Нет записей для группировки");
                }

                // получаем ноаую связть 2 этапа с треьим
                var stNewRef = st2ToSave.Values
                .Select(x => new { x.Id, st3Id = x.Stage3Version.Id })
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, y => y.Select(x => x.st3Id).FirstOrDefault());

                // получаем новый объем по новой связи 
                var newVolumeDict = st1Data
                                 .Select(x => new 
                                    {
                                        x.Volume,
                                        st3Id = stNewRef.ContainsKey(x.st2Id)? stNewRef[x.st2Id] : x.st3Id
                                    })
                                 .GroupBy(x => x.st3Id)
                                    .ToDictionary(
                                        x => x.Key,
                                        y => y.Sum(z => z.Volume));

                // формируем логи актуализации
                var log = new VersionActualizeLog();
                log.ActualizeType = VersionActualizeType.ActualizeGroup;
                log.DateAction = DateTime.Now;
                log.Municipality = municipalityId > 0 ? new Municipality() { Id = municipalityId } : null;
                log.ProgramVersion = new ProgramVersion() { Id = versionId };
                log.UserName = User.Name;

                var logRecords = new List<ActualizeVersionLogRecord>();

                // Формируемстроки для логов
                foreach (var kvp in st3ToSave)
                {
                    if (deleteSt3Ids.Contains(kvp.Key) || !oldData.ContainsKey(kvp.Key) || !newVolumeDict.ContainsKey(kvp.Key))
                        continue;
                    var rec = kvp.Value;

                    var old = oldData[kvp.Key];
                    var newVolume = newVolumeDict[kvp.Key];

                    var logRecord = new ActualizeVersionLogRecord
                    {
                        TypeAction = VersionActualizeType.ActualizeGroup,
                        Action = "Группировка ООИ",
                        Description = string.Format(
                                        "Группировка ООИ"),
                        Address = old.address,
                        Ceo = old.ceoNames,
                        PlanYear = old.year,
                        Volume = old.volume,
                        Sum = old.sum,
                        Number = old.number,
                        ChangeNumber = rec.IndexNumber != old.number ? rec.IndexNumber : 0,
                        ChangeSum = rec.Sum != old.sum ? rec.Sum : 0m,
                        ChangeCeo = rec.CommonEstateObjects != old.ceoNames ? rec.CommonEstateObjects : null,
                        ChangePlanYear = rec.Year != old.year ? rec.Year : 0,
                        ChangeVolume = newVolume != old.volume ? newVolume : 0
                    };

                    logRecords.Add(logRecord);
                }

                // Формируемстроки для логов
                foreach (var oldId in deleteSt3Ids)
                {
                    if (!newRef.ContainsKey(oldId) || !oldData.ContainsKey(oldId))
                        continue;

                    var rec = newRef[oldId];

                    if (!st3ToSave.ContainsKey(rec.Id) || !newVolumeDict.ContainsKey(rec.Id))
                        continue;

                    var old = oldData[oldId];
                    var newVolume = newVolumeDict[rec.Id];

                    var logRecord = new ActualizeVersionLogRecord
                    {
                        TypeAction = VersionActualizeType.ActualizeGroup,
                        Action = "Группировка ООИ",
                        Description = string.Format(
                                        "Группировка ООИ"),
                        Address = old.address,
                        Ceo = old.ceoNames,
                        PlanYear = old.year,
                        Volume = old.volume,
                        Sum = old.sum,
                        Number = old.number,
                        ChangeNumber = rec.IndexNumber != old.number ? rec.IndexNumber : 0,
                        ChangeSum = rec.Sum != old.sum ? rec.Sum : 0m,
                        ChangeCeo = rec.CommonEstateObjects != old.ceoNames ? rec.CommonEstateObjects : null,
                        ChangePlanYear = rec.Year != old.year ? rec.Year : 0,
                        ChangeVolume = newVolume != old.volume ? newVolume : 0
                    };

                    logRecords.Add(logRecord);

                }

                if (logRecords.Any())
                {
                    log.CountActions = logRecords.Count();
                    log.LogFile = LogService.CreateLogFile(
                        logRecords.OrderBy(x => x.Address).OrderBy(x => x.Number), baseParams);
                }
                else
                {
                    return new BaseDataResult(false, "Логи отсутсвуют");
                }

                using (var session = sessionProvider.OpenStatelessSession())
                {
                    using (var tr = session.BeginTransaction(IsolationLevel.Serializable))
                    {
                        try
                        {
                            st3ToSave.ForEach(kvp =>
                            {
                                if (!deleteSt3Ids.Contains(kvp.Key))
                                    session.Update(unProxy.GetUnProxyObject(kvp.Value));
                            });

                            st2ToSave.ForEach(kvp =>
                            {
                                session.Update(unProxy.GetUnProxyObject(kvp.Value));
                            });
                            
                            st2ToSave.Clear();
                            st3ToSave.Clear();

                            tr.Commit();
                        }
                        catch (Exception)
                        {
                            tr.Rollback();
                            throw;
                        }
                    }
                }

                // ненужно удаление запииват ьв отдельную транзакцию
                using (var session = sessionProvider.OpenStatelessSession())
                {
                    using (var tr = session.BeginTransaction(IsolationLevel.Serializable))
                    {
                        try
                        {
                            deleteSt3Ids.ForEach(
                                id => session.Delete(unProxy.GetUnProxyObject(st3Dict[id])));

                            session.Insert(log);


                            st2ToSave.Clear();
                            st3ToSave.Clear();

                            tr.Commit();
                        }
                        catch (Exception)
                        {
                            tr.Rollback();
                            throw;
                        }
                    }
                }

                return new BaseDataResult();
            }
            finally
            {
                Container.Release(versSt3Domain);
                Container.Release(ceoDomain);
                Container.Release(versSt2Domain);
                Container.Release(unProxy);
            }
        }

        public IDataResult ActualizeOrder(BaseParams baseParams)
        {
            /* Требование 57368
              Расчет очередности
Требуется: в версии должна быть возможность пересчета очередности. Очередность надо пересчитать только за выбранный период. А остальным записям, которые не пересчитываются, присвоить номер по порядку. 
Это делается для того, чтобы пересчитать очередь по новым баллам. Пересчитать надо с 2015 года, не трогая 2014 год, т.к. уже создана Краткосрочная программа.
Реализовать:
1. Записям, для которых пересчет баллов не осуществляется и их Скорректированный год < Начала периода проведения корректировки, присвоить номера от 1 до записи, у которой осуществляется пересчет баллов. 
   Нумерацию осуществлять в том порядке, в котором до этого был порядок очередей.
2. Пересчитать очередности осуществлять за выбранный период по тому же алгоритму, как в разделе "Долгосрочная программа", кнопка "Очередность". Алгоритм (это реализовано, надо просто перенести):
    •	Сначала отсортировать записи по критерию "Потребность проведения капитального ремонта на дату приватизации". Присваивается значение 1 или 0. Сортировка от 1 до 0. Дополнение: если записи были сгруппированы, то проверку осуществлять по наименьшему значению: (Год установки или последнего кап.ремонта + Срок эксплуатации).
    •	Рассчитать баллы по параметрам очередности (раздел "Параметры программы капитального ремонта--Параметры очередности). Дополнение: если записи сгруппированы, то балл считать для дома только 1 раз. Т.е. не надо для каждого ООИ считать балл и потом суммировать их.
3. Записям, которым был пересчитан балл присвоить номера с номера = (Максимальный номер из предыдущего года + 1) и т.д.
4. Записям, у которых Скорректированный год < Окончание периода проведения корректировки, присвоить номера с номера = (Максимальный номер из предыдущего года + 1) и т.д. в соответствии с ранее выстроенным порядком очереди. 

             */
            var versionId = baseParams.Params.GetAs<long>("versionId");
            var municipalityId = baseParams.Params.GetAs<long>("municipalityId");
            var actualizeStart = baseParams.Params.GetAs("yearStart", 0);

            var quantParamsDomain = Container.Resolve<IDomainService<QuantPriorityParam>>();
            var qualityParamsDomain = Container.Resolve<IDomainService<QualityPriorityParam>>();
            var versionSt3Domain = Container.Resolve<IDomainService<VersionRecord>>();
            var versionSt1Domain = Container.Resolve<IDomainService<VersionRecordStage1>>();
            var versionSt2Domain = Container.Resolve<IDomainService<VersionRecordStage2>>();
            var strElDomain = Container.Resolve<IDomainService<StructuralElement>>();
            var priorityParamsService = Container.ResolveAll<IPriorityParams>();

            try
            {
                var priorityParam = priorityParamsService.ToDictionary(x => x.Id);
                var quantParams = quantParamsDomain.GetAll().ToList();
                var qualityParams = qualityParamsDomain.GetAll().ToList();

                var dictLifetimes = strElDomain.GetAll()
                    .Select(x => new
                    {
                        x.Id,
                        x.LifeTime,
                        x.LifeTimeAfterRepair
                    })
                    .AsEnumerable()
                    .ToDictionary(x => x.Id);

                var ver3recsQuery =
                    versionSt3Domain.GetAll()
                                    .Where(x => x.ProgramVersion.Id == versionId && x.RealityObject.Municipality.Id == municipalityId )
                                    .Where(x => x.CorrectYear >= actualizeStart);

                var dictRoWearout = versionSt1Domain.GetAll()
                .Where(x => ver3recsQuery.Any(y => y.Id == x.Stage2Version.Stage3Version.Id))
                .Where(x => x.StructuralElement != null)
                .Select(x => new
                {
                    x.StructuralElement.Wearout,
                    Key = x.StructuralElement.RealityObject.Id + "_" + x.StructuralElement.StructuralElement.Group.CommonEstateObject.Id
                })
                .AsEnumerable()
                .GroupBy(x => x.Key)
                .Select(x => new
                {
                    x.Key,
                    Count = x.Count(),
                    Wearout = x.Sum(y => y.Wearout)
                })
                .ToDictionary(x => x.Key, y => y.Count > 0 ? y.Wearout / y.Count : 0);

                var verSt1Recs = versionSt1Domain.GetAll()
                    .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.Id == versionId && x.Stage2Version.Stage3Version.RealityObject.Municipality.Id == municipalityId )
                                    .Where(x => x.Stage2Version.Stage3Version.CorrectYear >= actualizeStart )
                    .Select(x => new
                        {
                            x.Id,
                            x.Year,
                            st3Id = x.Stage2Version.Stage3Version.Id,
                            StrElId = x.StrElement.Id,
                            RoId = x.RealityObject.Id,
                            CeoId = x.StrElement.Group.CommonEstateObject.Id,
                            LastOverhaulYear = x.StructuralElement != null ? x.StructuralElement.LastOverhaulYear : 0,
                            x.Sum
                        })
                        .AsEnumerable()
                        .GroupBy(x => x.st3Id)
                        .ToDictionary(x => x.Key, y => y.GroupBy(x => x.CeoId ).ToDictionary(x => x.Key, z => z.ToList()));

                // получаем макимальное значение номера на начало периода актуализации
                var maxIndexBefore = 0;
                var queryBefore =
                    versionSt3Domain.GetAll()
                                    .Where(
                                        x =>
                                        x.ProgramVersion.Id == versionId
                                        && x.RealityObject.Municipality.Id == municipalityId)
                                    .Where(x => x.CorrectYear < actualizeStart);
                if (queryBefore.Any())
                {
                    maxIndexBefore = versionSt3Domain.GetAll()
                                    .Where(x => x.ProgramVersion.Id == versionId && x.RealityObject.Municipality.Id == municipalityId)
                                    .Where(x => x.CorrectYear < actualizeStart)
                                    .Max(x => x.IndexNumber);
                }

                var ceoDict =
                    versionSt2Domain.GetAll()
                                    .Where(x => ver3recsQuery.Any(y => y.Id == x.Stage3Version.Id))
                                    .Select(
                                        x => new { x.Id, ceoId = x.CommonEstateObject.Id, st3Id = x.Stage3Version.Id })
                                    .AsEnumerable()
                                    .GroupBy(x => x.st3Id)
                                    .ToDictionary(x => x.Key, y => y.Select(z => z.ceoId).ToList());

                var volumeDict = versionSt1Domain.GetAll()
                                    .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.Id == versionId && x.Stage2Version.Stage3Version.RealityObject.Municipality.Id == municipalityId)
                                    .Select(
                                        x => new { st3Id = x.Stage2Version.Stage3Version.Id, x.Volume})
                                    .AsEnumerable()
                                    .GroupBy(x => x.st3Id)
                                    .ToDictionary(x => x.Key, y => y.Sum(z => z.Volume));
                
                var ver3recs =
                    ver3recsQuery.Select(
                        x =>
                        new
                            {
                                x.Id,
                                verSt3 = x, 
                                x.CorrectYear,
                                x.RealityObject.Address,
                                x.Sum,
                                x.CommonEstateObjects,
                                x.Year,
                                roId = x.RealityObject.Id,
                                x.Point,
                                x.IndexNumber,
                                x.RealityObject.EnergyPassport,
                                x.RealityObject.ConfirmWorkDocs,
                                x.RealityObject.ProjectDocs,
                                x.RealityObject.DateCommissioning,
                                x.RealityObject.BuildYear,
                                x.RealityObject.PrivatizationDateFirstApartment
                            })
                            .OrderBy(x => x.IndexNumber)
                            .ToList();

                var dictYears = new Dictionary<string, int>();

                foreach (var rec in ver3recs)
                {
                    var ver3 = rec.verSt3;

                    if (!ceoDict.ContainsKey(rec.Id))
                    {
                        continue;
                    }

                    // формируем объекты чтобы опитимизирвоать работу
                    var ro = new RealityObject
                    {
                        Id = rec.roId,
                        EnergyPassport = rec.EnergyPassport,
                        ProjectDocs = rec.ProjectDocs,
                        ConfirmWorkDocs = rec.ConfirmWorkDocs,
                        DateCommissioning = rec.DateCommissioning,
                        BuildYear = rec.BuildYear,
                        PrivatizationDateFirstApartment = rec.PrivatizationDateFirstApartment
                    };

                    // Этот объект сохранять небудем он нужен тольк одля того чтобы произвести высисления такие же как при расчете ДПКР 
                    var stage3 = new RealityObjectStructuralElementInProgrammStage3
                    {
                        RealityObject = ro,
                        CommonEstateObjects = rec.CommonEstateObjects,
                        Year = rec.Year,
                        Sum = rec.Sum
                    };

                    /*
                     * 3ий этап может быт ьскгруппирован, следовательно може тсодержат ьнесколько ООИ
                     * поэтому Балы берусться тольк омакимальный по одному из ООИ
                     * а потребность проведения кап ремонта берется по минимальной дате 
                     */

                    var maxPoints = 0;
                    var deathLine = 0;

                    foreach (var ceoId in ceoDict[rec.Id])
                    {
                        var point = 0;

                        var key = string.Format("{0}_{1}", rec.roId, ceoId);

                        foreach (var quant in quantParams)
                        {
                            var param = priorityParam[quant.Code];

                            if (quant.Code == "Wearout")
                            {
                                (param as WearoutPriorityParam).Wearout = dictRoWearout.ContainsKey(key)
                                    ? dictRoWearout[key]
                                    : 0;
                            }

                            var value = param.GetValue(stage3).ToDecimal();

                            decimal? minValue = null;
                            decimal? maxValue = null;

                            if (!string.IsNullOrEmpty(quant.MinValue))
                            {
                                minValue = quant.MinValue.ToDecimal();
                            }

                            if (!string.IsNullOrEmpty(quant.MaxValue))
                            {
                                maxValue = quant.MaxValue.ToDecimal();
                            }

                            point += (!minValue.HasValue || value >= minValue.Value) && (!maxValue.HasValue || value <= maxValue.Value)
                                ? quant.Point
                                : 0;
                        }

                        foreach (var quality in qualityParams)
                        {
                            var param = priorityParam[quality.Code];

                            if (quality.Value == (int)param.GetValue(stage3))
                            {
                                point += quality.Point;
                            }
                        }

                        // Фиксируем толко максимальное значение балов если несколько ООИ сгруппирвоаны 
                        if (point > maxPoints)
                        {
                            maxPoints = point;
                        }

                        var maxStrElYear = 0;
                        var maxStrElLastOverhaulYear = 0;
                        var maxStrElExploitYear = 0;
                        var maxStrElExploitYearAfterRepair = 0;

                        if (verSt1Recs.ContainsKey(rec.Id) && verSt1Recs[rec.Id].ContainsKey(ceoId))
                        {
                            var listStEls = verSt1Recs[rec.Id][ceoId];
                            foreach (var val in listStEls)
                            {
                                maxStrElYear = Math.Max(rec.Year, maxStrElYear);
                                maxStrElLastOverhaulYear = Math.Max(val.LastOverhaulYear, maxStrElLastOverhaulYear);
                                maxStrElExploitYear = Math.Max(dictLifetimes[val.StrElId].LifeTime, maxStrElExploitYear);
                                maxStrElExploitYearAfterRepair = Math.Max(dictLifetimes[val.StrElId].LifeTimeAfterRepair, maxStrElExploitYearAfterRepair);
                            }

                            if (!dictYears.ContainsKey(key))
                            {
                                dictYears.Add(key, maxStrElLastOverhaulYear);
                            }
                            else
                            {
                                dictYears[key] = maxStrElYear;
                            }


                            // Теперь вычисляем Год когда ООИ пришелбы капут 
                            var ceoDeadLine = 0;

                            if (dictYears[key] != maxStrElLastOverhaulYear)
                            {
                                ceoDeadLine = maxStrElYear + maxStrElExploitYearAfterRepair;
                            }
                            else
                            {
                                ceoDeadLine = maxStrElLastOverhaulYear + maxStrElExploitYear;
                            }

                            // теперь среди всех ООИ над овзять самый маленький год капута 
                            if (deathLine == 0)
                            {
                                deathLine = ceoDeadLine;
                            }
                            else if (deathLine > ceoDeadLine)
                            {
                                deathLine = ceoDeadLine;
                            }
                        }
                    }

                    ver3.Point = maxPoints;
                    ver3.NeedOverhaul = this.GetNeedOverhaul(stage3, deathLine);

                }

                var index = maxIndexBefore + 1;

                /*
                var dict = ver3recs.GroupBy(x => x.CorrectYear)
                                   .ToDictionary(
                                       x => x.Key,
                                       y =>
                                       y.GroupBy(x => x.verSt3.NeedOverhaul)
                                        .ToDictionary(z => z.Key, w => w.OrderBy(t => t.Point).ToList()));

                */

                foreach (var item in ver3recs.OrderBy(x => x.Year).ThenByDescending(x => x.verSt3.NeedOverhaul).ThenByDescending(x => x.verSt3.Point))
                {
                    item.verSt3.IndexNumber = index++;
                }

                if (!ver3recs.Any())
                {
                    return new BaseDataResult(false, "Нет записей изменения очередности");
                }

// формируем логи актуализации
                var log = new VersionActualizeLog();
                log.ActualizeType = VersionActualizeType.ActualizeOrder;
                log.DateAction = DateTime.Now;
                log.Municipality = municipalityId > 0 ? new Municipality() { Id = municipalityId} : null;
                log.ProgramVersion = new ProgramVersion() { Id = versionId };
                log.UserName = User.Name;

                var logRecords = new List<ActualizeVersionLogRecord>();
                
                // Формируемстроки для логов
                foreach (var rec in ver3recs)
                {
                    if (rec.IndexNumber == rec.verSt3.IndexNumber && rec.Point == rec.verSt3.Point) 
                        continue;

                    var logRecord = new ActualizeVersionLogRecord
                    {
                        TypeAction = VersionActualizeType.ActualizeOrder,
                        Action = "Изменение",
                        Description = string.Format(
                                        "Пересчет очередности с {0} года",
                                        actualizeStart),
                        Address = rec.Address,
                        Ceo = rec.CommonEstateObjects,
                        PlanYear = rec.Year,
                        Volume = volumeDict.ContainsKey(rec.Id) ? volumeDict[rec.Id] : 0m,
                        Sum = rec.Sum,
                        Number = rec.IndexNumber, // старый номер
                        ChangeNumber = rec.verSt3.IndexNumber // новый номер
                    };

                    logRecords.Add(logRecord);

                }

                if (logRecords.Any())
                {
                    log.CountActions = logRecords.Count();
                    log.LogFile = LogService.CreateLogFile(logRecords.OrderBy(x => x.Address).OrderBy(x => x.Number), baseParams);
                }
                else
                {
                    return new BaseDataResult(false, "Логи отсутсвуют");
                }

// формируем логи актуализации
                using (var session = Container.Resolve<ISessionProvider>().OpenStatelessSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        try
                        {
                            foreach (var rec in ver3recs)
                            {
                                if (rec.IndexNumber == rec.verSt3.IndexNumber && rec.Point == rec.verSt3.Point) continue; // если ничего непоменялось то не сохраняем

                                session.CreateSQLQuery(@"update OVRHL_VERSION_REC set INDEX_NUM=:indNum, POINT =:points  
                                            where id =:recId")
                                        .SetParameter("indNum", rec.verSt3.IndexNumber)
                                        .SetParameter("points", rec.verSt3.Point)
                                        .SetParameter("recId", rec.Id)
                                        .ExecuteUpdate();
                            }

                            session.Insert(log);

                            transaction.Commit();
                        }
                        catch (Exception exc)
                        {
                            transaction.Rollback();
                            throw exc;
                        }
                    }

                }

                return new BaseDataResult();
                
            }
            finally 
            {
                Container.Release(quantParamsDomain);
                Container.Release(quantParamsDomain);
                Container.Release(versionSt3Domain);
                Container.Release(strElDomain);
                Container.Release(versionSt1Domain);
                Container.Release(versionSt2Domain);
                Container.Release(priorityParamsService);
            }
        }

        private int GetNeedOverhaul(RealityObjectStructuralElementInProgrammStage3 stage3, int deadLine)
        {
            if (!stage3.RealityObject.PrivatizationDateFirstApartment.HasValue)
            {
                return 0;
            }

            var privDate = stage3.RealityObject.PrivatizationDateFirstApartment.Value.Year;

            return deadLine > privDate ? 0 : 1;
        }

        private IDataResult GetNewRecords(
            BaseParams baseParams,
            out List<VersionRecord> ver3S,
            out List<VersionRecordStage2> ver2S,
            out List<VersionRecordStage1> ver1S,
            IDictionary<long, RoStrElAfterRepair> afterRepaired = null,
            IEnumerable<long> roStrElList = null,
            IEnumerable<string> existRoStrInYear = null,
            IEnumerable<string> comEstObjGroupTypeCodes = null,
            bool comEstObjGroupTypeCodeIncluding = false)
        {
            ver3S = new List<VersionRecord>();
            ver2S = new List<VersionRecordStage2>();
            ver1S = new List<VersionRecordStage1>();

            var programVersionDomain = this.Container.ResolveDomain<ProgramVersion>();
            var roStructElDomain = this.Container.ResolveDomain<RealityObjectStructuralElement>();
            var versSt1Domain = this.Container.ResolveDomain<VersionRecordStage1>();
            var versSt2Domain = this.Container.ResolveDomain<VersionRecordStage2>();
            var versSt3Domain = this.Container.ResolveDomain<VersionRecord>();

            var roStructElService = this.Container.Resolve<IRealObjStructElementService>();
            var longProgramService = this.Container.Resolve<ILongProgramService>();
            var stagesService = this.Container.Resolve<IStage2Service>();

            var versionActualizeType = baseParams.Params.GetAs<VersionActualizeType>("versionActualizeType");
            var versionId = baseParams.Params.GetAs<long>("versionId");
            var municipalityId = baseParams.Params.GetAs<long>("municipalityId");
            var actualizeStart = baseParams.Params.GetAs("yearStart", 0);

            using (this.Container.Using(
                programVersionDomain, 
                roStructElDomain,
                versSt1Domain,
                versSt2Domain,
                versSt3Domain,
                roStructElService,
                longProgramService,
                stagesService))
            {
                var version = programVersionDomain.Get(versionId);

                var config = this.Container.GetGkhConfig<OverhaulTatConfig>();
                var periodEnd = config.ProgrammPeriodEnd;

                // Типы актуализаций, которые учитывают уже существующие записи версий программ
                // Если запись по КЭ дома есть в версии ДПКР,
                // то номые записи по данному КЭ будут формироваться
                // относительно последнего года корректировки
                var includeExistsVersionActualizeTypes = new[]
                {
                    VersionActualizeType.ActualizeLiftNewRecords
                };

                var includeExistsRecords = includeExistsVersionActualizeTypes.Contains(versionActualizeType);

                var versSt1Query = versSt1Domain.GetAll()
                    .Where(x =>
                        x.Stage2Version.Stage3Version.ProgramVersion.Id == versionId
                        && x.Stage2Version.Stage3Version.RealityObject.Municipality.Id == municipalityId)
                    /* Это нужно чтобы необращатьвнимания на то что КЭ уже отремонтирован в какомто году - это надо */
                    .WhereIf(afterRepaired != null && afterRepaired.Any(),
                        x => x.Stage2Version.Stage3Version.CorrectYear >= actualizeStart)
                    .WhereIfElseIf(comEstObjGroupTypeCodeIncluding, 
                        comEstObjGroupTypeCodes != null,
                        x => comEstObjGroupTypeCodes.Contains(x.StrElement.Group.CommonEstateObject.GroupType.Code),
                        x => !comEstObjGroupTypeCodes.Contains(x.StrElement.Group.CommonEstateObject.GroupType.Code));

                var roStructQuery = roStructElService.GetUsedInLongProgram()
                    .Where(x =>
                        x.RealityObject.Municipality.Id == municipalityId
                        || x.RealityObject.MoSettlement.Id == municipalityId)
                    .WhereIfElseIf(comEstObjGroupTypeCodeIncluding,
                        comEstObjGroupTypeCodes != null,
                        x => comEstObjGroupTypeCodes.Contains(x.StructuralElement.Group.CommonEstateObject.GroupType.Code),
                        x => !comEstObjGroupTypeCodes.Contains(x.StructuralElement.Group.CommonEstateObject.GroupType.Code));

                IDictionary<long, int> structElementPlanYearsDict = null;

                if (roStrElList == null && includeExistsRecords)
                {
                    // Существующие записи версий программы с КЭ домов,
                    // по которым можно добавить еще хотя бы одну работу
                    structElementPlanYearsDict = versSt1Query
                        .Where(x => roStructQuery.Any(y => y.Id == x.StructuralElement.Id))
                        .OrderBy(y => y.Stage2Version.Stage3Version.CorrectYear)
                        .AsEnumerable()
                        .GroupBy(x => x.StructuralElement.Id)
                        .Select(x => new
                        {
                            x.Key,
                            LastRecord = x.Last()
                        })
                        .Where(x => x.LastRecord.StrElement.LifeTime > 0 &&
                            x.LastRecord.Stage2Version.Stage3Version.CorrectYear + (x.LastRecord.StrElement.LifeTimeAfterRepair > 0
                                ? x.LastRecord.StrElement.LifeTimeAfterRepair : x.LastRecord.StrElement.LifeTime) <= periodEnd)
                        .ToDictionary(x => x.Key, y => y.LastRecord.Stage2Version.Stage3Version.CorrectYear);

                        roStrElList = structElementPlanYearsDict.Keys;
                }

                var newStructQuery = roStructQuery
                        .WhereIfElse(roStrElList != null,
                            x => roStrElList.Contains(x.Id) || !versSt1Query.Any(y => y.StructuralElement.Id == x.Id),
                            x => !versSt1Query.Any(y => y.StructuralElement.Id == x.Id));

                if (!newStructQuery.Any())
                {
                    return new BaseDataResult(false, "Новые записи не обнаружены");
                }

                var stage1ToSave = stagesService.GetStage1(
                    actualizeStart, periodEnd, version.Municipality.Id, newStructQuery, structElementPlanYearsDict);

                if (afterRepaired != null && afterRepaired.Any())
                {
                    var stage1ByRoStrEl = stage1ToSave.GroupBy(x => x.StructuralElement.Id)
                        .ToDictionary(x => x.Key, y => y.OrderBy(z => z.Year).ToList());

                    // необходимо понят ькакие записи уже существуют в нетронутых годах и относительно сроков эксплуатации сдвигать последующие записи
                    foreach (var kvp in afterRepaired)
                    {
                        if (!stage1ByRoStrEl.ContainsKey(kvp.Key))
                        {
                            continue;
                        }

                        // отталкиваемся от того что данный кэ уже учтен в какихто годах а значит берем этот год как год ремонта и прибавляем срок эксплуатации
                        int nextYear = kvp.Value.LastYearRepair + (kvp.Value.LifeTimeAfterRepair > 0 ? kvp.Value.LifeTimeAfterRepair : kvp.Value.LifeTime);

                        // всем записям которые неполностью удалились из дпкрприсваиваем год относителньо года последней записи находящейся в версии
                        foreach (var roStrEl in stage1ByRoStrEl[kvp.Key])
                        {
                            roStrEl.Year = nextYear;

                            nextYear = roStrEl.Year + (kvp.Value.LifeTimeAfterRepair > 0 ? kvp.Value.LifeTimeAfterRepair : kvp.Value.LifeTime);
                        }
                    }

                    stage1ToSave = stage1ToSave.Where(x => x.Year <= periodEnd).ToList();
                }

                if (existRoStrInYear != null && existRoStrInYear.Any())
                {
                    // поскольку мы заново расчитали относительно года актуализации новый срез данных, значит
                    // исключаем те комбинации которые уже существуют в системе, тоесть чтобы небыло дублей
                    stage1ToSave = stage1ToSave
                        .Where(x => !existRoStrInYear.Contains("{0}_{1}".FormatUsing(x.StructuralElement.Id, x.Year)))
                        .ToList();
                }

                if (!stage1ToSave.Any())
                {
                    return new BaseDataResult(false, "Новые записи не обнаружены");
                }

                var roIds = stage1ToSave.Select(x => x.StructuralElement.RealityObject.Id).Distinct().ToList();

                var stage2VerRecs = versSt2Domain.GetAll()
                    .Where(x => x.Stage3Version.ProgramVersion.Id == versionId && x.Stage3Version.RealityObject.Id == municipalityId)
                    .Where(x => roIds.Contains(x.Stage3Version.RealityObject.Id))
                    .WhereIfElseIf(comEstObjGroupTypeCodeIncluding, 
                        comEstObjGroupTypeCodes != null,
                        x => comEstObjGroupTypeCodes.Contains(x.CommonEstateObject.GroupType.Code),
                        x => !comEstObjGroupTypeCodes.Contains(x.CommonEstateObject.GroupType.Code))
                    .Select(x => new
                    {
                        Record = x,
                        RoId = x.Stage3Version.RealityObject.Id,
                        CeoId = x.CommonEstateObject.Id,
                        x.Stage3Version.Year
                    })
                    .AsEnumerable()
                    .GroupBy(x => "{0}_{1}_{2}".FormatUsing(x.RoId, x.CeoId, x.Year))
                    .ToDictionary(x => x.Key, y => y.Select(x => x.Record).FirstOrDefault());

                stagesService.GetStage2And3(stage1ToSave, out var stage2ToSave, out var stage3ToSave);

                var stage2Dict = stage2ToSave
                    .Where(x => x.Stage3 != null)
                    .GroupBy(x => x.Stage3)
                    .ToDictionary(x => x.Key, y => y.ToArray());

                var stage1Dict = stage1ToSave
                    .Where(x => x.Stage2 != null)
                    .GroupBy(x => x.Stage2)
                    .ToDictionary(x => x.Key, y => y.ToArray());

                foreach (var stage3 in stage3ToSave)
                {
                    var ver3 = new VersionRecord
                    {
                        ProgramVersion = version,
                        RealityObject = stage3.RealityObject,
                        Year = stage3.Year,
                        CorrectYear = stage3.Year,
                        CommonEstateObjects = stage3.CommonEstateObjects,
                        IndexNumber = 0,
                        Point = stage3.Point,
                        StoredCriteria = stage3.StoredCriteria,
                        ObjectCreateDate = DateTime.Now,
                        TypeDpkrRecord = TypeDpkrRecord.ActualizeAddRecord
                    };

                    ver3.Sum += stage3.Sum;
                    ver3.ObjectEditDate = DateTime.Now;

                    if (stage2Dict.TryGetValue(stage3, out var st2RecList))
                    {
                        foreach (var stage2 in st2RecList)
                        {
                            var st2Version =
                                stage2VerRecs.Get(
                                    "{0}_{1}_{2}".FormatUsing(
                                        stage2.RealityObject.Id, stage2.CommonEstateObject.Id, stage2.Year));

                            if (st2Version == null)
                            {
                                if (ver3.Id > 0)
                                {
                                    ver3.CommonEstateObjects = string.Format(
                                        "{0}, {1}", ver3.CommonEstateObjects, stage2.CommonEstateObject.Name);
                                }

                                st2Version = new VersionRecordStage2
                                {
                                    CommonEstateObject = new CommonEstateObject
                                    {
                                        Id = stage2.CommonEstateObject.Id
                                    },
                                    Stage3Version = ver3,
                                    CommonEstateObjectWeight = stage2.CommonEstateObject.Weight,
                                    ObjectCreateDate = DateTime.Now,
                                    TypeDpkrRecord = TypeDpkrRecord.ActualizeAddRecord
                                };
                            }
                            else
                            {
                                ver3 = st2Version.Stage3Version;
                                ver3.Sum += stage3.Sum;
                                ver3.ObjectEditDate = DateTime.Now;
                            }

                            st2Version.Sum += stage2.Sum;
                            st2Version.ObjectEditDate = DateTime.Now;
                            ver2S.Add(st2Version);

                            if (stage1Dict.TryGetValue(stage2, out var st1RecList))
                            {
                                foreach (var stage1 in st1RecList)
                                {
                                    ver1S.Add(new VersionRecordStage1
                                    {
                                        RealityObject = new RealityObject
                                        {
                                            Id = stage1.StructuralElement.RealityObject.Id
                                        },
                                        Stage2Version = st2Version,
                                        TypeDpkrRecord = TypeDpkrRecord.ActualizeAddRecord,
                                        Year = stage1.Year,
                                        StructuralElement = new RealityObjectStructuralElement
                                        {
                                            Id = stage1.StructuralElement.Id
                                        },
                                        Sum = stage1.Sum,
                                        SumService = stage1.ServiceCost,
                                        Volume = stage1.StructuralElement.Volume,
                                        ObjectCreateDate = DateTime.Now,
                                        StrElement = stage1.StructuralElement.StructuralElement,
                                        ObjectEditDate = DateTime.Now
                                    });
                                }
                            }
                        }
                    }

                    ver3S.Add(ver3);
                }

                // Тут ненужно пересчитывать индекс
                //ver3S = ChangeIndexNumber(ver3S, versSt3Query);
            }

            return new BaseDataResult();
        }

        public IDataResult ActualizeNewRecords(BaseParams baseParams)
        {
            /* требование 57368
             * Добавить новые записи. Условие добавления: 
             *      1. проверить в реестре жилых домов наличие. которые должны попадать в ДПКР, но их нет (Вновь добавленные дома, Дома с Тип дома = Многоквартирный и Состояние = Исправный и Ветхий). 
             *      2. В домах проверить список КЭ, которые должны быть в ДПКР, но их нет (Новые КЭ, Проверять КЭ со статусом с отметкой "Начальный"). Если новые записи добавлены, 
             *         то выводить сообщение: Программа успешно актуализирована.
             */

            var groupTypeCodes = baseParams.Params.GetAs<string>("groupTypeCodes").ToStringArray();
            var groupTypeCodeIncluding = baseParams.Params.GetAs<bool>("groupTypeCodeIncluding");
            var versionActualizeType = baseParams.Params.GetAs<VersionActualizeType>("versionActualizeType");

            var newRecordsGettingResult = this.GetNewRecords(baseParams, out var ver3S, out var ver2S, out var ver1S,
                comEstObjGroupTypeCodes: groupTypeCodes, comEstObjGroupTypeCodeIncluding: groupTypeCodeIncluding);

            if (!newRecordsGettingResult.Success)
            {
                return newRecordsGettingResult;
            }

            var logCreatingResult = this.CreateActualizationLog(baseParams, ver1S, ver3S, out var log,
                versionActualizeType, "Добавление", "Удовлетворяет условиям ДПКР");

            if (!logCreatingResult.Success)
            {
                return logCreatingResult;
            }

            this.SaveActualizationResult(log, ver1S, ver2S, ver3S);

            return new BaseDataResult();
        }

        public IDataResult ActualizeSum(BaseParams baseParams)
        {
            /* требовнаие 57368
             * Актуализировать стоимость. Обновляет стоимость ремонта, если были изменены: 
             * 1. у КЭ изменился объем (при расчете расценки по объему) 
             * 2. У дома изменена площадь (при расчете стоимости работ по Площади, проверять площадь, которая стоит в настройка). 
             * 3. Изменилась Расценка по работе  в справочнике "Расценки по работам".
             */
            var programVersionDomain = Container.ResolveDomain<ProgramVersion>();
            var versSt1Domain = Container.ResolveDomain<VersionRecordStage1>();
            var versSt2Domain = Container.ResolveDomain<VersionRecordStage2>();
            var versSt3Domain = Container.ResolveDomain<VersionRecord>();
            var workPriceDomain = Container.ResolveDomain<WorkPrice>();
            var structElementWorkDomain = Container.ResolveDomain<StructuralElementWork>();

            var versionId = baseParams.Params.GetAs<long>("versionId");
            var municipalityId = baseParams.Params.GetAs<long>("municipalityId");
            var actualizeStart = baseParams.Params.GetAs("yearStart", 0);
            var actualizeEnd = baseParams.Params.GetAs("yearEnd", 0);

            using (Container.Using(programVersionDomain, versSt1Domain, versSt2Domain, versSt3Domain, workPriceDomain, structElementWorkDomain))
            {
                var version = programVersionDomain.GetAll().FirstOrDefault(x => x.Id == versionId);
                var config = Container.GetGkhConfig<OverhaulTatConfig>();
                var periodStartYear = config.ProgrammPeriodStart;
                var servicePercent = config.ServiceCost;
                
                var verSt1Query = versSt1Domain.GetAll()
                    .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.Id == versionId 
                                && x.RealityObject.Municipality.Id == municipalityId 
                                && x.Stage2Version.Stage3Version.CorrectYear >= actualizeStart
                                && x.Stage2Version.Stage3Version.CorrectYear <= actualizeEnd);

                // фиксируем старые значения для того чтобы в логи записать информацию об изменении
                var OldDataSt1 = verSt1Query.Select(x => new { St3Id = x.Stage2Version.Stage3Version.Id, x.Volume, x.Sum })
                               .AsEnumerable();

                // В РТ могли работы забивать прямо в ДПКР а потом они переходили в версию
                // у некоторых записей Связи КЭ и Дома может не быть но объем пересчитать нужно 
                // поэтому если связь с домом по КЭ ест ьто берем объем от туда иначе берм объем сохраненный в версии
                var versSt1Recs = verSt1Query
                    .Select(x => new
                                    {
                                        x.Id,
                                        St2Id = x.Stage2Version.Id,
                                        St2 = x.Stage2Version,
                                        St3Id = x.Stage2Version.Stage3Version.Id,
                                        St3 = x.Stage2Version.Stage3Version,
                                        x.Stage2Version.Stage3Version.CorrectYear,
                                        x.Stage2Version.Stage3Version.Year,
                                        RoId = x.RealityObject.Id,
                                        x.Sum,
                                        x.SumService,
                                        CalculateBy = x.StructuralElement != null ? x.StructuralElement.StructuralElement.CalculateBy : x.StrElement.CalculateBy,
                                        StElId = x.StructuralElement != null ? x.StructuralElement.StructuralElement.Id : x.StrElement.Id,
                                        Volume = x.StructuralElement != null ? x.StructuralElement.Volume : x.Volume,
                                        x.RealityObject.AreaLiving,

                                    })
                    .ToList();

                // словарь для быстрого получения нужных записей 
                var versSt1Dect = verSt1Query.ToDictionary(x => x.Id);

                var st1RecToUpdate = new List<VersionRecordStage1>();
                var st2RecToUpdate = new Dictionary<long, VersionRecordStage2>();
                var st3RecToUpdate = new Dictionary<long, VersionRecord>();
                
                var dictPrices = workPriceDomain.GetAll()
                    .Where(x => x.Municipality != null && x.Municipality.Id == version.Municipality.Id)
                    .AsEnumerable()
                    .GroupBy(x => x.Municipality.Id)
                    .ToDictionary(  x => x.Key, 
                                    y => y.GroupBy(x => x.Year)
                                          .ToDictionary(x => x.Key, z => z.ToList()));
                
                var dictStructElWork = structElementWorkDomain.GetAll()
                            .Select(x => new
                            {
                                SeId = x.StructuralElement.Id,
                                JobId = x.Job.Id,
                                JobName = x.Job.Name
                            })
                            .AsEnumerable()
                            .GroupBy(x => x.SeId)
                            .ToDictionary(x => x.Key, y => y.GroupBy(x => x.JobId).ToDictionary(x => x.Key, z => z.Select(x => x.JobName).First()));

                var allPricesExist = true;
                var dict = new Dictionary<long, Dictionary<long, string>>();

                foreach (var st1Rec in versSt1Recs)
                {

                    var correctionYear = st1Rec.CorrectYear;

                    var jobs = dictStructElWork.ContainsKey(st1Rec.StElId)
                                    ? dictStructElWork[st1Rec.StElId]
                                    : new Dictionary<long, string>();

                    IEnumerable<WorkPrice> jobPrices;

                    if (dictPrices.ContainsKey(version.Municipality.Id))
                    {
                        jobPrices = this.GetActualPrices(
                            jobs, dictPrices[version.Municipality.Id], periodStartYear, correctionYear);
                    }
                    else
                    {
                        jobPrices = new List<WorkPrice>();
                    }

                    var sum = 0M;
                    var volume = st1Rec.Volume;

                    if (jobPrices.Any())
                    {
                        switch (st1Rec.CalculateBy)
                        {
                            case PriceCalculateBy.Volume:
                                sum = jobPrices.Sum(x => st1Rec.Volume * x.NormativeCost);    
                                break;
                            case PriceCalculateBy.AreaLivingNotLivingMkd:
                            case PriceCalculateBy.TotalArea:
                            case PriceCalculateBy.LivingArea:
                                sum = st1Rec.AreaLiving.HasValue
                                          ? jobPrices.Sum(x => st1Rec.AreaLiving.Value * x.SquareMeterCost)
                                                     .ToDecimal()
                                          : 0m;
                                break;
                        }
                    }
                    else
                    {
                        // Отсутствуют необходимые расценки
                        allPricesExist = false;
                        
                        // Фиксируем их в словарь чтобы потом выдать ошибку по расценкам которых нет
                        if (!dict.ContainsKey(version.Municipality.Id))
                        {
                            dict.Add(version.Municipality.Id, new Dictionary<long, string>());
                        }

                        foreach (var job in jobs)
                        {
                            if (!dict[version.Municipality.Id].ContainsKey(job.Key))
                            {
                                dict[version.Municipality.Id].Add(job.Key, job.Value);
                            }
                        }

                        if (!allPricesExist)
                        {
                            var text = "Отсутсвуют расценки по работам:" + Environment.NewLine;
                            
                            foreach (var muId in dict.Keys)
                            {
                                var muName = version.Municipality.Name;
                                text += "МО: " + muName + Environment.NewLine;

                                text +=
                                    dict[muId].Select(x => x.Value)
                                              .Aggregate(
                                                  (result, current) =>
                                                  (string.IsNullOrWhiteSpace(result) ? current : ", " + current));

                                text += Environment.NewLine;
                            }

                            return new BaseDataResult(false, text);
                        }
                    }

                    var serviceCost = (sum * (servicePercent / 100M)).RoundDecimal(2);

                    // Теперь получив все значения над овыяснить относительно уже имеющихся данны
                    var rec1 = versSt1Dect[st1Rec.Id];

                    if (rec1.Sum != sum || rec1.SumService != serviceCost || rec1.Volume != st1Rec.Volume)
                    {
                        var st3Rec = st3RecToUpdate.Get(st1Rec.St3Id) ?? st1Rec.St3;
                        st3Rec.Sum += sum + serviceCost - st1Rec.Sum - st1Rec.SumService;
                        
                        st3Rec.ObjectEditDate = DateTime.Now;

                        if (!st3RecToUpdate.ContainsKey(st3Rec.Id))
                        {
                            st3RecToUpdate.Add(st3Rec.Id, st3Rec);
                        }

                        var st2Rec = st2RecToUpdate.Get(st1Rec.St2Id) ?? st1Rec.St2;
                        st2Rec.Sum += sum + serviceCost - st1Rec.Sum - st1Rec.SumService;
                        st2Rec.ObjectEditDate = DateTime.Now;

                        if (!st2RecToUpdate.ContainsKey(st2Rec.Id))
                        {
                            st2RecToUpdate.Add(st2Rec.Id, st2Rec);
                        }

                        rec1.Sum = sum;
                        rec1.SumService = serviceCost;
                        rec1.ObjectEditDate = DateTime.Now;
                        rec1.Volume = volume;
                        st1RecToUpdate.Add(rec1);
                    }
                }

                if (!st1RecToUpdate.Any() && !st2RecToUpdate.Any() && !st3RecToUpdate.Any())
                {
                    return new BaseDataResult(false, "Нет записей для изменения стоимости");
                }

// формируем логи актуализации
                
                // Старое значение объемов
                var volumeDict = OldDataSt1
                               .GroupBy(x => x.St3Id)
                               .ToDictionary(x => x.Key, y => y.Sum(z => z.Volume));

                // Старое значение суммы
                var sumDict = OldDataSt1
                               .GroupBy(x => x.St3Id)
                               .ToDictionary(x => x.Key, y => y.Sum(z => z.Sum));

                // новое значение объемов
                var newVolumeDict =
                    st1RecToUpdate.Select(x => new { St3Id = x.Stage2Version.Stage3Version.Id, x.Volume})
                                  .GroupBy(x => x.St3Id)
                                  .ToDictionary(x => x.Key, y => y.Sum(z => z.Volume));

                // новое значение 
                var roAddress =
                    verSt1Query.Select(x => new { roId = x.RealityObject.Id, x.RealityObject.Address })
                               .AsEnumerable()
                               .GroupBy(x => x.roId)
                               .ToDictionary(x => x.Key, y => y.Select(z => z.Address).FirstOrDefault());
                
                var log = new VersionActualizeLog();
                log.ActualizeType = VersionActualizeType.ActualizeSum;
                log.DateAction = DateTime.Now;
                log.Municipality = municipalityId > 0 ? new Municipality() { Id = municipalityId } : null;
                log.ProgramVersion = new ProgramVersion() { Id = versionId };
                log.UserName = User.Name;

                var logRecords = new List<ActualizeVersionLogRecord>();

                // Формируем строки для логов
                var recs = st3RecToUpdate.Values;

                foreach (var rec in recs)
                {
                    var oldVolume = volumeDict.ContainsKey(rec.Id) ? volumeDict[rec.Id] : 0m;
                    var newVolume = newVolumeDict.ContainsKey(rec.Id) ? newVolumeDict[rec.Id] : 0m;
                    var oldSum = sumDict.ContainsKey(rec.Id) ? sumDict[rec.Id] : 0m;

                    var logRecord = new ActualizeVersionLogRecord
                    {
                        TypeAction = VersionActualizeType.ActualizeSum,
                        Action = "Изменение",
                        Description = "Актуализация стоимости",
                        Address = roAddress.ContainsKey(rec.RealityObject.Id) ? roAddress[rec.RealityObject.Id] : rec.RealityObject.Address,
                        Ceo = rec.CommonEstateObjects,
                        PlanYear = rec.Year,
                        Volume = oldVolume,
                        Number = rec.IndexNumber,
                        Sum = oldSum,
                        ChangeVolume = oldVolume != newVolume ? newVolume : 0m, // Проставляем только в случае изменения
                        ChangeSum = oldSum != rec.Sum ? rec.Sum : 0m // проставляем только в случае изменения
                    };

                    logRecords.Add(logRecord);

                }

                if (logRecords.Any())
                {
                    log.CountActions = logRecords.Count();
                    log.LogFile = LogService.CreateLogFile(logRecords.OrderBy(x => x.Address).OrderBy(x => x.Number), baseParams);
                }
                else
                {
                    return new BaseDataResult(false, "Логи отсутсвуют");
                }

                using (var transaction = Container.Resolve<IDataTransaction>())
                {
                    try
                    {

                        st3RecToUpdate.Values.ForEach(versSt3Domain.Update);
                        st2RecToUpdate.Values.ForEach(versSt2Domain.Update);
                        st1RecToUpdate.ForEach(versSt1Domain.Update);

                        LogDomain.Save(log);

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

            return new BaseDataResult();
            
        }

        // поскольку в справочнике расценок может небыть нужного года то берем ближайшие актуальные расценки к нужному нам году
        private List<WorkPrice> GetActualPrices(Dictionary<long, string> jobs, Dictionary<int, List<WorkPrice>> prices, int periodStartProgram, int correctionYear)
        {
            List<WorkPrice> result = null;

            while (periodStartProgram <= correctionYear)
            {
                if (prices.ContainsKey(correctionYear) && prices[correctionYear].Any(x => jobs.ContainsKey(x.Job.Id)))
                {
                    result = prices[correctionYear].Where(x => jobs.ContainsKey(x.Job.Id)).ToList();
                    break;
                }

                // уменьшаем год корректирвоки приблежаясь к году Начала программу тем самым чтобы на следующем проходе получит актуальные расценки
                correctionYear--;
            }

            return result ?? new List<WorkPrice>();
        }

        public IDataResult ActualizeYear(BaseParams baseParams)
        {
            /* требование 57368
             * Актуализировать год.  при Актуализации программы учитывать изменение года ремонта КЭ в паспорте жилого дома:
•	в кнопку "Актуализировать ДПКР" добавить новую функцию (и кнопку) - "Изменение года ремонта КЭ";
•	среди всех КЭ из записей Версии программы находить те, у которых было изменено значение поля "Год установки или последнего капремонта" (значение получать из: Жилой дом / Конструктивные характеристики);
•	НЕ выполнять проверку на то, каким образом изменился год - в большую или меньшую сторону (заказчик будет отслеживать это самостоятельно);
•	удалять из Версии записи со всеми такими КЭ (при этом сдвигать очередь вверх);
•	выполнять проверку на наличие записей в КПКР - если запись попала в краткосрочную программу, она не может быть удалена из версии;
•	в Версию добавлять записи по этим КЭ с пересчетом их планового года ремонта;
•	рассчитывать Плановый год КР, основываясь на новом значении поля "Год установки или последнего капремонта", Новый плановый год ремонта = Год установки + Срок эксплуатации (значение получать из Справочники / ООИ / ГКЭ / КЭ);
•	2ое и дальнейшее попадание записи КЭ в Версию программы рассчитывать по формуле: Плановый год ремонта + Срок эксплуатации КЭ;
•	записи должны добавляться в Новый плановый год с приоритетом = 0;
•	в записях, у которых был изменен Плановый год добавлять сообщение в поле "Изменения";
•	текст сообщения: "Было изменено вследствие изменения Года установки/последнего капремонта КЭ. Дата изменения: ДД.ММ.Год ЧЧ.ММ.".

             */
            var programVersionDomain = Container.ResolveDomain<ProgramVersion>();
            var roStructElDomain = Container.ResolveDomain<RealityObjectStructuralElement>();
            var longProgramService = Container.Resolve<ILongProgramService>();
            var versSt1Domain = Container.ResolveDomain<VersionRecordStage1>();
            var versSt2Domain = Container.ResolveDomain<VersionRecordStage2>();
            var publishProgRecDomain = Container.ResolveDomain<PublishedProgramRecord>();
            var dpkrCorrectionDomain = Container.ResolveDomain<DpkrCorrectionStage2>();
            var shortProgramRecordDomain = Container.ResolveDomain<ShortProgramRecord>();
            var versSt3Domain = Container.ResolveDomain<VersionRecord>();
            var sessionProvider = Container.Resolve<ISessionProvider>();
            var unProxy = Container.Resolve<IUnProxy>();

            var versionId = baseParams.Params.GetAs<long>("versionId");
            var municipalityId = baseParams.Params.GetAs<long>("municipalityId");
            var actualizeStart = baseParams.Params.GetAs("yearStart", 0);

            try
            {
                List<VersionRecord> ver3S = null;
                List<VersionRecordStage2> ver2S = null;
                List<VersionRecordStage1> ver1S = null;

                // подготавливаем записи 2 этапа 
                var stage2Dict = versSt2Domain.GetAll()
                                .Where(
                                     x =>
                                     x.Stage3Version.ProgramVersion.Id == versionId
                                     && x.Stage3Version.RealityObject.Municipality.Id == municipalityId)
                                .AsEnumerable()
                                .GroupBy(x => x.Id)
                                .ToDictionary(x => x.Key, y => y.First());

                // подготавливаем записи 3 этапа 
                var stage3Dict = versSt3Domain.GetAll()
                                 .Where(
                                     x =>
                                     x.ProgramVersion.Id == versionId
                                     && x.RealityObject.Municipality.Id == municipalityId)
                                 .AsEnumerable()
                                .GroupBy(x => x.Id)
                                .ToDictionary(x => x.Key, y => y.First());

                var st1List =
                    versSt1Domain.GetAll()
                                 .Where(x => x.Stage2Version != null 
                                            && x.Stage2Version.Stage3Version != null 
                                            && x.Stage2Version.Stage3Version.ProgramVersion != null
                                            && x.Stage2Version.Stage3Version.RealityObject != null
                                            && x.StructuralElement != null)
                                 .Where(
                                     x =>
                                     x.Stage2Version.Stage3Version.ProgramVersion.Id == versionId
                                     && x.Stage2Version.Stage3Version.RealityObject.Municipality.Id == municipalityId)
                                 .Select(
                                     x =>
                                     new
                                         {
                                             RoSeId = x.StructuralElement.Id,
                                             x.StructuralElement.StructuralElement.LifeTime,
                                             x.StructuralElement.StructuralElement.LifeTimeAfterRepair,
                                             x.Stage2Version.Stage3Version.FixedYear,
                                             x.Stage2Version.Stage3Version.CorrectYear,
                                             x.Year,
                                             St1Id = x.Id,
                                             St1Sum = x.Sum,
                                             St1SumService = x.SumService,
                                             St2Id = x.Stage2Version.Id,
                                             St2Sum = x.Stage2Version.Sum,
                                             St3Id = x.Stage2Version.Stage3Version.Id,
                                             St3Sum = x.Stage2Version.Stage3Version.Sum
                                         })
                                 .ToList();

                var stage1VerRecs = st1List
                    .Where(x => x.CorrectYear >= actualizeStart)
                    .AsEnumerable()
                    .GroupBy(x => "{0}_{1}".FormatUsing(x.RoSeId, x.Year))
                    .ToDictionary(x => x.Key, y => y.Select(z => z.RoSeId).First());

                // поулаем список отремонтированных структурных элементов со своими сроками эксплуатации
                var St1Repaired =
                    st1List.Where(x => x.CorrectYear < actualizeStart || x.FixedYear)
                           .AsEnumerable()
                           .GroupBy(x => x.RoSeId)
                           .ToDictionary(
                               x => x.Key,
                               y =>
                               y.Select(
                                   z =>
                                   new RoStrElAfterRepair
                                       {
                                           RoStrElId = z.RoSeId,
                                           LifeTime = z.LifeTime,
                                           LifeTimeAfterRepair = z.LifeTimeAfterRepair,
                                           LastYearRepair = z.CorrectYear
                                       })
                                .OrderByDescending(z => z.LastYearRepair)
                                .FirstOrDefault());


                var allStage1VerRecs = st1List.Where(x => x.CorrectYear >= actualizeStart).ToList();

                // после этого когда мы удалили ненужные записи
                // теперь пытаемся найти записи которые будут новыми типа

                var changedRoStrEls = new List<long>();

                var notDeleteRoStrEls = st1List.Where(x => x.FixedYear).Select(x => "{0}_{1}".FormatUsing(x.RoSeId, x.Year)).ToList();

                /*
                var stage1ToSave = StagesService.GetStage1(periodStart, periodEnd, version.Municipality.Id, roStructElQuery);

                stage1ToSave.ForEach(x =>
                {
                    var key = "{0}_{1}".FormatUsing(x.StructuralElement.Id, x.Year);
                    if (!stage1VerRecs.ContainsKey(key))
                    {
                        if (!changedRoStrEls.Contains(x.StructuralElement.Id))
                            changedRoStrEls.Add(x.StructuralElement.Id);
                    }
                });
                */

                if (stage1VerRecs.Any() || changedRoStrEls.Any())
                {

                    var forDeleting = new List<long>();
                    forDeleting.AddRange(changedRoStrEls);

                    // и оставшиеся записи в версии также помечаем как необзходимые на удаление
                    foreach (var roStId in stage1VerRecs.Values.Distinct())
                    {
                        if (!forDeleting.Contains(roStId))
                        {
                            forDeleting.Add(roStId);
                        }
                    }

                    // теперь пытаемся для Якобы удаляемых элементов 
                    this.GetNewRecords(baseParams, out ver3S, out ver2S, out ver1S, St1Repaired, forDeleting);

                    if (ver1S.Any())
                    {
                        var newDictSt1 = ver1S.GroupBy(x => "{0}_{1}".FormatUsing(x.StructuralElement.Id, x.Year))
                                                .ToDictionary(x => x.Key, y => y.Select(z => z.StructuralElement.Id).First());

                        changedRoStrEls = new List<long>();

                        foreach (var kvp in newDictSt1)
                        {
                            if (stage1VerRecs.ContainsKey(kvp.Key))
                            {
                                if (!notDeleteRoStrEls.Contains(kvp.Key))
                                {
                                    notDeleteRoStrEls.Add(kvp.Key);
                                }

                                stage1VerRecs.Remove(kvp.Key);
                            }
                            else
                            {
                                if (!changedRoStrEls.Contains(kvp.Value))
                                    changedRoStrEls.Add(kvp.Value);
                            }
                        }

                    }

                    // и оставшиеся записи в версии также помечаем как необзходимые на удаление
                    foreach (var roStId in stage1VerRecs.Values.Distinct())
                    {
                        if (!changedRoStrEls.Contains(roStId))
                        {
                            changedRoStrEls.Add(roStId);
                        }
                    }
                }


                /*
                if (stage1VerRecs.Any())
                {
                    var roStrElIds = stage1VerRecs.Values.Distinct().ToList();

                    var roQuery = roStructElDomain.GetAll().Where(x => roStrElIds.Contains(x.Id));

                    var newStage1ToSave = StagesService.GetStage1(actualizeStart, periodEnd, version.Municipality.Id, roQuery);

                    newStage1ToSave.ForEach(x =>
                    {
                        var key = "{0}_{1}".FormatUsing(x.StructuralElement.Id, x.Year);
                        if (stage1VerRecs.ContainsKey(key))
                        {
                            stage1VerRecs.Remove(key);
                        }
                        else
                        {
                            if (!changedRoStrEls.Contains(x.StructuralElement.Id))
                                changedRoStrEls.Add(x.StructuralElement.Id);
                        }
                    });
                }
                */

                var Ids = allStage1VerRecs.Select(x => x.St1Id).Distinct().ToList();

                stage1VerRecs.Select(x => x.Value).ForEach(x =>
                    {
                        if (!changedRoStrEls.Contains(x)) 
                            changedRoStrEls.Add(x); 
                    });

                // значения объемов по 3 этапу
                var oldData = versSt1Domain.GetAll()
                                    .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.Id == versionId
                                        && x.Stage2Version.Stage3Version.RealityObject.Municipality.Id == municipalityId)
                                 .Select(
                                     x =>
                                     new
                                         {
                                             st3Id = x.Stage2Version.Stage3Version.Id,
                                             x.Volume,
                                             address = x.RealityObject.Address,
                                             ceoNames = x.Stage2Version.Stage3Version.CommonEstateObjects,
                                             x.Stage2Version.Stage3Version.Sum,
                                             x.Stage2Version.Stage3Version.Year,
                                             x.Stage2Version.Stage3Version.IndexNumber
                                         })
                                 .AsEnumerable()
                                 .GroupBy(x => x.st3Id)
                                    .ToDictionary(
                                        x => x.Key,
                                        y => new
                                        {

                                            
                                            ceoNames = y.Select(z => z.ceoNames).ToList().Any() ? 
                                                          y.Select(z => z.ceoNames)
                                                                .Distinct()
                                                                .ToList()
                                                                .Aggregate(
                                                                    (str, result) => string.IsNullOrEmpty(result) ? str : result + ", " + str) : string.Empty,
                                            volume = y.Sum(z => z.Volume),
                                            sum = y.Select(z => z.Sum).FirstOrDefault(),
                                            number = y.Select(z => z.IndexNumber).FirstOrDefault(),
                                            year = y.Select(z => z.Year).FirstOrDefault(),
                                            address = y.Select(z => z.address).FirstOrDefault()
                                        }
                                   );
                //логируем 
                var log = new VersionActualizeLog();
                log.ActualizeType = VersionActualizeType.ActualizeYear;
                log.DateAction = DateTime.Now;
                log.Municipality = municipalityId > 0 ? new Municipality() { Id = municipalityId } : null;
                log.ProgramVersion = new ProgramVersion() { Id = versionId };
                log.UserName = User.Name;

                var logRecords = new List<ActualizeVersionLogRecord>();

                var listChangeSt3 = new List<long>();
                var listDeletedSt3 = new List<long>();

                if (changedRoStrEls.Any())
                {

                    using (var transaction = Container.Resolve<IDataTransaction>())
                    {
                        try
                        {

                            var existIds =
                                shortProgramRecordDomain.GetAll()
                                                        .Where(y => Ids.Contains(y.Stage1.Id))
                                                        .Select(y => y.Stage1.Id)
                                                        .ToList();

                            allStage1VerRecs.Where(x => !existIds.Contains(x.St1Id))
                                // нужны те записи которых не было в краткосрочке
                                            .Where(x => changedRoStrEls.Contains(x.RoSeId)).ForEach(
                                                val =>
                                                    {

                                                        var key = "{0}_{1}".FormatUsing(val.RoSeId, val.Year);

                                                        if (notDeleteRoStrEls.Contains(key))
                                                        {
                                                            // нельзя удалять такую запись поскольку эта запись при добавлении все равно добавится 
                                                            return;
                                                        }

                                                        versSt1Domain.Delete(val.St1Id);

                                                        var st2 = stage2Dict[val.St2Id];
                                                        st2.Sum = val.St2Sum - val.St1Sum - val.St1SumService;
                                                        versSt2Domain.Update(st2);

                                                        var st3 = stage3Dict[val.St3Id];
                                                        st3.Sum = val.St3Sum - val.St1Sum;
                                                        versSt3Domain.Update(st3);

                                                        listChangeSt3.Add(st3.Id);
                                                    });

                            var st2ForDelete =
                                versSt2Domain.GetAll()
                                             .Where(
                                                 x =>
                                                 !versSt1Domain.GetAll().Any(y => y.Stage2Version.Id == x.Id)
                                                 && x.Stage3Version.ProgramVersion.Id == versionId);
                            dpkrCorrectionDomain.GetAll()
                                                .Where(
                                                    x =>
                                                    st2ForDelete.Any(y => y.Id == x.Stage2.Id)
                                                    && x.Stage2.Stage3Version.ProgramVersion.Id == versionId)
                                                .ForEach(x => dpkrCorrectionDomain.Delete(x.Id));
                            publishProgRecDomain.GetAll()
                                                .Where(
                                                    x =>
                                                    st2ForDelete.Any(y => y.Id == x.Stage2.Id)
                                                    && x.Stage2.Stage3Version.ProgramVersion.Id == versionId)
                                                .ForEach(x => publishProgRecDomain.Delete(x.Id));
                            st2ForDelete.ForEach(x => versSt2Domain.Delete(x.Id));

                            versSt3Domain.GetAll()
                                         .Where(
                                             x =>
                                             !versSt2Domain.GetAll().Any(y => y.Stage3Version.Id == x.Id)
                                             && x.ProgramVersion.Id == versionId)
                                         .Select(
                                             x =>
                                             new
                                                 {
                                                     x.Id,
                                                     x.RealityObject.Address,
                                                     x.Sum,
                                                     x.Year,
                                                     x.CommonEstateObjects,
                                                     x.IndexNumber
                                                 })
                                         .ForEach(
                                             x =>
                                                 {
                                                     // логируем Удаление Записи
                                                     var oldValues = oldData.ContainsKey(x.Id) ? oldData[x.Id] : null;

                                                     var logRecord = new ActualizeVersionLogRecord
                                                                         {
                                                                             TypeAction =
                                                                                 VersionActualizeType
                                                                                 .ActualizeYear,
                                                                             Action =
                                                                                 "Удаление",
                                                                             Description =
                                                                                 "Актуализация года",
                                                                             Address =
                                                                                 x.Address,
                                                                             Ceo =
                                                                                 oldValues
                                                                                 != null
                                                                                     ? oldValues
                                                                                           .ceoNames
                                                                                     : string
                                                                                           .Empty,
                                                                             PlanYear =
                                                                                 oldValues
                                                                                 != null
                                                                                     ? oldValues
                                                                                           .year
                                                                                     : 0,
                                                                             Volume =
                                                                                 oldValues
                                                                                 != null
                                                                                     ? oldValues
                                                                                           .volume
                                                                                     : 0,
                                                                             Sum =
                                                                                 oldValues
                                                                                 != null
                                                                                     ? oldValues
                                                                                           .sum
                                                                                     : 0,
                                                                             Number =
                                                                                 oldValues
                                                                                 != null
                                                                                     ? oldValues
                                                                                           .number
                                                                                     : 0
                                                                         };

                                                     logRecords.Add(logRecord);

                                                     listDeletedSt3.Add(x.Id);

                                                     versSt3Domain.Delete(x.Id);
                                                 });

                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }

                    // получаем итоговый список измененных объектов
                    listChangeSt3 = listChangeSt3.Where(x => !listDeletedSt3.Contains(x)).Distinct().ToList();

                    if (listChangeSt3.Any())
                    {
                        using (var session = sessionProvider.OpenStatelessSession())
                        {
                            using (var transaction = session.BeginTransaction())
                            {
                                try
                                {
                                    // Подготавливаем данные записей котоыре изменились чтобы 
                                    var changesData =
                                        versSt1Domain.GetAll()
                                                     .Where(
                                                         x => listChangeSt3.Contains(x.Stage2Version.Stage3Version.Id))
                                                     .Select(
                                                         x =>
                                                         new
                                                             {
                                                                 st3Id = x.Stage2Version.Stage3Version.Id,
                                                                 ceoNames = x.Stage2Version.CommonEstateObject.Name,
                                                                 x.Volume,
                                                                 x.Stage2Version.Stage3Version.Sum,
                                                                 x.Year,
                                                                 x.Stage2Version.Stage3Version.IndexNumber
                                                             })
                                                     .AsEnumerable()
                                                     .GroupBy(x => x.st3Id)
                                                     .ToDictionary(
                                                         x => x.Key,
                                                         y =>
                                                         new
                                                             {
                                                                 ceoNames = y.Select(z => z.ceoNames).ToList().Any() ? 
                                                                             y.Select(z => z.ceoNames)
                                                                              .Distinct()
                                                                              .ToList()
                                                                              .Aggregate(
                                                                                  (str, result) =>
                                                                                  string.IsNullOrEmpty(result)
                                                                                      ? str
                                                                                      : result + ", " + str) : string.Empty,
                                                                 volume = y.Sum(z => z.Volume),
                                                                 year = y.Select(z => z.Year).FirstOrDefault(),
                                                                 sum = y.Select(z => z.Sum).FirstOrDefault(),
                                                                 number = y.Select(z => z.IndexNumber).FirstOrDefault()
                                                             });

                                    foreach (var kvp in changesData)
                                    {
                                        var st3 = versSt3Domain.Load(kvp.Key);
                                        st3.CommonEstateObjects = kvp.Value.ceoNames;
                                        versSt3Domain.Save(st3);

                                        var oldAddress = string.Empty;
                                        var oldCeoNames = string.Empty;
                                        var oldYear = 0;
                                        var oldVolume = 0m;
                                        var oldSum = 0m;
                                        var oldNumber = 0;

                                        if (oldData.ContainsKey(kvp.Key))
                                        {
                                            var oldValues = oldData[kvp.Key];

                                            oldAddress = oldValues.address;
                                            oldCeoNames = oldValues.ceoNames;
                                            oldYear = oldValues.year;
                                            oldVolume = oldValues.volume;
                                            oldSum = oldValues.sum;
                                            oldNumber = oldValues.number;
                                        }

                                        var newValues = kvp.Value;

                                        // логируем Изменение Записи
                                        var logRecord = new ActualizeVersionLogRecord
                                                            {
                                                                TypeAction =
                                                                    VersionActualizeType
                                                                    .ActualizeYear,
                                                                Action = "Изменение",
                                                                Description =
                                                                    "Актуализация года",
                                                                Address = oldAddress,
                                                                Ceo = oldCeoNames,
                                                                PlanYear = oldYear,
                                                                Volume = oldVolume,
                                                                Sum = oldSum,
                                                                Number = oldNumber,
                                                                ChangeCeo =
                                                                    newValues.ceoNames
                                                                    != oldCeoNames
                                                                        ? newValues.ceoNames
                                                                        : null,
                                                                ChangeNumber =
                                                                    newValues.number
                                                                    != oldNumber
                                                                        ? newValues.number
                                                                        : 0,
                                                                ChangePlanYear =
                                                                    newValues.year != oldYear
                                                                        ? newValues.year
                                                                        : 0,
                                                                ChangeSum =
                                                                    newValues.sum != oldSum
                                                                        ? newValues.sum
                                                                        : 0m,
                                                                ChangeVolume =
                                                                    newValues.volume
                                                                    != oldVolume
                                                                        ? newValues.volume
                                                                        : 0m
                                                            };

                                        logRecords.Add(logRecord);

                                    }

                                    transaction.Commit();
                                }
                                catch (Exception)
                                {
                                    transaction.Rollback();
                                    throw;
                                }
                            }
                        }

                    }

                    var dataResult = GetNewRecords(
                        baseParams, out ver3S, out ver2S, out ver1S, St1Repaired, null, notDeleteRoStrEls);

                    using (var session = sessionProvider.OpenStatelessSession())
                    {
                        using (var transaction = session.BeginTransaction())
                        {
                            try
                            {

                                var newVolumeDict =
                                    ver1S.Select(x => new { x.Stage2Version.Stage3Version, x.Volume })
                                         .AsEnumerable()
                                         .GroupBy(x => x.Stage3Version)
                                         .ToDictionary(x => x.Key, y => y.Sum(z => z.Volume));

                                ver3S.ForEach(
                                    x =>
                                        {
                                            if (x.Id > 0)
                                            {
                                                session.Update(unProxy.GetUnProxyObject(x));

                                                var newVolume = 0m;
                                                if (newVolumeDict.ContainsKey(x))
                                                {
                                                    newVolume = newVolumeDict[x];
                                                }

                                                var oldAddress = string.Empty;
                                                var oldCeoNames = string.Empty;
                                                var oldYear = 0;
                                                var oldVolume = 0m;
                                                var oldSum = 0m;
                                                var oldNumber = 0;

                                                if (oldData.ContainsKey(x.Id))
                                                {
                                                    var oldValues = oldData[x.Id];

                                                    oldAddress = oldValues.address;
                                                    oldCeoNames = oldValues.ceoNames;
                                                    oldYear = oldValues.year;
                                                    oldVolume = oldValues.volume;
                                                    oldSum = oldValues.sum;
                                                    oldNumber = oldValues.number;
                                                }

                                                // логируем Изменение Записи
                                                var logRecord = new ActualizeVersionLogRecord
                                                                    {
                                                                        TypeAction =
                                                                            VersionActualizeType
                                                                            .ActualizeYear,
                                                                        Action = "Изменение",
                                                                        Description =
                                                                            "Актуализация года",
                                                                        Address = oldAddress,
                                                                        Ceo = oldCeoNames,
                                                                        PlanYear = oldYear,
                                                                        Volume = oldVolume,
                                                                        Sum = oldSum,
                                                                        Number = oldNumber,
                                                                        ChangeCeo =
                                                                            x
                                                                                .CommonEstateObjects
                                                                            != oldCeoNames
                                                                                ? x
                                                                                      .CommonEstateObjects
                                                                                : null,
                                                                        ChangeNumber =
                                                                            x.IndexNumber
                                                                            != oldNumber
                                                                                ? x
                                                                                      .IndexNumber
                                                                                : 0,
                                                                        ChangePlanYear =
                                                                            x.Year != oldYear
                                                                                ? x.Year
                                                                                : 0,
                                                                        ChangeSum =
                                                                            x.Sum != oldSum
                                                                                ? x.Sum
                                                                                : 0m,
                                                                        ChangeVolume =
                                                                            newVolume
                                                                            != oldVolume
                                                                                ? newVolume
                                                                                : 0m
                                                                    };

                                                logRecords.Add(logRecord);
                                            }
                                            else
                                            {
                                                session.Insert(x);

                                                var newVolume = 0m;
                                                if (newVolumeDict.ContainsKey(x))
                                                {
                                                    newVolume = newVolumeDict[x];
                                                }

                                                // логируем Добавление Записи
                                                var logRecord = new ActualizeVersionLogRecord
                                                                    {
                                                                        TypeAction =
                                                                            VersionActualizeType
                                                                            .ActualizeYear,
                                                                        Action = "Добавление",
                                                                        Description =
                                                                            "Актуализация года",
                                                                        Address =
                                                                            x.RealityObject
                                                                             .Address,
                                                                        Ceo =
                                                                            x
                                                                            .CommonEstateObjects,
                                                                        PlanYear = x.Year,
                                                                        Volume = newVolume,
                                                                        Sum = x.Sum,
                                                                        Number = x.IndexNumber
                                                                    };

                                                logRecords.Add(logRecord);

                                            }
                                        });

                                ver2S.ForEach(
                                    x =>
                                        {
                                            if (x.Id > 0) session.Update(x);
                                            else session.Insert(x);
                                        });

                                ver1S.ForEach(x => session.Insert(x));
                                transaction.Commit();
                            }
                            catch (Exception)
                            {
                                transaction.Rollback();
                                throw;
                            }
                        }
                    }

                    if (logRecords.Any())
                    {
                        log.CountActions = logRecords.Count();
                        log.LogFile = LogService.CreateLogFile(
                            logRecords.OrderBy(x => x.Address).OrderBy(x => x.Number), baseParams);
                    }
                    else
                    {
                        return new BaseDataResult(false, "Логи отсутсвуют");
                    }

                    LogDomain.Save(log);

                    return dataResult;
                }
                else
                {
                    return new BaseDataResult(false, "Нет изменений для Актуализации года");
                }
            }
            finally
            {
                Container.Release(programVersionDomain);
                Container.Release(roStructElDomain);
                Container.Release(longProgramService);
                Container.Release(versSt1Domain);
                Container.Release(versSt2Domain);
                Container.Release(publishProgRecDomain);
                Container.Release(dpkrCorrectionDomain);
                Container.Release(versSt3Domain);
                Container.Release(unProxy);
                Container.Release(shortProgramRecordDomain);
            }
            
            return new BaseDataResult();
        }
        
        private class RoStrElAfterRepair
        {
            public long RoStrElId { get; set; }
            public int LifeTime { get; set; }
            public int LifeTimeAfterRepair { get; set; }
            public int LastYearRepair { get; set; }
        }

        public IQueryable<VersionRecordStage1> GetDeletedEntriesQueryable(long versionId, int actualizeStartYear)
        {
            var roStructElDomain = Container.Resolve<IDomainService<RealityObjectStructuralElement>>();
            var roStrElService = Container.Resolve<IRealObjStructElementService>();
            var versStage1Domain = Container.Resolve<IDomainService<VersionRecordStage1>>();
            var shortRecordsDomain = Container.Resolve<IDomainService<ShortProgramRecord>>();

            try
            {
                var roStrElQuery = roStrElService.GetUsedInLongProgram();

                // Поулчаем те записи 1го этапа версии для которые уже считаются не актуальными
                return versStage1Domain.GetAll()
                        .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.Id == versionId)
                        .Where(x => x.Stage2Version.Stage3Version.CorrectYear >= actualizeStartYear)
                        .Where(x => !shortRecordsDomain.GetAll().Any(y => y.Stage1.Id == x.Id)) // нужны только записи, которых нет в краткосрочке
                        .Where(x => !roStrElQuery.Any(y => y.Id == x.StructuralElement.Id)); // нужны записи которых нет среди нужных для ДПКР КЭ
            }
            finally
            {
                Container.Release(roStructElDomain);
                Container.Release(roStrElService);
                Container.Release(versStage1Domain);
            }
        }

        public IDataResult ActualizeDeletedEntries(BaseParams baseParams)
        {

            /* Требование 57368
             •	назначение - проверка версии программы на наличие домов и КЭ, которых в ней быть не должно (было изменено состояние или тип дома, статус КЭ и т.д.) ;
             •	при нажатии на кнопку запускать проверку, подходят ли добавленные в Версию дома и их КЭ под условия включения в программу (механизм проверки реализован на начальном этапе формирования ДПКР - кнопка "Расчет ДПКР"):
                    o	если да, оставлять очередь без изменений;
                    o	если нет, удалять записи с элементами, не удовлетворяющими условиям проверки; при удалении каждой записи смещать нумерацию записей на 1 шаг назад);
             •	по итогам проверки выводить следующее сообщение:
                    o	"Лишние записи были удалены" в случае удаления из версии записей;
                    o	"Лишние записи не обнаружены", если все записи версии соответствуют условиям проверки.
             * - например, состояние дома было изменено с "Исправный" на "Аварийный". Записи со всеми КЭ этого дома должны быть удалены из Версии программы. При этом очередь должна быть смещена вверх.

             */
            var roStructElDomain = Container.Resolve<IDomainService<RealityObjectStructuralElement>>();
            var stage2Domain = Container.Resolve<IDomainService<VersionRecordStage2>>();
            var stage1Domain = Container.Resolve<IDomainService<VersionRecordStage1>>();
            var stage3Domain = Container.Resolve<IDomainService<VersionRecord>>();
            var unProxy = Container.Resolve<IUnProxy>();

            try
            {
                var versionId = baseParams.Params.GetAs<long>("versionId");
                var municipalityId = baseParams.Params.GetAs<long>("municipalityId");
                var actualizeStart = baseParams.Params.GetAs("yearStart", 0);

                var query = GetDeletedEntriesQueryable(versionId, actualizeStart);
                
                if (query.Any())
                {
                    var st1Query = stage1Domain.GetAll()
                                           .Fetch(x => x.Stage2Version)
                                           .ThenFetch(x => x.Stage3Version)
                                           .ThenFetch(x => x.ProgramVersion)
                                           .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.Id == versionId)
                                           .Where(x => query.Any(y => y.Stage2Version.Stage3Version.Id == x.Stage2Version.Stage3Version.Id));

                    var stage1IdsForDelete = query.Select(x => x.Id).ToHashSet();

                    var volumeDict = st1Query
                                        .AsEnumerable()
                                        .GroupBy(x => x.Stage2Version.Stage3Version.Id)
                                        .ToDictionary(x => x.Key, y => y.Sum(z => z.Volume));


                    var deleteVolumeDict = query
                                        .AsEnumerable()
                                        .GroupBy(x => x.Stage2Version.Stage3Version.Id)
                                        .ToDictionary(x => x.Key, y => y.Sum(z => z.Volume));

                    var st2Dict = st1Query
                        .AsEnumerable()
                        .GroupBy(x => x.Stage2Version)
                        .Select(x =>
                        {
                            var existSt1 = x.Where(y => !stage1IdsForDelete.Contains(y.Id)).ToArray();

                            x.Key.Sum = existSt1.SafeSum(y => y.Sum + y.SumService);

                            return new
                            {
                                x.Key,
                                Value = existSt1
                            };
                        })
                        .ToDictionary(x => x.Key, y => y.Value);

                    var stage2IdsForDelete = st2Dict.Where(x => !x.Value.Any()).Select(x => x.Key.Id).ToHashSet();

                    var st3Dict = st2Dict
                        .Select(x => x.Key)
                        .GroupBy(x => x.Stage3Version)
                        .ToDictionary(x => x.Key, y => y.Where(x => !stage2IdsForDelete.Contains(x.Id)).ToArray());

                    var stage3IdsForDelete = st3Dict.Where(x => !x.Value.Any()).Select(x => x.Key.Id).ToHashSet();

                    if (!stage1IdsForDelete.Any())
                    {
                        return new BaseDataResult(false, "Нет записей для удаления");
                    }

                    var log = new VersionActualizeLog();
                    log.ActualizeType = VersionActualizeType.ActualizeDeletedEntries;
                    log.DateAction = DateTime.Now;
                    log.Municipality = municipalityId > 0 ? new Municipality() { Id = municipalityId } : null;
                    log.ProgramVersion = new ProgramVersion() { Id = versionId };
                    log.UserName = User.Name;

                    var logRecords = new List<ActualizeVersionLogRecord>();

                    var st3ToUpdate = new List<VersionRecord>();
                    var st2ToUpdate = new List<VersionRecordStage2>();
                    foreach (var st3 in st3Dict)
                    {
                        var oldVolume = volumeDict.ContainsKey(st3.Key.Id) ? volumeDict[st3.Key.Id] : 0m;
                        var deleteVolume = deleteVolumeDict.ContainsKey(st3.Key.Id) ? deleteVolumeDict[st3.Key.Id] : 0m;

                        if (stage3IdsForDelete.Contains(st3.Key.Id))
                        {
                            var logRecord = new ActualizeVersionLogRecord
                            {
                                TypeAction = VersionActualizeType.ActualizeDeletedEntries,
                                Action = "Удаление",
                                Description = "Не удовлетворяет условиям ДПКР",
                                Address = st3.Key.RealityObject.Address,
                                Ceo = st3.Key.CommonEstateObjects,
                                PlanYear = st3.Key.Year,
                                Volume = oldVolume,
                                Sum = st3.Key.Sum,
                                Number = st3.Key.IndexNumber
                            };

                            logRecords.Add(logRecord);
                        }
                        else
                        {
                            var logRecord = new ActualizeVersionLogRecord
                            {
                                TypeAction = VersionActualizeType.ActualizeDeletedEntries,
                                Action = "Изменение",
                                Description = "Часть КЭ не удовлетворяет условиям ДПКР",
                                Address = st3.Key.RealityObject.Address,
                                Ceo = st3.Key.CommonEstateObjects,
                                PlanYear = st3.Key.Year,
                                Volume = oldVolume,
                                Sum = st3.Key.Sum,
                                Number = st3.Key.IndexNumber
                            };


                            st3.Key.Sum = st3.Value.SafeSum(y => y.Sum);
                            st3.Key.CommonEstateObjects = st3.Value.Select(y => y.CommonEstateObject.Name).Distinct().AggregateWithSeparator(", ");

                            logRecord.ChangeCeo = st3.Key.CommonEstateObjects;
                            logRecord.ChangeSum = st3.Key.Sum;
                            logRecord.ChangeVolume = oldVolume - deleteVolume;

                            logRecords.Add(logRecord);

                            st3ToUpdate.Add(st3.Key);
                            st2ToUpdate.AddRange(st3.Value);
                        }
                    }

                    if (logRecords.Any())
                    {
                        log.CountActions = logRecords.Count();
                        log.LogFile = LogService.CreateLogFile(logRecords.OrderBy(x => x.Address).OrderBy(x => x.Number), baseParams);
                    }
                    else
                    {
                        return new BaseDataResult(false, "Логи отсутсвуют");
                    }

                    var sessionProvider = Container.Resolve<ISessionProvider>();
                    using (var session = sessionProvider.OpenStatelessSession())
                    {
                        using (var transaction = session.BeginTransaction())
                        {
                            try
                            {

                                foreach (var id in stage1IdsForDelete)
                                {
                                    // удаляем связь с видами работ
                                    session.CreateSQLQuery(
                                        string.Format(@"delete from ovrhl_type_work_cr_st1 where st1_id = {0}", id))
                                            .ExecuteUpdate();

                                    // удаляем запись 1 этапа
                                    session.CreateSQLQuery(string.Format(@"delete from OVRHL_STAGE1_VERSION where id = {0}", id)).ExecuteUpdate();
                                }


                                foreach (var id in stage2IdsForDelete)
                                {

                                    // удаляем Корректировки по версии
                                    session.CreateSQLQuery(string.Format(@"delete from OVRHL_DPKR_CORRECT_ST2 where st2_version_id  = {0}", id)).ExecuteUpdate();

                                    // удаляем версию 1 этапа
                                    session.CreateSQLQuery(string.Format(@"delete from ovrhl_stage1_version where stage2_version_id = {0}", id)).ExecuteUpdate();

                                    // удаляем запись опубликованной программы
                                    session.CreateSQLQuery(string.Format(@"delete from ovrhl_publish_prg_rec where stage2_id = {0} ", id)).ExecuteUpdate();

                                    // удаляем запись 2 этапа
                                    session.CreateSQLQuery(string.Format(@"delete from OVRHL_STAGE2_VERSION where id = {0}", id)).ExecuteUpdate();
                                }


                                foreach (var id in stage3IdsForDelete)
                                {
                                    // удаляем версию 2 этапа
                                    session.CreateSQLQuery(string.Format(@"delete from ovrhl_stage2_version where st3_version_id = {0}", id)).ExecuteUpdate();

                                    // удаляем запись 2 этапа
                                    session.CreateSQLQuery(string.Format(@"delete from OVRHL_VERSION_REC where id = {0}", id)).ExecuteUpdate();
                                }

                                foreach (var st3 in st3ToUpdate)
                                {
                                    session.Update(unProxy.GetUnProxyObject(st3));
                                }

                                foreach (var st2 in st2ToUpdate)
                                {
                                    session.Update(unProxy.GetUnProxyObject(st2));
                                }

                                session.Insert(log);

                                transaction.Commit();
                            }
                            catch (Exception exc)
                            {
                                transaction.Rollback();
                                throw exc;
                            }
                        }

                    }

                    // Поскольку удалили чтото из версии то над опересчитать заново индекс
                    UpdateIndexNumber(versionId);
                }

                return new BaseDataResult();

            }
            finally
            {
                Container.Release(roStructElDomain);
                Container.Release(stage2Domain);
                Container.Release(stage1Domain);
                Container.Release(stage3Domain);
            }
        }

        private void UpdateIndexNumber(long versionId)
        {
            
            // Тут короче я прохожу по всем записям версии и просто проверяют если ест ькакието пустоты то заполняю их нужными индексами
            // следовательно вся очередь уплотняется к наименьшему значению
            
            var stage3Domain = Container.Resolve<IDomainService<VersionRecord>>();

            try
            {
                var data = stage3Domain.GetAll().Where(x => x.ProgramVersion.Id == versionId)
                                .OrderBy(x => x.IndexNumber)
                                .AsEnumerable();

                var listToUpdate = new List<VersionRecord>();

                var idx = 1;

                foreach (var rec in data)
                {
                    if (rec.IndexNumber != idx)
                    {
                        rec.IndexNumber = idx;
                        listToUpdate.Add(rec);
                    }

                    ++idx;
                }

                using (var tr = Container.Resolve<IDataTransaction>())
                {
                    try
                    {
                        listToUpdate.ForEach(stage3Domain.Update);
                        tr.Commit();
                    }
                    catch (Exception)
                    {

                        tr.Rollback();
                        throw;
                    }
                }
            }
            finally
            {
                Container.Release(stage3Domain);
            }
             
        }

        public IDataResult GetWarningMessage(BaseParams baseParams)
        {
            var versionId = baseParams.Params.GetAs<long>("versionId");
            var actualizeStart = baseParams.Params.GetAs("yearStart", 0);
            var actualizeEnd = baseParams.Params.GetAs("yearEnd", 0);
            var typeWorkSt1Domain = Container.ResolveDomain<TypeWorkCrVersionStage1>();
            var config = Container.GetGkhConfig<OverhaulTatConfig>();
            var startPeriod = config.ProgrammPeriodStart;
            var endPeriod = config.ProgrammPeriodEnd;

            using (Container.Using(typeWorkSt1Domain))
            {
                /*
                if (stage3Period > 0)
                {
                    return new BaseDataResult(false, @"Актуализация недоступна при периоде группировки больше 0");
                }
                */

                if (actualizeStart > 0)
                {
                    if (actualizeStart > actualizeEnd && actualizeEnd > 0)
                    {
                        return new BaseDataResult(false, "Начало периода актуазиации не может быть больше окончания периода актуализации");
                    }

                    if (actualizeStart < startPeriod)
                    {
                        return new BaseDataResult(false, string.Format("Начало периода актуализиации не должно быть меньше начала долгосрочной программы: {0}.", startPeriod));
                    }

                    if (actualizeEnd > endPeriod)
                    {
                        return new BaseDataResult(false, string.Format("Конец периода актуализиации не должен быть больше окончания долгосрочной программы: {0}.", endPeriod));
                    }    
                }

                var programs = typeWorkSt1Domain.GetAll()
                                     .Where(x => x.Stage1Version.Stage2Version.Stage3Version.ProgramVersion.Id == versionId)
                                     .Where(x => x.TypeWorkCr.ObjectCr.ProgramCr.TypeProgramStateCr != TypeProgramStateCr.Close)
                                     .Where(x => x.TypeWorkCr.ObjectCr.ProgramCr.TypeVisibilityProgramCr != TypeVisibilityProgramCr.Hidden)
                                     .WhereIf(
                                         actualizeStart > 0,
                                         x => x.TypeWorkCr.ObjectCr.ProgramCr.Period.DateStart.Year >= actualizeStart)
                                     .WhereIf(
                                         actualizeEnd > 0,
                                         x => x.TypeWorkCr.ObjectCr.ProgramCr.Period.DateStart.Year <= actualizeEnd)
                                     .Select(x => x.TypeWorkCr.ObjectCr.ProgramCr.Id)
                                     .Distinct()
                                     .ToList();
                
                if (programs.Any())
                {
                    var names = ProgramCrDomain.GetAll()
                                    .Where(x => programs.Contains(x.Id))
                                    .Select(x => x.Period.Name)
                                    .ToList()
                                    .Distinct()
                                    .Aggregate((x, y) => string.IsNullOrEmpty(y) ? x : y + "," + x);

                    return new BaseDataResult(false, string.Format("За период '{0}' создана краткосрочная программа. Актуализация долгосрочной программы за этот период не может быть проведена. Измените период актуализации и проведите актуализацию повторно.", names));
                }

                return new BaseDataResult(null);
            }
        }

        private List<VersionRecord> ChangeIndexNumber(List<VersionRecord> changedRecs, IQueryable<VersionRecord> verRecQuery)
        {
            var result = new List<VersionRecord>();

            var newRecsMinYear = changedRecs.SafeMin(x => x.Year);

            var recsForChangeIndexNum = verRecQuery
                                            .Where(x => newRecsMinYear > 0 && x.Year > newRecsMinYear)
                                            .ToList();

            var changedRecIds = changedRecs.Select(x => x.Id).ToHashSet();

            changedRecs.AddRange(recsForChangeIndexNum.Where(x => !changedRecIds.Contains(x.Id)));

            var changedRecsByYearDict = changedRecs
                .GroupBy(x => x.Year)
                .OrderBy(x => x.Key)
                .ToDictionary(x => x.Key, y => y.OrderByDescending(x => x.IndexNumber));

            var maxIndNumByYear = verRecQuery
                .Select(x =>
                    new
                    {
                        x.IndexNumber,
                        x.Year
                    })
                  .AsEnumerable()
                  .GroupBy(x => x.Year)
                  .Select(x => new { Year = x.Key, MaxIndNum = x.Max(y => y.IndexNumber) })
                  .ToList();

            var indexIncrement = 0;

            foreach (var changedRecsByYear in changedRecsByYearDict)
            {
                var yearMaxInd = maxIndNumByYear.Where(x => x.Year <= changedRecsByYear.Key).SafeMax(x => x.MaxIndNum);

                foreach (var changedRec in changedRecsByYear.Value)
                {
                    if (changedRec.Id > 0)
                    {
                        changedRec.IndexNumber += indexIncrement;
                    }
                    else
                    {
                        ++indexIncrement;
                        changedRec.IndexNumber = indexIncrement + yearMaxInd;
                    }

                    result.Add(changedRec);
                }
            }

            return result;
        }

        private IDataResult CreateActualizationLog(
            BaseParams baseParams,
            IEnumerable<VersionRecordStage1> ver1S,
            IEnumerable<VersionRecord> ver3S,
            out VersionActualizeLog log,
            VersionActualizeType versionActualizeType,
            string action,
            string description)
        {
            var versionId = baseParams.Params.GetAs<long>("versionId");
            var municipalityId = baseParams.Params.GetAs<long>("municipalityId");

            var volumeDict = ver1S
                .Select(x => new { x.Stage2Version.Stage3Version, x.Volume })
                .GroupBy(x => x.Stage3Version)
                .ToDictionary(x => x.Key, y => y.Sum(x => x.Volume));

            log = new VersionActualizeLog
            {
                ActualizeType = versionActualizeType,
                DateAction = DateTime.Now,
                Municipality = municipalityId > 0 ? new Municipality { Id = municipalityId } : null,
                ProgramVersion = new ProgramVersion { Id = versionId },
                UserName = this.User.Name
            };

            var logRecords = new List<ActualizeVersionLogRecord>();

            // Формируем строки для логов
            foreach (var rec in ver3S)
            {
                var logRecord = new ActualizeVersionLogRecord
                {
                    TypeAction = versionActualizeType,
                    Action = action,
                    Description = description,
                    Address = rec.RealityObject.Address,
                    Ceo = rec.CommonEstateObjects,
                    PlanYear = rec.Year,
                    Number = rec.IndexNumber,
                    Volume = volumeDict.ContainsKey(rec) ? volumeDict[rec] : 0m,
                    Sum = rec.Sum
                };

                logRecords.Add(logRecord);
            }

            if (logRecords.Any())
            {
                log.CountActions = logRecords.Count;
                log.LogFile = this.LogService.CreateLogFile(logRecords.OrderBy(x => x.Number), baseParams);
            }
            else
            {
                return new BaseDataResult(false, "Логи отсутсвуют");
            }

            return new BaseDataResult();
        }

        private void SaveActualizationResult(
            VersionActualizeLog log,
            List<VersionRecordStage1> ver1S,
            List<VersionRecordStage2> ver2S,
            List<VersionRecord> ver3S)
        {
            var sessionProvider = this.Container.Resolve<ISessionProvider>();
            var unProxy = this.Container.Resolve<IUnProxy>();

            var session = sessionProvider.OpenStatelessSession();
            var transaction = session.BeginTransaction();
            
            try
            {
                ver3S.ForEach(x =>
                {
                    if (x.Id > 0) session.Update(unProxy.GetUnProxyObject(x));
                    else session.Insert(x);
                });

                ver2S.ForEach(x =>
                {
                    if (x.Stage3Version != null)
                    {
                        if (x.Id > 0) session.Update(x);
                        else session.Insert(x);
                    }
                });

                ver1S.ForEach(x =>
                {
                    if (x.Stage2Version != null)
                    {
                        session.Insert(x);
                    }
                });

                session.Insert(log);

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                transaction.Dispose();
                session.Dispose();
                this.Container.Release(unProxy);
            }
        }
    }
}