namespace Bars.GkhCr.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;

    using B4.Modules.Reports;

    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Utils;
    using Bars.GkhCr.Entities;

    using Castle.Windsor;

    /// <summary>
    /// Генератор отчёта "Планирумые показатели выполнения программы КР"
    /// </summary>
    public class PlanedProgramIndicatorsReport : BasePrintForm
    {
        protected long[] municipalityListId;

        protected long programCrId;

        /// <summary>
        /// Создание экземпляра генератора отчёта
        /// </summary>
        public PlanedProgramIndicatorsReport()
            : base(new ReportTemplateBinary(Properties.Resources.PlanedProgramIndicators))
        {
        }

        /// <summary>
        /// Windsor контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Требуемые разрешения
        /// </summary>
        public override string RequiredPermission => "Reports.CR.PlanedProgramIndicators";

        /// <summary>
        /// Название отчёта
        /// </summary>
        public override string Name => "Планирумые показатели выполнения программы КР";

        /// <summary>
        /// Описание отчёта
        /// </summary>
        public override string Desciption => "Планирумые показатели выполнения программы КР";

        /// <summary>
        /// Группа отчётов
        /// </summary>
        public override string GroupName => "Формы для Фонда";

        /// <summary>
        /// Контроллер параметров для генераци отчёта
        /// </summary>
        public override string ParamsController => "B4.controller.report.PlanedProgramIndicators";

        /// <summary>
        /// Установка базовых параметров для отчёта
        /// </summary>
        /// <param name="baseParams"></param>
        public override void SetUserParams(BaseParams baseParams)
        {
            programCrId = baseParams.Params["programCrId"].ToInt();

            var municipalityStr = baseParams.Params["municipalityIds"].ToString();
            if (string.IsNullOrEmpty(municipalityStr))
            {
                municipalityListId = new long[0];
                return;
            }

            municipalityListId = municipalityStr.Split(',').Select(x => x.ToLong()).ToArray();
        }

        public override string ReportGenerator { get; set; }

        /// <summary>
        /// Генерация отчёта
        /// </summary>
        /// <param name="reportParams"></param>
        public override void PrepareReport(ReportParams reportParams)
        {
            var typeWorkCrService = this.Container.Resolve<IDomainService<TypeWorkCr>>();
            var municipalityService = this.Container.Resolve<IDomainService<Municipality>>();
            var objectCrService = this.Container.Resolve<IDomainService<ObjectCr>>();

            using (this.Container.Using(typeWorkCrService, municipalityService, objectCrService))
            {
                var objectCrDict = this.GetObjectCrDict(objectCrService, typeWorkCrService);

                var municipalities =
                    municipalityService.GetAll()
                        .WhereIf(this.municipalityListId.Length > 0, x => this.municipalityListId.Contains(x.Id))
                        .Select(x => new MunicipalityDataProxy { Id = x.Id, Name = x.Name, Group = x.Group })
                        .AsEnumerable().DistinctBy(x => x.Id).ToList();

                foreach (var municipality in municipalities.Where(x => !objectCrDict.ContainsKey(x)))
                {
                    objectCrDict.Add(municipality, null);
                }

                var counter = 0;
                var summuryAreaMkd = 0m;
                var summuryCountPeople = 0m;
                var summuryCountMkd4 = 0m;
                var summuryCostCr4 = 0m;

                var section = reportParams.ComplexReportParams.ДобавитьСекцию("section1");

                var dict = new Dictionary<string, Record>();
                foreach (var t in objectCrDict)
                {
                    var key = string.IsNullOrEmpty(t.Key.Group) ? t.Key.Name : t.Key.Group;

                    if (dict.ContainsKey(key))
                    {
                        if (t.Value != null)
                        {
                            dict[key].AreaMkd += t.Value.AreaMkd;
                            dict[key].NumberLiving += t.Value.NumberLiving;
                            dict[key].SumByTypeWorkCr += t.Value.SumByTypeWorkCr;
                            dict[key].CountHouses += t.Value.CountHouses;
                        }
                    }
                    else
                    {
                        if (t.Value != null)
                        {
                            dict.Add(key,
                                new Record
                                {
                                    AreaMkd = t.Value.AreaMkd,
                                    CountHouses = t.Value.CountHouses,
                                    NumberLiving = t.Value.NumberLiving,
                                    SumByTypeWorkCr = t.Value.SumByTypeWorkCr
                                });
                        }
                        else
                        {
                            dict.Add(key, new Record { AreaMkd = 0M, CountHouses = 0, NumberLiving = 0, SumByTypeWorkCr = 0M });
                        }
                    }
                }

                foreach (var objectCr in dict.OrderBy(x => x.Key))
                {
                    section.ДобавитьСтроку();

                    section["NumberPp"] = ++counter;
                    section["Mo"] = objectCr.Key;

                    if (objectCr.Value == null) continue;

                    section["AreaMkd"] = objectCr.Value.AreaMkd.HasValue ? objectCr.Value.AreaMkd.Value : 0M;
                    summuryAreaMkd += objectCr.Value.AreaMkd.HasValue ? objectCr.Value.AreaMkd.Value : 0m;

                    section["CountPeople"] = objectCr.Value.NumberLiving;
                    summuryCountPeople += objectCr.Value.NumberLiving.HasValue ? objectCr.Value.NumberLiving.Value : 0m;

                    section["CountMkd4"] = objectCr.Value.CountHouses;
                    summuryCountMkd4 += objectCr.Value.CountHouses;

                    section["CostCr4"] = objectCr.Value.SumByTypeWorkCr.HasValue ? objectCr.Value.SumByTypeWorkCr.Value : 0M;
                    summuryCostCr4 += objectCr.Value.SumByTypeWorkCr.HasValue ? objectCr.Value.SumByTypeWorkCr.Value : 0m;
                }

                reportParams.SimpleReportParams["AreaMkdSummury"] = summuryAreaMkd;
                reportParams.SimpleReportParams["CountPeopleSummury"] = summuryCountPeople;
                reportParams.SimpleReportParams["CountMkdSummury4"] = summuryCountMkd4;
                reportParams.SimpleReportParams["CostCrSummury4"] = summuryCostCr4;
            }
        }

        /// <summary>
        /// Генерация данных по работам КР
        /// Переопределяемый, так как в некоторых регионах отчёт формируется не по MoSettlement
        /// </summary>
        /// <param name="objectCrService"></param>
        /// <param name="typeWorkCrService"></param>
        /// <returns></returns>
        protected virtual Dictionary<MunicipalityDataProxy, Record> GetObjectCrDict(
            IDomainService<ObjectCr> objectCrService,
            IDomainService<TypeWorkCr> typeWorkCrService)
        {
            return objectCrService.GetAll()
                .WhereIf(this.municipalityListId.Length > 0, x => this.municipalityListId.Contains(x.RealityObject.MoSettlement.Id))
                .Where(x => x.ProgramCr.Id == this.programCrId
                    && typeWorkCrService.GetAll().Any(y => y.ObjectCr.Id == x.Id && y.FinanceSource.Code == "1"))
                .Select(x => new
                {
                    x.RealityObject.AreaMkd,
                    x.RealityObject.NumberLiving,
                    MunicipalityId = x.RealityObject.Municipality.Id,
                    MunicipalityName = x.RealityObject.Municipality.Name,
                    MunicipalityGroup = x.RealityObject.Municipality.Group,
                    SumByTypeWorkCr = typeWorkCrService.GetAll().Where(y => y.ObjectCr.Id == x.Id && y.FinanceSource.Code == "1").Sum(y => y.Sum)
                })
                .AsEnumerable()
                .GroupBy(x => new MunicipalityDataProxy { Id = x.MunicipalityId, Name = x.MunicipalityName, Group = x.MunicipalityGroup })
                .ToDictionary(x => x.Key,
                    x => new Record
                    {
                        AreaMkd = x.Sum(y => y.AreaMkd),
                        NumberLiving = x.Sum(y => y.NumberLiving),
                        SumByTypeWorkCr = x.Sum(y => y.SumByTypeWorkCr),
                        CountHouses = x.Count()
                    });
        }


        /// <summary>
        /// Так как данный класс испольуется в качестве ключа Dictionary, то ContainsKey не будет верно отрабатывать
        /// Реализовал интерфейс IEquatable для проверки совпадения по полям.
        /// </summary>
        protected class MunicipalityDataProxy : IEquatable<MunicipalityDataProxy>
        {
            /// <summary>
            /// Идентификатор
            /// </summary>
            public long Id { get; set; }

            /// <summary>
            /// Наименование
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Группа
            /// </summary>
            public string Group { get; set; }

            public override bool Equals(object obj)
            {
                return obj is MunicipalityDataProxy && Equals((MunicipalityDataProxy) obj);
            }

            /// <summary>
            /// Сравнение по трём свойствам Id, Name, Group
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public bool Equals(MunicipalityDataProxy other)
            {
                return other != null && Id == other.Id && Group == other.Group && Name == other.Name;
            }
        }

        protected class Record
        {
            /// <summary>
            /// Общая площадь МКД (кв.м.)
            /// </summary>
            public decimal? AreaMkd { get; set; }

            /// <summary>
            /// Количество проживающих
            /// </summary>
            public int? NumberLiving { get; set; }

            /// <summary>
            /// Общая плановая сумма
            /// </summary>
            public decimal? SumByTypeWorkCr { get; set; }

            /// <summary>
            /// Количесто домов
            /// </summary>
            public int CountHouses { get; set; }
        }
    }
}