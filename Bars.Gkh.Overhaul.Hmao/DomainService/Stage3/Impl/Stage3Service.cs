namespace Bars.Gkh.Overhaul.Hmao.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities.CommonEstateObject;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    public class Stage3Service : IStage3Service
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<RealityObjectStructuralElementInProgramm> Stage1DomainService { get; set; }

        public IDomainService<RealityObjectStructuralElementInProgrammStage3> DomainService { get; set; }

        /// <summary>
        /// Получение дерева ООИ с детализацией по КЭ
        /// </summary>
        /// <param name="baseParams">Содержащий st3Id - id 3 этапа ДПКР</param>
        /// <returns>Дерево</returns>
        public IDataResult ListDetails(BaseParams baseParams)
        {
            var stage3Id = baseParams.Params.GetAs<long>("st3Id");

            var data = this.Stage1DomainService.GetAll()
                .Where(x => x.Stage2.Stage3.Id == stage3Id)
                .Select(
                    x => new
                    {
                        Stage2Year = x.Stage2.Year,
                        Stage2Sum = x.Stage2.Sum,
                        Stage2CeoName = x.Stage2.CommonEstateObject.Name,
                        x.Sum,
                        x.ServiceCost,
                        StructElementName = x.StructuralElement.StructuralElement.Name,
                        UnitMeasureName = x.StructuralElement.StructuralElement.UnitMeasure.Name,
                        x.StructuralElement.Volume,
                        x.Year
                    })
                .AsEnumerable();

            var result = data
                .GroupBy(x => new {Year = x.Stage2Year, Sum = x.Stage2Sum, Name = x.Stage2CeoName})
                .Select(
                    x => new
                    {
                        x.Key.Year,
                        ServiceAndWorkSum = x.Sum(y => y.Sum) + x.Sum(y => y.ServiceCost),
                        x.Key.Name,
                        leaf = false,
                        WorkSum = x.Sum(y => y.Sum),
                        ServiceSum = x.Sum(y => y.ServiceCost),
                        Volume = x.Sum(y => y.Volume),
                        Children = x.Select(
                            y =>
                                new
                                {
                                    Name = y.StructElementName,
                                    WorkSum = y.Sum,
                                    ServiceSum = y.ServiceCost,
                                    Measure = y.UnitMeasureName,
                                    ServiceAndWorkSum = y.ServiceCost + y.Sum,
                                    y.Volume,
                                    y.Year,
                                    leaf = true
                                })
                            .AsEnumerable()
                    })
                .AsEnumerable();

            return new BaseDataResult(new {Children = result});
        }

        /// <summary>
        /// Получение краткой информации по 3 этапу ДПКР
        /// </summary>
        /// <param name="baseParams">Содержащий st3Id - id 3 этапа ДПКР</param>
        /// <returns>Объект</returns>
        public IDataResult GetInfo(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId("st3Id");

            var stage3 = this.DomainService.Get(id);

            if (stage3 == null)
            {
                return new BaseDataResult(false, "Не найдена информация по 3 этапу ДПКР");
            }

            var stage1Data = this.Stage1DomainService.GetAll()
                .Where(x => x.Stage2.Stage3.Id == id)
                .Select(
                    x => new
                    {
                        x.ServiceCost,
                        x.Sum
                    });

            decimal workSum = 0;
            decimal serviceSum = 0;

            if (stage1Data.Any())
            {
                workSum = stage1Data.Sum(x => x.Sum);
                serviceSum = stage1Data.Sum(x => x.ServiceCost);
            }

            var identity = this.Container.Resolve<IUserIdentity>();

            var proxy = new
            {
                stage3.RealityObject.Address,
                stage3.IndexNumber,
                stage3.Point,
                stage3.Year,
                WorkSum = workSum,
                ServiceSum = serviceSum,
                ServiceAndWorkSum = workSum + serviceSum,
                ShowWorks = this.Container.Resolve<IAuthorizationService>().Grant(identity, "Ovrhl.LongProgram.WorkView")
            };

            return new BaseDataResult(proxy);
        }

        /// <summary>
        /// Получение видов работ по 3 этапу ДПКР
        /// </summary>
        /// <param name="baseParams">Содержащий st3Id - id 3 этапа ДПКР</param>
        /// <returns>Список видов работ</returns>
        public IDataResult ListWorkTypes(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId("st3Id");

            var stage3 = this.DomainService.Get(id);

            if (stage3 == null)
            {
                return new BaseDataResult(false, "Не найдена информация по 3 этапу ДПКР");
            }

            // идентификаторы и суммы конструктивных элементов, которые связаны с этой записью 3 этапа
            var stage1StructElementsInfo = this.Stage1DomainService.GetAll()
                .Where(x => x.Stage2.Stage3.Id == id)
                .Select(
                    x => new
                    {
                        SeId = x.StructuralElement.StructuralElement.Id,
                        x.Sum,
                        x.ServiceCost
                    });

            var strElWorkDomain = this.Container.Resolve<IDomainService<StructuralElementWork>>();
            var roStructElemService = this.Container.Resolve<IDomainService<RealityObjectStructuralElement>>();
            var result = new List<object>();

            using (this.Container.Using(strElWorkDomain, roStructElemService))
            {
                var roStrEls = roStructElemService.GetAll()
                    .Where(x => stage1StructElementsInfo.Any(y => y.SeId == x.StructuralElement.Id))
                    .Where(x => x.RealityObject.Id == stage3.RealityObject.Id)
                    .Select(
                        x => new RoStrElProxy
                        {
                            SeId = x.StructuralElement.Id,
                            Volume = x.Volume,
                            CalculateBy = x.StructuralElement.CalculateBy,
                            AreaLiving = x.RealityObject.AreaLiving,
                            AreaLivingNotLivingMkd = x.RealityObject.AreaLivingNotLivingMkd,
                            AreaMkd = x.RealityObject.AreaMkd,
                            MuId = x.RealityObject.Municipality.Id,
                            RoId = x.RealityObject.Id,
                            CapitalGroupId = ((long?) x.RealityObject.CapitalGroup.Id) ?? 0
                        })
                    .ToList();

                /* Получили связки КЭ - работа для КЭ из первого этапа */
                var strElWorks = strElWorkDomain.GetAll()
                    .Where(x => stage1StructElementsInfo.Any(y => y.SeId == x.StructuralElement.Id))
                    .Select(
                        x => new
                        {
                            SeId = x.StructuralElement.Id,
                            SeName = x.StructuralElement.Name,
                            WorkName = x.Job.Work.Name,
                            x.Job.Work.TypeWork,
                            JobId = x.Job.Id,
                        })
                    .AsEnumerable()
                    .GroupBy(x => new {x.SeId, x.SeName})
                    .ToDictionary(x => x.Key, y => y.ToList());

                foreach (var strElWork in strElWorks)
                {
                    var roStrEl = roStrEls.FirstOrDefault(x => x.SeId == strElWork.Key.SeId);
                    var stage1StructElement = stage1StructElementsInfo.FirstOrDefault(x => x.SeId == strElWork.Key.SeId);

                    foreach (var elWork in strElWork.Value)
                    {
                        result.Add(
                            new
                            {
                                StructElement = elWork.SeName,
                                WorkKind = elWork.WorkName,
                                WorkType = elWork.TypeWork,
                                Volume = roStrEl.Return(x => x.Volume),
                                Sum = stage1StructElement.Return(x => x.Sum) + stage1StructElement.Return(x => x.ServiceCost)
                            });
                    }
                }
            }

            return new ListDataResult(result, result.Count);
        }

        /// <summary>
        /// Получение записей 3 этапа ДПКР с КЭ
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Список записей 3 этапа</returns>
        public IQueryable<VersionRecordDto> ListWithStructElements(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var version = loadParams.Filter.GetAs<long>("version");
            var showHidden = baseParams.Params.GetAs<bool>("showHidden");
            var showMain = baseParams.Params.GetAs<bool>("ShowMainVersion");
            var showSubversion = baseParams.Params.GetAs<bool>("ShowSubVersion");

            var stage1Domain = this.Container.ResolveDomain<VersionRecordStage1>();
            var stage3Domain = this.Container.ResolveDomain<VersionRecord>();
            var structElGrAttrDomain = this.Container.ResolveDomain<StructuralElementGroupAttribute>();
            var structElAttrValDomain = this.Container.ResolveDomain<RealityObjectStructuralElementAttributeValue>();
            var TypeWorkCrVersionStage1Domain = this.Container.ResolveDomain<TypeWorkCrVersionStage1>();

            var test = structElAttrValDomain.GetAll()
                .Where(x => x.Attribute.Name == "Номер подъезда")
                .Where(x => x.Value != "")
                .Select(x => new
                {
                    x.Object.Id,
                    Value = "подъезд " + x.Value
                });
            Dictionary<long, string> strElAttribVal = new Dictionary<long, string>();
            if (test != null)
            {
                try
                {
                    strElAttribVal = test.AsEnumerable().GroupBy(x => x.Id)
                       .ToDictionary(x => x.Key, y => y.Distinct().AggregateWithSeparator(x => x.Value, ", "));
                }
                catch (Exception e)
                {
                    string str = e.ToString();
                }
            }

            using (this.Container.Using(stage1Domain, stage3Domain))
            {
                //словарь КЭ
                var structElDict = stage1Domain.GetAll()
                    //.Where(x => x.VersionRecordState != Hmao.Enum.VersionRecordState.NonActual)
                    .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.Id == version)
                    .Select(
                        x => new
                        {
                            x.Stage2Version.Stage3Version.Id,
                            StructuralElement = x.StructuralElement.Name != "" && x.StructuralElement.Name != null? x.StructuralElement.Name: x.StructuralElement.StructuralElement.Name

                        })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.Distinct().AggregateWithSeparator(x => x.StructuralElement, ", "));

                var structElatrDict = stage1Domain.GetAll()
                    .Where(x => x.VersionRecordState != Hmao.Enum.VersionRecordState.NonActual)
                    .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.Id == version)
                    .Select(
                        x => new
                        {
                            x.Stage2Version.Stage3Version.Id,
                            StructuralElement = strElAttribVal.ContainsKey(x.StructuralElement.Id) ? " " + strElAttribVal[x.StructuralElement.Id] : ""

                        })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.Distinct().AggregateWithSeparator(x => x.StructuralElement, ", "));


                //словарь КПКР
                var kpkrDict = TypeWorkCrVersionStage1Domain.GetAll()
                    .Where(x => x.Stage1Version.VersionRecordState != Hmao.Enum.VersionRecordState.NonActual)
                    .Where(x => x.Stage1Version.Stage2Version.Stage3Version.ProgramVersion.Id == version)
                    .Select(
                        x => new
                        {
                            x.Stage1Version.Stage2Version.Stage3Version.Id,
                            KPKR = x.TypeWorkCr.ObjectCr.ProgramCr.Name
                        })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.Distinct().AggregateWithSeparator(x => x.KPKR, ", "));

                return stage3Domain.GetAll()
                    .Where(x => x.ProgramVersion.Id == version)
                    .WhereIf(!showSubversion && !showHidden && !showMain, x=> x.Id==0)
                    .WhereIf(!showSubversion && !showHidden && showMain, x => x.Show && !x.SubProgram)
                    .WhereIf(!showSubversion && showHidden && showMain, x => !x.SubProgram)
                    .WhereIf(showSubversion && !showHidden && showMain, x => x.Show)
                    .WhereIf(showSubversion && showHidden && !showMain, x => x.SubProgram)
                    .WhereIf(showSubversion && !showHidden && !showMain, x => x.SubProgram && x.Show)
                    .WhereIf(!showSubversion && showHidden && !showMain, x=> !x.Show)
                    .Select(
                        x => new
                        {
                            x.Id,
                            Municipality = x.RealityObject.Municipality.Name,
                            RealityObject = x.RealityObject.Address,
                            HouseNumber = x.RealityObject.FiasAddress.House,
                            x.CommonEstateObjects,
                            x.Year,
                            x.FixedYear,
                            x.YearCalculated,
                            x.ChangeBasisType,
                            x.IndexNumber,
                            x.IsChangedYear,
                            x.Point,
                            x.Sum,
                            x.Changes,
                            x.Show,
                            x.SubProgram,
                            x.Remark,
                            x.WorkCode
                        })
                    .AsEnumerable()
                    .Select(
                        x => new VersionRecordDto
                        {
                            Id = x.Id,
                            Municipality = x.Municipality,
                            RealityObject = x.RealityObject,
                            CommonEstateObjects = x.CommonEstateObjects,
                            Year = x.Year,
                            FixedYear = x.FixedYear,
                            YearCalculated = x.YearCalculated,
                            ChangeBasisType = x.ChangeBasisType,
                            IndexNumber = x.IndexNumber,
                            IsChangedYear = x.IsChangedYear,
                            HouseNumber = x.HouseNumber,
                            Point = x.Point,
                            Sum = x.Sum,
                            Changes = x.Changes,
                            StructuralElements = structElDict.Get(x.Id) ?? string.Empty,
                            EntranceNum = structElatrDict.Get(x.Id) ?? string.Empty,
                            KPKR = kpkrDict.Get(x.Id) ?? string.Empty,
                            Hidden = !x.Show,
                            IsSubProgram = x.SubProgram,
                            Remark = x.Remark,
                            WorkCode = x.WorkCode
                        })
                    .AsQueryable()
                    .Filter(loadParams, this.Container)
                    .OrderIf(loadParams.Order.Length == 0, true, x => x.Year)
                    .OrderThenIf(loadParams.Order.Length == 0, true, x => x.IndexNumber)
                    .Order(loadParams);
            }
        }
    }

    internal class RoStrElProxy
    {
        public long SeId;
        public decimal Volume;
        public PriceCalculateBy CalculateBy;
        public decimal? AreaLiving;
        public decimal? AreaMkd;
        public decimal? AreaLivingNotLivingMkd;
        public long MuId;
        public long CapitalGroupId;
        public long RoId;
    }
}