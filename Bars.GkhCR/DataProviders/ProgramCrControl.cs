namespace Bars.GkhCr.DataProviders
{
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Analytics.Data;
    using Bars.B4.Modules.Analytics.Enums;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Utils;
    using Bars.GkhCr.DomainService;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;
    using Bars.GkhCr.Meta;
    using Castle.Windsor;

    public class ProgramCrControl : BaseCollectionDataProvider<КонтрольКР>
    {
        public ProgramCrControl(IWindsorContainer container)
            : base(container)
        {
        }

        protected override IQueryable<КонтрольКР> GetDataInternal(BaseParams baseParams)
        {
            var realObjDomain = Container.ResolveDomain<RealityObject>();
            var objectCrDomain = Container.ResolveDomain<ObjectCr>();
            var typeWorkCrDomain = Container.ResolveDomain<TypeWorkCr>();
            var defectListDomain = Container.ResolveDomain<DefectList>();
            var contractCrDomain = Container.ResolveDomain<ContractCr>();
            var builderContractTypeWorkDomain = Container.ResolveDomain<BuildContractTypeWork>();
            var performedWorkActDomain = Container.ResolveDomain<PerformedWorkAct>();
            var realObjDecInfoService = Container.Resolve<IRealObjDecInfoService>();
            var dpkrTypeWorkService = Container.Resolve<IDpkrTypeWorkService>();

            var programCrId = baseParams.Params[string.Format("{0}_programCr", Key)].ToLong();

            try
            {
                var workIdHasDefectList = defectListDomain.GetAll()
                    .Where(x => x.ObjectCr.ProgramCr.Id == programCrId)
                    .Select(x => new
                    {
                        WorkId = x.Work.Id,
                        ObjCrId = x.ObjectCr.Id
                    })
                    .AsEnumerable()
                    .Select(x => "{0}_{1}".FormatUsing(x.WorkId, x.ObjCrId))
                    .ToHashSet();

                var contractCrInfoDict = contractCrDomain.GetAll()
                    .Where(x => x.ObjectCr.ProgramCr.Id == programCrId)
                    .Where(x => x.TypeContractObject.Key == "Psd")
                    .Select(x => new
                    {
                        x.ObjectCr.Id,
                        BuilderName = x.Contragent.Name,
                        x.DocumentNum,
                        x.DateFrom,
                        x.SumContract
                    })
                    .ToList()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.First());

                var builderContractInfoDict = builderContractTypeWorkDomain.GetAll()
                    .Where(x => x.BuildContract.ObjectCr.ProgramCr.Id == programCrId)
                    .Select(x => new
                    {
                        TypeWorkId = x.TypeWork.Id,
                        Builder = x.BuildContract.Builder.Contragent.Name,
                        x.Sum,
                        x.BuildContract.DocumentNum,
                        x.BuildContract.DocumentDateFrom,
                        x.BuildContract.DateEndWork
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.TypeWorkId)
                    .ToDictionary(x => x.Key, y => y.First());

                var perfWorkActInfoDict = performedWorkActDomain.GetAll()
                    .Where(x => x.ObjectCr.ProgramCr.Id == programCrId)
                    .Select(x => new
                    {
                        TypeWorkId = x.TypeWorkCr.Id,
                        x.Sum,
                        x.DateFrom
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.TypeWorkId)
                    .ToDictionary(x => x.Key, y => new
                    {
                        DateFrom = y.SafeMax(x => x.DateFrom),
                        Sum = y.SafeSum(x => x.Sum.ToDecimal())
                    });

                var roIdsHasDecProtocol = new HashSet<long>();

                if (realObjDecInfoService != null)
                {
                    var roQuery = realObjDomain.GetAll()
                        .Where(x => objectCrDomain.GetAll()
                            .Where(y => y.ProgramCr.Id == programCrId)
                            .Any(y => y.RealityObject.Id == x.Id));

                    roIdsHasDecProtocol = realObjDecInfoService.GetRealityObjHasDecProtocol(roQuery);
                }

                var typeWorkQuery = typeWorkCrDomain.GetAll()
                    .Where(x => x.ObjectCr.ProgramCr.Id == programCrId);

                var typeWorkYearDict = new Dictionary<long, int>();

                if (dpkrTypeWorkService != null)
                {
                    typeWorkYearDict = dpkrTypeWorkService.GetTypeWorkDpkrYear(typeWorkQuery);
                }

                var data = typeWorkQuery
                    .Select(x => new
                    {
                        TypeWorkId = x.Id,
                        ObjectId = x.ObjectCr.Id,
                        RealObjId = x.ObjectCr.RealityObject.Id,
                        WorkId = x.Work.Id,
                        MuName = x.ObjectCr.RealityObject.Municipality.Name,
                        SettName = x.ObjectCr.RealityObject.MoSettlement.Name,
                        x.ObjectCr.RealityObject.Address,
                        WorkName = x.Work.Name,
                        x.Volume,
                        x.PercentOfCompletion
                    })
                    .AsEnumerable()
                    .Select(x =>
                    {
                        var result = new КонтрольКР
                        {
                            МР = x.MuName,
                            МО = x.SettName,
                            Адрес = x.Address,
                            ВидРабот = x.WorkName,
                            МощностьПоПроекту = x.Volume.ToDecimal(),
                            ПроцентВыполнения = x.PercentOfCompletion.ToDecimal(),
                            ДоговорСобственника = roIdsHasDecProtocol.Contains(x.RealObjId) ? "да" : "нет",
                            СрокРемонта = typeWorkYearDict.Get(x.TypeWorkId),
                            ДефектнаяВедомость =
                                workIdHasDefectList.Contains("{0}_{1}".FormatUsing(x.WorkId, x.ObjectId)) ? "да" : "нет"
                        };

                        var contractInfo = contractCrInfoDict.Get(x.ObjectId);

                        if (contractInfo != null)
                        {
                            result.ДоговорПСД_Дата = contractInfo.DateFrom.HasValue
                                ? contractInfo.DateFrom.Value.ToShortDateString()
                                : string.Empty;
                            result.ДоговорПСД_Номер = contractInfo.DocumentNum;
                            result.ДоговорПСД_Подрядчик = contractInfo.BuilderName;
                            result.ДоговорПСД_Стоимость = contractInfo.SumContract.ToDecimal();
                        }

                        var builderContractInfo = builderContractInfoDict.Get(x.TypeWorkId);

                        if (builderContractInfo != null)
                        {
                            result.ДоговорКР_Номер = builderContractInfo.DocumentNum;
                            result.ДоговорКР_Подрядчик = builderContractInfo.Builder;
                            result.ДоговорКР_Стоимость = builderContractInfo.Sum.ToDecimal();
                            result.ДоговорКР_Дата = builderContractInfo.DocumentDateFrom.HasValue
                                ? builderContractInfo.DocumentDateFrom.Value.ToShortDateString()
                                : string.Empty;
                            result.ФактИсполнДоговора = builderContractInfo.DateEndWork.HasValue
                                ? builderContractInfo.DateEndWork.Value.ToShortDateString()
                                : string.Empty;
                        }

                        var perfActInfo = perfWorkActInfoDict.Get(x.TypeWorkId);

                        if (perfActInfo != null)
                        {
                            result.ВыданоПСД = "да";
                            result.ФактИсполнения = perfActInfo.DateFrom.HasValue
                                ? perfActInfo.DateFrom.Value.ToShortDateString()
                                : string.Empty;
                            result.СтоимостьПоДоговору = perfActInfo.Sum;
                        }
                        else
                        {
                            result.ВыданоПСД = "нет";
                        }

                        return result;
                    })
                    .ToList();

                return data.AsQueryable();
            }
            finally
            {
                Container.Release(realObjDomain);
                Container.Release(objectCrDomain);
                Container.Release(typeWorkCrDomain);
                Container.Release(defectListDomain);
                Container.Release(contractCrDomain);
                Container.Release(builderContractTypeWorkDomain);
                Container.Release(performedWorkActDomain);
                Container.Release(realObjDecInfoService);
                Container.Release(dpkrTypeWorkService);
            }
        }

        public override string Name
        {
            get { return "Контроль КР"; }
        }

        public override string Key
        {
            get { return typeof(ProgramCrControl).Name; }
        }

        public override string Description
        {
            get { return Name; }
        }

        public override IEnumerable<DataProviderParam> Params
        {
            get
            {
                return new List<DataProviderParam>
                {
                    new DataProviderParam
                    {
                        Name = string.Format("{0}_programCr", Key),
                        ParamType = ParamType.Catalog,
                        Label = "Программа кап.ремонта",
                        Additional = "ProgramCr",
                        Required = true
                    }
                };
            }
        }
    }
}
